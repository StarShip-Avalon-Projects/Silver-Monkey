using NUnit.Framework;
using SilverMonkeyEngine;
using Furcadia;
using Monkeyspeak;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Furcadia.Net;
using Furcadia.Net.Utils.ServerParser;

namespace SmEngineTests
{
    [TestFixture]
    public class SilerMonkeyEngineTests
    {
        private static BotSession Proxy;

        private const string PingTest = @"<name shortname='gerolkae'>Gerolkae</name>: ping";
        private const string WhisperTest = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"hi\" to you. ]</font>";
        private const string PingTest2 = @"<name shortname='gerolkae'>Gerolkae</name>: Ping";
        private const string WhisperTest2 = "<font color='whisper'>[ <name shortname='gerolkae' src='whisper-from'>Gerolkae</name> whispers, \"Hi\" to you. ]</font>";
        private const string YouWhisper = "<font color='whisper'>[You whisper \"Logged on\" to<name shortname='gerolkae' forced src='whisper-to'>Gerolkae</name>. ]</font>";
        private const string YouWhisper2 = "<font color='whisper'>[ You whisper \"Logged on\" to <name shortname='gerolkae' forced src='whisper-to'>Gerolkae</name>. ]</font>";
        private const string YouShouYo = "<font color='shout'>You shout, \"Yo Its Me\"</font>";
        private const string GeroShout = "<font color='shout'>{S} <name shortname='gerolkae'>Gerolkae</name> shouts: ping</font>";
        private const string Emote = "<font color='emote'><name shortname='silvermonkey'>Silver|Monkey</name> Emoe</font>";

        private const string Emit = "<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Silver|Monkey has arrived...</font>";
        private const string SpokenEmit = "<font color='emit'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Blah</font>";
        private const string EmitWarning = "<font color='warning'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> (<name shortname='silvermonkey'>Silver|Monkey</name> just emitted.)</font>";

        private const string CookieBank = "<font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>";

        [SetUp]
        public void Initialize()
        {
            Monkeyspeak.Logging.Logger.ErrorEnabled = true;
            Monkeyspeak.Logging.Logger.DebugEnabled = true;
            Monkeyspeak.Logging.Logger.InfoEnabled = true;
            Monkeyspeak.Logging.Logger.SingleThreaded = true;
            Furcadia.Logging.Logger.ErrorEnabled = true;
            Furcadia.Logging.Logger.DebugEnabled = true;
            Furcadia.Logging.Logger.InfoEnabled = true;
            Furcadia.Logging.Logger.SingleThreaded = true;
            var BotFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Silver Monkey.bini");
            var MsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Bugreport 165 From Jake.ms");
            var CharacterFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "silvermonkey.ini");

            BotOptions options = new BotOptions(ref BotFile)
            {
                Standalone = true,
                CharacterIniFile = CharacterFile
            };

            options.MonkeySpeakEngineOptions.MS_File = MsFile;
            options.MonkeySpeakEngineOptions.MS_Engine_Enable = true;
            options.SaveBotSettings();

            Proxy = new BotSession(options);
            Proxy.Error += OnErrorException;
            Proxy.ServerData2 += OnServerData;
            Proxy.ClientData2 += OnClientData;
            Task.Run(() => Proxy.ConnetAsync());
        }

        [TestCase(WhisperTest, "hi")]
        [TestCase(PingTest, "ping")]
        [TestCase(WhisperTest2, "Hi")]
        [TestCase(PingTest2, "Ping")]
        [TestCase(YouWhisper, "Logged on")]
        [TestCase(YouShouYo, "Yo Its Me")]
        [TestCase(GeroShout, "ping")]
        [TestCase(EmitWarning, "(<name shortname='silvermonkey'>Silver|Monkey</name> just emitted.)")]
        [TestCase(Emit, "Silver|Monkey has arrived...")]
        [TestCase(SpokenEmit, "Blah")]
        [TestCase(Emote, "Emoe")]
        public void ChannelTextIs(string testc, string ExpectedValue)
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(10);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
             {
                 var ServeObject = (ChannelObject)sender;
                 Assert.That(Proxy.ServerStatus == ConnectionPhase.Connected && ExpectedValue == ServeObject.Player.Message);
             };

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Proxy.ParseServerChannel(testc, false);
        }

        [TestCase(WhisperTest, "Gerolkae")]
        [TestCase(PingTest, "Gerolkae")]
        [TestCase(YouWhisper, "Silver Monkey")]
        [TestCase(YouWhisper2, "Silver Monkey")]
        [TestCase(GeroShout, "Gerolkae")]
        [TestCase(YouShouYo, "Silver Monkey")]
        [TestCase(EmitWarning, "Silver Monkey")]
        [TestCase(Emit, "Unknown")]
        [TestCase(SpokenEmit, "Unknown")]
        [TestCase(Emote, "Silver monkey")]
        public void ExpectedCharachter(string testc, string ExpectedValue)
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(10);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
            {
                var ServeObject = (ChannelObject)sender;
                Assert.That(Proxy.ServerStatus == ConnectionPhase.Connected && ExpectedValue.ToFurcadiaShortName() == ServeObject.Player.ShortName);
            };

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Proxy.ParseServerChannel(testc, false);
        }

        [TestCase(YouWhisper, "whisper")]
        [TestCase(YouWhisper2, "whisper")]
        [TestCase(WhisperTest, "whisper")]
        [TestCase(PingTest, "say")]
        [TestCase(YouShouYo, "shout")]
        [TestCase(GeroShout, "shout")]
        [TestCase(EmitWarning, "@emit")]
        [TestCase(Emit, "@emit")]
        [TestCase(SpokenEmit, "@emit")]
        [TestCase(Emote, "emote")]
        public void ExpectedChannelNameIs(string testc, string ExpectedValue)
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(10);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

            Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
            {
                var ServeObject = (ChannelObject)sender;
                Assert.That(Proxy.ServerStatus == ConnectionPhase.Connected && ExpectedValue == ServeObject.Channel);
            };

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Proxy.ParseServerChannel(testc, false);
        }

        [Test]
        public void BotHasConnectedTest()
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(10);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }
            Assert.That(Proxy.ServerStatus == ConnectionPhase.Connected);
        }

        [Test]
        public void BotServerSocketTest()
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(10);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }
            Assert.That(Proxy.IsServerSocketConnected);
        }

        [Test]
        public void BotTestClientIsNotConnected()
        {
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(10);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }

            Console.WriteLine($"ServerStatus: {Proxy.ServerStatus}");
            Console.WriteLine($"ClientStatus: {Proxy.ClientStatus}");
            Assert.That(Proxy.ServerStatus == ConnectionPhase.Connected && Proxy.IsClientSocketConnected == false);
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

        [TearDown]
        public void Cleanup()
        {
            Proxy.Disconnect();
            Proxy.Dispose();
        }
    }
}