using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SilverMonkey.Extentions
{
    /// <summary>
    ///
    /// </summary>
    public static class RichTextBoxEtentions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="text"></param>
        public static void SetText(this RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Clear();
            Paragraph para = new Paragraph(new Run(text))
            {
                Margin = new System.Windows.Thickness(0),
                Padding = new System.Windows.Thickness(0)
            };
            richTextBox.Document.Blocks.Add(para);
        }

        /// <summary>
        /// Appends the paragraph.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <param name="text">The text.</param>
        public static void AppendParagraph(this RichTextBox richTextBox, string text, Brush brush = null)
        {
            if (brush == null)
                brush = Brushes.Black;
            Paragraph para = new Paragraph(new Run(text))
            {
                Margin = new System.Windows.Thickness(0),
                Padding = new System.Windows.Thickness(0),
                Foreground = brush
            };
            richTextBox.Document.Blocks.Add(para);
            richTextBox.ScrollToEnd();
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="richTextBox">The rich text box.</param>
        /// <returns></returns>
        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd).Text;
        }
    }
}