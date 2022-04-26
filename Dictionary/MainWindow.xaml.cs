using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dictionary.Models;
using Dictionary.Services;
using Dictionary.View;
using System.Speech.Synthesis;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDictionary dictionary;
        private readonly SpeechSynthesizer synthesizer;
        public MainWindow()
        {
            InitializeComponent();
            dictionary = FileDictionary.GetDictionary("Data");
            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            Closed += (sender, e) => FileDictionary.SaveDictionary("Data");
        }
        private void SearchWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchView_Input.Text.Length <= 1)
            {
                SearchView_Result.Children.Clear();
                return;
            }
            List<string> targets = dictionary.SearchTarget(SearchView_Input.Text.ToLower());
            foreach (var change in e.Changes)
            {
                if (change.AddedLength > 0 && SearchView_Result.Children.Count > 0)
                {
                    for (int i = SearchView_Result.Children.Count - 1; i >= 0; i--)
                    {
                        if (SearchView_Result.Children[i] is not Button)
                        {
                            SearchView_Result.Children.RemoveAt(i);
                            continue;
                        }
                        Button button = (Button)SearchView_Result.Children[i];
                        string? target = button.Content.ToString();
                        if (target == null) SearchView_Result.Children.Remove(button);
                        else if (!targets.Contains(target)) SearchView_Result.Children.Remove(button);
                    }
                }
                else
                {
                    SearchView_Result.Children.Clear();
                    foreach (string target in targets)
                    {
                        Button button = new();
                        button.Content = target;
                        button.Click += View_Click;
                        SearchView_Result.Children.Add(button);
                    }
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var word = new Word(SearchView_Input.Text.ToLower(), "");
            var addButton = new Button();
            var cancelButton = new Button();
            var buttons = new List<Button>();
            addButton.Content = "Add Word";
            addButton.Click += (sender, e) => dictionary.Add(word);
            cancelButton.Content = "Cancel";
            buttons.Add(addButton);
            buttons.Add(cancelButton);
            new WordEdit(word, buttons).ShowDialog();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button) return;
            string? target = ((Button)sender).Content.ToString();
            if (target == null) return;
            ViewWord(target);
        }

        private void Speech_Click(object sender, RoutedEventArgs e)
        {
            string target = WordView_Target.Text.ToLower();
            if (target.Length > 0) synthesizer.Speak(target);
        }

        private void SpeechEdit_Click(object sender, RoutedEventArgs e)
        {
            var phrase = WordView_Speech.Content.ToString();
            if (phrase == null) return;
            if (phrase.Contains(' ')) return;
            var speech = dictionary.GetSpeech(WordView_Target.Text);
            if (speech == null) speech = new WordSpeech(WordView_Target.Text, "");
            var newSpeech = new WordSpeech(speech);
            var dialog = new WordSpeechEdit(newSpeech);
            dialog.ResultEvent += result =>
            {
                if (result.Equals("Save"))
                {
                    dictionary.RemoveSpeech(speech.Target);
                    dictionary.AddSpeech(newSpeech);
                    ViewWord(newSpeech.Target);
                }
                else if (result.Equals("Delete"))
                {
                    dictionary.RemoveSpeech(speech.Target);
                    ViewWord(newSpeech.Target);
                }
            };
            dialog.ShowDialog();
        }

        private void ViewWord(string target)
        {
            List<Word> words = dictionary.Get(target);
            string? speech = dictionary.GetPhrase(target);
            WordView_Target.Text = target;
            if (speech != null) WordView_Speech.Content = speech;
            else WordView_Speech.Content = "";
            WordView_Result.Children.Clear();
            for (int i = 0; i < words.Count; i++)
            {
                Word word = words[i];
                WordView wordView = new(i + 1, word);
                wordView.EditClick += EditWord;
                wordView.DeleteClick += DeleteWord;
                WordView_Result.Children.Add(wordView);
            }
        }

        private void EditWord(Word word)
        {
            var newWord = new Word(word);
            var saveButton = new Button();
            var cancelButton = new Button();
            var buttons = new List<Button>();
            saveButton.Content = "Save";
            saveButton.Click += (sender, e) =>
            {
                dictionary.Remove(word.Target, word.Explain);
                dictionary.Add(newWord);
                ViewWord(newWord.Target);
            };
            cancelButton.Content = "Cancel";
            buttons.Add(saveButton);
            buttons.Add(cancelButton);
            new WordEdit(newWord, buttons).ShowDialog();
        }

        private void DeleteWord(Word word)
        {
            var result = MessageBox.Show("Delete word:\n\t" + word.Target + ": " + word.Explain, "Confirm", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                dictionary.Remove(word.Target, word.Explain);
                ViewWord(word.Target);
            }
        }
    }
}