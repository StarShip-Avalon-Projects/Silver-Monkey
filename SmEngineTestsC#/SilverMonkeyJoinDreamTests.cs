using Engine.BotSession;
using Furcadia.Logging;
using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using MonkeyCore2.IO;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using static Libraries.MsLibHelper;
using static SmEngineTests.Utilities;

namespace SmEngineTests
{
    [TestFixture]
    public class SilverMonkeyJoinDreamTests
    {
        #region Private Fields

        private const string CookieBank = "<font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>";
        private const string Emit = "<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Silver|Monkey has arrived...</font>";
        private const string EmitBlah = "<font color='emit'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Blah</font>";
        private const string EmitTest = "<font color='emit'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> test</font>";
        private const string EmitWarning = "<font color='warning'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> (<name shortname='silvermonkey'>Silver|Monkey</name> just emitted.)</font>";
        private const string Emote = "<font color='emote'><name shortname='silvermonkey'>Silver|Monkey</name> Emoe</font>";
        private const string GeroSayPing = "<name shortname='gerolkae'>Gerolkae</name>: ping";
        private const string GeroShout = "<font color='shout'>{S} <name shortname='gerolkae'>Gerolkae</name> shouts: ping</font>";
        private const string GeroWhisperCunchatize = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"crunchatize\" to you. ]</font>";
        private const string GeroWhisperHi = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"Hi\" to you. ]</font>";
        private const string GeroWhisperRollOut = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"roll out\" to you. ]</font>";
        private const string PingTest = @"<name shortname='gerolkae'>Gerolkae</name>: ping";
        private const string PingTest2 = @"<name shortname='gerolkae'>Gerolkae</name>: Ping";
        private const string WhisperTest = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"hi\" to you. ]</font>";
        private const string YouShouYo = "<font color='shout'>You shout, \"Yo Its Me\"</font>";

        //  private const string YouWhisper = "<font color='whisper'>[You whisper \"Logged on\" to<name shortname='gerolkae' forced src='whisper-to'>Gerolkae</name>. ]</font>";
        private const string YouWhisper2 = "<font color='whisper'>[ You whisper \"Logged on\" to <name shortname='gerolkae' forced src='whisper-to'>Gerolkae</name>. ]</font>";

        private Bot Proxy;
        private BotOptions options;

        #endregion Private Fields

        #region Public Properties

        public BotOptions Options { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void BotHasConnected(bool StandAlone = true)
        {
            Proxy.StandAlone = StandAlone;
            Task.Run(() => Proxy.ConnetAsync()).Wait();
            HaltFor(ConnectWaitTime);

            Assert.Multiple(() =>
            {
                Assert.That(Proxy.ServerStatus,
                    Is.EqualTo(ConnectionPhase.Connected),
                    $"Proxy.ServerStatus {Proxy.ServerStatus}");
                Assert.That(Proxy.IsServerSocketConnected,
                    Is.EqualTo(true),
                    $"Proxy.IsServerSocketConnected {Proxy.IsServerSocketConnected}");
                if (StandAlone)
                {
                    Assert.That(Proxy.ClientStatus,
                        Is.EqualTo(ConnectionPhase.Disconnected),
                         $"Proxy.ClientStatus {Proxy.ClientStatus}");
                    Assert.That(Proxy.IsClientSocketConnected,
                        Is.EqualTo(false),
                         $"Proxy.IsClientSocketConnected {Proxy.IsClientSocketConnected}");
                    Assert.That(Proxy.FurcadiaClientIsRunning,
                        Is.EqualTo(false),
                        $"Proxy.FurcadiaClientIsRunning {Proxy.FurcadiaClientIsRunning}");
                }
                else
                {
                    Assert.That(Proxy.ClientStatus,
                        Is.EqualTo(ConnectionPhase.Connected),
                        $"Proxy.ClientStatus {Proxy.ClientStatus}");
                    Assert.That(Proxy.IsClientSocketConnected,
                        Is.EqualTo(true),
                        $"Proxy.IsClientSocketConnected {Proxy.IsClientSocketConnected}");
                    Assert.That(Proxy.FurcadiaClientIsRunning,
                        Is.EqualTo(true),
                        $"Proxy.FurcadiaClientIsRunning {Proxy.FurcadiaClientIsRunning}");
                }
            });
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DisconnectTests();
            Proxy.ClientData2 -= data => Proxy.SendToServer(data);
            Proxy.ServerData2 -= data => Proxy.SendToClient(data);
            Proxy.Error -= (e, o) => Logger.Error($"{e} {o}");

            Proxy.Dispose();
            Options = null;
        }

        public void DisconnectTests()
        {
            Proxy.ServerStatusChanged += (sender, e) =>
            {
                if (e.ConnectPhase == ConnectionPhase.Disconnected)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(Proxy.ServerStatus,
                             Is.EqualTo(ConnectionPhase.Disconnected),
                            $"Proxy.ServerStatus {Proxy.ServerStatus}");
                        Assert.That(Proxy.IsServerSocketConnected,
                             Is.EqualTo(false),
                            $"Proxy.IsServerSocketConnected {Proxy.IsServerSocketConnected}");
                        Assert.That(Proxy.ClientStatus,
                             Is.EqualTo(ConnectionPhase.Disconnected),
                             $"Proxy.ClientStatus {Proxy.ClientStatus}");
                        Assert.That(Proxy.IsClientSocketConnected,
                             Is.EqualTo(false),
                             $"Proxy.IsClientSocketConnected {Proxy.IsClientSocketConnected}");
                        Assert.That(Proxy.FurcadiaClientIsRunning,
                             Is.EqualTo(false),
                            $"Proxy.FurcadiaClientIsRunning {Proxy.FurcadiaClientIsRunning}");
                    });
                }
            };
            Proxy.DisconnectServerAndClientStreams();
            Proxy.ServerStatusChanged -= (sender, e) =>
            {
                if (e.ConnectPhase == ConnectionPhase.Disconnected)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(Proxy.ServerStatus,
                             Is.EqualTo(ConnectionPhase.Disconnected),
                            $"Proxy.ServerStatus {Proxy.ServerStatus}");
                        Assert.That(Proxy.IsServerSocketConnected,
                             Is.EqualTo(false),
                            $"Proxy.IsServerSocketConnected {Proxy.IsServerSocketConnected}");
                        Assert.That(Proxy.ClientStatus,
                             Is.EqualTo(ConnectionPhase.Disconnected),
                             $"Proxy.ClientStatus {Proxy.ClientStatus}");
                        Assert.That(Proxy.IsClientSocketConnected,
                             Is.EqualTo(false),
                             $"Proxy.IsClientSocketConnected {Proxy.IsClientSocketConnected}");
                        Assert.That(Proxy.FurcadiaClientIsRunning,
                             Is.EqualTo(false),
                            $"Proxy.FurcadiaClientIsRunning {Proxy.FurcadiaClientIsRunning}");
                    });
                }
            };
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            var BotFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Silver Monkey.bini");
            var MsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Bugreport 165 From Jake.ms");
            var CharacterFile = Path.Combine(Paths.FurcadiaCharactersFolder,
                "dbugger.ini");
            var MsEngineOption = new EngineOptoons()
            {
                MonkeySpeakScriptFile = MsFile,
                IsEnabled = true,
                BotController = @"Gerolkae"
            };
            options = new BotOptions(BotFile)
            {
                Standalone = true,
                CharacterIniFile = CharacterFile,
                MonkeySpeakEngineOptions = MsEngineOption,
                ResetSettingTime = 10
            };

            options.SaveBotSettings();

            Proxy = new Bot(options);
            Proxy.Error += (e, o) => Logger.Error($"{e} {o}");
            BotHasConnected(Proxy.StandAlone);
        }

        [Test]
        public void DownLoadDreamAndCheckVariables()
        {
            Proxy.ProcessServerInstruction += (data, handled) =>
            {
                // Arrival in a dream, Variables should be set
                if (data is DreamBookmark)
                {
                    HaltFor(1);
                    Assert.Multiple(() =>
                    {
                        var Var = Proxy.MSpage.GetVariable(DreamNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");
                        Var = Proxy.MSpage.GetVariable(DreamOwnerVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");
                    });
                }
            };
            Proxy.SendFormattedTextToServer("`fdl furc://silvermonkey/");
            Proxy.ProcessServerInstruction -= (data, handled) =>
            {
                // Arrival in a dream, Variables should be set
                if (data is DreamBookmark)
                {
                    HaltFor(1);
                    Assert.Multiple(() =>
                    {
                        var Var = Proxy.MSpage.GetVariable(DreamNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");
                        Var = Proxy.MSpage.GetVariable(DreamOwnerVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");
                    });
                }
            };
        }

        [TestCase("furc://furrabiannights/", "furrabiannights")]
        [TestCase("furc://theshoddyribbon:murdermysteryhotelwip/", "The Shoddy Ribbon", "Murder Mystery Hotel (WIP)")]
        [TestCase("furc://silvermonkey:stargatebase/", "SilverMonkey", "Stargate Base")]
        [TestCase("furc://imaginarium/", "imaginarium")]
        [TestCase("furc://vinca/", "vinca")]
        [Author("Gerolkae")]
        public void DreamBookmarkSettingsTest(string DreamUrl, string DreamOwner, string DreamTitle = null)
        {
            Proxy.ProcessServerInstruction += (data, handled) =>
            {
                if (data is DreamBookmark)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(Proxy.Dream.DreamOwner, Is.EqualTo(DreamOwner.ToFurcadiaShortName()), $"Dream Owner: {Proxy.Dream.DreamOwner}");
                        if (string.IsNullOrWhiteSpace(Proxy.Dream.Title))
                            Assert.That(Proxy.Dream.DreamUrl, Is.EqualTo($"furc://{DreamOwner.ToFurcadiaShortName()}/"), $"Dream URL {Proxy.Dream.DreamUrl}");
                        else
                            Assert.That(Proxy.Dream.DreamUrl, Is.EqualTo($"furc://{DreamOwner.ToFurcadiaShortName()}:{DreamTitle.ToFurcadiaShortName()}/"), $"Dream URL {Proxy.Dream.DreamUrl}");
                    });
                }
            };

            Proxy.SendFormattedTextToServer($"`fdl {DreamUrl}");

            Proxy.ProcessServerInstruction -= (data, handled) =>
            {
                if (data is DreamBookmark)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(Proxy.Dream.DreamOwner, Is.EqualTo(DreamOwner.ToFurcadiaShortName()), $"Dream Owner: {Proxy.Dream.DreamOwner}");
                        if (string.IsNullOrWhiteSpace(Proxy.Dream.Title))
                            Assert.That(Proxy.Dream.DreamUrl, Is.EqualTo($"furc://{DreamOwner.ToFurcadiaShortName()}/"), $"Dream URL {Proxy.Dream.DreamUrl}");
                        else
                            Assert.That(Proxy.Dream.DreamUrl, Is.EqualTo($"furc://{DreamOwner.ToFurcadiaShortName()}:{DreamTitle.ToFurcadiaShortName()}/"), $"Dream URL {Proxy.Dream.DreamUrl}");
                    });
                }
            };
        }

        [TestCase("furc://furrabiannights/", "furrabiannights")]
        [TestCase("furc://theshoddyribbon:murdermysteryhotelwip/", "The Shoddy Ribbon", "Murder Mystery Hotel (WIP)")]
        [TestCase("furc://silvermonkey:stargatebase/", "SilverMonkey", "Stargate Base")]
        [TestCase("furc://imaginarium/", "imaginarium")]
        [TestCase("furc://vinca/", "vinca")]
        [Author("Gerolkae")]
        public void ConstanVariablesAreSet(string DreamUrl, string DreamOwner, string DreamTitle = null)
        {
            Proxy.ProcessServerInstruction += (data, handled) =>
            {
                if (data is DreamBookmark)
                {
                    Assert.Multiple(() =>
                    {
                        var Var = Proxy.MSpage.GetVariable(TriggeringFurreNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(TriggeringFurreShortNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(BotControllerVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(BotNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(DreamNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(BanishListVariable);
                        Assert.That(Var.Value,
                            Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(BanishNameVariable);
                        Assert.That(Var.Value,
                            Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");
                    });
                }
            };

            Proxy.SendFormattedTextToServer($"`fdl {DreamUrl}");

            Proxy.ProcessServerInstruction -= (data, handled) =>
            {
                if (data is DreamBookmark)
                {
                    Assert.Multiple(() =>
                    {
                        var Var = Proxy.MSpage.GetVariable(TriggeringFurreNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(TriggeringFurreShortNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(BotControllerVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(BotNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(DreamNameVariable);
                        Assert.That(Var.Value,
                            !Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(BanishListVariable);
                        Assert.That(Var.Value,
                            Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");

                        Var = Proxy.MSpage.GetVariable(BanishNameVariable);
                        Assert.That(Var.Value,
                            Is.EqualTo(null),
                            $"Constant Variable: '{Var}' ");
                        Assert.That(Var.IsConstant,
                            Is.EqualTo(true),
                            $"Constant Variable: '{Var}' ");
                    });
                }
            };
        }

        #endregion Public Methods
    }
}