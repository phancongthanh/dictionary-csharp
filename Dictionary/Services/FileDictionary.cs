using System;
using System.Collections.Generic;
using Dictionary.Models;

namespace Dictionary.Services
{
    public class FileDictionary
    {
        private static IDictionary? dictionary = null;

        private FileDictionary()
        {

        }

        public static void LoadDictionary(string folderPath)
        {
            if (dictionary == null) throw new ArgumentNullException();
            // Load words.
            string[] words = System.IO.File.ReadAllLines(folderPath + "/dictionary.txt");
            foreach (string word in words)
            {
                try
                {
                    dictionary.Add(Word.GetWord(word));
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine(word);
                }
            }

            // Load speeches.
            string[] speeches = System.IO.File.ReadAllLines(folderPath + "/speeches.txt");
            foreach (string speech in speeches)
            {
                try
                {
                    dictionary.AddSpeech(WordSpeech.GetSpeech(speech));
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine(speech);
                }
            }
        }

        public static void SaveDictionary(string folderPath)
        {
            if (dictionary == null) throw new ArgumentNullException();
            List<string> words = new();
            dictionary.ForEach(delegate (Word word) { words.Add(word.ToString()); });
            List<string> speeches = new();
            dictionary.ForEach(delegate (WordSpeech speech) { speeches.Add(speech.ToString()); });
            System.IO.File.WriteAllLines(folderPath + "/dictionary.txt", words);
            System.IO.File.WriteAllLines(folderPath + "/speeches.txt", speeches);
        }

        public static IDictionary GetDictionary(string folderPath)
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary();
                LoadDictionary(folderPath);
                return dictionary;
            }
            else return dictionary;
        }
    }
}
