using Engine.BotSession;
using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using IO;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using static SmEngineTests.Utilities;
using FurcLog = Furcadia.Logging;
using MsLog = Monkeyspeak.Logging;

namespace SmEngineTests.BotScriptTests
{
    [TestFixture]
    public class TurretTests_Alt_SilverMonkey
    {
        // (0:34) When the bot enters a Dream,
        //    (1:35) and the Dream Name is not {The Shoddy Ribbon: Murder Mystery Hotel(WIP)},
        //    (1:35) and the Dream Name is not {Furrabian Nights},
        //       (5:10) say {THIS IS NOT MY HOME! YOU'RE NOT MY MINTY!}.
        //       (5:10) say {`fdl furc://theshoddyribbon}.

        #region Private Fields

        private static Bot Proxy;

        #endregion Private Fields

        #region Public Properties

        public BotOptions Options { get; private set; }

        #endregion Public Properties

        [OneTimeSetUp]
        public void Initialize()
        {
            MsLog.Logger.InfoEnabled = true;
            MsLog.Logger.SuppressSpam = true;
            MsLog.Logger.ErrorEnabled = true;
            MsLog.Logger.WarningEnabled = true;
            MsLog.Logger.SingleThreaded = true;
            MsLog.Logger.LogOutput = new MsLog.MultiLogOutput(new MsLog.FileLogOutput(Paths.SilverMonkeyErrorLogPath, MsLog.Level.Debug), new MsLog.FileLogOutput(Path.Combine(Paths.SilverMonkeyErrorLogPath, "TurretTests.log"), MsLog.Level.Warning), new MsLog.FileLogOutput(Path.Combine(Paths.SilverMonkeyErrorLogPath), MsLog.Level.Debug), new MsLog.FileLogOutput(Path.Combine(Paths.SilverMonkeyErrorLogPath), MsLog.Level.Error));

            FurcLog.Logger.InfoEnabled = true;
            FurcLog.Logger.SuppressSpam = true;
            FurcLog.Logger.ErrorEnabled = true;
            FurcLog.Logger.WarningEnabled = true;
            FurcLog.Logger.SingleThreaded = true;
            FurcLog.Logger.LogOutput = new FurcLog.MultiLogOutput(new FurcLog.FileLogOutput(IO.Paths.SilverMonkeyErrorLogPath, FurcLog.Level.Debug), new FurcLog.FileLogOutput(Paths.SilverMonkeyErrorLogPath, FurcLog.Level.Error));

            var BotFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Silver Monkey.bini");
            var MsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Turret Tests.ms");
            var CharacterFile = Path.Combine(Paths.FurcadiaCharactersFolder,
                "silvermonkey.ini");
            var MsEngineOption = new EngineOptoons()
            {
                MonkeySpeakScriptFile = MsFile,
                IsEnabled = true,
                BotController = @"Gerolkae"
            };
            Options = new BotOptions(BotFile)
            {
                Standalone = true,
                CharacterIniFile = CharacterFile,
                MonkeySpeakEngineOptions = MsEngineOption,
                ResetSettingTime = 10
            };

            Options.SaveBotSettings();

            Proxy = new Bot(Options);
            Proxy.Error += (e, o) => MsLog.Logger.Error($"{e} {o}");

            BotHasConnected_StandAlone(Proxy.StandAlone);
        }

        [TestCase("furc://furrabiannights/", "The Shoddy Ribbon", "Murder Mystery Hotel (WIP)")]
        [TestCase("furc://theshoddyribbon:murdermysteryhotelwip/", "The Shoddy Ribbon", "Murder Mystery Hotel (WIP)")]
        [TestCase("furc://silvermonkey:stargatebase/", "The Shoddy Ribbon", "Murder Mystery Hotel (WIP)")]
        [TestCase("furc://imaginarium/", "The Shoddy Ribbon", "Murder Mystery Hotel (WIP)")]
        [TestCase("furc://vinca/", "vinca")]
        [Author("Gerolkae")]
        public void DreamBookmarkSettingsTest(string DreamUrl, string DreamOwner, string DreamTitle = null)
        {
            Proxy.SendFormattedTextToServer($"`fdl {DreamUrl}");
            HaltFor(DreamEntranceDelay);

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

            HaltFor(1);
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

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (Proxy == null)
                return;
            BotHaseDisconnected();
            Proxy.ClientData2 -= (data) => Proxy.SendToServer(data);
            Proxy.ServerData2 -= (data) => Proxy.SendToClient(data);
            Proxy.Error -= (e, o) =>
            FurcLog.Logger.Error<TurretTests_Alt_SilverMonkey>($"{e} {o}");
            Proxy.MSpage.Error -= (page, handler, trigger, ex) =>
                FurcLog.Logger.Error($"{page} {handler}  {trigger}  {ex}");
            Proxy.Dispose();
            Options = null;
        }

        public void BotHasConnected_StandAlone(bool StandAlone)
        {
            Task.Run(() => Proxy.ConnetAsync()).Wait();
            Proxy.MSpage.Error += (page, handler, trigger, ex) =>
                MsLog.Logger.Error($"{page} {handler}  {trigger}  {ex}");
            MsLog.Logger.LogOutput = new MsLog.MultiLogOutput(new MsLog.FileLogOutput(Path.Combine(Paths.SilverMonkeyErrorLogPath, "TurretTests.log"), MsLog.Level.Debug), new MsLog.FileLogOutput(Paths.SilverMonkeyErrorLogPath, MsLog.Level.Error));
            FurcLog.Logger.LogOutput = new FurcLog.MultiLogOutput(new FurcLog.FileLogOutput(IO.Paths.SilverMonkeyErrorLogPath, FurcLog.Level.Debug), new FurcLog.FileLogOutput(IO.Paths.SilverMonkeyErrorLogPath, FurcLog.Level.Error));
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

        public void BotHaseDisconnected()
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
    }
}