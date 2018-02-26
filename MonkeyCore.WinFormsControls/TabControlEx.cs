using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using static Controls.NativeMethods;
using System.Collections.Generic;

namespace Controls
{
    [ToolboxBitmap(typeof(TabControl))]
    public class TabControlEx : TabControl
    {
        protected Dictionary<Button, TabPage> CloseButtonCollection = new Dictionary<Button, TabPage>();

        private bool _ShowCloseButtonOnTabs = true;

        public event CancelEventHandler CloseButtonClick;

        private static string Spaces(int n)
        {
            return new String(' ', n);
        }

        public bool ShowCloseButtonOnTabs
        {
            get
            {
                return _ShowCloseButtonOnTabs;
            }
            set
            {
                _ShowCloseButtonOnTabs = value;
                foreach (var btn in CloseButtonCollection.Keys)
                {
                    btn.Visible = _ShowCloseButtonOnTabs;
                }

                RePositionCloseButtons();
            }
        }

        public void RePositionCloseButtons()
        {
            foreach (var item in CloseButtonCollection)
            {
                RePositionCloseButtons(item.Value);
            }
        }

        public void RePositionCloseButtons(TabPage tp)
        {
            Button btn = CloseButtonOfTabPage(tp);
            if (btn != null)
            {
                tp.Text = tp.Text.Trim() + Spaces(5);
                int tpIndex = TabPages.IndexOf(tp);
                if (tpIndex >= 0)
                {
                    Rectangle rect = GetTabRect(tpIndex);
                    if (SelectedTab == tp)
                    {
                        btn.BackColor = Color.Red;
                        btn.Size = new Size((rect.Height - 2), (rect.Height - 2));
                        btn.Location = new Point((rect.X
                                        + (rect.Width
                                        - (rect.Height - 2))), (rect.Y + 2));
                    }
                    else
                    {
                        btn.BackColor = Color.FromKnownColor(KnownColor.ButtonFace);
                        btn.Size = new Size((rect.Height - 2), (rect.Height - 2));
                        btn.Location = new Point((rect.X
                                        + (rect.Width
                                        - (rect.Height - 2))), (rect.Y + 2));
                    }

                    btn.Visible = ShowCloseButtonOnTabs;
                    btn.BringToFront();
                }
            }
        }

        protected virtual Button AddCloseButton(TabPage tp)
        {
            Button closeButton = new Button
            {
                // TODO: Give a good visual appearance to the Close button,
                //        maybe by assigning images etc. Here I have not used
                //        images to keep things simple.
                Font = new Font("Microsoft Sans Serif", 5, FontStyle.Bold),
                ForeColor = Color.FromKnownColor(KnownColor.ButtonFace),
                BackColor = Color.FromKnownColor(KnownColor.ButtonShadow),
                FlatStyle = FlatStyle.System,
                Text = "X",
                Tag = tp
            };
            return closeButton;
        }

        protected Button CloseButtonOfTabPage(TabPage tp)
        {
            return (from item in CloseButtonCollection where item.Value == tp select item.Key).FirstOrDefault();
        }

        protected virtual void OnCloseButtonClick(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                Button btn = (Button)sender;
                TabPage tp = (TabPage)btn.Tag;
                CancelEventArgs ee = new CancelEventArgs();
                CloseButtonClick(sender, ee);
                if (!ee.Cancel)
                {
                    TabPages.Remove(tp);
                    RePositionCloseButtons();
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            TabPage tp = (TabPage)e.Control;

            Rectangle rect = GetTabRect(TabPages.IndexOf(tp));
            Button btn = this.AddCloseButton(tp);
            btn.Size = new Size((rect.Height - 3), (rect.Height - 3));
            btn.Location = new Point((rect.X
                            + (rect.Width
                            - (rect.Height - 1))), (rect.Y + 1));
            SetParent(btn.Handle, this.Handle);
            btn.TabIndex = TabPages.IndexOf(tp);
            btn.Click += new System.EventHandler(this.OnCloseButtonClick);
            CloseButtonCollection.Add(btn, tp);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            Button btn = CloseButtonOfTabPage((TabPage)e.Control);
            btn.Click -= OnCloseButtonClick;
            CloseButtonCollection.Remove(btn);
            SetParent(btn.Handle, IntPtr.Zero);
            btn.Dispose();
            base.OnControlRemoved(e);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            RePositionCloseButtons();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            RePositionCloseButtons();
        }
    }
}