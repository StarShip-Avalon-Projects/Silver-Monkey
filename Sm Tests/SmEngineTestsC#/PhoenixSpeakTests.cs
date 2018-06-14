using Engine.BotSession;
using Furcadia.Logging;
using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using IO;
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
        private object EventLock = new object();
        private const string PsGetCharacters = "PS Ok: get: result: b='<array>', bloodofaraven666='<array>', bloodstar='<array>', CJ='<array>', cjkilman='<array>', g='<array>', gerolkae='<array>', h='<array>', queenchrysalis='<array>', r='<array>', s='<array>', silvermonkey='<array>', w='<array>'";
        private const string PsGetCharacterOk = "PS Ok: get: result: sys_lastused_date='2/9/2013 4:31:39 AM', testvar1=1, testvar2='Test2'";
        private const string PsGetCharacterError = "PS Error: get: Query error: (-1) Unexpected character '^' at column 17";
        private const string PsGetCharacterError2 = "PS Error: get: Query error: Field 'john' does not exist";

        private const string PsIdGetCharacters = "PS 666 Ok: get: result: b='<array>', bloodofaraven666='<array>', bloodstar='<array>', CJ='<array>', cjkilman='<array>', g='<array>', gerolkae='<array>', h='<array>', queenchrysalis='<array>', r='<array>', s='<array>', silvermonkey='<array>', w='<array>'";
        private const string PsIdGetCharacterOk = "PS 666 Ok: get: result: sys_lastused_date='2/9/2013 4:31:39 AM', testvar1=1, testvar2='Test2'";
        private const string PsIdGetCharacterError = "PS 666 Error: get: Query error: (-1) Unexpected character '^' at column 17";
        private const string PsIdGetCharacterError2 = "PS 666 Error: get: Query error: Field 'john' does not exist";

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

        [OneTimeSetUp]
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

            Proxy = new Bot(options);
            Proxy.ClientData2 += (ClintData) => Proxy.SendToServer(ClintData);
            Proxy.ServerData2 += (ServerData) => Proxy.SendToClient(ServerData);
            Proxy.Error += (e, o) => Logger.Error($"{e} {o}");

            BotHasConnected_StandAlone();
        }

        [TestCase(PsGetCharacters, PsGetCharacters)]
        public void TestPsCharacterList_StandAlone(string PSData, string ExpectedValue, string ExpectedChannel = "text")
        {
            lock (EventLock)
            {
                bool IsTested = false;
                Proxy.ProcessServerChannelData += (sender, Args) =>
                {
                    if (!IsTested && sender is ChannelObject ServeObject)
                    {
                        IsTested = true;

                        Assert.That(ServeObject.Player.Message.Trim(),
                            Is.EqualTo(ExpectedValue.Trim()),
                            $"Player.Message '{ServeObject.Player.Message}' ExpectedValue: {ExpectedValue}"
                            );
                        Assert.That(Args.Channel,
                            Is.EqualTo(ExpectedChannel),
                            $"Player.Message '{Args.Channel}' ExpectedValue: {ExpectedChannel}"
                            );
                    }
                };

                Proxy.ParseServerChannel(PSData, false);

                Proxy.ProcessServerChannelData -= (sender, Args) =>
                {
                    if (!IsTested && sender is ChannelObject ServeObject)
                    {
                        IsTested = true;

                        Assert.That(ServeObject.Player.Message.Trim(),
                            Is.EqualTo(ExpectedValue.Trim()),
                            $"Player.Message '{ServeObject.Player.Message}' ExpectedValue: {ExpectedValue}"
                            );
                        Assert.That(Args.Channel,
                            Is.EqualTo(ExpectedChannel),
                            $"Player.Message '{Args.Channel}' ExpectedValue: {ExpectedChannel}"
                            );
                    }
                };
            }
        }

        public void BotHasConnected_StandAlone()
        {
            Task.Run(() => Proxy.ConnetAsync()).Wait();

            Assert.Multiple(() =>
            {
                Assert.That(Proxy.ServerStatus,
                    Is.EqualTo(ConnectionPhase.Connected),
                    $"Proxy.ServerStatus {Proxy.ServerStatus}");
                Assert.That(Proxy.IsServerSocketConnected,
                    Is.EqualTo(true),
                    $"Proxy.IsServerSocketConnected {Proxy.IsServerSocketConnected}");
                if (Proxy.StandAlone)
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
            Proxy.Disconnect();
            if (!Proxy.StandAlone)
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

        [OneTimeTearDown]
        public void Cleanup()
        {
            DisconnectTests();

            Proxy.ClientData2 -= (data) => Proxy.SendToServer(data);
            Proxy.ServerData2 -= (data) => Proxy.SendToClient(data);
            Proxy.Error -= (e, o) => Logger.Error($"{e} {o}");

            Proxy.Dispose();
            options = null;
        }
    }
}