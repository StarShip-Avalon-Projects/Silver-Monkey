using NUnit.Framework;
using SilverMonkeyEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmEngineTests
{
    [TestFixture]
    public class SilerMonkeyEngineTests
    {
        private BotSession Proxy;

        private void ProxySessionInitialize()
        {
            var BotFile = "";
            BotOptions options = new BotOptions(ref BotFile);
            options.MonkeySpeakEngineOptions.MS_File = "";

            Proxy = new BotSession(ref options);
            Proxy.Connect();
        }

        [Test]
        public void TestMethod()
        {
            if (Proxy.ServerStatus != Furcadia.Net.ConnectionPhase.Connected)
            {
                ProxySessionInitialize();
            }
        }
    }
}