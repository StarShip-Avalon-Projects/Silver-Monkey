using Libraries.Web;
using Monkeyspeak;
using System;
using System.Collections.Generic;

namespace Libraries
{
    /// <summary>
    /// Provides web interface for getting a list of Variables from a web server
    /// <para>
    /// Does support HTTPS connections
    /// </para>
    /// <para>
    /// Effects: (5:10) - (5:60)
    /// </para>
    /// </summary>
    public class MsWebRequests : MonkeySpeakLibrary
    {
        #region Private Fields

        private WebRequests[] Webrequest;

        private List<IVariable> WebStack = new List<IVariable>();

        private Uri WebURL;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 70;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of vars to use to pass runtime vars to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            Webrequest = null;
            WebURL = null;
            Add(TriggerCategory.Cause,
                r => true,
                "When the bot receives a variable list by sending the Web-Cache.");

            Add(TriggerCategory.Condition,
                WebArrayEqualTo,
                "and Web-Table {...} is equal to {...},");

            Add(TriggerCategory.Condition,
                WebArrayNotEqualTo,
                "and Web-Cache setting {...} is not equal to {...},");

            Add(TriggerCategory.Condition,
                WebArrayContainArrayField,
                "and the Web-Cache contains field named {...},");

            Add(TriggerCategory.Condition,
                WebArrayNotContainArrayField,
                "and the Web-Cache doesn\'t contain field named {...},");

            Add(TriggerCategory.Effect,
                RemoveWebStack,
                "remove variable %Variable from the Web-Cache.");

            Add(TriggerCategory.Effect,
                SetURL,
                " Set the web URL to {...},");

            Add(TriggerCategory.Effect,
                RememberSetting,
                " remember setting {...} from Web-Cache and store it into variable %Variable.");
            Add(TriggerCategory.Effect,
                SendGetWebStack,
                "send GET request to send the Web-Cache to URL.");

            Add(TriggerCategory.Effect,
                StoreWebStack,
                "store variable %Variable to the Web-Cache.");

            Add(TriggerCategory.Effect,
                SendWebStack,
                "send POST request to send the Web-Cache to URL.");

            Add(TriggerCategory.Effect,
                ClearWebStack,
                "clear the Web-Cache.");
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// (5:19) clear the Web-Cache.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// True Always
        /// </returns>
        private bool ClearWebStack(TriggerReader reader)
        {
            if (WebStack != null && WebStack.Count > 0)
            {
                WebStack.Clear();
            }

            return true;
        }

        /// <summary>
        /// (5:11) remember setting {...} from Web-Cache and store it into
        /// variable %Variable.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool RememberSetting(TriggerReader reader)
        {
            var setting = reader.Page.SetVariable(new Variable($"%{reader.ReadString()}", false));
            var var = reader.ReadVariable(true);
            if (WebStack.Contains(setting))
            {
                var.Value = WebStack[WebStack.IndexOf(setting)];
            }

            return true;
        }

        /// <summary>
        /// (5:60) remove variable %Variable from the Web-Cache
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool RemoveWebStack(TriggerReader reader)
        {
            var var = reader.ReadVariable();
            WebStack.Remove(var);
            return true;
        }

        /// <summary>
        /// (5:16) send GET request to send the Web-Cache to URL.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool SendGetWebStack(TriggerReader reader)
        {
            Web.WebRequests ws = new Web.WebRequests(WebURL, reader);
            var WebPage = ws.WGet(WebStack);
            WebStack = WebPage.WebStack;
            if (WebPage.ReceivedPage)
            {
                reader.Page.Execute(70);
            }

            if (WebPage.Status != 0)
            {
                throw new WebException(WebPage.ErrMsg);
            }

            return true;
        }

        /// <summary>
        /// (5:18) send post request to send the Web-Cache to the web host.
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool SendWebStack(TriggerReader reader)
        {
            WebData WebPage = new WebData();
            WebRequests ws = new WebRequests(WebURL, reader);
            WebPage = ws.WPost(WebStack);
            WebStack = WebPage.WebStack;
            if (WebPage.ReceivedPage)
            {
                reader.Page.Execute(70);
            }
            return true;
        }

        private bool SetURL(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (5:17) store variable %Variable to the Web-Cache
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool StoreWebStack(TriggerReader reader)
        {
            var var = reader.ReadVariable();
            if (var != null)
            {
                if (WebStack.Contains(var))
                {
                    WebStack[WebStack.IndexOf(var)] = var;
                }
                else
                {
                    WebStack.Add(var);
                }
            }

            return true;
        }

        /// <summary>
        /// (1:32) and the Web-Cache contains field named {...},
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool WebArrayContainArrayField(TriggerReader reader)
        {
            var var = reader.Page.SetVariable(new Variable($"%{reader.ReadString()}", false));
            return WebStack.Contains(var);
        }

        /// <summary>
        /// (1:30) and Web-Cache setting {...} is equal to {...},
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool WebArrayEqualTo(TriggerReader reader)
        {
            string setting;

            setting = WebStack[WebStack.IndexOf(reader.ReadVariable())].Value.ToString(); ;

            return true;
        }

        /// <summary>
        /// (1:33) and the Web-Cache doesn't contain field named {...},
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns></returns>
        private bool WebArrayNotContainArrayField(TriggerReader reader)
        {
            return !WebStack.Contains(reader.Page.SetVariable(new Variable($"%{reader.ReadString()}", false)));
        }

        /// <summary>
        /// (1:31) and Web-Cache setting {...} is not equal to {...},
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <returns>
        /// </returns>
        private bool WebArrayNotEqualTo(TriggerReader reader)
        {
            throw new NotImplementedException();
            //WebVariable setting = new WebVariable(reader.ReadString());
            //if (WebStack.Contains(setting))
            //{
            //    setting = WebStack[WebStack.IndexOf(setting)].Value.ToString();
            //}

            //return (setting.Value != reader.ReadString());
        }

        #endregion Private Methods
    }
}