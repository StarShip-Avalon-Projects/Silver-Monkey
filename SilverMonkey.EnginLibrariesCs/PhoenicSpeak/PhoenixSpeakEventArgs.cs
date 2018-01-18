namespace Engine.Libraries.PhoenixSpeak
{
    public enum PsSystemRunning
    {
        Error = -1,

        PsNone,

        PsBackup,

        PsRestore,

        PsPrune,
    }

    // '' <summary>
    // '' Phoenix mSpeak Even Arguments
    // '' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix Speak</see>
    // '' </summary>
    public class PhoenixSpeakEventArgs : Furcadia.Net.NetChannelEventArgs
    {
        public PhoenixSpeakEventArgs()
        {
            Channel = "PhoenixSpeak";
        }

        // '' <summary>
        // '' Do we have too much Phoienix-Speak Data then the Server can send
        // '' to us?
        // '' </summary>
        public bool PageOverFlow;

        // '' <summary>
        // '' PhoenixSpeak id for cerver/client instructions
        // '' </summary>
        // '' <returns>
        // '' </returns>
        public short id;
    }
}