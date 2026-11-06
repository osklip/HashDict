# HashDict

Słownik (klucz -> wartość) oparty na tablicy mieszającej. Kolizje są rozwiązywane
metodą łańcuchową. Interfejs: Windows Forms.

## Pliki

- `HashDict/ChainedHashTable.cs` – słownik (funkcja skrótu DJB2, kubełki jako listy, powiększanie tablicy).
- `HashDict/Form1.cs` – okno: podpina metody słownika pod przyciski.
- `HashDict/Form1.Designer.cs` – układ okna (generowany przez Visual Studio).

## Uruchomienie

```
dotnet run --project HashDict
```

Wymagany jest .NET SDK 10 (Windows).

## Jak to działa

1. Funkcja skrótu zamienia klucz na liczbę.
2. Numer kubełka = skrót % liczba kubełków.
3. Kubełek to początek listy. Klucze o tym samym numerze (kolizja) trafiają na tę samą listę.
4. Szukanie, dodawanie i usuwanie przeglądają tylko jedną listę (średnio O(1)).
5. Gdy tablica jest zapełniona w ponad 75%, podwajamy liczbę kubełków i przenosimy elementy.
