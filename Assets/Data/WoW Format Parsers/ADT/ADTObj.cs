using Assets.Data.WoW_Format_Parsers;
using Assets.WoWEditSettings;
using System.Collections.Generic;
using System.IO;
using CASCLib;
using UnityEngine;
using System;

namespace Assets.Data.WoW_Format_Parsers.ADT
{
    public class ADTObj
    {
        public uint Version;
        public void ReadMVER(BinaryReader reader)
        {
            Version = reader.ReadUInt32();
        }
        
        // Placement information for doodads (M2 models). //
        // Additional to this, the models to render are referenced in each MCRF chunk. //
        public void ReadMDDF(BinaryReader reader, uint MDDFsize)
        {
            Flags f = new Flags();
            ADTObjData.modelBlockData.M2Info = new List<ADTObjData.M2PlacementInfo>();
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MDDFsize)
            {
                ADTObjData.M2PlacementInfo data = new ADTObjData.M2PlacementInfo();

                data.nameId     = reader.ReadUInt32();                                          // references an entry in the MMID chunk, specifying the model to use.
                                                                                                // if flag mddf_entry_is_filedata_id is set, a file data id instead, ignoring MMID.
                data.uniqueID   = reader.ReadInt32();                                           // This ID should be unique for all ADTs currently loaded.
                                                                                                // Best, they are unique for the whole map. Blizzard has these unique for the whole game.
                float Y         = (reader.ReadSingle() - 17066) * -1 / Settings.WorldScale;     //-- pos X
                float Z         = reader.ReadSingle() / Settings.WorldScale;                    //-- Height
                float X         = (reader.ReadSingle() - 17066) * -1 / Settings.WorldScale;     //-- pos Z
                data.position   = new Vector3(X, Z, Y);                                         // This is relative to a corner of the map. Subtract 17066 from the non vertical values and you should start to see 
                                                                                                // something that makes sense. You'll then likely have to negate one of the non vertical values in whatever coordinate 
                                                                                                // system you're using to finally move it into place.
                float rotX      = reader.ReadSingle();                                          //-- rot X
                float rotZ      = 180 - reader.ReadSingle();                                    //-- rot Y
                float rotY      = reader.ReadSingle();                                          //-- rot Z
                data.rotation   = Quaternion.Euler(new Vector3(rotX, rotZ, rotY));              // degrees. This is not the same coordinate system orientation like the ADT itself! (see history.)
                data.scale      = reader.ReadUInt16() / 1024.0f;                                // 1024 is the default size equaling 1.0f.
                data.flags      = f.ReadMDDFFlags(reader);                                      // values from struct MDDFFlags.

                if (!ADTObjData.modelBlockData.M2Path.ContainsKey(data.nameId))
                    ADTObjData.modelBlockData.M2Path.Add(data.nameId, data.nameId);

                ADTObjData.modelBlockData.M2Info.Add(data);
            }
        }

        // Placement information for WMOs. //
        // Additional to this, the WMOs to render are referenced in each MCRF chunk. (?) //
        public void ReadMODF(BinaryReader reader, uint MODFsize)
        {
            Flags f = new Flags();
            ADTObjData.modelBlockData.WMOInfo = new List<ADTObjData.WMOPlacementInfo>();
            long currentPos = reader.BaseStream.Position;

            while (reader.BaseStream.Position < currentPos + MODFsize)
            {
                ADTObjData.WMOPlacementInfo data = new ADTObjData.WMOPlacementInfo();
                data.nameId     = reader.ReadUInt32();                                          // references an entry in the MMID chunk, specifying the model to use.
                                                                                                // if flag mddf_entry_is_filedata_id is set, a file data id instead, ignoring MMID.
                data.uniqueID   = reader.ReadInt32();                                           // This ID should be unique for all ADTs currently loaded.
                                                                                                // Best, they are unique for the whole map. Blizzard has these unique for the whole game.
                float Y         = (reader.ReadSingle() - 17066) * -1 / Settings.WorldScale;     //-- pos X
                float Z         = reader.ReadSingle() / Settings.WorldScale;                    //-- Height
                float X         = (reader.ReadSingle() - 17066) * -1 / Settings.WorldScale;     //-- pos Z
                data.position   = new Vector3(X, Z, Y);

                // same as in MDDF.
                float rotX      = reader.ReadSingle();                                          //-- rot X
                float rotZ      = 180 - reader.ReadSingle();                                    //-- rot Y
                float rotY      = reader.ReadSingle();                                          //-- rot Z
                data.rotation   = Quaternion.Euler(new Vector3(rotX, rotZ, rotY));
                data.extents    = reader.ReadBoundingBoxes();                                   // position plus the transformed wmo bounding box. used for defining if they are rendered as well as collision.
                data.flags      = f.ReadMODFFlags(reader);                                      // values from enum MODFFlags.
                data.doodadSet  = reader.ReadUInt16();                                          // which WMO doodad set is used.
                data.nameSet    = reader.ReadUInt16();                                          // which WMO name set is used. Used for renaming goldshire inn to northshire inn while using the same model.
                data.Scale      = reader.ReadUInt16() / 1024.0f;                                // Legion+: scale, 1024 means 1 (same as MDDF). Padding in 0.5.3 alpha.

                if (!ADTObjData.modelBlockData.WMOPath.ContainsKey(data.nameId))
                    ADTObjData.modelBlockData.WMOPath.Add(data.nameId, data.nameId);

                ADTObjData.modelBlockData.WMOInfo.Add(data);
            }
        }

        // Chunk Data //
        public void ReadMCNKObj(BinaryReader reader,  int MCNKchunkNumber, uint MCNKsize)
        {
            if (reader.BaseStream.Length == reader.BaseStream.Position)
                return;

            long MCNKchnkPos = reader.BaseStream.Position;
            long streamPosition = reader.BaseStream.Position;
            while (streamPosition < MCNKchnkPos + MCNKsize)
            {
                reader.BaseStream.Position = streamPosition;
                ADTChunkId chunkID = (ADTChunkId)reader.ReadInt32();
                uint chunkSize = reader.ReadUInt32();
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
        public void ReadMCRD(BinaryReader reader, int MCNKchunkNumber, uint MCRDsize)
        {
            List<int> MDDFentries = new List<int>();
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MCRDsize)
            {
                MDDFentries.Add(reader.ReadInt32());
            }
        }

        // MCNK.nMapObjRefs into the file's MODF //
        public void ReadMCRW(BinaryReader reader, int MCNKchunkNumber, uint MCRWsize)
        {
            List<int> MODFentries = new List<int>();
            long currentPos = reader.BaseStream.Position;
            while (reader.BaseStream.Position < currentPos + MCRWsize)
            {
                MODFentries.Add(reader.ReadInt32());
            }
        }

        // Move the stream forward upon finding unknown chunks //
        public static void SkipUnknownChunk(BinaryReader reader, ADTChunkId chunkID, uint chunkSize)
        {
            if (Enum.IsDefined(typeof(ADTChunkId), chunkID))
                Debug.Log($"Missing chunk ID : {chunkID}");

            reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
        }
    }
}