using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SilverMonkey
{
    public static class RichTextBoxEtentions
    {
        public static void SetText(this RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Clear();
            Paragraph para = new Paragraph(new Run(text));
            para.Margin = new System.Windows.Thickness(0);
            para.Padding = new System.Windows.Thickness(0);
            richTextBox.Document.Blocks.Add(para);
        }

        public static void AppendParagraph(this RichTextBox richTextBox, string text)
        {
            Paragraph para = new Paragraph(new Run(text));
            para.Margin = new System.Windows.Thickness(0);
            para.Padding = new System.Windows.Thickness(0);
            richTextBox.Document.Blocks.Add(para);
        }

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd).Text;
        }
    }
}