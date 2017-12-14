using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using MonkeyCore;
using NUnit.Framework;
using SilverMonkeyEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using static SilverMonkeyEngine.Engine.Libraries.MsLibHelper;

using static SmEngineTests.Utilities;

namespace SmEngineTests
{
    [TestFixture]
    public class SilerMonkeyEngineTests
    {
        private BotOptions options;
        private BotSession Proxy;
        private const int ConnectWaitTime = 10;
        private const int DreamEntranceDelay = 10;
        private const int CleanupDelayTime = 10;

        private readonly string SettingsFile;
        private readonly string BackupSettingsFile;

        private const string GeroSayPing = "<name shortname='gerolkae'>Gerolkae</name>: ping";
        private const string GeroWhisperCunchatize = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"crunchatize\" to you. ]</font>";
        private const string GeroWhisperRollOut = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"roll out\" to you. ]</font>";
        private const string PingTest = @"<name shortname='gerolkae'>Gerolkae</name>: ping";
        private const string WhisperTest = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"hi\" to you. ]</font>";
        private const string PingTest2 = @"<name shortname='gerolkae'>Gerolkae</name>: Ping";
        private const string GeroWhisperHi = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"Hi\" to you. ]</font>";

        //  private const string YouWhisper = "<font color='whisper'>[You whisper \"Logged on\" to<name shortname='gerolkae' forced src='whisper-to'>Gerolkae</name>. ]</font>";
        private const string YouWhisper2 = "<font color='whisper'>[ You whisper \"Logged on\" to <name shortname='gerolkae' forced src='whisper-to'>Gerolkae</name>. ]</font>";

        private const string YouShouYo = "<font color='shout'>You shout, \"Yo Its Me\"</font>";
        private const string GeroShout = "<font color='shout'>{S} <name shortname='gerolkae'>Gerolkae</name> shouts: ping</font>";
        private const string Emote = "<font color='emote'><name shortname='silvermonkey'>Silver|Monkey</name> Emoe</font>";

        private const string Emit = "<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Silver|Monkey has arrived...</font>";
        private const string SpokenEmit = "<font color='emit'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Blah</font>";
        private const string EmitWarning = "<font color='warning'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> (<name shortname='silvermonkey'>Silver|Monkey</name> just emitted.)</font>";

        private const string CookieBank = "<font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>";

        public SilerMonkeyEngineTests()
        {
            SettingsFile = Path.Combine(Paths.FurcadiaSettingsPath, @"settings.ini");
            BackupSettingsFile = Path.Combine(Paths.FurcadiaSettingsPath, @"BackupSettings.ini");
            File.Copy(SettingsFile, BackupSettingsFile, true);
        }

        [SetUp]
        public void Initialize()
        {
            Furcadia.Logging.Logger.SingleThreaded = false;
            if (!File.Exists(BackupSettingsFile))
                throw new Exception("BackupSettingsFile Doesn't Exists");
            var BotFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Silver Monkey.bini");
            var MsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Bugreport 165 From Jake.ms");
            var CharacterFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "silvermonkey.ini");
            var MsEngineOption = new SilverMonkeyEngine.Engine.EngineOptoons()
            {
                MonkeySpeakScriptFile = MsFile,
                MS_Engine_Enable = true,
                BotController = @"Gerolkae"
            };
            options = new BotOptions(ref BotFile)
            {
                Standalone = true,
                CharacterIniFile = CharacterFile,
                MonkeySpeakEngineOptions = MsEngineOption
            };

            options.SaveBotSettings();

            Proxy = new BotSession(options);
            Proxy.ClientData2 += (ClintData) => Proxy.SendToServer(ClintData);
            Proxy.ServerData2 += (ServerData) => Proxy.SendToClient(ServerData);
        }

        [TestCase(WhisperTest, "hi")]
        [TestCase(PingTest, "ping")]
        [TestCase(GeroWhisperHi, "Hi")]
        [TestCase(PingTest2, "Ping")]
        //    [TestCase(YouWhisper, "Logged on")]
        [TestCase(YouShouYo, "Yo Its Me")]
        [TestCase(GeroShout, "ping")]
        [TestCase(EmitWarning, "(<name shortname='silvermonkey'>Silver|Monkey</name> just emitted.)")]
        [TestCase(Emit, "Silver|Monkey has arrived...")]
        [TestCase(SpokenEmit, "Blah")]
        [TestCase(Emote, "Emoe")]
        public void ChannelTextIs(string testc, string ExpectedValue)
        {
            Task.Run(() => Proxy.ConnetAsync()).Wait();

            HaltFor(ConnectWaitTime);

            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
             {
                 var ServeObject = (ChannelObject)sender;
                 Assert.That(ServeObject.Player.Message, Is.EqualTo(ExpectedValue));
             };

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Proxy.ParseServerChannel(testc, false);
        }

        [TestCase(WhisperTest, "Gerolkae")]
        [TestCase(PingTest, "Gerolkae")]
        //   [TestCase(YouWhisper, "Silver Monkey")]
        [TestCase(YouWhisper2, "Silver Monkey")]
        [TestCase(GeroShout, "Gerolkae")]
        [TestCase(YouShouYo, "Silver Monkey")]
        [TestCase(EmitWarning, "Silver Monkey")]
        [TestCase(Emit, "Furcadia Game Server")]
        [TestCase(SpokenEmit, "Furcadia Game Server")]
        [TestCase(Emote, "Silver monkey")]
        [TestCase(GeroWhisperHi, "Gerolkae")]
        public void ExpectedCharachter(string testc, string ExpectedValue)
        {
            BotHasConnectedStandAlone();

            Proxy.ProcessServerChannelData += (sender, Args) =>
           {
               var ServeObject = (ChannelObject)sender;
               Assert.That(ServeObject.Player.ShortName, Is.EqualTo(ExpectedValue.ToFurcadiaShortName()));
           };

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Proxy.ParseServerChannel(testc, false);
            Proxy.ProcessServerChannelData -= (sender, Args) =>
            {
                var ServeObject = (ChannelObject)sender;
                Assert.That(ServeObject.Player.ShortName, Is.EqualTo(ExpectedValue.ToFurcadiaShortName()));
            };
            DisconnectTests();
        }

        //   [TestCase(YouWhisper, "whisper")]
        [TestCase(YouWhisper2, "whisper")]
        [TestCase(WhisperTest, "whisper")]
        [TestCase(GeroWhisperHi, "whisper")]
        [TestCase(PingTest, "say")]
        [TestCase(YouShouYo, "shout")]
        [TestCase(GeroShout, "shout")]
        [TestCase(EmitWarning, "@emit")]
        [TestCase(Emit, "@emit")]
        [TestCase(SpokenEmit, "@emit")]
        [TestCase(Emote, "emote")]
        public void ExpectedChannelNameIs(string ChannelCode, string ExpectedValue)
        {
            BotHasConnectedStandAlone();
            HaltFor(DreamEntranceDelay);

            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
            {
                var ServeObject = (ChannelObject)sender;
                Assert.That(Args.Channel, Is.EqualTo(ExpectedValue));
            };

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Proxy.ParseServerChannel(ChannelCode, false);
            DisconnectTests();
        }

        [Test]
        public void ConstanVariablesAreSet()
        {
            BotHasConnectedStandAlone();
            HaltFor(DreamEntranceDelay);

            Assert.Multiple(() =>
            {
                var Var = Proxy.MSpage.GetVariable(TriggeringFurreNameVariable);
                Assert.IsTrue(Var.Value != null, $"Constant Variable: '{Var}' ");
                Assert.IsTrue(Var.IsConstant == true, $"Constant Variable: '{Var}' ");

                Var = Proxy.MSpage.GetVariable(TriggeringFurreShortNameVariable);
                Assert.IsTrue(Var.Value != null, $"Constant Variable: '{Var}' ");
                Assert.IsTrue(Var.IsConstant == true, $"Constant Variable: '{Var}' ");

                Var = Proxy.MSpage.GetVariable(BotControllerVariable);
                Assert.IsTrue(Var.Value != null, $"Constant Variable: '{Var}' ");
                Assert.IsTrue(Var.IsConstant == true, $"Constant Variable: '{Var}' ");

                Var = Proxy.MSpage.GetVariable(BotNameVariable);
                Assert.IsTrue(Var.Value != null, $"Constant Variable: '{Var}' ");
                Assert.IsTrue(Var.IsConstant == true, $"Constant Variable: '{Var}' ");

                Var = Proxy.MSpage.GetVariable(DreamNameVariable);
                Assert.IsTrue(Var.Value != null, $"Constant Variable: '{Var}' ");
                Assert.IsTrue(Var.IsConstant == true, $"Constant Variable: '{Var}' ");

                Var = Proxy.MSpage.GetVariable(DreamOwnerVariable);
                Assert.IsTrue(Var.Value != null, $"Constant Variable: '{Var}' ");
                Assert.IsTrue(Var.IsConstant == true, $"Constant Variable: '{Var}' ");

                Var = Proxy.MSpage.GetVariable(BanishListVariable);
                Assert.IsTrue(Var.Value == null, $"Constant Variable: '{Var}' ");
                Assert.IsTrue(Var.IsConstant == true, $"Constant Variable: '{Var}' ");

                Var = Proxy.MSpage.GetVariable(BanishNameVariable);
                Assert.IsTrue(Var.Value == null, $"Constant Variable: '{Var}' ");
                Assert.IsTrue(Var.IsConstant == true, $"Constant Variable: '{Var}' ");
            });
            DisconnectTests();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void DreamInfoIsSet_StandAlone(bool StandAlone)
        {
            BotHasConnectedStandAlone(StandAlone);
            HaltFor(DreamEntranceDelay);

            Assert.Multiple(() =>
            {
                Assert.That(Proxy.InDream, Is.EqualTo(true), "Bot has not joined a dream");
                Assert.IsTrue(Proxy.Dream.Rating != null, $"Dream Rating is '{Proxy.Dream.Rating}'");
                Assert.IsTrue(Proxy.Dream.Name != null, $"Dream Name is '{Proxy.Dream.Name}'");
                Assert.IsTrue(Proxy.Dream.Owner != null, $"Dream Owner is '{Proxy.Dream.Owner}'");
                Assert.IsTrue(Proxy.BotController != null, $"BotController is '{Proxy.BotController}'");
                Assert.IsTrue(Proxy.Dream.ShortName != null, $"Dream ShortName is '{Proxy.Dream.ShortName}'");
                Assert.IsTrue(Proxy.Dream.URL != null, $"Dream URL is '{Proxy.Dream.URL}'");
                //  Assert.IsTrue(Proxy.Dream.Lines > 0, $"DragonSpeak Lines {Proxy.Dream.Lines}");

                Assert.IsTrue(string.IsNullOrWhiteSpace(Proxy.BanishName),
                    $"BanishName is '{Proxy.BanishName}'");
                Assert.IsTrue(Proxy.BanishList != null,
                    $"BanishList is '{Proxy.BanishList}'");
                Assert.IsTrue(Proxy.BanishList.Count == 0,
                    $"BanishList is '{Proxy.BanishList.Count}'");
            });
            DisconnectTests(StandAlone);
        }

        [TestCase(GeroShout, "ping")]
        public void ProxySession_InstructionObjectPlayerIs(string testc, string ExpectedValue)
        {
            BotHasConnectedStandAlone();

            //  Proxy.Error += OnErrorException;
            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
            {
                ChannelObject InstructionObject = (ChannelObject)sender;
                Assert.That(InstructionObject.Player.Message, Is.EqualTo(ExpectedValue));
            };
            Task.Run(() => Proxy.SendFormattedTextToServer("- Shout")).Wait();

            Proxy.ParseServerChannel(testc, false);
            DisconnectTests();
        }

        public void BotHasConnectedStandAlone(bool StandAlone = true)
        {
            Proxy.StandAlone = StandAlone;
            Task.Run(() => Proxy.ConnetAsync()).Wait();

            HaltFor(ConnectWaitTime);

            Assert.Multiple(() =>
            {
                Assert.That(Proxy.ServerStatus == ConnectionPhase.Connected,
                    $"Proxy.ServerStatus {Proxy.ServerStatus}");
                Assert.That(Proxy.IsServerSocketConnected == true,
                    $"Proxy.IsServerSocketConnected {Proxy.IsServerSocketConnected}");
                if (StandAlone)
                {
                    Assert.That(Proxy.ClientStatus == ConnectionPhase.Disconnected,
                         $"Proxy.ClientStatus {Proxy.ClientStatus}");
                    Assert.That(Proxy.IsClientSocketConnected == false,
                         $"Proxy.IsClientSocketConnected {Proxy.IsClientSocketConnected}");
                    Assert.That(Proxy.FurcadiaClientIsRunning == false,
                        $"Proxy.FurcadiaClientIsRunning {Proxy.FurcadiaClientIsRunning}");
                }
                else
                {
                    Assert.That(Proxy.ClientStatus == ConnectionPhase.Connected,
                        $"Proxy.ClientStatus {Proxy.ClientStatus}");
                    Assert.That(Proxy.IsClientSocketConnected == true,
                        $"Proxy.IsClientSocketConnected {Proxy.IsClientSocketConnected}");
                    Assert.That(Proxy.FurcadiaClientIsRunning == false,
                        $"Proxy.FurcadiaClientIsRunning {Proxy.FurcadiaClientIsRunning}");
                }
            });
        }

        public void DisconnectTests(bool StandAlone = false)
        {
            // Incase We're not standalone, Kill the left over Client;
            if (!StandAlone)
                if (Proxy.FurcadiaClientIsRunning)
                    Task.Run(() => Proxy.CloseClient()).Wait();
            HaltFor(CleanupDelayTime);
            Task.Run(() => Proxy.Disconnect()).Wait();

            Assert.Multiple(() =>
            {
                Assert.That(Proxy.ServerStatus == ConnectionPhase.Disconnected,
                    $"Proxy.ServerStatus {Proxy.ServerStatus}");
                Assert.That(Proxy.IsServerSocketConnected == false,
                    $"Proxy.IsServerSocketConnected {Proxy.IsServerSocketConnected}");
                Assert.That(Proxy.ClientStatus == ConnectionPhase.Disconnected,
                     $"Proxy.ClientStatus {Proxy.ClientStatus}");
                Assert.That(Proxy.IsClientSocketConnected == false,
                     $"Proxy.IsClientSocketConnected {Proxy.IsClientSocketConnected}");
                Assert.That(Proxy.FurcadiaClientIsRunning == false,
                    $"Proxy.FurcadiaClientIsRunning {Proxy.FurcadiaClientIsRunning}");
            });
        }

        [TearDown]
        public void Cleanup()
        {
            //if (Proxy.IsServerSocketConnected)
            //{
            //    if (Proxy.FurcadiaClientIsRunning)
            //        Task.Run(() => Proxy.CloseClient()).Wait();
            //    HaltFor(CleanupDelayTime);
            //    Task.Run(() => Proxy.Disconnect()).Wait();
            //}

            //  HaltFor(5);
            Proxy.ClientData2 -= (data) => Proxy.SendToServer(data);
            Proxy.ServerData2 -= (data) => Proxy.SendToClient(data);

            Proxy.Dispose();
            options = null;
        }
    }
}