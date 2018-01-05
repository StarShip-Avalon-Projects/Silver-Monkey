using Furcadia.Net.DreamInfo;
using Furcadia.Net.Proxy;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static MsLibHelper;

/// <summary>
/// The base library in which all Silver Monkey's Monkey Speak Libraries
/// are built on. This Library contains the commonly used functions for
/// all the other libraries
/// </summary>
public class MonkeySpeakLibrary : BaseLibrary
{
    #region Public Fields

    /// <summary>
    /// The arguments
    /// </summary>
    public object[] args = null;

    #endregion Public Fields

    #region Private Fields

    /// <summary>
    /// Current Dream the Bot is in
    /// <para/>
    /// Referenced as a Monkeyspeak Parameter.
    /// <para/>
    /// Updates when ever Monkey Speak needs it through <see cref="Monkeyspeak.Page.Execute(Integer(), Object())"/>
    /// </summary>
    private Dream _dream;

    private DateTime _FurcTime;
    private Timer FurcTimeTimer;

    #endregion Private Fields

    #region Public Properties

    /// <summary>
    /// Connected Furre representing Silver Monkey
    /// </summary>
    /// <value>
    /// The connected furre.
    /// </value>
    public Furre ConnectedFurre { get; set; }

    /// <summary>
    /// Gets or sets the current dream information for the dream Silver Monkey is located in.
    /// </summary>
    /// <value>The dream information.</value>
    public Dream DreamInfo
    {
        get { return _dream; }
        set { _dream = value; }
    }

    /// <summary>
    /// Current Furcadia Standard Time (fst)
    /// </summary>
    /// <returns>
    /// Furcadia Time Object in Furcadia Standard Time (fst)
    /// </returns>
    public DateTime FurcTime
    {
        get
        {
            return _FurcTime;
        }
    }

    /// <summary>
    /// Reference to the Main Bot Session for the bot
    /// </summary>
    public ProxySession ParentBotSession { get; internal set; }

    /// <summary>
    /// Current Triggering Furre
    /// <para/>
    /// Referenced as a Monkeyspeak <see cref="BaseLibrary.Initialize(object[])"/> Argument.
    /// <para/>
    /// Updates when ever Monkey Speak needs it through <see cref="Page.Execute(int[],
    /// object[])"/> or <see cref="Page.ExecuteAsync(int[], object[])"/>
    /// </summary>
    public Furre Player { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Gets the type of the argumets of.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T[] GetArgumetsOfType<T>()
    {
        if (args != null && args.Length > 0)
            return args.OfType<T>().ToArray();
        return new T[0];
    }

    /// <summary>
    /// Gets the argumet.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public T GetArgumet<T>(int index = 0)
    {
        if (args != null && args.Length > index)
            return (T)args[index];
        return default(T);
    }

    /// <summary>
    /// checks the <see cref="FurreList"/>
    /// in the <see cref="DreamInfo">Dream Parameter</see>
    /// for the Target Furre.
    /// </summary>
    /// <param name="TargetFurre">
    /// Target Furre
    /// </param>
    /// <returns>
    /// True if the furre is in the dream <see cref="FurreList"/>
    /// </returns>
    public bool InDream(Furre TargetFurre)
    {
        bool found = false;
        foreach (Furre Fur in DreamInfo.Furres)
        {
            if (Fur == TargetFurre)
            {
                found = true;
                break;
            }
        }

        return found;
    }

    /// <summary>
    /// Initializes this instance. Add your trigger handlers here.
    /// </summary>
    /// <param name="args">
    /// Parametized argument of objects to use to pass runtime objects to a library at initialization
    /// </param>
    public override void Initialize(params object[] args)
    {
        this.args = args;

        FurcTimeTimer = new Timer(TimeUpdate, null, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));

        var bot = GetArgumet<ProxySession>();
        if (bot != null)
        {
            ParentBotSession = bot;
            DreamInfo = ParentBotSession.Dream;
            Player = (Furre)ParentBotSession.Player;
        }
    }

    /// <summary>
    /// Seperate function for unit testing
    /// </summary>
    /// <param name="Furr"></param>
    /// <returns></returns>
    public bool IsConnectedCharacter(Furre Furr)
    {
        if (ParentBotSession != null)
        {
            return ParentBotSession.IsConnectedCharacter(Furr);
        }

        return false;
    }

    /// <summary>
    /// Set <see cref="DreamInfo"/> from <see cref="TriggerReader.GetParameter"></see>
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">DreamInfo not set</exception>
    public bool ReadDreamParams(TriggerReader reader)
    {
        Dream dreamInfo = reader.GetParametersOfType<Dream>()[0];
        bool ParamSet = (dreamInfo != null);
        if (ParamSet)
        {
            if (string.IsNullOrWhiteSpace(dreamInfo.Name))
            {
                throw new ArgumentException("DreamInfo not set");
            }

            DreamInfo = dreamInfo;
            UpdateCurrentDreamVariables(DreamInfo, reader.Page);
        }

        return ParamSet;
    }

    /// <summary>
    /// Reads the triggering furre parameters.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns></returns>
    public bool ReadTriggeringFurreParams(TriggerReader reader)
    {
        bool ParamSet = false;
        Furre ActiveFurre = reader.GetParametersOfType<Furre>()[0];
        if (ActiveFurre != null)
        {
            Player = ActiveFurre;
            if (ActiveFurre.FurreID != -1)
            {
                ParamSet = true;
            }

            UpdateTriggerigFurreVariables(Player, reader.Page);
        }

        return ParamSet;
    }

    /// <summary>
    /// Send a raw instruction to the client
    /// </summary>
    /// <param name="message">
    /// Message sring
    /// </param>
    public void SendClientMessage(ref string message)
    {
        ParentBotSession.SendToClient(message);
    }

    /// <summary>
    /// Send Formated Text to Server
    /// </summary>
    /// <param name="message">Client to server instruction</param>
    /// <returns>True is the Server is Connected</returns>
    public bool SendServer(string message)
    {
        if (ParentBotSession == null)
        {
            return false;
        }

        if (ParentBotSession.IsServerSocketConnected)
        {
            ParentBotSession.SendFormattedTextToServer(message);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Sends the server asynchronous.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public async Task SendServerAsync(string message)
    {
        await Task.Run(() => this.SendServer(message));
    }

    /// <summary>
    /// Called when page is disposing or resetting.
    /// </summary>
    /// <param name="page">The page.</param>
    public override void Unload(Page page)
    {
        FurcTimeTimer.Dispose();
    }

    #endregion Public Methods

    #region Protected Methods

    /// <summary>
    /// <para>Comparisons are done with Fucadia Markup Stripped</para>
    /// </summary>
    /// <param name="reader"><see cref="TriggerReader"/></param>
    /// <returns>True if the %MESSAGE system variable contains the specified string</returns>
    protected virtual bool MsgContains(TriggerReader reader)
    {
        ReadTriggeringFurreParams(reader);
        var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString().ToLower();
        var msg = Player.Message;
        return msg.Contains(msMsg.ToStrippedFurcadiaMarkupString().ToLower());
    }

    /// <summary>
    ///  <para>Comparisons are done with Fucadia Markup Stripped</para>
    /// </summary>
    /// <param name="reader"><see cref="TriggerReader"/></param>
    /// <returns>true if the System %MESSAGE varible ends with the specified string</returns>
    protected virtual bool MsgEndsWith(TriggerReader reader)
    {
        ReadTriggeringFurreParams(reader);
        var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString();
        var msg = Player.Message.ToStrippedFurcadiaMarkupString();
        // Debug.Print("Msg = " & msg)
        return (msg.ToLower().EndsWith(msMsg.ToLower())
                    & !IsConnectedCharacter(Player));
    }

    /// <summary>
    /// the Main Message is Comparason function
    /// </summary>
    /// <param name="reader">
    /// <see cref="TriggerReader"/>
    /// </param>
    /// <returns>
    /// true on success
    /// </returns>
    protected virtual bool MsgIs(TriggerReader reader)
    {
        ReadTriggeringFurreParams(reader);
        string msg = Player.Message.ToStrippedFurcadiaMarkupString();
        string test = reader.ReadString().ToStrippedFurcadiaMarkupString();
        return msg.ToLower() == test.ToLower();
    }

    /// <summary>
    /// (1:14) and triggering furre's message doesn't end with {.},
    /// </summary>
    /// <param name="reader"><see cref="TriggerReader"/></param>
    /// <returns></returns>
    protected bool MsgNotEndsWith(TriggerReader reader)
    {
        ReadTriggeringFurreParams(reader);
        string msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString().ToLower();
        string msg = Player.Message.ToStrippedFurcadiaMarkupString().ToLower();
        return (!msg.ToLower().EndsWith(msMsg.ToLower())
                    & !IsConnectedCharacter(Player));
    }

    /// <summary>
    /// (1:11) and triggering furre's message starts with {.},
    /// </summary>
    /// <param name="reader"><see cref="TriggerReader"/></param>
    /// <returns></returns>
    protected bool MsgStartsWith(TriggerReader reader)
    {
        ReadTriggeringFurreParams(reader);
        string msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString().ToLower();
        string msg = Player.Message.ToStrippedFurcadiaMarkupString().ToLower();
        return (msg.ToLower().StartsWith(msMsg.ToLower())
                    & !IsConnectedCharacter(Player));
    }

    /// <summary>
    /// Generic base Furre named {...} is Triggering Furre
    /// </summary>
    /// <param name="reader"><see cref="TriggerReader"/></param>
    /// <returns>True on Name match</returns>
    /// <remarks>
    /// any name is acepted and converted to Furcadia Machine name (ShortName version, lowercase
    /// with special characters stripped)
    /// </remarks>
    public bool NameIs(TriggerReader reader)
    {
        return reader.ReadString().ToFurcadiaShortName() == Player.ShortName;
    }

    #endregion Protected Methods

    #region Private Methods

    /// <summary>
    /// Furcadia Clock updater
    /// </summary>
    /// <param name="obj">
    /// Nothing
    /// </param>
    private void TimeUpdate(object obj)
    {
        _FurcTime = DateTime.Now;
    }

    #endregion Private Methods
}