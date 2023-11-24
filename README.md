# VoiceCalculator_WPF

## Opis

Aplikacja "VoiceCalculator" to prosty kalkulator głosowy napisany w języku C# z wykorzystaniem Windows Presentation Foundation (WPF). Pozwala użytkownikowi wykonywać podstawowe operacje matematyczne za pomocą mowy, a wyniki są prezentowane w interfejsie użytkownika oraz odczytywane przy użyciu syntezatora mowy.

## Wymagania

- System operacyjny Windows
- Dźwiękowe urządzenie wejścia (mikrofon)
- Dźwiękowe urządzenie wyjścia (głośniki)
- Pakiet Microsoft Speech Platform
- Pakiet NAudio.CoreApi
- Pakiet Humanizer

## Instrukcje Instalacji

1. Skompiluj projekt przy użyciu środowiska Visual Studio lub innego kompilatora obsługującego język C#.
2. Upewnij się, że na urządzeniu są zainstalowane wymienione pakiety: Microsoft Speech Platform, NAudio.CoreApi, Humanizer.
3. Uruchom aplikację.

## Funkcje

- Dodawanie, odejmowanie, mnożenie i dzielenie liczb za pomocą komend głosowych.
- Wyświetlanie wprowadzonych operacji matematycznych w interfejsie graficznym.
- Obsługa czyszczenia bieżącego wprowadzenia.
- Odczytywanie wyników działania przy użyciu syntezatora mowy.
- Pomoc oraz informacje o wybranym mikrofonie.

## Obsługa Aplikacji

1. **Wprowadzanie Danych:** Wykorzystaj komendy głosowe, aby wprowadzać cyfry, operatory matematyczne oraz komendy specjalne, takie jak "wyczyść" czy "wykonaj".

2. **Odczyt Wyników:** Po wprowadzeniu równania, użyj komendy "wykonaj". Wynik działania zostanie odczytany przez syntezator mowy.

3. **Czyszczenie Danych:** W razie potrzeby skorzystaj z komendy "wyczyść", aby zresetować wprowadzone dane.

4. **Informacje o Mikrofonie:** Sprawdź, który mikrofon jest używany w aplikacji, korzystając z informacji wyświetlanych w interfejsie.

## Dodatkowe Informacje

- Aplikacja wykorzystuje technologię Windows Presentation Foundation (WPF) dla interfejsu graficznego.
- Rozpoznawanie mowy oparte jest na pakiecie Microsoft Speech Platform.
- Dźwiękowe operacje obsługiwane są przez pakiet NAudio.CoreApi.
