namespace HashDict;

// Słownik (klucz -> wartość) oparty na tablicy mieszającej.
// Kolizje rozwiązujemy metodą łańcuchową: każdy kubełek to lista elementów.
public class ChainedHashTable
{
    // Pojedynczy element listy w kubełku.
    private class Node
    {
        public string Key;
        public string Value;
        public Node? Next; // następny element w tym samym kubełku (null = koniec listy)

        public Node(string key, string value, Node? next)
        {
            Key = key;
            Value = value;
            Next = next;
        }
    }

    // Gdy elementów jest więcej niż 75% liczby kubełków, powiększamy tablicę.
    // Dzięki temu listy w kubełkach pozostają krótkie.
    private const double MaxLoadFactor = 0.75;

    // Tablica kubełków. Kubełek to początek listy (albo null, gdy jest pusty).
    private Node?[] buckets = new Node?[8];

    // Liczba elementów w słowniku.
    public int Count { get; private set; }

    // Liczba kubełków.
    public int BucketCount => buckets.Length;

    // Funkcja skrótu DJB2: zamienia tekst na liczbę.
    // Dla każdego znaku: hash = hash * 33 + znak. Start od 5381.
    public static uint Hash(string key)
    {
        uint hash = 5381;

        // unchecked: przy przepełnieniu liczba zaczyna się od zera (tak ma być).
        unchecked
        {
            foreach (char c in key)
                hash = hash * 33 + c;
        }

        return hash;
    }

    // Numer kubełka dla klucza: reszta z dzielenia skrótu przez liczbę kubełków.
    public int GetBucketIndex(string key)
    {
        return (int)(Hash(key) % (uint)buckets.Length);
    }

    // Dodaje parę klucz-wartość. Jeśli klucz już jest, zmienia tylko jego wartość.
    // Zwraca true, gdy klucz był nowy.
    public bool Add(string key, string value)
    {
        int index = GetBucketIndex(key);

        // 1. Szukamy klucza na liście w kubełku.
        for (Node? node = buckets[index]; node != null; node = node.Next)
        {
            if (node.Key == key)
            {
                node.Value = value; // klucz istnieje: podmieniamy wartość
                return false;
            }
        }

        // 2. Jeśli tablica jest za bardzo zapełniona, powiększamy ją.
        if (Count + 1 > buckets.Length * MaxLoadFactor)
        {
            Resize();
            index = GetBucketIndex(key); // kubełków jest więcej, więc indeks się zmienia
        }

        // 3. Wstawiamy nowy element na początek listy.
        buckets[index] = new Node(key, value, buckets[index]);
        Count++;
        return true;
    }

    // Zwraca wartość dla klucza albo null, gdy klucza nie ma.
    public string? Get(string key)
    {
        for (Node? node = buckets[GetBucketIndex(key)]; node != null; node = node.Next)
        {
            if (node.Key == key)
                return node.Value;
        }

        return null;
    }

    // Usuwa klucz. Zwraca true, gdy klucz istniał.
    public bool Remove(string key)
    {
        int index = GetBucketIndex(key);
        Node? previous = null; // poprzedni element na liście

        for (Node? node = buckets[index]; node != null; node = node.Next)
        {
            if (node.Key == key)
            {
                if (previous == null)
                    buckets[index] = node.Next; // usuwamy pierwszy element listy
                else
                    previous.Next = node.Next;  // przeskakujemy usuwany element

                Count--;
                return true;
            }

            previous = node;
        }

        return false;
    }

    // Usuwa wszystko i wraca do 8 kubełków.
    public void Clear()
    {
        buckets = new Node?[8];
        Count = 0;
    }

    // Liczba kolizji: ile elementów leży w kubełku, w którym już coś było.
    // Przykład: kubełek z 3 elementami daje 2 kolizje.
    public int CountCollisions()
    {
        int collisions = 0;

        foreach (Node? first in buckets)
        {
            int length = 0;
            for (Node? node = first; node != null; node = node.Next)
                length++;

            if (length > 1)
                collisions += length - 1;
        }

        return collisions;
    }

    // Zwraca elementy z kubełka o podanym numerze (do wyświetlenia w oknie).
    public List<KeyValuePair<string, string>> GetBucket(int index)
    {
        var items = new List<KeyValuePair<string, string>>();

        for (Node? node = buckets[index]; node != null; node = node.Next)
            items.Add(new KeyValuePair<string, string>(node.Key, node.Value));

        return items;
    }

    // Powiększa tablicę dwukrotnie i przenosi wszystkie elementy.
    // Indeks zależy od liczby kubełków, więc trzeba go policzyć od nowa.
    private void Resize()
    {
        Node?[] old = buckets;
        buckets = new Node?[old.Length * 2];

        foreach (Node? first in old)
        {
            Node? node = first;
            while (node != null)
            {
                Node? next = node.Next; // zapamiętujemy, bo zaraz zmienimy node.Next

                int index = GetBucketIndex(node.Key);
                node.Next = buckets[index];
                buckets[index] = node;

                node = next;
            }
        }
    }
}
