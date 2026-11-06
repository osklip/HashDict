using HashDict.Core;
using Xunit;

namespace HashDict.Tests;

public class MethodsTests
{
    [Fact]
    public void GenerateEntries_Returns_Requested_Count_Of_Unique_Keys()
    {
        var entries = Methods.GenerateEntries(500);

        Assert.Equal(500, entries.Count);

        var keys = new HashSet<string>();
        foreach (var pair in entries)
            keys.Add(pair.Key);

        Assert.Equal(500, keys.Count);
    }

    [Fact]
    public void GenerateEntries_Is_Deterministic_For_Same_Seed()
    {
        var first = Methods.GenerateEntries(20, seed: 7);
        var second = Methods.GenerateEntries(20, seed: 7);

        for (int i = 0; i < first.Count; i++)
        {
            Assert.Equal(first[i].Key, second[i].Key);
            Assert.Equal(first[i].Value, second[i].Value);
        }
    }

    [Fact]
    public void GenerateEntries_Negative_Count_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Methods.GenerateEntries(-1));
    }

    [Fact]
    public void FormatBucket_Empty_Chain()
    {
        var chain = new List<KeyValuePair<string, string>>();

        Assert.Equal("[ 3] -", Methods.FormatBucket(3, 2, chain));
    }

    [Fact]
    public void FormatBucket_Chain_With_Two_Elements()
    {
        var chain = new List<KeyValuePair<string, string>>
        {
            new("ala", "kot"),
            new("ola", "pies"),
        };

        Assert.Equal("[ 3] (ala: kot) -> (ola: pies)", Methods.FormatBucket(3, 2, chain));
    }

    [Fact]
    public void FormatDuration_Uses_Microseconds_Below_One_Millisecond()
    {
        string text = Methods.FormatDuration(TimeSpan.FromMilliseconds(0.25));

        Assert.True(text.EndsWith("µs"));
    }

    [Fact]
    public void FormatDuration_Uses_Milliseconds_From_One_Millisecond()
    {
        string text = Methods.FormatDuration(TimeSpan.FromMilliseconds(12.5));

        Assert.True(text.EndsWith("ms"));
    }
}
