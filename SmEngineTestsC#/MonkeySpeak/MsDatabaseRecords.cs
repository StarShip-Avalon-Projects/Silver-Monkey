using Engine.BotSession;
using Furcadia.Logging;
using Furcadia.Net;
using MonkeyCore2.IO;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using static Libraries.MsLibHelper;
using static SmEngineTests.Utilities;

namespace SmEngineTests.MonkeySpeak
{
    [TestFixture]
    public class MsDatabaseRecords_Alt_AtlantisFacility
    {
        #region Private Fields

        private Bot Proxy;

        #endregion Private Fields

        #region Public Properties

        public string BackupSettingsFile { get; private set; }
        public BotOptions Options { get; private set; }
        public string SettingsFile { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void BotHasConnected_StandAlone(bool StandAlone = true)
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

        public void BotHaseDisconnected()
        {
            Proxy.DisconnectServerAndClientStreams();
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

        [TearDown]
        public void Cleanup()
        {
            Proxy.ClientData2 -= (data) => Proxy.SendToServer(data);
            Proxy.ServerData2 -= (data) => Proxy.SendToClient(data);
            Proxy.Error -= (e, o) => Logger.Error($"{e} {o}");

            Proxy.Dispose();
            Options = null;
        }

        [TestCase(true)]
        // [TestCase(false)]
        public void ConstanVariablesAreSet(bool StandAlone)
        {
            BotHasConnected_StandAlone(StandAlone);
            if (!Proxy.StandAlone)
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
            DisconnectTests();
        }

        public void DisconnectTests()
        {
            Proxy.DisconnectServerAndClientStreams();
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

        [TestCase(true)]
        //  [TestCase(false)]
        public void DreamInfoIsSet_StandAlone(bool StandAlone)
        {
            BotHasConnected_StandAlone(StandAlone);
            if (!Proxy.StandAlone)
                HaltFor(DreamEntranceDelay);

            Assert.Multiple(() =>
            {
                Assert.That(Proxy.InDream,
                    Is.EqualTo(true),
                    "Bot has not joined a dream");
                Assert.That(Proxy.Dream.Rating,
                    !Is.EqualTo(null),
                    $"Dream Rating is '{Proxy.Dream.Rating}'");
                Assert.That(Proxy.Dream.Name,
                    !Is.EqualTo(null),
                    $"Dream Name is '{Proxy.Dream.Name}'");
                if (Proxy.Dream.IsPermanent)
                {
                    Assert.That(Proxy.Dream.DreamOwner,
                        !Is.EqualTo(null),
                        $"Dream DreamOwner is '{Proxy.Dream.DreamOwner}'");
                    var Var = Proxy.MSpage.GetVariable(DreamOwnerVariable);
                    Assert.That(Var.Value,
                        !Is.EqualTo(null),
                        $"Constant Variable: '{Var}' ");
                }
                else
                {
                    Assert.That(Proxy.Dream.DreamOwner,
                        Is.EqualTo(null),
                        $"Dream DreamOwner is '{Proxy.Dream.DreamOwner}'");
                    //private dreams most likley to be personal or ddream packs
                    // Dream Owner shoule be set
                    var Var = Proxy.MSpage.GetVariable(DreamOwnerVariable);
                    Assert.That(Var.Value,
                        Is.EqualTo(null),
                        $"Constant Variable: '{Var}' ");
                }
                Assert.That(Proxy.BotController,
                    !Is.EqualTo(null),
                    $"BotController is '{Proxy.BotController}'");
                Assert.That(Proxy.Dream.URL,
                    !Is.EqualTo(null),
                    $"Dream URL is '{Proxy.Dream.URL}'");
                //Assert.That(Proxy.Dream.Lines,
                //    Is.GreaterThan(0),
                //    $"DragonSpeak Lines {Proxy.Dream.Lines}");
                Assert.That(string.IsNullOrWhiteSpace(Proxy.BanishName),
                    $"BanishName is '{Proxy.BanishName}'");
                Assert.That(Proxy.BanishList,
                    !Is.EqualTo(null),
                    $"BanishList is '{Proxy.BanishList}'");
                Assert.That(Proxy.BanishList.Count,
                    Is.EqualTo(0),
                    $"BanishList is '{Proxy.BanishList.Count}'");
            });
            DisconnectTests();
        }

        [SetUp]
        public void Initialize()
        {
            var BotFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Silver Monkey.bini");
            var MsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "MsDatabaseRecords.ms");
            var CharacterFile = Path.Combine(Paths.FurcadiaCharactersFolder,
                "atlantisfacility.ini");
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
            Proxy.Error += (e, o) => Logger.Error($"{e} {o}");
        }

        #endregion Public Methods
    }
}