using Assets.Data.WoW_Format_Parsers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Data.WoW_Format_Parsers.ADT
{
    public class ADTObj
    {
        public void ReadMVER(BinaryReader reader)
        {
            reader.BaseStream.Position += 4;
        }

        // List of filenames for M2 models that appear in this map tile. //
        public void ReadMMDX(BinaryReader reader, int MMDXsize)
        {
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MMDXsize)
            {
                int position    = (int)(reader.BaseStream.Position - currentPos);
                string path     = reader.ReadNullTerminatedString();
                if (path != "")
                {
                    ADTObjData.modelBlockData.M2Paths.Add(position, path);
                }
            }
        }

        // List of offsets of model filenames in the MMDX chunk. //
        public void ReadMMID(BinaryReader reader, int MMIDsize)
        {
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MMIDsize)
            {
                ADTObjData.modelBlockData.M2Offsets.Add(reader.ReadInt32());
            }
        }

        // List of filenames for WMOs (world map objects) that appear in this map tile. //
        public void ReadMWMO(BinaryReader reader, int MWMOsize)
        {
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MWMOsize)
            {
                int position    = (int)(reader.BaseStream.Position - currentPos);
                string path     = reader.ReadNullTerminatedString();
                if (path != "")
                {
                    ADTObjData.modelBlockData.WMOPaths.Add(position, path);
                }
            }
        }

        // List of offsets of WMO filenames in the MWMO chunk. //
        public void ReadMWID(BinaryReader reader, int MWIDsize)
        {
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MWIDsize)
            {
                ADTObjData.modelBlockData.WMOOffsets.Add(reader.ReadInt32());
            }
        }

        // Placement information for doodads (M2 models). //
        // Additional to this, the models to render are referenced in each MCRF chunk. //
        public void ReadMDDF(BinaryReader reader, int MDDFsize)
        {
            Flags f = new Flags();
            ADTObjData.modelBlockData.M2Info = new List<ADTObjData.M2PlacementInfo>();
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MDDFsize)
            {
                ADTObjData.M2PlacementInfo data = new ADTObjData.M2PlacementInfo();

                // References an entry in the MMID chunk, specifying the model to use.
                data.nameID     = reader.ReadInt32();

                // This ID should be unique for all ADTs currently loaded.
                // Best, they are unique for the whole map. Blizzard has these unique for the whole game.
                data.uniqueID   = reader.ReadInt32();

                // This is relative to a corner of the map. Subtract 17066 from the non vertical values and you should start to see 
                // something that makes sense. You'll then likely have to negate one of the non vertical values in whatever coordinate 
                // system you're using to finally move it into place.
                float Y = (reader.ReadSingle() - 17066) * -1 / Settings.worldScale; //-- pos X
                float Z = reader.ReadSingle() / Settings.worldScale; //-- Height
                float X = (reader.ReadSingle() - 17066) * -1 / Settings.worldScale; //-- pos Z
                data.position = new Vector3(X, Z, Y);

                // degrees. This is not the same coordinate system orientation like the ADT itself! (see history.)
                float rotX = reader.ReadSingle();       //-- rot X
                float rotZ = 180 - reader.ReadSingle(); //-- rot Y
                float rotY = reader.ReadSingle();       //-- rot Z
                data.rotation = Quaternion.Euler(new Vector3(rotX, rotZ, rotY));

                // 1024 is the default size equaling 1.0f.
                data.scale = reader.ReadUInt16() / 1024.0f;

                // values from struct MDDFFlags.
                data.flags = f.ReadMDDFFlags(reader);

                ADTObjData.modelBlockData.M2Info.Add(data);
            }
        }

        // Placement information for WMOs. //
        // Additional to this, the WMOs to render are referenced in each MCRF chunk. (?) //
        public void ReadMODF(BinaryReader reader, int MODFsize)
        {
            Flags f = new Flags();
            ADTObjData.modelBlockData.WMOInfo = new List<ADTObjData.WMOPlacementInfo>();
            long currentPos = reader.BaseStream.Position;

            while (reader.BaseStream.Position < currentPos + MODFsize)
            {
                ADTObjData.WMOPlacementInfo data = new ADTObjData.WMOPlacementInfo();
                data.nameID     = reader.ReadInt32();                                   // references an entry in the MWID chunk, specifying the model to use.
                data.uniqueID   = reader.ReadInt32();                                   // this ID should be unique for all ADTs currently loaded. Best, they are unique for the whole map.

                // same as in MDDF.
                float Y = (reader.ReadSingle() - 17066) * -1 / Settings.worldScale;   //-- pos X
                float Z = reader.ReadSingle() / Settings.worldScale;                  //-- Height
                float X = (reader.ReadSingle() - 17066) * -1 / Settings.worldScale;   //-- pos Z
                data.position = new Vector3(X, Z, Y);

                // same as in MDDF.
                float rotX = reader.ReadSingle();                                       //-- rot X
                float rotZ = 180 - reader.ReadSingle();                                 //-- rot Y
                float rotY = reader.ReadSingle();                                       //-- rot Z
                data.rotation   = Quaternion.Euler(new Vector3(rotX, rotZ, rotY));
                data.extents    = reader.ReadBoundingBoxes();                           // position plus the transformed wmo bounding box. used for defining if they are rendered as well as collision.
                data.flags      = f.ReadMODFFlags(reader);                              // values from enum MODFFlags.
                data.doodadSet  = reader.ReadUInt16();                                  // which WMO doodad set is used.

                // which WMO name set is used. Used for renaming goldshire inn to northshire inn while using the same model.
                data.nameSet    = reader.ReadUInt16();

                // Legion(?)+: has data finally, looks like scaling (same as MDDF). Padding in 0.5.3 alpha. 
                int unk         = reader.ReadUInt16();

                ADTObjData.modelBlockData.WMOInfo.Add(data);
            }
        }

        // Chunk Data //
        public void ReadMCNKObj(BinaryReader reader, string mapname, int MCNKchunkNumber, int MCNKsize)
        {
            if (reader.BaseStream.Length == reader.BaseStream.Position)
                return;

            long MCNKchnkPos = reader.BaseStream.Position;
            long streamPosition = reader.BaseStream.Position;
            while (streamPosition < MCNKchnkPos + MCNKsize)
            {
                reader.BaseStream.Position = streamPosition;
                ADTChunkId chunkID = (ADTChunkId)reader.ReadInt32();
                int chunkSize = reader.ReadInt32();
                streamPosition = reader.BaseStream.Position + chunkSize;
                switch (chunkID)
                {
                    case ADTChunkId.MCRD:
                        ReadMCRD(reader, MCNKchunkNumber, chunkSize); // MCNK.nDoodadRefs into the file's MDDF
                        break;
                    case ADTChunkId.MCRW:
                        ReadMCRW(reader, MCNKchunkNumber, chunkSize); // MCNK.nMapObjRefs into the file's MODF
                        break;
                    default:
                        SkipUnknownChunk(reader, chunkID, chunkSize);
                        break;
                }
            }
        }

        /////////////////////
        ///// Subchunks /////
        /////////////////////

        // MCNK.nDoodadRefs into the file's MDDF //
        public void ReadMCRD(BinaryReader reader, int MCNKchunkNumber, int MCRDsize)
        {
            List<int> MDDFentries = new List<int>();
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MCRDsize)
            {
                MDDFentries.Add(reader.ReadInt32());
            }
        }

        // MCNK.nMapObjRefs into the file's MODF //
        public void ReadMCRW(BinaryReader reader, int MCNKchunkNumber, int MCRWsize)
        {
            List<int> MODFentries = new List<int>();
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MCRWsize)
            {
                MODFentries.Add(reader.ReadInt32());
            }
        }

        // Move the stream forward upon finding unknown chunks //
        public static void SkipUnknownChunk(BinaryReader reader, ADTChunkId chunkID, int chunkSize)
        {
            Debug.Log("Missing chunk ID : " + chunkID);
            reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
        }
    }

}