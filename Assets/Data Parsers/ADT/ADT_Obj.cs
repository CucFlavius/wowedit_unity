using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class ADT
{

    // List of filenames for M2 models that appear in this map tile. //
    public static void ReadMMDX(Stream ADTobjstream, int MMDXsize)
    {
        long currentPos = ADTobjstream.Position;
        while (ADTobjstream.Position < currentPos + MMDXsize)
        {
            int position = (int)(ADTobjstream.Position - currentPos);
            string path = ReadNullTerminatedString(ADTobjstream);
            if (path != "")
            {
                blockData.M2Paths.Add(position, path);
            }
        }
    }

    // List of offsets of model filenames in the MMDX chunk. //
    public static void ReadMMID(Stream ADTobjstream, int MMIDsize)
    {
        long currentPos = ADTobjstream.Position;
        while (ADTobjstream.Position < currentPos + MMIDsize)
        {
            blockData.M2Offsets.Add(ReadLong(ADTobjstream));
        }
    }

    // List of filenames for WMOs (world map objects) that appear in this map tile. //
    public static void ReadMWMO(Stream ADTobjstream, int MWMOsize)
    {
        long currentPos = ADTobjstream.Position;
        while (ADTobjstream.Position < currentPos + MWMOsize)
        {
            int position = (int)(ADTobjstream.Position - currentPos);
            string path = ReadNullTerminatedString(ADTobjstream);
            if (path != "")
            {
                blockData.WMOPaths.Add(position, path);
            }
        }
    }

    // List of offsets of WMO filenames in the MWMO chunk. //
    public static void ReadMWID(Stream ADTobjstream, int MWIDsize)
    {
        long currentPos = ADTobjstream.Position;
        while (ADTobjstream.Position < currentPos + MWIDsize)
        {
            blockData.WMOOffsets.Add(ReadLong(ADTobjstream));
        }
    }

    // Placement information for doodads (M2 models). //
    // Additional to this, the models to render are referenced in each MCRF chunk. //
    public static void ReadMDDF(Stream ADTobjstream, int MDDFsize)
    {
        blockData.M2Info = new List<M2PlacementInfo>();
        long currentPos = ADTobjstream.Position;
        while (ADTobjstream.Position < currentPos + MDDFsize)
        {
            M2PlacementInfo data = new M2PlacementInfo();

            // References an entry in the MMID chunk, specifying the model to use.
            data.nameID = ReadLong(ADTobjstream);

            // This ID should be unique for all ADTs currently loaded.
            // Best, they are unique for the whole map. Blizzard has these unique for the whole game.
            data.uniqueID = ReadLong(ADTobjstream);

            // This is relative to a corner of the map. Subtract 17066 from the non vertical values and you should start to see 
            // something that makes sense. You'll then likely have to negate one of the non vertical values in whatever coordinate 
            // system you're using to finally move it into place.
            float Y = ((ReadFloat(ADTobjstream) - 17066) * -1) / Settings.worldScale; //-- pos X
            float Z = (ReadFloat(ADTobjstream)) / Settings.worldScale; //-- Height
            float X = ((ReadFloat(ADTobjstream) - 17066) * -1) / Settings.worldScale; //-- pos Z
            data.position = new Vector3(X, Z, Y);

            // degrees. This is not the same coordinate system orientation like the ADT itself! (see history.)
            float rotX = ReadFloat(ADTobjstream); //-- rot X
            float rotZ = 180 - ReadFloat(ADTobjstream); //-- rot Y
            float rotY = ReadFloat(ADTobjstream); //-- rot Z
            data.rotation = Quaternion.Euler(new Vector3(rotX, rotZ, rotY));

            // 1024 is the default size equaling 1.0f.
            data.scale = ReadShort(ADTobjstream) / 1024.0f;

            // values from struct MDDFFlags.
            data.flags = ReadMDDFFlags(ADTobjstream);

            blockData.M2Info.Add(data);
        }
    }

    // Placement information for WMOs. //
    // Additional to this, the WMOs to render are referenced in each MCRF chunk. (?) //
    public static void ReadMODF(Stream ADTobjstream, int MODFsize)
    {
        blockData.WMOInfo = new List<WMOPlacementInfo>();
        long currentPos = ADTobjstream.Position;
        while (ADTobjstream.Position < currentPos + MODFsize)
        {
            WMOPlacementInfo data = new WMOPlacementInfo();

            // references an entry in the MWID chunk, specifying the model to use.
            data.nameID = ReadLong(ADTobjstream);

            // this ID should be unique for all ADTs currently loaded. Best, they are unique for the whole map.
            data.uniqueID = ReadLong(ADTobjstream);

            // same as in MDDF.
            float Y = ((ReadFloat(ADTobjstream) - 17066) * -1) / Settings.worldScale; //-- pos X
            float Z = (ReadFloat(ADTobjstream)) / Settings.worldScale; //-- Height
            float X = ((ReadFloat(ADTobjstream) - 17066) * -1) / Settings.worldScale; //-- pos Z
            data.position = new Vector3(X, Z, Y);

            // same as in MDDF.
            float rotX = ReadFloat(ADTobjstream); //-- rot X
            float rotZ = 180 - ReadFloat(ADTobjstream); //-- rot Y
            float rotY = ReadFloat(ADTobjstream); //-- rot Z
            data.rotation = Quaternion.Euler(new Vector3(rotX, rotZ, rotY));

            // position plus the transformed wmo bounding box. used for defining if they are rendered as well as collision.
            data.extents = ReadBoundingBox(ADTobjstream);

            // values from enum MODFFlags.
            data.flags = ReadMODFFlags(ADTobjstream);

            // which WMO doodad set is used.
            data.doodadSet = ReadShort(ADTobjstream);

            // which WMO name set is used. Used for renaming goldshire inn to northshire inn while using the same model.
            data.nameSet = ReadShort(ADTobjstream);

            // Legion(?)+: has data finally, looks like scaling (same as MDDF). Padding in 0.5.3 alpha. 
            int unk = ReadShort(ADTobjstream);

            blockData.WMOInfo.Add(data);
        }
    }

    // Chunk Data //
    public static void ReadMCNKObj (Stream ADTobjstream, string mapname, int MCNKchunkNumber, int MCNKsize)
    {
        if (ADTobjstream.Length == ADTobjstream.Position)
            return;

        long MCNKchnkPos = ADTobjstream.Position;
        long streamPosition = ADTobjstream.Position;
        while (streamPosition < MCNKchnkPos + MCNKsize)
        {
            ADTobjstream.Position = streamPosition;
            int chunkID = ReadLong(ADTobjstream);
            int chunkSize = ReadLong(ADTobjstream);
            streamPosition = ADTobjstream.Position + chunkSize;
            switch (chunkID)
            {
                case (int)ADTchunkID.MCRD:
                    ReadMCRD(ADTobjstream, MCNKchunkNumber, chunkSize); // MCNK.nDoodadRefs into the file's MDDF
                    break;
                case (int)ADTchunkID.MCRW:
                    ReadMCRW(ADTobjstream, MCNKchunkNumber, chunkSize); // MCNK.nMapObjRefs into the file's MODF
                    break;
                default:
                    SkipUnknownChunk(ADTobjstream, chunkID, chunkSize);
                    break;
            }
        }
    }

    /////////////////////
    ///// Subchunks /////
    /////////////////////

    // MCNK.nDoodadRefs into the file's MDDF //
    public static void ReadMCRD(Stream ADTobjstream, int MCNKchunkNumber, int MCRDsize)
    {
        List<int> MDDFentries = new List<int>();
        long currentPos = ADTobjstream.Position;
        while (ADTobjstream.Position < currentPos + MCRDsize)
        {
            MDDFentries.Add(ReadLong(ADTobjstream));
        }
    }

    // MCNK.nMapObjRefs into the file's MODF //
    public static void ReadMCRW(Stream ADTobjstream, int MCNKchunkNumber, int MCRWsize)
    {
        List<int> MODFentries = new List<int>();
        long currentPos = ADTobjstream.Position;
        while (ADTobjstream.Position < currentPos + MCRWsize)
        {
            MODFentries.Add(ReadLong(ADTobjstream));
        }
    }
}
