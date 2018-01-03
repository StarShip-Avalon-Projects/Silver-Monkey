using Monkeyspeak;
using Furcadia.Net.DreamInfo;
using System.Text.RegularExpressions;

namespace Libraries
{
    public sealed class MsLibHelper
    {
        #region Public Fields

        /// <summary>
        /// The banish list variable
        /// </summary>
        public const string BanishListVariable = "%BANISHLIST";

        /// <summary>
        /// The banish name variable
        /// </summary>
        public const string BanishNameVariable = "%BANISHNAME";

        /// <summary>
        /// The bot controller variable
        /// </summary>
        public const string BotControllerVariable = "%BOTCONTROLLER";

        /// <summary>
        /// The bot name variable
        /// <para />
        /// %BOTNAME
        /// </summary>
        public const string BotNameVariable = "%BOTNAME";

        /// <summary>
        /// The dream name variable
        /// </summary>
        public const string DreamNameVariable = "%DREAMNAME";

        /// <summary>
        /// The dream owner variable
        /// </summary>
        public const string DreamOwnerVariable = "%DREAMOWNER";

        /// <summary>
        /// The triggering furre message variable
        /// </summary>
        public const string MessageVariable = "%MESSAGE";

        /// <summary>
        /// The sm reg ex options
        /// </summary>
        public const RegexOptions SmRegExOptions = RegexOptions.Compiled;

        /// <summary>
        /// The triggering furre name variable
        /// </summary>
        public const string TriggeringFurreNameVariable = "%NAME";

        /// <summary>
        /// The triggering furre short name variable
        /// </summary>
        public const string TriggeringFurreShortNameVariable = "%SHORTNAME";

        #endregion Public Fields

        #region Public Methods

        /// <summary>
        /// Reads the variable or number.
        /// <para />
        /// Default Value is False
        /// </summary>
        /// <param name="reader">The  <see cref="TriggerReader"/></param>
        /// <param name="addIfNotExist">Add Variable to Variable Scope is it Does not exist,</param>
        /// <returns><see cref="Double"/></returns>
        public static double ReadVariableOrNumber(TriggerReader reader, bool addIfNotExist)
        {
            double result = 0;
            if (reader.PeekVariable())
            {
                var value = reader.ReadVariable(addIfNotExist).Value.ToString();
                double.TryParse(value, out result);
            }
            else if (reader.PeekNumber())
            {
                result = reader.ReadNumber();
            }

            return result;
        }

        /// <summary>
        /// Updates the current dream variables.
        /// <para/>
        /// <see cref="DreamOwnerVariable"/>
        /// <para/>
        /// <see cref="DreamNameVariable"/>
        /// </summary>
        /// <param name="ActiveDream">The active dream.</param>
        /// <param name="MonkeySpeakPage">The monkey speak page.</param>
        public static void UpdateCurrentDreamVariables(Dream ActiveDream, Page MonkeySpeakPage)
        {
            if (!MonkeySpeakPage.HasVariable(DreamOwnerVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(DreamOwnerVariable, ActiveDream.Owner));
            }

            ((ConstantVariable)MonkeySpeakPage.GetVariable(DreamOwnerVariable)).SetValue(ActiveDream.Owner);

            if (!MonkeySpeakPage.HasVariable(DreamNameVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(DreamNameVariable, ActiveDream.Name));
            }

            ((ConstantVariable)MonkeySpeakPage.GetVariable(DreamNameVariable)).SetValue(ActiveDream.Name);
        }

        /// <summary>
        /// Updates the triggerig furre variables.
        /// </summary>
        /// <param name="ActivePlayer">The active player.</param>
        /// <param name="MonkeySpeakPage">The monkey speak page.</param>
        public static void UpdateTriggerigFurreVariables(Furre ActivePlayer, Page MonkeySpeakPage)
        {
            if (!MonkeySpeakPage.HasVariable(MessageVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(MessageVariable, ActivePlayer.Message));
            }

             ((ConstantVariable)MonkeySpeakPage.GetVariable(MessageVariable)).SetValue(ActivePlayer.Message);

            if (!MonkeySpeakPage.HasVariable(TriggeringFurreShortNameVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(TriggeringFurreShortNameVariable, ActivePlayer.ShortName));
            }

           ((ConstantVariable)MonkeySpeakPage.GetVariable(TriggeringFurreShortNameVariable)).SetValue(ActivePlayer.ShortName);

            if (!MonkeySpeakPage.HasVariable(TriggeringFurreNameVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(TriggeringFurreNameVariable, ActivePlayer.Name));
            }

           ((ConstantVariable)MonkeySpeakPage.GetVariable(TriggeringFurreNameVariable)).SetValue(ActivePlayer.Name);
        }

        #endregion Public Methods
    }
}