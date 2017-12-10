using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;
using NUnit.Framework;
using SilverMonkeyEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MonkeyCore;

namespace SmEngineTests
{
    [TestFixture]
    public class SilerMonkeyEngineTests
    {
        private BotOptions options;
        private BotSession Proxy;
        private const int ConnectWaitTime = 15;
        private readonly string originalHash;
        private readonly string SettingsFile;

        private const string BaseHash = "8f785f3c2c76a1039338cbd269e27c15861d3";

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
            SettingsFile = Path.Combine(Paths.ApplicationSettingsPath, @"settings.ini");
            originalHash = GetFileHash(SettingsFile);
        }

        [SetUp]
        public void Initialize()
        {
            if (originalHash != BaseHash)
                throw new NUnit.Framework.AssertionException("Unexpected Hash Difference");
            if (Proxy != null)
                Furcadia.Logging.Logger.Error<BotSession>(Proxy);

            var BotFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Silver Monkey.bini");
            var MsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Bugreport 165 From Jake.ms");
            var CharacterFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "silvermonkey.ini");

            options = new BotOptions(ref BotFile)
            {
                Standalone = true,
                CharacterIniFile = CharacterFile
            };

            options.MonkeySpeakEngineOptions.MS_File = MsFile;
            options.MonkeySpeakEngineOptions.MS_Engine_Enable = true;
            options.SaveBotSettings();

            Proxy = new BotSession(options);
            // Proxy.Error += OnErrorException;
            Proxy.ServerData2 += OnServerData;
            Proxy.ClientData2 += OnClientData;
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
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ConnectWaitTime);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

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
            Task.Run(() => Proxy.ConnetAsync()).Wait();
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ConnectWaitTime);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
            {
                var ServeObject = (ChannelObject)sender;
                Assert.That(ServeObject.Player.ShortName, Is.EqualTo(ExpectedValue.ToFurcadiaShortName()));
            };

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Proxy.ParseServerChannel(testc, false);
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
        public void ExpectedChannelNameIs(string testc, string ExpectedValue)
        {
            Task.Run(() => Proxy.ConnetAsync()).Wait();
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ConnectWaitTime);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
            {
                var ServeObject = (ChannelObject)sender;
                Assert.That(Args.Channel, Is.EqualTo(ExpectedValue));
            };

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Proxy.ParseServerChannel(testc, false);
        }

        [Test]
        public void BotHasConnectedTest()
        {
            Task.Run(() => Proxy.ConnetAsync()).Wait();
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ConnectWaitTime);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }
            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Assert.That(Proxy.ServerStatus == ConnectionPhase.Connected);
        }

        [Test]
        public void ServerSocketIsConneted()
        {
            Task.Run(() => Proxy.ConnetAsync()).Wait();
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ConnectWaitTime);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }
            Assert.That(Proxy.IsServerSocketConnected);
        }

        [Test]
        public void FurcadiaClientIsNotConnected()
        {
            Task.Run(() => Proxy.ConnetAsync()).Wait();
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ConnectWaitTime);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Console.WriteLine($"Proxy.IsClientSocketConnected: {Proxy.IsClientSocketConnected}");
            Assert.That(Proxy.ServerStatus == ConnectionPhase.Connected && Proxy.IsClientSocketConnected == false);
        }

        [Test]
        public void ClientStatusIsDisconnected()
        {
            Task.Run(() => Proxy.ConnetAsync()).Wait();
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ConnectWaitTime);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Console.WriteLine($"Proxy.IsClientSocketConnected: {Proxy.IsClientSocketConnected}");
            Assert.That(Proxy.ClientStatus == ConnectionPhase.Disconnected && Proxy.IsClientSocketConnected == false);
        }

        [TestCase(GeroShout, "ping")]
        public void ProxySession_InstructionObjectPlayerIs(string testc, string ExpectedValue)
        {
            Task.Run(() => Proxy.ConnetAsync()).Wait();
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ConnectWaitTime);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

            //  Proxy.Error += OnErrorException;
            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
            {
                ChannelObject InstructionObject = (ChannelObject)sender;
                Assert.That(InstructionObject.Player.Message, Is.EqualTo(ExpectedValue));
            };
            Proxy.SendFormattedTextToServer("- Shout");
            end = DateTime.Now + TimeSpan.FromSeconds(4);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }
            Proxy.ParseServerChannel(testc, false);
            end = DateTime.Now + TimeSpan.FromSeconds(5);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }
        }

        private void OnErrorException(Exception e, object o, string text)
        {
            Console.WriteLine($"{e} {text}");
        }

        private void OnServerData(string data)
        {
            Proxy.SendToClient(data);
        }

        private void OnClientData(string data)
        {
            Proxy.SendToServer(data);
        }

        public string GetFileHash(string filename)
        {
            var hash = new SHA1Managed();
            var clearBytes = File.ReadAllBytes(filename);
            var hashedBytes = hash.ComputeHash(clearBytes);
            return ConvertBytesToHex(hashedBytes);
        }

        public string ConvertBytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x"));
            }
            return sb.ToString();
        }

        [TearDown]
        public void Cleanup()
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(ConnectWaitTime);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }
            // Incase We're not standalone, Kill the left over Client;
            if (Proxy.FurcadiaClientIsRunning)
                Task.Run(() => Proxy.CloseClient()).Wait();
            Proxy.Disconnect();
            Proxy.Dispose();
            options = null;
            var ResetHash = GetFileHash(SettingsFile);
            if (ResetHash != originalHash)
                throw new NUnit.Framework.AssertionException("Faulty settings.ini");
        }
    }
}