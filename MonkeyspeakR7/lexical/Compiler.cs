using Shared.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Monkeyspeak.lexical
{
    internal class Compiler
    {
        private Version version;

        public Compiler(MonkeyspeakEngine engine)
        {
            version = engine.Options.Version;
        }

        /// <summary>
        /// Compiler version number
        /// </summary>
        public Version Version
        {
            get { return version; }
        }

        private TriggerList ReadVersion6_5(BinaryReader reader)
        {
            var triggerList = new TriggerList();
            int triggerListCount = reader.ReadInt32();
            for (int i = 0; i <= triggerListCount - 1; i++)
            {
                int triggerCount = reader.ReadInt32();
                for (int j = 0; j <= triggerCount - 1; j++)
                {
                    var trigger = new Trigger
                    {
                        Category = (TriggerCategory)reader.ReadInt32(),
                        Id = reader.ReadInt32()
                    };

                    int triggerContentCount = reader.ReadInt32();
                    if (triggerContentCount > 0)
                        for (int k = triggerContentCount - 1; k >= 0; k--)
                        {
                            byte type = reader.ReadByte();
                            if (type == 1) // String
                            {
                                trigger.contents.Enqueue(reader.ReadString());
                            }

                            if (type == 2) // Double
                            {
                                trigger.contents.Enqueue(reader.ReadDouble());
                            }

                            // type == 3 reserved
                            // type == 4 reserved
                        }
                    Logger.Debug<Compiler>($"Decompiled {trigger}");
                    triggerList.Add(trigger);
                }
            }
            return triggerList;
        }

        private TriggerList ReadVersion1(BinaryReader reader)
        {
            var triggerList = new TriggerList();
            int triggerListCount = reader.ReadInt32();
            for (int i = 0; i <= triggerListCount - 1; i++)
            {
                int triggerCount = reader.ReadInt32();
                for (int j = 0; j <= triggerCount - 1; j++)
                {
                    var trigger = new Trigger
                    {
                        Category = (TriggerCategory)reader.ReadByte(),
                        Id = reader.ReadInt32()
                    };
                    string description = reader.ReadString(); // no longer used, here for compatibility.
                    int triggerContentCount = reader.ReadInt32();
                    if (triggerContentCount > 0)
                        for (int k = 0; k <= triggerContentCount - 1; k++)
                        {
                            if (reader.ReadBoolean())
                            {
                                byte type = reader.ReadByte();
                                if (type == 1) // String
                                {
                                    trigger.contents.Enqueue(reader.ReadString());
                                }

                                if (type == 2) // Double
                                {
                                    trigger.contents.Enqueue(reader.ReadDouble());
                                }
                            }
                        }
                    triggerList.Add(trigger);
                }
            }
            return triggerList;
        }

        public List<TriggerList> DecompileFromStream(Stream stream)
        {
            var triggerLists = new List<TriggerList>();
            using (var decompressed = new DeflateStream(stream, CompressionMode.Decompress))
            using (var reader = new BinaryReader(decompressed))
            {
                var fileVersion = new Version(reader.ReadInt32(), reader.ReadInt32()); // use for versioning comparison
                Logger.Debug<Compiler>($"Reading version {fileVersion}");
                switch (fileVersion.Major)
                {
                    case 1:
                        triggerLists.Add(ReadVersion1(reader));
                        break;

                    case 6:
                        triggerLists.Add(ReadVersion6_5(reader));
                        break;

                    default:
                        triggerLists.Add(ReadVersion6_5(reader));
                        break;
                }
            }
            return triggerLists;
        }

        public void CompileToStream(List<TriggerList> triggerBlocks, Stream stream)
        {
            using (var compressed = new DeflateStream(stream, CompressionMode.Compress))
            using (var writer = new BinaryWriter(compressed))
            {
                writer.Write(version.Major);
                writer.Write(version.Minor);

                writer.Write(triggerBlocks.Count);
                for (int i = 0; i <= triggerBlocks.Count - 1; i++)
                {
                    TriggerList triggerBlock = triggerBlocks[i];
                    writer.Write(triggerBlock.Count);
                    for (int j = 0; j <= triggerBlock.Count - 1; j++)
                    {
                        Trigger trigger = triggerBlock[j];
                        writer.Write((int)trigger.Category);
                        writer.Write(trigger.Id);

                        writer.Write(trigger.contents.Count);
                        var enumerator = trigger.contents.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            object content = enumerator.Current;
                            if (content is string)
                            {
                                writer.Write((byte)1);
                                writer.Write((string)content);
                            }
                            else if (content is double)
                            {
                                writer.Write((byte)2);
                                writer.Write((double)content);
                            }
                            else if (content is IDictionary<string, object>)
                                writer.Write((byte)3); // reserved
                            else writer.Write((byte)4); // reserved
                        }
                        Logger.Debug<Compiler>($"Compiled {trigger}");
                    }
                }
                writer.Flush();
            }
        }
    }
}