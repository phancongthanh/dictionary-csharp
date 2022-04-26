using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dictionary.Models;

namespace Dictionary.View
{
    /// <summary>
    /// Interaction logic for WordEdit.xaml
    /// </summary>
    public partial class WordEdit : Window
    {
        private readonly Word word;
        public WordEdit(Word word, List<Button> buttons)
        {
            this.word = word;
            InitializeComponent();
            foreach (string format in Word.AllFormat)
            {
                Word_Format.Items.Add(format);
            }
            if (word.Target.Length > 0) Word_Target.Text = word.Target;
            else Word_Target.Text = "Target";
            Word_Explain.Text = word.Explain;
            if (word.Format != null) Word_Format.Text = word.Format;
            else Word_Format.Text = Word.AllFormat[0];
            Word_Example.Text = word.Example;
            foreach (var button in buttons)
            {
                button.Click += (sender, e) => Close();
                Word_Buttons.Children.Add(button);
            }
        }

        private void Word_Target_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                word.Target = Word_Target.Text.ToLower();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Target Error:" + exception.Message);
            }
        }

        private void Word_Explain_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                word.Explain = Word_Explain.Text;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Explain Error:" + exception.Message);
            }
        }

        private void Word_Format_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                word.Format = Word_Format.SelectedItem.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Format Error:" + exception.Message);
            }
        }

        private void Word_Example_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                word.Example = Word_Example.Text;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Example Error:" + exception.Message);
            }
        }
    }
}
