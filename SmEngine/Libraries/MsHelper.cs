using Monkeyspeak;
using Furcadia.Net.DreamInfo;
using System.Text.RegularExpressions;

namespace Engine.Libraries
{
    public sealed class MsLibHelper
    {
        // '' <summary>
        // '' the last text the bot has seen, Usually the Triggering furre's message
        // '' </summary>
        public const string MessageVariable = "%MESSAGE";

        public const RegexOptions SmRegExOptions = RegexOptions.Compiled;

        // '' <summary>
        // '' Name of the connected furre, IE: the Bots name
        // '' </summary>
        public const string BotNameVariable = "%BOTNAME";

        public const string BotControllerVariable = "%BOTCONTROLLER";

        public const string BanishNameVariable = "%BANISHNAME";

        public const string BanishListVariable = "%BANISHLIST";

        public const string TriggeringFurreShortNameVariable = "%SHORTNAME";
        public const string TriggeringFurreNameVariable = "%NAME";
        public const string DreamOwnerVariable = "%DREAMOWNER";
        public const string DreamNameVariable = "%DREAMNAME";

        public static void UpdateTriggerigFurreVariables(ref Furre ActivePlayer, ref Page MonkeySpeakPage)
        {
            if (!MonkeySpeakPage.HasVariable(MessageVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(MessageVariable, ActivePlayer.Message));
            }

            var cv = (ConstantVariable)MonkeySpeakPage.GetVariable(MessageVariable);
            cv.SetValue(ActivePlayer.Message);
            if (!MonkeySpeakPage.HasVariable(TriggeringFurreShortNameVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(TriggeringFurreShortNameVariable, ActivePlayer.ShortName));
            }

            cv = (ConstantVariable)MonkeySpeakPage.GetVariable(TriggeringFurreShortNameVariable);
            cv.SetValue(ActivePlayer.ShortName);
            if (!MonkeySpeakPage.HasVariable(TriggeringFurreNameVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(TriggeringFurreNameVariable, ActivePlayer.Name));
            }

            cv = (ConstantVariable)MonkeySpeakPage.GetVariable(TriggeringFurreNameVariable);
            cv.SetValue(ActivePlayer.Name);
        }

        // '' <summary>
        // '' update Bot Constant Variables for the Current Dream
        // '' <para/>
        // '' <see cref="DreamOwnerVariable"/>
        // '' <para/>
        // '' <see cref="DreamNameVariable"/>
        // '' </summary>
        // '' <param name="ActiveDream"></param>
        // '' <param name="MonkeySpeakPage"></param>
        public static void UpdateCurrentDreamVariables(ref Dream ActiveDream, ref Page MonkeySpeakPage)
        {
            if (!MonkeySpeakPage.HasVariable(DreamOwnerVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(DreamOwnerVariable, ActiveDream.Owner));
            }

            var cv = (ConstantVariable)MonkeySpeakPage.GetVariable(DreamOwnerVariable);
            cv.SetValue(ActiveDream.Owner);
            if (!MonkeySpeakPage.HasVariable(DreamNameVariable))
            {
                MonkeySpeakPage.SetVariable(new ConstantVariable(DreamNameVariable, ActiveDream.Name));
            }

            cv = (ConstantVariable)MonkeySpeakPage.GetVariable(DreamNameVariable);
            cv.SetValue(ActiveDream.Name);
        }

        // '' <summary>
        // '' Reads a Double or a MonkeySpeak Variable
        // '' </summary>
        // '' <param name="reader">
        // '' <see cref="TriggerReader"/>
        // '' </param>
        // '' <param name="addIfNotExist">
        // '' Add Variable to Variable Scope is it Does not exist,
        // '' <para>
        // '' Default Value is False
        // '' </para>
        // '' </param>
        // '' <returns>
        // '' <see cref="Double"/>
        // '' </returns>
        public static double ReadVariableOrNumber(TriggerReader reader, bool addIfNotExist)
        {
            double result = 0;
            // Warning!!! Optional parameters not supported
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
    }
}