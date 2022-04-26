using System;
using System.Collections.Generic;
using System.Linq;
using Dictionary.Models;

namespace Dictionary.Services
{
    public class Dictionary : IDictionary
    {
        private readonly List<Word> words = new();
        private readonly Dictionary<string, WordSpeech> speeches = new();

        public int Size { get { return words.Count; } }

        /**
        * Add word to dictionary.
        */
        public bool Add(Word word)
        {
            Word? oldWord = Get(word.Target, word.Explain);
            if (oldWord != null)
            {
                oldWord.Target = word.Target;
                oldWord.Explain = word.Explain;
                oldWord.Format = word.Format;
                oldWord.Example = word.Example;
                return true;
            }
            words.Add(word);
            int i = Size - 1;
            for (; i > 0; i--)
            {
                if (word.CompareTo(words[i - 1]) > 0) break;
                words[i] = words[i - 1];
            }
            words[i] = word;
            return true;
        }

        public bool AddSpeech(WordSpeech speech)
        {
            WordSpeech? oldSpeech = speeches.GetValueOrDefault(speech.Target);
            if (oldSpeech != null) oldSpeech.Speech = speech.Speech;
            else speeches.Add(speech.Target, speech);
            return true;
        }

        public void ForEach(Action<Word> wordAction)
        {
            foreach (var word in words) wordAction(word);
        }

        public void ForEach(Action<WordSpeech> wordSpeechAction)
        {
            foreach (var speech in speeches) wordSpeechAction(speech.Value);
        }

        /**
         * Get words list by target.
         */
        public List<Word> Get(string target)
        {
            List<Word> wordList = new();
            if (Size == 0) return wordList;
            int index = 0;
            int begin = 0;
            int end = words.Count - 1;
            while (begin <= end)
            {
                index = (begin + end) / 2;
                int diff = target.CompareTo(words[index].Target);
                if (diff == 0) break;
                if (diff > 0) begin = index + 1;
                else end = index - 1;
            }
            for (int i = index; i >= 0; i--)
            {
                if (words[i].Target.Equals(target)) wordList.Add(words[i]);
                else break;
            }
            for (int i = index + 1; i < words.Count; i++)
            {
                if (words[i].Target.Equals(target)) wordList.Add(words[i]);
                else break;
            }
            return wordList;
        }

        /**
         * Get word.
         */
        public Word? Get(string target, string explain)
        {
            if (Size == 0) return null;
            Word word = new(target, explain);
            int begin = 0;
            int end = Size - 1;
            while (begin <= end)
            {
                int index = (begin + end) / 2;
                int diff = word.CompareTo(words[index]);
                if (diff == 0) return words[index];
                if (diff > 0) begin = index + 1;
                else end = index - 1;
            }
            return null;
        }

        public string? GetPhrase(string target)
        {
            string result = "";
            string[] words = target.Split(" ");
            for (int i = 0; i < words.Length; i++)
            {
                WordSpeech? speech = GetSpeech(words[i]);
                if (speech != null)
                {
                    if (result.Equals("")) result += speech.Speech;
                    else result += " " + speech.Speech;
                }
            }
            if (result.Equals("")) return null;
            else return "/" + result + "/";
        }

        public WordSpeech? GetSpeech(string target)
        {
            return speeches.GetValueOrDefault(target);
        }

        public bool Remove(string target)
        {
            words.RemoveAll(word => word.Target == target);
            return true;
        }

        public bool Remove(string target, string explain)
        {
            words.RemoveAll(word => word.Target == target && word.Explain == explain);
            return true;
        }

        public bool RemoveSpeech(string target)
        {
            return speeches.Remove(target);
        }

        public List<string> SearchTarget(string key)
        {
            int index = 0;
            int begin = 0;
            int end = Size - 1;
            while (begin <= end)
            {
                index = (begin + end) / 2;
                int diff = key.CompareTo(words[index].Target);
                if (diff == 0) break;
                if (diff > 0) begin = index + 1;
                else end = index - 1;
            }
            HashSet<string> targets = new();
            for (int i = index; i >= 0; i--)
            {
                Word word = words[i];
                if (word.Target.Contains(key)) targets.Add(word.Target);
                else break;
            }
            for (int i = index + 1; i < Size; i++)
            {
                Word word = words[i];
                if (word.Target.Contains(key)) targets.Add(word.Target);
                else break;
            }
            return targets.ToList();
        }
    }
}
