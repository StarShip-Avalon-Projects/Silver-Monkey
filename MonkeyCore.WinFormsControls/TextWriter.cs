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

        public TextBoxWriter(TextBox control)
        {
            this.control = control;
            control.HandleCreated += OnHandleCreated;
            Builder = new StringBuilder();
        }

        public TextBoxWriter(RichTextBox control)
        {
            this.control = control;
            control.HandleCreated += OnHandleCreated;
        }

        public delegate void AppendTextDelegate(string Text);

        public override Encoding Encoding
        {
            get
            {
                return Encoding.Default;
            }
        }

        public override void Write(char ch)
        {
            this.Write(ch.ToString());
        }

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
                //  (ByRef lb As Object, ByRef obj As Object, ByRef newColor As fColorEnum)
                // Main.AddDataToList(Main.log_, s, 0)
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