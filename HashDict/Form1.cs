using System.Diagnostics;
using HashDict.Core;

namespace HashDict;

// Obsługa interfejsu: zdarzenia kontrolek i wyświetlanie zawartości słownika.
// Cała logika tablicy mieszającej znajduje się w bibliotece HashDict.Core.
public partial class Form1 : Form
{
    // Powyżej tej liczby kubełków widok pokazuje tylko niepuste.
    private const int ShowOnlyUsedAbove = 256;

    // Maksymalna liczba linii w widoku (duże tablice nie spowalniają okna).
    private const int MaxListedBuckets = 2000;

    private readonly ChainedHashTable table = new();

    public Form1()
    {
        InitializeComponent();
        RefreshView();
        labelStatus.Text = "Wpisz klucz i wartość, a następnie kliknij „Dodaj / zmień”.";
    }

    // ---------- Zdarzenia ----------

    private void buttonAdd_Click(object? sender, EventArgs e)
    {
        if (!TryReadKey(out string key))
            return;

        string value = textBoxValue.Text;

        long start = Stopwatch.GetTimestamp();
        bool added = table.Add(key, value);
        TimeSpan elapsed = Stopwatch.GetElapsedTime(start);

        int bucket = table.GetBucketIndex(key);
        RefreshView(bucket);

        string action = added ? $"Dodano klucz „{key}”" : $"Zmieniono wartość klucza „{key}”";
        labelStatus.Text = $"{action} (kubełek {bucket}, pozycja w łańcuchu {table.GetChainPosition(key)}). " +
                           $"Czas: {Methods.FormatDuration(elapsed)}";
    }

    private void buttonFind_Click(object? sender, EventArgs e)
    {
        if (!TryReadKey(out string key))
            return;

        long start = Stopwatch.GetTimestamp();
        bool found = table.TryGet(key, out string? value);
        TimeSpan elapsed = Stopwatch.GetElapsedTime(start);

        if (!found)
        {
            labelStatus.Text = $"Nie znaleziono klucza „{key}”. Czas: {Methods.FormatDuration(elapsed)}";
            return;
        }

        int bucket = table.GetBucketIndex(key);
        textBoxValue.Text = value;
        RefreshView(bucket);

        labelStatus.Text = $"Znaleziono klucz „{key}” (kubełek {bucket}, pozycja w łańcuchu {table.GetChainPosition(key)}). " +
                           $"Czas: {Methods.FormatDuration(elapsed)}";
    }

    private void buttonRemove_Click(object? sender, EventArgs e)
    {
        if (!TryReadKey(out string key))
            return;

        long start = Stopwatch.GetTimestamp();
        bool removed = table.Remove(key);
        TimeSpan elapsed = Stopwatch.GetElapsedTime(start);

        RefreshView();

        labelStatus.Text = removed
            ? $"Usunięto klucz „{key}”. Czas: {Methods.FormatDuration(elapsed)}"
            : $"Nie ma klucza „{key}”, nic nie usunięto.";
    }

    private void buttonClear_Click(object? sender, EventArgs e)
    {
        table.Clear();
        RefreshView();
        labelStatus.Text = "Wyczyszczono słownik.";
    }

    private void buttonGenerate_Click(object? sender, EventArgs e)
    {
        int count = (int)numericUpDownCount.Value;
        var entries = Methods.GenerateEntries(count, seed: Environment.TickCount);
        int before = table.Count;

        long start = Stopwatch.GetTimestamp();
        foreach (var pair in entries)
            table.Add(pair.Key, pair.Value);
        TimeSpan elapsed = Stopwatch.GetElapsedTime(start);

        RefreshView();

        int added = table.Count - before;
        labelStatus.Text = $"Dodano {added} nowych kluczy ({count - added} istniejących zaktualizowano). " +
                           $"Czas: {Methods.FormatDuration(elapsed)}, średnio {Methods.FormatDuration(elapsed / count)} na parę.";
    }

    // ---------- Operacje na interfejsie ----------

    // Odczytuje klucz z pola tekstowego (białe znaki na brzegach są pomijane).
    private bool TryReadKey(out string key)
    {
        key = textBoxKey.Text.Trim();

        if (key.Length > 0)
            return true;

        labelStatus.Text = "Wpisz klucz.";
        textBoxKey.Focus();
        return false;
    }

    // Odświeża listę kubełków i statystyki. highlightBucket >= 0 zaznacza wskazany kubełek.
    private void RefreshView(int highlightBucket = -1)
    {
        List<List<KeyValuePair<string, string>>> chains = table.GetChains();
        int width = chains.Count.ToString().Length;
        bool onlyUsed = chains.Count > ShowOnlyUsedAbove;

        int selectedIndex = -1;
        int listed = 0;
        bool truncated = false;

        listBoxBuckets.BeginUpdate();
        listBoxBuckets.Items.Clear();

        for (int i = 0; i < chains.Count; i++)
        {
            if (onlyUsed && chains[i].Count == 0)
                continue;

            if (listed >= MaxListedBuckets)
            {
                truncated = true;
                break;
            }

            if (i == highlightBucket)
                selectedIndex = listBoxBuckets.Items.Count;

            listBoxBuckets.Items.Add(Methods.FormatBucket(i, width, chains[i]));
            listed++;
        }

        if (selectedIndex >= 0)
            listBoxBuckets.SelectedIndex = selectedIndex;

        listBoxBuckets.EndUpdate();

        string note = onlyUsed ? " | Widok: tylko niepuste kubełki" : "";
        if (truncated)
            note += $" (pierwsze {MaxListedBuckets})";

        labelStats.Text = Methods.FormatStats(table.GetStats()) + note;
    }
}
