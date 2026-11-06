using System.Diagnostics.CodeAnalysis;

namespace HashDict.Core;

// Statystyki tablicy mieszającej.
// Collisions = liczba elementów, które nie są pierwsze w swoim kubełku
//              (Count - UsedBuckets), czyli ile elementów "dzieli" kubełek z innymi.
public readonly record struct TableStats(
    int Count,
    int BucketCount,
    int UsedBuckets,
    int Collisions,
    int LongestChain,
    double LoadFactor);

// Słownik klucz-wartość (string -> string) oparty na tablicy mieszającej.
// Kolizje są rozwiązywane metodą łańcuchową: każdy kubełek zawiera listę
// jednokierunkową elementów, których klucze mają ten sam indeks.
public sealed class ChainedHashTable
{
    // Element listy w kubełku.
    private sealed class Node
    {
        public Node(string key, string value, Node? next)
        {
            Key = key;
            Value = value;
            Next = next;
        }

        public string Key { get; }
        public string Value { get; set; }
        public Node? Next { get; set; }
    }

    public const int DefaultCapacity = 8;

    // Gdy współczynnik zapełnienia (Count / liczba kubełków) przekroczyłby tę wartość,
    // tablica jest powiększana dwukrotnie, a elementy są mieszane od nowa.
    public const double MaxLoadFactor = 0.75;

    private Node?[] _buckets;

    public ChainedHashTable(int capacity = DefaultCapacity)
    {
        if (capacity < 1)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Liczba kubełków musi być dodatnia.");

        _buckets = new Node?[capacity];
    }

    public int Count { get; private set; }

    public int BucketCount => _buckets.Length;

    // Funkcja skrótu djb2: hash = hash * 33 + znak, start od 5381.
    // Arytmetyka na uint jest cykliczna (przepełnienie jest zamierzone).
    public static uint Hash(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        unchecked
        {
            uint hash = 5381;
            foreach (char c in key)
                hash = (hash << 5) + hash + (uint)c;
            return hash;
        }
    }

    // Indeks kubełka dla klucza przy bieżącej liczbie kubełków.
    public int GetBucketIndex(string key) => IndexFor(key, _buckets.Length);

    private static int IndexFor(string key, int bucketCount) => (int)(Hash(key) % (uint)bucketCount);

    // Dodaje parę lub zmienia wartość istniejącego klucza.
    // Zwraca true, gdy klucz był nowy; false, gdy zaktualizowano istniejący.
    public bool Add(string key, string value)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        int index = IndexFor(key, _buckets.Length);

        for (Node? node = _buckets[index]; node != null; node = node.Next)
        {
            if (node.Key == key)
            {
                node.Value = value;
                return false;
            }
        }

        if (Count + 1 > _buckets.Length * MaxLoadFactor)
        {
            Resize(_buckets.Length * 2);
            index = IndexFor(key, _buckets.Length);
        }

        // Nowy element trafia na początek łańcucha.
        _buckets[index] = new Node(key, value, _buckets[index]);
        Count++;
        return true;
    }

    public bool TryGet(string key, [NotNullWhen(true)] out string? value)
    {
        ArgumentNullException.ThrowIfNull(key);

        for (Node? node = _buckets[IndexFor(key, _buckets.Length)]; node != null; node = node.Next)
        {
            if (node.Key == key)
            {
                value = node.Value;
                return true;
            }
        }

        value = null;
        return false;
    }

    public bool ContainsKey(string key) => TryGet(key, out _);

    // Usuwa klucz. Zwraca true, jeśli klucz istniał.
    public bool Remove(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        int index = IndexFor(key, _buckets.Length);
        Node? previous = null;

        for (Node? node = _buckets[index]; node != null; previous = node, node = node.Next)
        {
            if (node.Key != key)
                continue;

            if (previous == null)
                _buckets[index] = node.Next;
            else
                previous.Next = node.Next;

            Count--;
            return true;
        }

        return false;
    }

    // Usuwa wszystkie elementy i przywraca początkową liczbę kubełków.
    public void Clear()
    {
        _buckets = new Node?[DefaultCapacity];
        Count = 0;
    }

    // Pozycja klucza w łańcuchu swojego kubełka (od 1); 0, jeśli klucza nie ma.
    public int GetChainPosition(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        int position = 1;
        for (Node? node = _buckets[IndexFor(key, _buckets.Length)]; node != null; node = node.Next, position++)
        {
            if (node.Key == key)
                return position;
        }

        return 0;
    }

    // Migawka zawartości: element i listy to łańcuch kubełka o indeksie i.
    public List<List<KeyValuePair<string, string>>> GetChains()
    {
        var chains = new List<List<KeyValuePair<string, string>>>(_buckets.Length);

        foreach (Node? head in _buckets)
        {
            var chain = new List<KeyValuePair<string, string>>();
            for (Node? node = head; node != null; node = node.Next)
                chain.Add(new KeyValuePair<string, string>(node.Key, node.Value));
            chains.Add(chain);
        }

        return chains;
    }

    public TableStats GetStats()
    {
        int used = 0;
        int longest = 0;

        foreach (Node? head in _buckets)
        {
            if (head == null)
                continue;

            used++;
            int length = 0;
            for (Node? node = head; node != null; node = node.Next)
                length++;
            longest = Math.Max(longest, length);
        }

        return new TableStats(
            Count,
            _buckets.Length,
            used,
            Count - used,
            longest,
            (double)Count / _buckets.Length);
    }

    // Powiększa tablicę i przenosi istniejące węzły do nowych kubełków
    // (indeks zależy od liczby kubełków, więc trzeba go policzyć ponownie).
    private void Resize(int newSize)
    {
        var newBuckets = new Node?[newSize];

        foreach (Node? head in _buckets)
        {
            Node? node = head;
            while (node != null)
            {
                Node? next = node.Next;
                int index = IndexFor(node.Key, newSize);
                node.Next = newBuckets[index];
                newBuckets[index] = node;
                node = next;
            }
        }

        _buckets = newBuckets;
    }
}
