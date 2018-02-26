using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static Controls.NativeMethods;

namespace MonkeyCore.WinForms.Controls
{
    /// <summary>
    /// Extended RichTextBox with URL formatting
    /// </summary>
    public class RichTextBoxEx : System.Windows.Forms.RichTextBox
    {
        private IntPtr oldEventMask;

        private int updating;

        private List<string> _protocols;

        private Control instance;

        private VerticalAlignment value;

        public RichTextBoxEx()
        {
            this.DoubleBuffered = true;
            this.DetectUrls = true;
            this._protocols = new List<string>();
            this._protocols.AddRange(new string[] {
                        "http://",
                        "help://",
                        "furc://",
                        "file://",
                        "mailto://",
                        "ftp://",
                        "https://",
                        "gopher://",
                        "nntp://",
                        "prospero://",
                        "telnet://",
                        "news://",
                        "wais://",
                        "command://",
                        "outlook://"});
        }

        /// <summary>
        /// Gets and Sets the Horizontal Scroll position of the control.
        /// </summary>
        public int HScrollPos
        {
            get
            {
                return GetScrollPos(this.Handle, SBOrientation.SB_HORZ);
            }
            set
            {
                SetScrollPos(this.Handle, SBOrientation.SB_HORZ, value, true);
            }
        }

        public int VScrollPos
        {
            get
            {
                return GetScrollPos(this.Handle, SBOrientation.SB_VERT);
            }
            set
            {
                SetScrollPos(this.Handle, SBOrientation.SB_VERT, value, true);
            }
        }

        public List<string> Protocols
        {
            get
            {
                return this._protocols;
            }
        }

        public VerticalAlignment VerticalContentAlignment { get; set; }

        [Editor(("System.Windows.Forms.Design.StringCollectionEditor," + "System.Design, Version=4.5.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), typeof(System.Drawing.Design.UITypeEditor))]
        public new bool DetectUrls
        {
            get
            {
                return base.DetectUrls;
            }
            set
            {
                base.DetectUrls = value;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                BeginUpdate();
                base.Text = value;
                EndUpdate();
            }
        }

        /// <summary>
        /// Appends text to the current text of a text box.
        /// </summary>
        /// <param name="text">The text to append to the current contents of the text box.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void AppendText(string text)
        {
            BeginUpdate();
            base.AppendText(text);
            EndUpdate();
        }

        /// <summary>
        /// Maintains performance while updating.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is recommended to call this method before doing any major
        /// updates that you do not wish the user to see. Remember to call
        /// EndUpdate When you are finished with the update. Nested calls
        /// are supported.
        /// </para>
        /// <para>
        /// Calling this method will prevent redrawing. It will also setup
        /// the event mask of the underlying richedit control so that no
        /// events are sent.
        /// </para>
        /// </remarks>
        public void BeginUpdate()
        {
            //  Deal with nested calls.
            updating++;
            if ((updating > 1))
            {
                return;
            }

            //  Prevent the control from raising any events.
            oldEventMask = SendMessage(this.Handle, EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero);
            //  Prevent the control from redrawing itself.
            SendMessage(this.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        // Public Sub urlClicked(ByVal sender As Object, ByVal e As LinkClickedEventArgs) Handles Me.LinkClicked
        //     MessageBox.Show(e.LinkText)
        // End Sub
        /// <summary>
        /// Resumes drawing and event handling.
        /// </summary>
        /// <remarks>
        /// This method should be called every time a call is made made to
        /// BeginUpdate. It resets the event mask to it's original value and
        /// enables redrawing of the control.
        /// </remarks>
        public void EndUpdate()
        {
            //  Deal with nested calls.
            updating--;
            if ((updating > 0))
            {
                return;
            }

            //  Allow the control to redraw itself.
            SendMessage(this.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            //  Allow the control to raise event messages.
            SendMessage(this.Handle, EM_SETEVENTMASK, IntPtr.Zero, oldEventMask);
        }

        /// <summary>
        /// Get the link style for the current selection
        /// </summary>
        /// <returns>
        /// 0: link style not set, 1: link style set, -1: mixed
        /// </returns>
        public int GetSelectionLink()
        {
            return GetSelectionStyle(CFM_LINK, CFE_LINK);
        }

        /// <summary>
        /// Insert a given text as a link into the RichTextBox at the
        /// current insert position.
        /// </summary>
        /// <param name="text">
        /// Text to be inserted
        /// </param>
        public void InsertLink(string text)
        {
            InsertLink(text, this.SelectionStart);
        }

        /// <summary>
        /// Insert a given text at a given position as a link.
        /// </summary>
        /// <param name="text">
        /// Text to be inserted
        /// </param>
        /// <param name="position">
        /// Insert position
        /// </param>
        public void InsertLink(string text, int position)
        {
            if (position < 0 || position > this.Text.Length)
            {
                throw new ArgumentOutOfRangeException("position");
            }

            this.SelectionStart = position;
            this.SelectedText = text;
            this.Select(position, text.Length);
            this.SetSelectionLink(true);
            this.Select((position + text.Length), 0);
        }

        /// <summary>
        /// Insert a given text at at the current input position as a link.
        /// The link text is followed by a hash (#) and the given hyperlink
        /// text, both of them invisible. When clicked on, the whole link
        /// text and hyperlink string are given in the LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">
        /// Text to be inserted
        /// </param>
        /// <param name="hyperlink">
        /// Invisible hyperlink string to be inserted
        /// </param>
        public void InsertLink(string text, string hyperlink)
        {
            InsertLink(text, hyperlink, this.SelectionStart);
        }

        /// <summary>
        /// Insert a given text at a given position as a link. The link text
        /// is followed by a hash (#) and the given hyperlink text, both of
        /// them invisible. When clicked on, the whole link text and
        /// hyperlink string are given in the LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">
        /// Text to be inserted
        /// </param>
        /// <param name="hyperlink">
        /// Invisible hyperlink string to be inserted
        /// </param>
        /// <param name="position">
        /// Insert position
        /// </param>
        public void InsertLink(string text, string hyperlink, int position)
        {
            if (position < 0 || position > this.Text.Length)
            {
                throw new ArgumentOutOfRangeException("position");
            }

            //  BeginUpdate()
            this.SelectionStart = position;
            this.SelectedRtf = ("{\\rtf1\\ansi\\deff0\\uc1 "
                        + (text + ("\\v #"
                        + (hyperlink + "\\v0}"))));
            this.Select(position, (text.Length
                            + (hyperlink.Length + 1)));
            this.SetSelectionLink(true);
            this.Select((position
                            + (text.Length
                            + (hyperlink.Length + 1))), 0);
            //   Me.Select(position + text.Length + 1 + hyperlink.Length, 0)
            //  EndUpdate()
        }

        /// <summary>
        /// Set the current selection's link style
        /// </summary>
        /// <param name="link">
        /// true: set link style, false: clear link style
        /// </param>
        public void SetSelectionLink(bool link)
        {
            SetSelectionStyle(CFM_LINK, link ? CFE_LINK : 0);
        }

        private int GetSelectionStyle(int mask, int effect)
        {
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = Marshal.SizeOf(cf);
            cf.szFaceName = new char[] { (char)31 };
            IntPtr wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);
            IntPtr res = SendMessage(Handle, EM_GETCHARFORMAT, wpar, lpar);
            cf = ((CHARFORMAT2_STRUCT)(Marshal.PtrToStructure(lpar, typeof(CHARFORMAT2_STRUCT))));
            int state;
            //  dwMask holds the information which properties are consistent
            //  throughout the selection:
            if (cf.dwMask == mask)
            {
                if (cf.dwEffects == effect)
                {
                    state = 1;
                }
                else
                {
                    state = 0;
                }
            }
            else
            {
                state = -1;
            }

            Marshal.FreeCoTaskMem(lpar);
            return state;
        }

        private void SetSelectionStyle(int mask, int effect)
        {
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = Marshal.SizeOf(cf);
            cf.dwMask = mask;
            cf.dwEffects = effect;
            IntPtr wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);
            IntPtr res = SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar);
            Marshal.FreeCoTaskMem(lpar);
        }
    }
}