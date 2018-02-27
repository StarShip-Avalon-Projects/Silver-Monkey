using Libraries;
using Monkeyspeak;
using IO;
using System.Media;
using System;

namespace Libraries
{
    /// <summary>
    /// Simple way to play wave files
    /// <para/>
    /// Default wave folder: <see cref="Paths.SilverMonkeyBotPath"/>
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class MsSound : MonkeySpeakLibrary, IDisposable
    {
        private bool disposedValue;

        private SoundPlayer simpleSound;

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId
        {
            get
            {
                return 2010;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //  Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            this.Dispose(true);
        }

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of vars to use to pass runtime vars to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            Add(TriggerCategory.Effect,
                r =>
                {
                    var SoundFile = Paths.CheckBotFolder(r.ReadString());
                    using (var PlaySound = new SoundPlayer(SoundFile))
                    {
                        PlaySound.Play();
                    }
                    return true;
                }, "play the wave file {...}.");

            Add(TriggerCategory.Effect,
                r =>
                {
                    if (simpleSound != null)
                    {
                        var SoundFile = Paths.CheckBotFolder(r.ReadString());
                        simpleSound = new SoundPlayer(SoundFile);
                        simpleSound.PlayLooping();
                    }
                    return true;
                },
                "play the wave file {...} in a loop. if theres not one playing");

            Add(TriggerCategory.Effect,
                StopSound,
                "stop playing the sound file.");
        }

        private bool StopSound(TriggerReader reader)
        {
            if (simpleSound != null)
            {
                var SoundFile = IO.Paths.CheckBotFolder(reader.ReadString(true));
                //  simpleSound = New SoundPlayer(SoundFile)
                simpleSound.Stop();
                simpleSound.Dispose();
            }

            return true;
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (simpleSound != null)
                    {
                        simpleSound.Dispose();
                        simpleSound = null;
                    }
                }
            }

            disposedValue = true;
        }
    }
}