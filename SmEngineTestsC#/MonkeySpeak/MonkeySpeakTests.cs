using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monkeyspeak;
using Monkeyspeak.Logging;
using NUnit.Framework;

namespace SmEngineTests.MonkeySpeak
{
    [TestFixture]
    internal class MonkeySpeakTests
    {
        private string ShoutScript = @"
(0:6) When someone says {Activate module},
 (1:5) and the triggering furre's name is {Vito Blackthornio},
    (5:100) set variable %mod to {1}.
    (5:0) say {Module Activated, Thank you Master!!!!}.

(0:6) When someone says {deactivate module},
 (1:5) and the triggering furre's name is {Vito Blackthornio},
    (5:100) set variable %mod to {0}.
    (5:0) say {Module Deactivated, You sadden me, Master!!!!!}.

(0:90) When the bot enters a Dream,
(0:1) When the bot logs into furcadia,
(5:6) whisper {Bot active in dream %DREAMNAME} to {%BOTCONTROLLER}.

(0:10) When someone shouts something with {fuck} in it,
(5:5) whisper {Please do not swear in shouts! Thank you #SA} to the triggering furre.
";

        [Test]
        public void LexerPrint()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(ShoutScript)))
            using (Lexer lexer = new Lexer(new MonkeyspeakEngine(), new SStreamReader(stream)))
            {
                foreach (var token in lexer.Read())
                {
                    if (token.Type != TokenType.COMMENT)
                        Logger.Info($"{token} = {new string(lexer.Read(token.ValueStartPosition, token.Length))}");
                }
            }
        }
    }
}