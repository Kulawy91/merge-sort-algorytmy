using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("Wybierz sposób dostarczenia danych:");
        Console.WriteLine("1. Wprowadź dane z klawiatury (np. 1 2, 3 4, 5 6)");
        Console.WriteLine("2. Wczytaj dane z pliku tekstowego");
        Console.WriteLine("3. Generuj dane losowo");

        int wybor = Convert.ToInt32(Console.ReadLine());

        List<Tuple<int, int>> ciagi = new List<Tuple<int, int>>();

        switch (wybor)
        {
            case 1:
                Console.WriteLine("Podaj ciągi par liczb całkowitych (x, y), oddzielając każdą parę przecinkiem i spacją. Aby zakończyć, naciśnij Enter:");
                Console.WriteLine("Przykład: 1 2, 3 4, 5 6");
                WprowadzDaneZKlawiatury(ciagi);
                break;

            case 2:
                Console.WriteLine("Podaj ścieżkę do pliku tekstowego z danymi:");
                string sciezkaPliku = Console.ReadLine();
                WczytajDaneZPliku(sciezkaPliku, ciagi);
                break;

            case 3:
                Console.WriteLine("Podaj liczbę ciągów do wygenerowania losowo:");
                int liczbaCiągów = Convert.ToInt32(Console.ReadLine());
                GenerujDaneLosowo(liczbaCiągów, ciagi);
                break;

            default:
                Console.WriteLine("Niepoprawny wybór.");
                return;
        }

        Console.WriteLine("Nieposortowane ciągi:");
        WyswietlCiągi(ciagi);

        MergeSort(ciagi);

        Console.WriteLine("Posortowane ciągi:");
        WyswietlCiągi(ciagi);
    }

    static void WprowadzDaneZKlawiatury(List<Tuple<int, int>> ciagi)
    {
        string input = Console.ReadLine();
        string[] pary = input.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var para in pary)
        {
            string[] elementy = para.Split(' ');

            if (elementy.Length == 2 && int.TryParse(elementy[0], out int x) && int.TryParse(elementy[1], out int y))
            {
                ciagi.Add(Tuple.Create(x, y));
            }
            else
            {
                Console.WriteLine($"Błąd w formacie dla pary: {para}. Pominięto.");
            }
        }
    }

    static void WczytajDaneZPliku(string sciezkaPliku, List<Tuple<int, int>> ciagi)
    {
        try
        {
            string line;
            using (StreamReader reader = new StreamReader(sciezkaPliku))
            {
                line = reader.ReadLine();
            }

            if (!string.IsNullOrEmpty(line))
            {
                string[] pary = line.Split(',');

                foreach (var para in pary)
                {
                    string[] elementy = para.Trim().Split(' ');

                    if (elementy.Length == 2 && int.TryParse(elementy[0], out int x) && int.TryParse(elementy[1], out int y))
                    {
                        ciagi.Add(Tuple.Create(x, y));
                    }
                    else
                    {
                        Console.WriteLine($"Błąd w formacie dla pary: {para}. Pominięto.");
                    }
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"Błąd odczytu pliku: {e.Message}");
        }
    }

    static void GenerujDaneLosowo(int liczbaCiągów, List<Tuple<int, int>> ciagi)
    {
        Random random = new Random();
        for (int i = 0; i < liczbaCiągów; i++)
        {
            int x = random.Next(100); // Przykładowy zakres dla x
            int y = random.Next(100); // Przykładowy zakres dla y
            ciagi.Add(Tuple.Create(x, y));
        }
    }

    static void MergeSort(List<Tuple<int, int>> ciagi)
    {
        if (ciagi.Count <= 1)
            return;

        int srodek = ciagi.Count / 2;

        List<Tuple<int, int>> lewaCzesc = new List<Tuple<int, int>>();
        List<Tuple<int, int>> prawaCzesc = new List<Tuple<int, int>>();

        for (int i = 0; i < srodek; i++)
            lewaCzesc.Add(ciagi[i]);

        for (int i = srodek; i < ciagi.Count; i++)
            prawaCzesc.Add(ciagi[i]);

        MergeSort(lewaCzesc);
        MergeSort(prawaCzesc);

        Scalanie(ciagi, lewaCzesc, prawaCzesc);
    }

    static void Scalanie(List<Tuple<int, int>> wynik, List<Tuple<int, int>> lewa, List<Tuple<int, int>> prawa)
    {
        int indeksLewy = 0, indeksPrawy = 0, indeksWynik = 0;

        while (indeksLewy < lewa.Count && indeksPrawy < prawa.Count)
        {
            if (Porownaj(lewa[indeksLewy], prawa[indeksPrawy]) <= 0)
            {
                wynik[indeksWynik] = lewa[indeksLewy];
                indeksLewy++;
            }
            else
            {
                wynik[indeksWynik] = prawa[indeksPrawy];
                indeksPrawy++;
            }
            indeksWynik++;
        }

        while (indeksLewy < lewa.Count)
        {
            wynik[indeksWynik] = lewa[indeksLewy];
            indeksLewy++;
            indeksWynik++;
        }

        while (indeksPrawy < prawa.Count)
        {
            wynik[indeksWynik] = prawa[indeksPrawy];
            indeksPrawy++;
            indeksWynik++;
        }
    }

    static int Porownaj(Tuple<int, int> a, Tuple<int, int> b)
    {
        if (a.Item1 != b.Item1)
        {
            return a.Item1.CompareTo(b.Item1);
        }
        else
        {
            return a.Item2.CompareTo(b.Item2);
        }
    }

    static void WyswietlCiągi(List<Tuple<int, int>> ciagi)
    {
        foreach (var ciag in ciagi)
        {
            Console.WriteLine($"({ciag.Item1}, {ciag.Item2})");
        }
        Console.WriteLine();
    }
}
