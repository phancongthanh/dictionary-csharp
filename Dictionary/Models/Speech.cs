namespace Dictionary.Models
{
    public class WordSpeech
    {
        public string Target { get; set; }
        public string Speech { get; set; }

        public WordSpeech(string target, string speech)
        {
            Target = target;
            Speech = speech;
        }

        public WordSpeech(WordSpeech speech)
        {
            Target = speech.Target;
            Speech = speech.Speech;
        }

        override
        public string ToString()
        {
            return Target + "\t" + Speech;
        }

        public static WordSpeech GetSpeech(string data)
        {
            string target = data.Split("\t")[0];
            string speech = data.Split("\t")[1];
            return new WordSpeech(target, speech);
        }
    }
}
