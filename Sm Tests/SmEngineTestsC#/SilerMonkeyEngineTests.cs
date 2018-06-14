using Engine.BotSession;
using Furcadia.Logging;
using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using IO;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using static Libraries.MsLibHelper;
using static SmEngineTests.Utilities;

namespace SmEngineTests
{
    [TestFixture]
    public class SilerMonkeyEngineTests_Alt_DBugger
    {
        private object parseLock = new object();
        private const string GeroSayPing = "<name shortname='gerolkae'>Gerolkae</name>: ping";
        private const string GeroWhisperCunchatize = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"crunchatize\" to you. ]</font>";
        private const string GeroWhisperRollOut = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"roll out\" to you. ]</font>";

        // private const string PingTest = @"<name shortname='gerolkae'>Gerolkae</name>: ping";
        private const string WhisperTest = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"hi\" to you. ]</font>";

        // private const string PingTest2 = @"<name shortname='gerolkae'>Gerolkae</name>: Ping";
        private const string GeroWhisperHi = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"Hi\" to you. ]</font>";

        //  private const string YouWhisper = "<font color='whisper'>[You whisper \"Logged on\" to<name shortname='gerolkae' forced src='whisper-to'>Gerolkae</name>. ]</font>";
        private const string YouWhisper2 = "<font color='whisper'>[ You whisper \"Logged on\" to <name shortname='gerolkae' forced src='whisper-to'>Gerolkae</name>. ]</font>";

        private const string YouShouYo = "<font color='shout'>You shout, \"Yo Its Me\"</font>";
        private const string GeroShout = "<font color='shout'>{S} <name shortname='gerolkae'>Gerolkae</name> shouts: ping</font>";
        private const string Emote = "<font color='emote'><name shortname='silvermonkey'>Silver|Monkey</name> Emoe</font>";

        private const string Emit = "<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Silver|Monkey has arrived...</font>";
        private const string EmitTest = "<font color='emit'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> test</font>";
        private const string EmitBlah = "<font color='emit'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Blah</font>";
        private const string EmitWarning = "<font color='warning'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> (<name shortname='silvermonkey'>Silver|Monkey</name> just emitted.)</font>";

        private const string CookieBank = "<font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>";
        private Bot Proxy;

        public string SettingsFile { get; private set; }
        public string BackupSettingsFile { get; private set; }
        public BotOptions options { get; private set; }

        public SilerMonkeyEngineTests_Alt_DBugger()
        {
            SettingsFile = Path.Combine(Paths.FurcadiaSettingsPath, @"settings.ini");
            BackupSettingsFile = Path.Combine(Paths.FurcadiaSettingsPath, @"BackupSettings.ini");
            File.Copy(SettingsFile, BackupSettingsFile, true);
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            var BotFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Silver Monkey.bini");
            var MsFile = Path.Combine(IO.Paths.SilverMonkeyDocumentsPath,
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
                Standalone = false,
                CharacterIniFile = CharacterFile,
                MonkeySpeakEngineOptions = MsEngineOption,
                ResetSettingTime = 10
            };

            options.SaveBotSettings();

            Proxy = new Bot(options);
            Proxy.Error += (e, o) => Logger.Error($"{e} {o}");
            BotHasConnected(Proxy.StandAlone);
        }

        [TestCase(WhisperTest, "hi")]
        [TestCase(GeroWhisperHi, "Hi")]
        [TestCase(YouShouYo, "Yo Its Me")]
        [TestCase(GeroShout, "ping")]
        [TestCase(EmitWarning, "(<name shortname='silvermonkey'>Silver|Monkey</name> just emitted.)")]
        [TestCase(Emit, "Silver|Monkey has arrived...")]
        [TestCase(EmitBlah, "Blah")]
        [TestCase(Emote, " Emoe")]
        [TestCase(EmitTest, "test")]
        public void ChannelTextIs(string testc, string ExpectedValue)
        {
            lock (parseLock)
            {
                bool IsTested = false;
                Proxy.ProcessServerChannelData += (sender, Args) =>
                {
                    if (!IsTested && sender is ChannelObject ServeObject)
                    {
                        Assert.That(ServeObject.Player.Message.Trim(),
                            Is.EqualTo(ExpectedValue.Trim()));
                        IsTested = true;
                    }
                };

                Proxy.ParseServerChannel(testc, false);

                Proxy.ProcessServerChannelData -= (sender, Args) =>
                {
                    if (!IsTested && sender is ChannelObject ServeObject)
                    {
                        Assert.That(ServeObject.Player.Message.Trim(),
                            Is.EqualTo(ExpectedValue.Trim()));
                        IsTested = true;
                    }
                };

                Logger.Debug<SilerMonkeyEngineTests_Alt_DBugger>($"ServerStatus: {Proxy.ServerStatus}");
                Logger.Debug<SilerMonkeyEngineTests_Alt_DBugger>($"ClientStatus: {Proxy.ClientStatus}");
            }
        }

        [TestCase(WhisperTest, "whisper", "Gerolkae")]
        [TestCase(YouWhisper2, "whisper", "you")]
        [TestCase(GeroShout, "shout", "Gerolkae")]
        [TestCase(YouShouYo, "shout", "you")]

        //Furre just emitted notice
        [TestCase(EmitWarning, "@emit", "Silver Monkey")]
        [TestCase(Emit, "@emit", "Furcadia Game Server")]
        [TestCase(EmitBlah, "@emit", "Furcadia Game Server")]
        [TestCase(Emote, "emote", "Silver Monkey")]
        [TestCase(GeroWhisperHi, "whisper", "Gerolkae")]
        public void ExpectedCharachter(string testc, string channel, string ExpectedValue)
        {
            lock (parseLock)
            {
                bool IsTested = false;
                Proxy.ProcessServerChannelData += (sender, Args) =>
                {
                    if (!IsTested && sender is ChannelObject ServeObject)
                    {
                        if (ExpectedValue == "you")
                            Assert.That(ServeObject.Player,
                                Is.EqualTo(Proxy.ConnectedFurre));
                        else
                            Assert.That(ServeObject.Player.ShortName,
                                Is.EqualTo(ExpectedValue.ToFurcadiaShortName()));
                        IsTested = true;
                    }
                };

                Proxy.ParseServerChannel(testc, false);

                Proxy.ProcessServerChannelData -= (sender, Args) =>
                {
                    if (!IsTested && sender is ChannelObject ServeObject)
                    {
                        if (ExpectedValue == "you")
                            Assert.That(ServeObject.Player,
                                Is.EqualTo(Proxy.ConnectedFurre));
                        else
                            Assert.That(ServeObject.Player.ShortName,
                                Is.EqualTo(ExpectedValue.ToFurcadiaShortName()));
                        IsTested = true;
                    }
                };

                Logger.Debug<SilerMonkeyEngineTests_Alt_DBugger>($"ServerStatus: {Proxy.ServerStatus}");
                Logger.Debug<SilerMonkeyEngineTests_Alt_DBugger>($"ClientStatus: {Proxy.ClientStatus}");
            }
        }

        [TestCase(GeroShout, "ping")]
        public void SilverMonkeyEngine_InstructionObjectPlayerIs(string testc, string ExpectedValue)
        {
            lock (parseLock)
            {
                bool IsTested = false;
                //Turn the channel on
                Proxy.SendFormattedTextToServer("- Shout");
                HaltFor(1);
                Proxy.ProcessServerChannelData += (sender, Args) =>
                {
                    if (!IsTested && sender is ChannelObject InstructionObject)
                    {
                        Assert.That(InstructionObject.Player.Message.Trim(),
                            Is.EqualTo(ExpectedValue.Trim()));
                        IsTested = true;
                    }
                };

                Proxy.ParseServerChannel(testc, false);

                Proxy.ProcessServerChannelData -= (sender, Args) =>
                {
                    if (!IsTested && sender is ChannelObject InstructionObject)
                    {
                        Assert.That(InstructionObject.Player.Message.Trim(),
                            Is.EqualTo(ExpectedValue.Trim()));
                        IsTested = true;
                    }
                };
            }
        }

        public void BotHasConnected(bool StandAlone = true)
        {
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

        public void DisconnectTests()
        {
            Proxy.ServerDisconnected += () =>
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
            };
            Proxy.Disconnect();
            Proxy.ServerDisconnected -= () =>
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
            };
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DisconnectTests();
            Proxy.Error -= (e, o) => Logger.Error($"{e} {o}");
            Proxy.Dispose();
            options = null;
        }
    }
}