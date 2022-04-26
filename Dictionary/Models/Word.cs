using System;

namespace Dictionary.Models
{
    public class Word : IComparable<Word>
    {
        private string target = "";
        private string explain = "";
        private string? format = null;
        private string? example = null;

        public string Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public string Explain
        {
            get
            {
                return explain;
            }
            set
            {
                explain = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public string? Format
        {
            get
            {
                return format;
            }
            set
            {
                foreach (var item in AllFormat)
                    if (item.Equals(value))
                        format = value;
            }
        }

        public string? Example
        {
            get
            {
                return example;
            }
            set
            { 
                example = value;
            }
        }

        public Word(string target, string explain)
        {
            Target = target;
            Explain = explain;
        }

        public Word(Word word)
        {
            Target = word.Target;
            Explain = word.Explain;
            Format = word.Format;
            Example = word.Example;
        }

        public static readonly string[] AllFormat = { "Khác", "Tính từ", "Trạng từ", "Danh từ", "Động từ", "Giới từ", "Mạo từ", "Phó từ"};


        /**
         * Word to String.
         */
        override
        public string ToString()
        {
            var result = target + "\t" + explain + "\t";
            if (format != null) result += format + "\t";
            else result += " \t";
            result += example;
            return result;
        }

        /**
         * String to Word.
         */
        public static Word GetWord(string data)
        {
            string[] wordString = data.Split("\t");
            if (wordString.Length < 2) throw new FormatException();
            Word word = new(wordString[0], wordString[1]);
            if (wordString.Length >= 3) word.Format = wordString[2];
            if (wordString.Length >= 4) word.Example = wordString[3];
            return word;
        }

        /**
         * Equals twos words.
         */
        public Boolean Equals(Word word)
        {
            if (word == null) return false;
            return target.Equals(word.target) && explain.Equals(explain);
        }

        public int CompareTo(Word? other)
        {
            if (other is null) return -1;
            int diff = target.CompareTo(other.target);
            if (diff != 0) return diff;
            else return explain.CompareTo(other.explain);
        }
    }
}
