using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Controls.NativeMethods;

namespace Controls
{
    [ToolboxBitmap(typeof(TabControl))]
    public class TabControlEx : TabControl
    {
        #region Protected Fields

        protected Dictionary<Button, TabPage> CloseButtonCollection = new Dictionary<Button, TabPage>();

        #endregion Protected Fields

        #region Private Fields

        private bool _ShowCloseButtonOnTabs = true;

        #endregion Private Fields

        #region Public Events

        /// <summary>
        /// Occurs when [close button click].
        /// </summary>
        public event CancelEventHandler CloseButtonClick;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether [show close button on tabs].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show close button on tabs]; otherwise, <c>false</c>.
        /// </value>
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

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Reposition the close buttons.
        /// </summary>
        public void RePositionCloseButtons()
        {
            foreach (var item in CloseButtonCollection)
            {
                RePositionCloseButtons(item.Value);
            }
        }

        /// <summary>
        /// Res the position close buttons.
        /// </summary>
        /// <param name="tp">The tp.</param>
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

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Adds the close button.
        /// </summary>
        /// <param name="tp">The tp.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Closes the button of tab page.
        /// </summary>
        /// <param name="tp">The tp.</param>
        /// <returns></returns>
        protected Button CloseButtonOfTabPage(TabPage tp)
        {
            return (from item in CloseButtonCollection where item.Value == tp select item.Key).FirstOrDefault();
        }

        /// <summary>
        /// Called when [close button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.ControlAdded" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs" /> that contains the event data.</param>
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

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.ControlRemoved" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs" /> that contains the event data.</param>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            Button btn = CloseButtonOfTabPage((TabPage)e.Control);
            btn.Click -= OnCloseButtonClick;
            CloseButtonCollection.Remove(btn);
            SetParent(btn.Handle, IntPtr.Zero);
            btn.Dispose();
            base.OnControlRemoved(e);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl" /> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            RePositionCloseButtons();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.
        /// </summary>
        /// <param name="levent">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            RePositionCloseButtons();
        }

        #endregion Protected Methods

        #region Private Methods

        private static string Spaces(int n)
        {
            return new String(' ', n);
        }

        #endregion Private Methods
    }
}