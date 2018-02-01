using Engine.BotSession;
using Furcadia.Logging;
using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using MonkeyCore2.IO;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using static SmEngineTests.Utilities;

namespace SmEngineTests
{
    [TestFixture]
    internal class PhoenixSpeakTests
    {
        private const string PsGetCharacters = "PS Ok: get: result: b='<array>', bloodofaraven666='<array>', bloodstar='<array>', CJ='<array>', cjkilman='<array>', g='<array>', gerolkae='<array>', h='<array>', queenchrysalis='<array>', r='<array>', s='<array>', silvermonkey='<array>', w='<array>'";
        private const string PsGetCharacterGerolkae = "PS Ok: get: result: sys_lastused_date='2/9/2013 4:31:39 AM', testvar1=1, testvar2='Test2'";
        private const string PsGetCharacterJohn = "PS Error: get: Query error: (-1) Unexpected character '^' at column 17";
        private const string PsGetCharacterJohn2 = "PS Error: get: Query error: Field 'john' does not exist";

        public string SettingsFile { get; private set; }
        public string BackupSettingsFile { get; private set; }
        public BotOptions options { get; private set; }
        public Bot Proxy { get; private set; }

        public PhoenixSpeakTests()
        {
            SettingsFile = Path.Combine(Paths.FurcadiaSettingsPath, @"settings.ini");
            BackupSettingsFile = Path.Combine(Paths.FurcadiaSettingsPath, @"BackupSettings.ini");
            File.Copy(SettingsFile, BackupSettingsFile, true);
        }

        [SetUp]
        public void Initialize()
        {
            if (!File.Exists(BackupSettingsFile))
                throw new Exception("BackupSettingsFile Doesn't Exists");
            var BotFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Silver Monkey.bini");
            var MsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Bugreport 165 From Jake.ms");
            var CharacterFile = Path.Combine(Paths.FurcadiaCharactersFolder,
                "silvermonkey.ini");
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
            Proxy.ClientData2 += (ClintData) => Proxy.SendToServer(ClintData);
            Proxy.ServerData2 += (ServerData) => Proxy.SendToClient(ServerData);
            Proxy.Error += (e, o) => Logger.Error($"{e} {o}");
        }

        [Test]
        public void TestPsCharacterList()
        {
            var Standalone = false;
            BotHasConnected_StandAlone(Standalone);
            HaltFor(DreamEntranceDelay);

            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
            {
                var ServeObject = (ChannelObject)sender;
                Assert.That(ServeObject.Player.Message,
                    Is.EqualTo(""));
            };

            Proxy.ParseServerChannel(PsGetCharacters, false);

            DisconnectTests(Standalone);
        }

        public void BaseTest(bool Standalone)
        {
            BotHasConnected_StandAlone(Standalone);
            HaltFor(DreamEntranceDelay);
            //
            Assert.Multiple(() =>
            {
            });

            DisconnectTests(Standalone);
        }

        public void BotHasConnected_StandAlone(bool StandAlone = false)
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
                        Is.EqualTo(ConnectionPhase.Connected),
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

        public void DisconnectTests(bool StandAlone = false)
        {
            Proxy.DisconnectServerAndClientStreams();
            HaltFor(CleanupDelayTime);

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

        [TearDown]
        public void Cleanup()
        {
            Proxy.ClientData2 -= (data) => Proxy.SendToServer(data);
            Proxy.ServerData2 -= (data) => Proxy.SendToClient(data);
            Proxy.Error -= (e, o) => Logger.Error($"{e} {o}");

            Proxy.Dispose();
            options = null;
        }
    }
}