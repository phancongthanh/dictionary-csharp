using System;
using System.Collections.Generic;
using Dictionary.Models;

namespace Dictionary.Services
{
    public interface IDictionary
    {
        bool Add(Word word);

        List<Word> Get(string target);

        Word? Get(string target, string explain);

        List<string> SearchTarget(string key);

        bool Remove(string target);
        bool Remove(string target, string explain);

        bool AddSpeech(WordSpeech speech);

        WordSpeech? GetSpeech(string target);

        string? GetPhrase(string target);

        bool RemoveSpeech(string target);

        void ForEach(Action<Word> wordAction);

        void ForEach(Action<WordSpeech> wordSpeechAction);
    }
}
