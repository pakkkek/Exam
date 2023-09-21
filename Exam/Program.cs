using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class DictionaryEntry
{
    public string Word { get; set; }
    public List<string> Translations { get; set; }
}

class Dictionary
{
    public string Name { get; set; }
    public string LanguageFrom { get; set; }
    public string LanguageTo { get; set; }
    public List<DictionaryEntry> Entries { get; set; }

    public Dictionary(string name, string from, string to)
    {
        Name = name;
        LanguageFrom = from;
        LanguageTo = to;
        Entries = new List<DictionaryEntry>();
    }
}

class Program
{
    private static List<Dictionary> dictionaries = new List<Dictionary>();
    private static string currentDictionary = null;

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Create dictionary");
            Console.WriteLine("2. Choose dictionary");
            Console.WriteLine("3. Add word and translation");
            Console.WriteLine("4. Replace a word or translation");
            Console.WriteLine("5. Delete a word or translation");
            Console.WriteLine("6. Search translation of a word");
            Console.WriteLine("7. Export dictionary");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an action: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateDictionary();
                    break;
                case "2":
                    SelectDictionary();
                    break;
                case "3":
                    AddWordTranslation();
                    break;
                case "4":
                    ReplaceWordTranslation();
                    break;
                case "5":
                    DeleteWordTranslation();
                    break;
                case "6":
                    SearchTranslation();
                    break;
                case "7":
                    ExportDictionary();
                    break;
                case "8":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Wrong choice. Try again.");
                    break;
            }
        }
    }

    private static void CreateDictionary()
    {
        Console.Write("Enter the name of the dictionary: ");
        string name = Console.ReadLine();
        Console.Write("Enter the language from which we are translating: ");
        string fromLanguage = Console.ReadLine();
        Console.Write("Enter the language into which we are translating: ");
        string toLanguage = Console.ReadLine();

        Dictionary dictionary = new Dictionary(name, fromLanguage, toLanguage);
        dictionaries.Add(dictionary);

        Console.WriteLine($"Dictionary \"{name}\" is created.");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private static void SelectDictionary()
    {
        Console.WriteLine("Available dictionaries:");

        for (int i = 0; i < dictionaries.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {dictionaries[i].Name}");
        }

        Console.Write("Select a dictionary: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < dictionaries.Count)
        {
            currentDictionary = dictionaries[index].Name;
            Console.WriteLine($"Dictionary selected \"{currentDictionary}\".");
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Wrong choice of dictionary.");
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }
    }

    private static void AddWordTranslation()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("First, select a dictionary.");
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter a word in the source language: ");
        string word = Console.ReadLine();

        Console.Write("Enter translation(s) separated by commas: ");
        string translationsInput = Console.ReadLine();

        List<string> translations = translationsInput.Split(',').Select(t => t.Trim()).ToList();

        foreach (var dictionary in dictionaries)
        {
            if (dictionary.Name == currentDictionary)
            {
                dictionary.Entries.Add(new DictionaryEntry
                {
                    Word = word,
                    Translations = translations
                });
                Console.WriteLine($"Word \"{word}\" and his translations ({string.Join(", ", translations)}) added to dictionary \"{currentDictionary}\".");
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
                return;
            }
        }

        Console.WriteLine($"Dictionary \"{currentDictionary}\" not found.");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private static void ReplaceWordTranslation()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("First, select a dictionary.");
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter the word you want to replace: ");
        string wordToReplace = Console.ReadLine();

        foreach (var dictionary in dictionaries)
        {
            if (dictionary.Name == currentDictionary)
            {
                for (int i = 0; i < dictionary.Entries.Count; i++)
                {
                    if (dictionary.Entries[i].Word == wordToReplace)
                    {
                        Console.WriteLine($"Found word \"{wordToReplace}\" in dictionary \"{currentDictionary}\".");
                        Console.Write("Enter a new word: ");
                        string newWord = Console.ReadLine();

                        Console.Write("Enter new translation(s) separated by commas:");
                        string translationsInput = Console.ReadLine();

                        List<string> newTranslations = translationsInput.Split(',').Select(t => t.Trim()).ToList();

                        dictionary.Entries[i].Word = newWord;
                        dictionary.Entries[i].Translations = newTranslations;

                        Console.WriteLine($"Word \"{wordToReplace}\" was replaced by \"{newWord}\" and its translations have been changed to ({string.Join(", ", newTranslations)}) in dictionary \"{currentDictionary}\".");
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        return;
                    }
                }

                Console.WriteLine($"Word \"{wordToReplace}\" not found in dictionary \"{currentDictionary}\".");
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
                return;
            }
        }

        Console.WriteLine($"Dictionary \"{currentDictionary}\" not found.");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private static void DeleteWordTranslation()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("First, select a dictionary.");
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter the word you want to delete:");
        string wordToDelete = Console.ReadLine();

        foreach (var dictionary in dictionaries)
        {
            if (dictionary.Name == currentDictionary)
            {
                for (int i = 0; i < dictionary.Entries.Count; i++)
                {
                    if (dictionary.Entries[i].Word == wordToDelete)
                    {
                        dictionary.Entries.RemoveAt(i);
                        Console.WriteLine($"Word \"{wordToDelete}\" and its translations have been removed from the dictionary \"{currentDictionary}\".");
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                        return;
                    }
                }

                Console.WriteLine($"Word \"{wordToDelete}\" not found in dictionary \"{currentDictionary}\".");
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
                return;
            }
        }

        Console.WriteLine($"Dictionary \"{currentDictionary}\" not found.");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private static void SearchTranslation()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("First, select a dictionary.");
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter a word to search for translation: ");
        string wordToSearch = Console.ReadLine();

        foreach (var dictionary in dictionaries)
        {
            if (dictionary.Name == currentDictionary)
            {
                Console.WriteLine($"Search for translation of a word \"{wordToSearch}\" in dictionary \"{currentDictionary}\":");

                bool found = false;
                foreach (var entry in dictionary.Entries)
                {
                    if (entry.Word == wordToSearch || entry.Translations.Contains(wordToSearch))
                    {
                        Console.WriteLine($"Word: {entry.Word}");
                        Console.WriteLine($"Translation(s): {string.Join(", ", entry.Translations)}");
                        found = true;
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"Word \"{wordToSearch}\" not found in dictionary \"{currentDictionary}\".");
                }

                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
                return;
            }
        }

        Console.WriteLine($"Dictionary \"{currentDictionary}\" not found.");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private static void ExportDictionary()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("First, select a dictionary.");
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
            return;
        }

        string fileName = $"{currentDictionary}.txt";

        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var dictionary in dictionaries)
            {
                if (dictionary.Name == currentDictionary)
                {
                    foreach (var entry in dictionary.Entries)
                    {
                        writer.WriteLine($"{entry.Word} - {string.Join(", ", entry.Translations)}");
                    }
                    break;
                }
            }
        }

        Console.WriteLine($"Dictionary \"{currentDictionary}\" exported to file \"{fileName}\".");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }
}