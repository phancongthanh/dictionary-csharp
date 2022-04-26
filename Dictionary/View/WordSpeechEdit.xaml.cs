using System;
using System.Windows;
using System.Windows.Controls;
using Dictionary.Models;

namespace Dictionary.View
{
    /// <summary>
    /// Interaction logic for WordSpeechEdit.xaml
    /// </summary>
    public partial class WordSpeechEdit : Window
    {
        private readonly WordSpeech speech;

        public event Action<string> ResultEvent = delegate (string result) { };
        public WordSpeechEdit(WordSpeech speech)
        {
            InitializeComponent();
            this.speech = speech;
            Title = speech.Target;
            Word_Speech.Text = speech.Speech;
        }

        private void AddLetter_Click(object sender, RoutedEventArgs e)
        {
            Word_Speech.Text += ((Button)sender).Content.ToString();
        }

        private void DeleteLetter_Click(object sender, RoutedEventArgs e)
        {
            Word_Speech.Text = Word_Speech.Text.Remove(Word_Speech.Text.Length - 1);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Word_Speech.Text.Length > 0)
            {
                speech.Speech = Word_Speech.Text;
                ResultEvent.Invoke("Save");
                Close();
            }
            else MessageBox.Show("Invalid speech!");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ResultEvent.Invoke("Delete");
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ResultEvent.Invoke("Cancel");
            Close();
        }
    }
}
