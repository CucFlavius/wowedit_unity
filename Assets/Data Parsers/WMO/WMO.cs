using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public static partial class WMO
{

    public static bool ThreadWorking;
    public static GroupData groupDataBuffer;

    public static void Load (string dataPath, int uniqueID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        wmoData = new WMOData();

        wmoData.dataPath = dataPath;
        wmoData.uniqueID = uniqueID;
        wmoData.position = position;
        wmoData.rotation = rotation;
        wmoData.scale = scale;

        wmoData.Info = new HeaderData();
        wmoData.texturePaths = new Dictionary<int, string>();
        wmoData.textureData = new Dictionary<string, Texture2Ddata>();
        wmoData.MOGNgroupnames = new Dictionary<int, string>();
        wmoData.materials = new List<WMOMaterial>();

        wmoData.groupsData = new List<GroupData>();
        try
        {
            ThreadWorking = true;

            ParseWMO_Root(dataPath);
            ParseWMO_Groups(dataPath);

            AllWMOData.Enqueue(wmoData);

            ThreadWorking = false;
        }
        catch
        {
            Debug.Log("Error : Trying to parse WMO - " + dataPath);
        }
    }

    private static void ParseWMO_Root(string dataPath)
    {
        Stream WMOrootstream = Casc.GetFileStream(dataPath);

        long streamPosition = 0;
        while (streamPosition < WMOrootstream.Length)
        {
            WMOrootstream.Position = streamPosition;
            int chunkID = ReadLong(WMOrootstream);
            int chunkSize = ReadLong(WMOrootstream);
            streamPosition = WMOrootstream.Position + chunkSize;

            switch (chunkID)
            {
                case (int)WMOChunkID.MVER:
                    ReadMVER(WMOrootstream); // root file version
                    break;
                case (int)WMOChunkID.MOHD:
                    ReadMOHD(WMOrootstream); // root file header
                    break;
                case (int)WMOChunkID.MOTX:
                    ReadMOTX(WMOrootstream, chunkSize); // texture paths
                    break;
                case (int)WMOChunkID.MOMT:
                    ReadMOMT(WMOrootstream, chunkSize); // materials
                    break;
                case (int)WMOChunkID.MOUV:
                    ReadMOUV(WMOrootstream); // texture animation // optional
                    break;
                case (int)WMOChunkID.MOGN:
                    ReadMOGN(WMOrootstream, chunkSize); // list of group names
                    break;
                case (int)WMOChunkID.MOGI:
                    ReadMOGI(WMOrootstream); // Group information for WMO groups
                    break;
                case (int)WMOChunkID.MOSB:
                    ReadMOSB(WMOrootstream, chunkSize); // Skybox model filename
                    break;
                case (int)WMOChunkID.MOPV:
                    ReadMOPV(WMOrootstream, chunkSize); // Portal vertices
                    break;
                case (int)WMOChunkID.MOPT:
                    ReadMOPT(WMOrootstream); // Portal information
                    break;
                case (int)WMOChunkID.MOPR:
                    ReadMOPR(WMOrootstream, chunkSize); // Portal references from groups
                    break;
                case (int)WMOChunkID.MOVV:
                    ReadMOVV(WMOrootstream, chunkSize); // Visible block vertices
                    break;
                case (int)WMOChunkID.MOVB:
                    ReadMOVB(WMOrootstream, chunkSize); // Visible block list
                    break;
                case (int)WMOChunkID.MOLT:
                    ReadMOLT(WMOrootstream); // Lighting information.
                    break;
                case (int)WMOChunkID.MODS:
                    ReadMODS(WMOrootstream, chunkSize); // This chunk defines doodad sets.
                    break;
                case (int)WMOChunkID.MODN:
                    ReadMODN(WMOrootstream, chunkSize); //List of filenames for M2
                    break;
                case (int)WMOChunkID.MODD:
                    ReadMODD(WMOrootstream, chunkSize); // Information for doodad instances.
                    break;
                case (int)WMOChunkID.MFOG:
                    ReadMFOG(WMOrootstream, chunkSize); // Fog information.
                    break;
                case (int)WMOChunkID.MCVP:
                    ReadMCVP(WMOrootstream, chunkSize); // Convex Volume Planes.
                    break;
                case (int)WMOChunkID.GFID:
                    ReadGFID(WMOrootstream, chunkSize); // required when WMO is loaded from fileID
                    break;
                default:
                    SkipUnknownChunk(WMOrootstream, chunkID, chunkSize);
                    break;
            }
        }
        WMOrootstream.Close();
        WMOrootstream = null;
    }

    private static void ParseWMO_Groups(string dataPath)
    {
        wmoData.groupsData = new List<GroupData>(wmoData.Info.nGroups);

        string noExtension = Path.GetFileNameWithoutExtension(dataPath);
        string directoryPath = Path.GetDirectoryName(dataPath);

        //for (int grp = 0; grp < 1; grp++)
        for (int grp = 0; grp < wmoData.Info.nGroups; grp++)
        {
            groupDataBuffer = new GroupData();

            string fileNumber = grp.ToString("000");
            string fullPath = directoryPath + @"\" + noExtension + "_" + fileNumber + ".wmo";

            Stream WMOgroupstream = Casc.GetFileStream(fullPath);

            int MVER = ReadLong(WMOgroupstream);
            int MVERsize = ReadLong(WMOgroupstream);
            ReadMVER(WMOgroupstream); // root file version
            int MOGP = ReadLong(WMOgroupstream);
            int MOGPsize = ReadLong(WMOgroupstream);
            ReadMOGP(WMOgroupstream); // group header (contains the rest of the chunks)

            int MOTVcount = 0;
            int MOCVcount = 0;
            long streamPosition = WMOgroupstream.Position;
            while (streamPosition < WMOgroupstream.Length)
            {
                WMOgroupstream.Position = streamPosition;
                int chunkID = ReadLong(WMOgroupstream);
                int chunkSize = ReadLong(WMOgroupstream);
                streamPosition = WMOgroupstream.Position + chunkSize;
                switch (chunkID)
                {
                    case (int)WMOChunkID.MOPY:
                        ReadMOPY(WMOgroupstream, chunkSize); // Material info for triangles
                        break;
                    case (int)WMOChunkID.MOVI:
                        ReadMOVI(WMOgroupstream, chunkSize); // Vertex indices for triangles
                        break;
                    case (int)WMOChunkID.MOVT:
                        ReadMOVT(WMOgroupstream, chunkSize); // Vertices chunk
                        break;
                    case (int)WMOChunkID.MONR:
                        ReadMONR(WMOgroupstream, chunkSize); // Normals chunk
                        break;
                    case (int)WMOChunkID.MOTV:
                        {
                            MOTVcount++;
                            if (MOTVcount == 1)
                                ReadMOTV(WMOgroupstream, chunkSize); // Texture coordinates
                            else if (MOTVcount == 2)
                                ReadMOTV2(WMOgroupstream, chunkSize);
                            else if (MOTVcount == 3)
                                ReadMOTV3(WMOgroupstream, chunkSize);
                        }
                        break;
                    case (int)WMOChunkID.MOBA:
                        ReadMOBA(WMOgroupstream, chunkSize); // Render batches
                        break;
                    case (int)WMOChunkID.MOCV:
                        {
                            MOCVcount++;
                            if (MOCVcount == 1)
                                ReadMOCV(WMOgroupstream, chunkSize);
                            else if (MOCVcount == 2)
                                ReadMOCV2(WMOgroupstream, chunkSize);
                        }
                        break;
                    default:
                        SkipUnknownChunk(WMOgroupstream, chunkID, chunkSize);
                        break;
                }
            }

            wmoData.groupsData.Add(groupDataBuffer);

            WMOgroupstream.Close();
            WMOgroupstream = null;

        }
    }

    public static void SkipUnknownChunk(Stream WMOrootstream, int chunkID, int chunkSize)
    {
        //Debug.Log("Unknown chunk : " + (Enum.GetName(typeof(WMOChunkID), chunkID)).ToString() + " | Skipped");
        WMOrootstream.Seek(chunkSize, SeekOrigin.Current);
    }
}

