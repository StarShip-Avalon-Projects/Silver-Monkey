using NUnit.Framework;
using System;
using BotSession;
using Furcadia.Logging;
using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MsLibHelper;
using static SmEngineTests.Utilities;
using System.IO;

namespace SmEngineTests
{
    [TestFixture]
    public class QueryTests
    {
        public const string GeroJoinBot = "<font color='query'><name shortname='gerolkae'>Gerolkae</name> requests permission to join your company. To accept the request, <a href='command://summon'>click here</a> or type `summon and press &lt;enter&gt;.</font>";

        private Bot Proxy;

        public string SettingsFile { get; private set; }
        public string BackupSettingsFile { get; private set; }
        public BotOptions options { get; private set; }

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
                MonkeySpeakEngineOptions = MsEngineOption
            };

            options.SaveBotSettings();

            Proxy = new Bot(options);
            Proxy.Error += (e, o) => Logger.Error($"{e} {o}");
        }

        [TestCase(GeroJoinBot, "Gerolkae")]
        public void ExpectedQueryCharacter(string testc, string ExpectedValue)
        {
            BotHasConnected_StandAlone();

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
                Assert.That(ServeObject.Player.ShortName,
                    Is.EqualTo(ExpectedValue.ToFurcadiaShortName()));
            };
            BotHaseDisconnected_Standalone();
        }

        [TestCase(GeroJoinBot, "join")]
        public void ChannelIsQueryOfType(string ChannelCode, string ExpectedValue)
        {
            BotHasConnected_StandAlone();
            HaltFor(DreamEntranceDelay);

            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
            {
                Assert.Multiple(() =>
                {
                    var ServeObject = (ChannelObject)sender;
                    Assert.That(Args.Channel,
                        Is.EqualTo("guery"));
                    Assert.That(Args.Channel,
                        Is.EqualTo(ExpectedValue));
                });
            };

            Proxy.ParseServerChannel(ChannelCode, false);
            BotHaseDisconnected_Standalone();
        }

        // TODO: Need MonkeySpeak Script
        public void QueryMonkeySpeakVariablesAreSet()
        {
            BotHasConnected_StandAlone();
            HaltFor(DreamEntranceDelay);

            Assert.Multiple(() =>
            {
                var Var = Proxy.MSpage.GetVariable(TriggeringFurreNameVariable);
                Assert.That(Var.Value,
                    !Is.EqualTo(null),
                    $"Constant Variable: '{Var}' ");
                Assert.That(Var.IsConstant,
                    Is.EqualTo(true),
                    $"Constant Variable: '{Var}' ");
                Assert.That(Var.Value.ToString(),
                    Is.EqualTo("Gerolkae"),
                    $"Constant Variable: '{Var}' ");

                Var = Proxy.MSpage.GetVariable(TriggeringFurreShortNameVariable);
                Assert.That(Var.Value,
                    !Is.EqualTo(null),
                    $"Constant Variable: '{Var}' ");
                Assert.That(Var.IsConstant,
                    Is.EqualTo(true),
                    $"Constant Variable: '{Var}' ");
                Assert.That(Var.Value.ToString(),
                    Is.EqualTo("gerolkae"),
                    $"Constant Variable: '{Var}' ");
            });
            BotHaseDisconnected_Standalone();
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

        public void BotHaseDisconnected_Standalone(bool StandAlone = false)
        {
            Proxy.Disconnect();
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