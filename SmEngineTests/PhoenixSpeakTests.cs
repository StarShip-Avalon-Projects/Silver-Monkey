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
using Furcadia.Logging;

namespace SmEngineTests
{
    [TestFixture]
    internal class PhoenixSpeakTests
    {
        private BotSession Proxy;
        private const string PsGetCharacters = "PS Ok: get: result: b='<array>', bloodofaraven666='<array>', bloodstar='<array>', CJ='<array>', cjkilman='<array>', g='<array>', gerolkae='<array>', h='<array>', queenchrysalis='<array>', r='<array>', s='<array>', silvermonkey='<array>', w='<array>'";
        private const string PsGetCharacterGerolkae = "PS Ok: get: result: sys_lastused_date='2/9/2013 4:31:39 AM', testvar1=1, testvar2='Test2'";
        private const string PsGetCharacterJohn = "PS Error: get: Query error: (-1) Unexpected character '^' at column 17";
        private const string PsGetCharacterJohn2 = "PS Error: get: Query error: Field 'john' does not exist";

        [SetUp]
        public void Initialize()
        {
            Monkeyspeak.Logging.Logger.ErrorEnabled = true;
            Monkeyspeak.Logging.Logger.DebugEnabled = true;
            Monkeyspeak.Logging.Logger.InfoEnabled = true;
            Monkeyspeak.Logging.Logger.SingleThreaded = false;
            Furcadia.Logging.Logger.ErrorEnabled = true;
            Furcadia.Logging.Logger.DebugEnabled = true;
            Furcadia.Logging.Logger.InfoEnabled = true;
            Furcadia.Logging.Logger.SingleThreaded = false;
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

        //[Test]
        //public void TestPsCharacterList()
        //{
        //    DateTime end = DateTime.Now + TimeSpan.FromSeconds(10);
        //    while (true)
        //    {
        //        Thread.Sleep(100);
        //        if (end < DateTime.Now) break;
        //    }

        //    Proxy.ProcessServerChannelData += delegate (object sender, ParseChannelArgs Args)
        //    {
        //        var ServeObject = (ChannelObject)sender;
        //        Assert.That(ServeObject.Player.Message, Is.EqualTo(""));
        //    };

        //    Logger.Debug($"ServerStatus: {Proxy.ServerStatus}");
        //    Logger.Debug($"ClientStatus: {Proxy.ClientStatus}");
        //    Proxy.ParseServerChannel(PsGetCharacters, false);
        //}

        private void OnErrorException(Exception e, object o, string text)
        {
            Logger.Error($"{e} {text}");
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
            DateTime end = DateTime.Now + TimeSpan.FromSeconds(10);
            while (true)
            {
                Thread.Sleep(100);
                if (end < DateTime.Now) break;
            }
            Proxy.Disconnect();
            Proxy.Dispose();
        }
    }
}