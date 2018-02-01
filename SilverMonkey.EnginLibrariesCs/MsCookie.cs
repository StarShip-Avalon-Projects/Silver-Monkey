using Monkeyspeak;

namespace Libraries
{
    /// <summary>
    /// Furcadia Cookie Interface
    /// <para>
    /// <see href="http://furcadia.com/cookies/Cookie%20Economy.html"/>
    /// </para>
    /// <para>
    /// <see href="http://www.furcadia.com/cookies/"/>
    /// </para>
    /// </summary>
    public sealed class MsCookie : MonkeySpeakLibrary
    {
        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 42;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// (5:45) set variable %Variable to the cookie message the bot received.
        /// </summary>
        /// <param name="reader">
        /// <see cref="triggerreader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public bool EatCookie(TriggerReader reader)
        {
            IVariable CookieVar = reader.ReadVariable(true);
            CookieVar.Value = Player.Message;
            // add Machine Name parser
            return true;
        }

        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            //// (0:42) When some one gives a cookie to the bot,
            //Add(TriggerCategory.Cause, 42,
            //    r => ReadTriggeringFurreParams(r),
            //    "When some one gives a cookie to the bot,");

            //// (0:43) When a furre named {...} gives a cookie to the bot,
            //Add(TriggerCategory.Cause, 43,
            //    r => NameIs(r),
            //    "When a furre named {...} gives a cookie to the bot,");

            //// (0:44) When anyone gives a cookie to someone the bot can see,
            //Add(TriggerCategory.Cause, 44,
            //    r => ReadTriggeringFurreParams(r),
            //    "When anyone gives a cookie to someone the bot can see,");

            //// (0:49) When bot eats a cookie,
            //Add(TriggerCategory.Cause, 45,
            //    r => ReadTriggeringFurreParams(r),
            //    "When bot eats a cookie,");

            //// (0:95) When the Bot sees ""You do not have any cookies to give away right now!",
            //Add(TriggerCategory.Cause, 46,
            //    r => true,
            //    "When the Bot sees \"You do not have any cookies to give away right now!\",");

            //// (0:46) When bot eats a cookie,
            //Add(TriggerCategory.Cause, 47,
            //      r => true,
            //      "When the Bot sees \"Your cookies are ready.\",");

            //Add(TriggerCategory.Effect,
            //    r => EatCookie(r),
            //    "set variable %Variable to the cookie message the bot received.");
        }

        public override void Unload(Page page)
        {
        }

        #endregion Public Methods
    }
}