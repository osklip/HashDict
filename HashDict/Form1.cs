namespace HashDict;

// Okno programu. Podpina metody słownika pod przyciski i pokazuje jego zawartość.
// Cała logika słownika jest w klasie ChainedHashTable.
public partial class Form1 : Form
{
    // Nasz słownik.
    private readonly ChainedHashTable table = new();

    public Form1()
    {
        InitializeComponent();
        RefreshView();
    }

    // Przycisk "Dodaj / zmień".
    private void buttonAdd_Click(object? sender, EventArgs e)
    {
        string? key = ReadKey();
        if (key == null)
            return;

        bool isNew = table.Add(key, textBoxValue.Text);
        int bucket = table.GetBucketIndex(key);
        RefreshView(bucket);

        labelStatus.Text = isNew
            ? $"Dodano klucz „{key}” do kubełka {bucket}."
            : $"Zmieniono wartość klucza „{key}” (kubełek {bucket}).";
    }

    // Przycisk "Szukaj".
    private void buttonFind_Click(object? sender, EventArgs e)
    {
        string? key = ReadKey();
        if (key == null)
            return;

        string? value = table.Get(key);

        if (value == null)
        {
            labelStatus.Text = $"Nie znaleziono klucza „{key}”.";
            return;
        }

        textBoxValue.Text = value;
        int bucket = table.GetBucketIndex(key);
        RefreshView(bucket);
        labelStatus.Text = $"Znaleziono klucz „{key}” w kubełku {bucket}.";
    }

    // Przycisk "Usuń".
    private void buttonRemove_Click(object? sender, EventArgs e)
    {
        string? key = ReadKey();
        if (key == null)
            return;

        bool removed = table.Remove(key);
        RefreshView();

        labelStatus.Text = removed
            ? $"Usunięto klucz „{key}”."
            : $"Nie ma klucza „{key}”.";
    }

    // Przycisk "Wyczyść".
    private void buttonClear_Click(object? sender, EventArgs e)
    {
        table.Clear();
        RefreshView();
        labelStatus.Text = "Wyczyszczono słownik.";
    }

    // Czyta klucz z pola tekstowego. Zwraca null, gdy pole jest puste.
    private string? ReadKey()
    {
        string key = textBoxKey.Text.Trim();

        if (key == "")
        {
            labelStatus.Text = "Wpisz klucz.";
            return null;
        }

        return key;
    }

    // Odświeża listę kubełków i statystyki.
    // Jeśli podano numer kubełka, zaznacza go na liście.
    private void RefreshView(int selectBucket = -1)
    {
        listBoxBuckets.Items.Clear();

        for (int i = 0; i < table.BucketCount; i++)
        {
            var items = table.GetBucket(i);

            // Przykład linii: [3] (ala: kot) -> (ola: pies)
            string line = $"[{i}] ";
            if (items.Count == 0)
                line += "-";
            else
                line += string.Join(" -> ", items.Select(p => $"({p.Key}: {p.Value})"));

            listBoxBuckets.Items.Add(line);
        }

        if (selectBucket >= 0)
            listBoxBuckets.SelectedIndex = selectBucket;

        labelStats.Text = $"Elementy: {table.Count} | Kubełki: {table.BucketCount} | Kolizje: {table.CountCollisions()}";
    }
}
