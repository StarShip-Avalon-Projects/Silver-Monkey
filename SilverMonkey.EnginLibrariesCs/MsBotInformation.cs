using Monkeyspeak;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
///
/// </summary>
/// <seealso cref="Monkeyspeak.Libraries.BaseLibrary" />
public class MsBotInformation : MonkeySpeakLibrary
{
    #region Public Properties

    /// <summary>
    /// Gets the base identifier.
    /// </summary>
    /// <value>
    /// The base identifier.
    /// </value>
    public override int BaseId => 0;

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// (5:42) start a new instance to Silver Monkey with bot-file {..}.
    /// </summary>IO
    /// <param name="reader"><see cref="TriggerReader"/></param>
    /// <returns>true on success</returns>
    public static bool StartNewBot(TriggerReader reader)
    {
        var File = reader.ReadString();
        ProcessStartInfo p = new ProcessStartInfo()
        {
            Arguments = File,
            FileName = "SilverMonkey.exe",
            WorkingDirectory = IO.Paths.ApplicationPath
        };

        Process.Start(p);
        return true;
    }

    /// <summary>
    /// (5:41) Disconnect the bot from the Furcadia game server.
    /// </summary>
    /// <param name="reader"><see cref="TriggerReader"/></param>
    /// <returns>true on success</returns>
    public bool FurcadiaDisconnect(TriggerReader reader)
    {
        Task.Run(() => ParentBotSession.Disconnect()).Wait();
        return !ParentBotSession.IsServerSocketConnected;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="args"></param>
    public override void Initialize(params object[] args)
    {
        Add(TriggerCategory.Cause, 0,
             r => true,
            "When the Monkey Speak Engine starts the script,");
        Add(TriggerCategory.Cause, 1,
             r => true,
             "When the bot logs into Furcadia,");

        Add(TriggerCategory.Cause, 2,
            r => true,
            "When the bot logs out of Furcadia,");

        Add(TriggerCategory.Cause, 3,
            r => true,
            "When the Furcadia client disconnects or closes,");

        // (0:92) When the bot detects the "Your throat is tired. Please wait a few seconds"message,
        Add(TriggerCategory.Cause, 5,
            r =>
            {
                var rtn = r.GetParametersOfType<bool?>().FirstOrDefault();
                if (rtn != null)
                    return (bool)rtn;
                return false;
            },
            "When the bot detects the \"Your throat is tired. Please wait a few seconds\"message,");

        // (0:93) When the bot resumes processing after seeing "Your throat is tired"message,
        Add(TriggerCategory.Cause, 6,
            r =>
            {
                var rtn = r.GetParametersOfType<bool?>().FirstOrDefault();
                if (rtn != null)
                    return (bool)rtn;
                return false;
            },
            "When the bot resumes processing after seeing \"Your throat is tired\"message,");

        // (1:904) and the triggering furre is the Bot Controller,
        Add(TriggerCategory.Condition,
            r => TriggeringFurreIsBotController(r),
            "and the triggering furre is the Bot Controller,");

        // (1:905) and the triggering furre is not the Bot Controller,
        Add(TriggerCategory.Condition,
         r => !TriggeringFurreIsBotController(r),
            "and the triggering furre is not the Bot Controller,");

        // (5:40) Switch the bot to stand alone mode and close the Furcadia client.
        Add(TriggerCategory.Effect,
            r => StandAloneMode(r),
            "switch the bot to stand alone mode and close the Furcadia client.");

        // (5:41) Disconnect the bot from the Furcadia game server.
        Add(TriggerCategory.Effect,
            r => FurcadiaDisconnect(r),
            "disconnect the bot from the Furcadia game server.");

        // (5:42) start a new instance to Silver Monkey with botfile {..}.
        Add(TriggerCategory.Effect,
            r => StartNewBot(r),
            "start a new instance to Silver Monkey with bot-file {..}.");
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

    private bool StandAloneMode(TriggerReader reader)
    {
        ParentBotSession.StandAlone = true;
        ParentBotSession.CloseClient();
        return true;
    }

    #endregion Private Methods
}