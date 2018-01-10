using Monkeyspeak;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
///
/// </summary>
/// <seealso cref="Monkeyspeak.Libraries.BaseLibrary" />
public class MsBotInformation : MonkeySpeakLibrary
{
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
            (t) => true,
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
        Add(TriggerCategory.Cause, 92,
            r => true,
            "When the bot detects the \"Your throat is tired. Please wait a few seconds\"message,");

        // (0:93) When the bot resumes processing after seeing "Your throat is tired"message,
        Add(TriggerCategory.Cause, 93,
            r => true,
            "When the bot resumes processing after seeing \"Your throat is tired\"message,");

        // (5:40) Switch the bot to stand alone mode and close the Furcadia client.
        Add(TriggerCategory.Effect, 40,
            r => StandAloneMode(r),
            "switch the bot to stand alone mode and close the Furcadia client.");

        // (5:41) Disconnect the bot from the Furcadia game server.
        Add(TriggerCategory.Effect, 41,
            r => FurcadiaDisconnect(r),
            "disconnect the bot from the Furcadia game server.");

        // (5:42) start a new instance to Silver Monkey with botfile {..}.
        Add(TriggerCategory.Effect, 42,
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

    /// <summary>
    /// (5:40) Switch the bot to stand alone mode and close the Furcadia client.
    /// </summary>
    /// <param name="reader">
    /// <see cref="TriggerReader"/>
    /// </param>
    /// <returns>
    /// true on success
    /// </returns>
    private bool StandAloneMode(TriggerReader reader)
    {
        ParentBotSession.StandAlone = true;
        Task.Run(() => ParentBotSession.CloseClient()).Wait();
        return true;
    }

    #endregion Private Methods
}