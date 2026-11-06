using HashDict.Core;
using Xunit;

namespace HashDict.Tests;

public class ChainedHashTableTests
{
    // Klucze o identycznej wartości funkcji skrótu (djb2 jest liniowa, a "Aa" i "B@" dają ten sam skrót,
    // więc kolejne złożenia też). Trafiają do tego samego kubełka niezależnie od liczby kubełków.
    private static readonly string[] Colliding = { "AaAa", "AaB@", "B@Aa", "B@B@" };

    [Fact]
    public void Colliding_Keys_Have_Equal_Hash()
    {
        uint expected = ChainedHashTable.Hash(Colliding[0]);

        foreach (string key in Colliding)
            Assert.Equal(expected, ChainedHashTable.Hash(key));
    }

    [Fact]
    public void Hash_Matches_Known_Values()
    {
        Assert.Equal(5381u, ChainedHashTable.Hash(""));
        Assert.Equal(177670u, ChainedHashTable.Hash("a")); // 5381 * 33 + 97
    }

    [Fact]
    public void Add_NewKey_ReturnsTrue_And_IncreasesCount()
    {
        var table = new ChainedHashTable();

        Assert.True(table.Add("ala", "kot"));
        Assert.Equal(1, table.Count);
    }

    [Fact]
    public void Add_ExistingKey_UpdatesValue_Without_ChangingCount()
    {
        var table = new ChainedHashTable();
        table.Add("ala", "kot");

        Assert.False(table.Add("ala", "pies"));
        Assert.Equal(1, table.Count);

        Assert.True(table.TryGet("ala", out string? value));
        Assert.Equal("pies", value);
    }

    [Fact]
    public void Keys_Are_Case_Sensitive()
    {
        var table = new ChainedHashTable();
        table.Add("a", "1");
        table.Add("A", "2");

        Assert.Equal(2, table.Count);
        Assert.True(table.TryGet("a", out string? lower));
        Assert.True(table.TryGet("A", out string? upper));
        Assert.Equal("1", lower);
        Assert.Equal("2", upper);
    }

    [Fact]
    public void TryGet_MissingKey_ReturnsFalse()
    {
        var table = new ChainedHashTable();
        table.Add("ala", "kot");

        Assert.False(table.TryGet("ola", out string? value));
        Assert.True(value == null);
        Assert.False(table.ContainsKey("ola"));
        Assert.True(table.ContainsKey("ala"));
    }

    [Fact]
    public void Remove_ExistingKey_ReturnsTrue_And_RemovesIt()
    {
        var table = new ChainedHashTable();
        table.Add("ala", "kot");

        Assert.True(table.Remove("ala"));
        Assert.Equal(0, table.Count);
        Assert.False(table.ContainsKey("ala"));
    }

    [Fact]
    public void Remove_MissingKey_ReturnsFalse()
    {
        var table = new ChainedHashTable();
        table.Add("ala", "kot");

        Assert.False(table.Remove("ola"));
        Assert.Equal(1, table.Count);
    }

    [Fact]
    public void Colliding_Keys_Share_One_Bucket_And_Are_All_Retrievable()
    {
        var table = new ChainedHashTable();

        for (int i = 0; i < Colliding.Length; i++)
            table.Add(Colliding[i], i.ToString());

        TableStats stats = table.GetStats();
        Assert.Equal(4, stats.Count);
        Assert.Equal(1, stats.UsedBuckets);
        Assert.Equal(3, stats.Collisions);
        Assert.Equal(4, stats.LongestChain);

        for (int i = 0; i < Colliding.Length; i++)
        {
            Assert.True(table.TryGet(Colliding[i], out string? value));
            Assert.Equal(i.ToString(), value);
        }
    }

    [Fact]
    public void Remove_From_Head_Middle_And_Tail_Of_Chain_Keeps_The_Others()
    {
        // Kolejność w łańcuchu: nowe elementy trafiają na początek,
        // więc po dodaniu 0..3 łańcuch to: [3] -> [2] -> [1] -> [0].
        string head = Colliding[3];
        string middle = Colliding[2];
        string tail = Colliding[0];
        string other = Colliding[1];

        var table = new ChainedHashTable();
        foreach (string key in Colliding)
            table.Add(key, "x");

        Assert.True(table.Remove(head));
        Assert.True(table.Remove(tail));
        Assert.True(table.Remove(middle));

        Assert.Equal(1, table.Count);
        Assert.True(table.ContainsKey(other));
        Assert.False(table.ContainsKey(head));
        Assert.False(table.ContainsKey(middle));
        Assert.False(table.ContainsKey(tail));
    }

    [Fact]
    public void GetChainPosition_Reflects_Insertion_At_Head()
    {
        var table = new ChainedHashTable();
        table.Add(Colliding[0], "a");
        table.Add(Colliding[1], "b");

        Assert.Equal(1, table.GetChainPosition(Colliding[1])); // dodany później, na początku
        Assert.Equal(2, table.GetChainPosition(Colliding[0]));
        Assert.Equal(0, table.GetChainPosition("brak"));
    }

    [Fact]
    public void Table_Grows_And_Keeps_All_Elements()
    {
        var table = new ChainedHashTable();
        int initialBuckets = table.BucketCount;

        for (int i = 0; i < 1000; i++)
            table.Add("klucz_" + i, "wartość_" + i);

        Assert.Equal(1000, table.Count);
        Assert.True(table.BucketCount > initialBuckets);
        Assert.True(table.GetStats().LoadFactor <= ChainedHashTable.MaxLoadFactor);

        for (int i = 0; i < 1000; i++)
        {
            Assert.True(table.TryGet("klucz_" + i, out string? value));
            Assert.Equal("wartość_" + i, value);
        }
    }

    [Fact]
    public void GetBucketIndex_Is_Within_Range()
    {
        var table = new ChainedHashTable(5);

        foreach (string key in new[] { "", "a", "ala ma kota", "zażółć gęślą jaźń" })
        {
            int index = table.GetBucketIndex(key);
            Assert.True(index >= 0 && index < table.BucketCount);
        }
    }

    [Fact]
    public void GetChains_Contains_Every_Element_Once()
    {
        var table = new ChainedHashTable();
        for (int i = 0; i < 50; i++)
            table.Add("k" + i, i.ToString());

        List<List<KeyValuePair<string, string>>> chains = table.GetChains();

        int total = 0;
        foreach (var chain in chains)
            total += chain.Count;

        Assert.Equal(table.BucketCount, chains.Count);
        Assert.Equal(50, total);
    }

    [Fact]
    public void Clear_Removes_Everything_And_Resets_Buckets()
    {
        var table = new ChainedHashTable();
        for (int i = 0; i < 100; i++)
            table.Add("k" + i, "v");

        table.Clear();

        Assert.Equal(0, table.Count);
        Assert.Equal(ChainedHashTable.DefaultCapacity, table.BucketCount);
        Assert.False(table.ContainsKey("k1"));
    }

    [Fact]
    public void Stats_Of_Empty_Table_Are_Zero()
    {
        TableStats stats = new ChainedHashTable().GetStats();

        Assert.Equal(0, stats.Count);
        Assert.Equal(0, stats.UsedBuckets);
        Assert.Equal(0, stats.Collisions);
        Assert.Equal(0, stats.LongestChain);
        Assert.Equal(0.0, stats.LoadFactor);
    }

    [Fact]
    public void Null_Arguments_Throw()
    {
        var table = new ChainedHashTable();

        Assert.Throws<ArgumentNullException>(() => table.Add(null!, "x"));
        Assert.Throws<ArgumentNullException>(() => table.Add("x", null!));
        Assert.Throws<ArgumentNullException>(() => table.TryGet(null!, out _));
        Assert.Throws<ArgumentNullException>(() => table.Remove(null!));
    }

    [Fact]
    public void Invalid_Capacity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ChainedHashTable(0));
    }
}
