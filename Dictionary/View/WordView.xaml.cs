using System;
using System.Windows.Controls;
using System.Windows;
using Dictionary.Models;
using System.Collections.Generic;
using Dictionary.Services;

namespace Dictionary.View
{
    /// <summary>
    /// Interaction logic for WordView.xaml
    /// </summary>
    public partial class WordView : UserControl
    {
        private readonly Word word;

        public event Action<Word> EditClick = delegate { };
        public event Action<Word> DeleteClick = delegate { };

        public WordView(int number, Word word)
        {
            this.word = word;
            InitializeComponent();
            Word_Explain.Text = number + "." + word.Explain;
            if (word.Format != null) Word_Format.Text = "(" + word.Format + ")";
            if (word.Example != null)
                if (word.Example.Length > 0)
                    Word_Example.Text = "- " + word.Example;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditClick.Invoke(word);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteClick.Invoke(word);
        }
    }
}
