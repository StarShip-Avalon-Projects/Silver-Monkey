using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Controls
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IO.TextWriter" />
    public class TextBoxWriter : TextWriter
    {
        private StringBuilder Builder;

        private TextBoxBase control;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxWriter"/> class.
        /// </summary>
        /// <param name="control">The control.</param>
        public TextBoxWriter(TextBox control)
        {
            this.control = control;
            control.HandleCreated += OnHandleCreated;
            Builder = new StringBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxWriter"/> class.
        /// </summary>
        /// <param name="control">The control.</param>
        public TextBoxWriter(RichTextBox control)
        {
            this.control = control;
            control.HandleCreated += OnHandleCreated;
            Builder = new StringBuilder();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Text">The text.</param>
        public delegate void AppendTextDelegate(string Text);

        /// <summary>
        /// When overridden in a derived class, returns the character encoding in which the output is written.
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                return Encoding.Default;
            }
        }

        /// <summary>
        /// Writes the specified ch.
        /// </summary>
        /// <param name="ch">The ch.</param>
        public override void Write(char ch)
        {
            this.Write(ch.ToString());
        }

        /// <summary>
        /// Writes the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        public override void Write(string s)
        {
            if (control.IsHandleCreated)
            {
                this.AppendText(s);
            }
            else
            {
                this.BufferText(ref s);
            }
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="s">The s.</param>
        public override void WriteLine(string s)
        {
            this.Write(s + Environment.NewLine);
        }

        private void AppendText(string Text)
        {
            if (control.InvokeRequired)
            {
                AppendTextDelegate d = new AppendTextDelegate(AppendText);
                control.Invoke(d, Text);
            }
            else
            {
                if (Builder.Length > 0)
                {
                    control.AppendText(Builder.ToString());
                    Builder.Clear();
                }

                control.AppendText(Text);
            }
        }

        private void BufferText(ref string s)
        {
            Builder.Append(s);
        }

        private void OnHandleCreated(object sender, EventArgs e)
        {
            if (Builder.Length > 0)
            {
                control.AppendText(Builder.ToString());
                Builder.Clear();
            }
        }
    }
}