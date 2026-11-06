# HashDict

Słownik (klucz -> wartość) oparty na tablicy mieszającej z łańcuchową obsługą kolizji,
z interfejsem Windows Forms.

## Struktura

- `HashDict.Core` – logika bez interfejsu:
  - `ChainedHashTable` – tablica mieszająca (funkcja skrótu djb2, kolizje rozwiązywane listami
    jednokierunkowymi w kubełkach, automatyczne dwukrotne powiększanie przy współczynniku
    zapełnienia powyżej 0,75),
  - `Methods` – funkcje pomocnicze (generowanie danych, formatowanie widoku i czasu).
- `HashDict` – aplikacja Windows Forms (`Form1` tylko podpina metody pod zdarzenia).
- `HashDict.Tests` – testy jednostkowe (xUnit).

## Uruchomienie

```
dotnet run --project HashDict
dotnet test
```

Wymagany jest .NET SDK 10 (Windows).

## Jak to działa

Indeks kubełka to `hash(klucz) % liczba_kubełków`. Elementy o tym samym indeksie tworzą
łańcuch (nowe trafiają na początek). Operacje dodawania, szukania i usuwania przeglądają
tylko jeden łańcuch, więc przy równomiernym rozkładzie kluczy działają w średnim czasie O(1).

Statystyka „kolizje" w aplikacji to liczba elementów, które nie są pierwsze w swoim kubełku.
