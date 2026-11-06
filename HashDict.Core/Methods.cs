using System.Globalization;

namespace HashDict.Core;

// Funkcje pomocnicze bez odwołań do interfejsu użytkownika.
public static class Methods
{
    // Generuje deterministyczne pary "klucz_i" -> losowa liczba (dla danego ziarna).
    public static List<KeyValuePair<string, string>> GenerateEntries(int count, int seed = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        var random = new Random(seed);
        var entries = new List<KeyValuePair<string, string>>(count);

        for (int i = 1; i <= count; i++)
        {
            string value = random.Next(0, 1_000_000).ToString(CultureInfo.InvariantCulture);
            entries.Add(new KeyValuePair<string, string>($"klucz_{i}", value));
        }

        return entries;
    }

    // Jedna linia widoku: "[ 3] (klucz: wartość) -> (klucz2: wartość2)" albo "[ 3] -" dla pustego kubełka.
    public static string FormatBucket(int index, int indexWidth, IReadOnlyList<KeyValuePair<string, string>> chain)
    {
        string label = $"[{index.ToString(CultureInfo.InvariantCulture).PadLeft(indexWidth)}] ";

        if (chain.Count == 0)
            return label + "-";

        return label + string.Join(" -> ", chain.Select(pair => $"({pair.Key}: {pair.Value})"));
    }

    public static string FormatStats(TableStats s)
    {
        return $"Elementy: {s.Count} | Kubełki: {s.BucketCount} (użyte: {s.UsedBuckets}) | " +
               $"Kolizje: {s.Collisions} | Najdłuższy łańcuch: {s.LongestChain} | " +
               $"Współczynnik zapełnienia: {s.LoadFactor.ToString("F2", CultureInfo.CurrentCulture)}";
    }

    // Czas w czytelnej jednostce: mikrosekundy poniżej 1 ms, w przeciwnym razie milisekundy.
    public static string FormatDuration(TimeSpan duration)
    {
        double ms = duration.TotalMilliseconds;

        return ms < 1
            ? $"{(ms * 1000).ToString("F1", CultureInfo.CurrentCulture)} µs"
            : $"{ms.ToString("F2", CultureInfo.CurrentCulture)} ms";
    }
}
