//////////////////////////////////////////////////////////////////////////
//      Unity Editor Extension                                          //
//      Menu -> WoWEdit -> DB2 -> Get Latest Definitions                //
//      Parses XML DB2 Definitions and hardcodes them to C# classes     //
//////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

public class DB2_Definitions_Updater : MonoBehaviour
{
    private static List<string> keyList = new List<string>();
    private static List<string> xmlData = new List<string>();
    private static List<string> cSharpFileBuffer = new List<string>();

    [MenuItem("WoWEdit/DB2/Get Latest Definitions")]
    private static void GetLatestDefinitions()
    {
        keyList = new List<string>(Settings.DB2XMLDefinitions.Keys);
        xmlData = new List<string>();

        for (int xS = 0; xS < Settings.DB2XMLDefinitions.Count; xS++)
        {
            WebClient client = new WebClient();
            xmlData.Add(client.DownloadString(Settings.DB2XMLDefinitions[keyList[xS]]));
            ParseXML(xS);
        }
    }

    private static void ParseXML(int number)
    {
        cSharpFileBuffer = new List<string>();
        string[] rawLines = Regex.Split(xmlData[number], "\r\n|\r|\n");

        cSharpFileBuffer.Add("public static partial class DB2");
        cSharpFileBuffer.Add("{");
        string[] splits1 = keyList[number].Replace(" ", ".").Split(new char[] { '.' });
        string version = splits1[0] + "_"+ splits1[1] + splits1[2] + splits1[3] + "_" + splits1[4].Trim('(').Trim(')');
        cSharpFileBuffer.Add("\t" + "public static class Definitions_" + version);
        cSharpFileBuffer.Add("\t" + "{");

        for (int l = 0; l < rawLines.Length; l++)
        {
            string[] splitLine = rawLines[l].Trim().Split(new char[] { ' ' });
            if (splitLine[0].Trim() == "<Table")
            {
                string className = splitLine[1].Split(new char[] { '=' })[1].Trim('"');
                cSharpFileBuffer.Add("\t" +  "\t" + "public class _" + className);
                cSharpFileBuffer.Add("\t" + "\t" + "{");
            }
            if (splitLine[0].Trim() == "<Field")
            {
                // regular variable //
                if (splitLine[2].Split(new char[] { '=' })[0].Trim('"') == "Type" && splitLine[1].Split(new char[] { '=' })[0].Trim('"') == "Name")
                {
                    string type = "";
                    string name = "";
                    string arrayDeclare = "";
                    // check for array //
                    if (splitLine[3].Split(new char[] { '=' })[0] == "ArraySize")
                    {
                        type = splitLine[2].Split(new char[] { '=' })[1].Trim('"') + "[]";
                        arrayDeclare = " = new " + splitLine[2].Split(new char[] { '=' })[1].Trim('"') + "[" + splitLine[3].Split(new char[] { '=' })[1].Trim('"') + "]";
                        name = splitLine[1].Split(new char[] { '=' })[1].Trim('"');
                    }
                    else
                    {
                        type = splitLine[2].Split(new char[] { '=' })[1].Trim('"');
                        name = splitLine[1].Split(new char[] { '=' })[1].Trim('"');
                    }
                    cSharpFileBuffer.Add("\t" + "\t" + "\t" + "public " + type + " " + name + arrayDeclare + ";");
                }
                // vector 3 //
                else if (splitLine[2].Split(new char[] { '=' })[0].Trim('"') == "ArraySize" && splitLine[2].Split(new char[] { '=' })[1].Trim('"') == "3")
                {
                    cSharpFileBuffer.Add("\t" + "\t" + "\t" + "UnityEngine.Vector3" + " " + "Unknown" + ";");
                }
            }
            if (splitLine[0].Trim() == "</Table>")
            {
                cSharpFileBuffer.Add("\t" + "\t" + "}");
            }
        }
        cSharpFileBuffer.Add("\t" + "}");
        cSharpFileBuffer.Add("}");
        WriteDefinitionsFile(version);
    }

    private static void WriteDefinitionsFile(string version)
    {
        File.Delete(Application.dataPath + @"/Data/WoW Format Parsers/DB2/Definitions/" + version + ".cs");
        using (TextWriter tw = new StreamWriter(Application.dataPath + @"/Data/WoW Format Parsers/DB2/Definitions/" + version + ".cs"))
        {
            foreach (string s in cSharpFileBuffer)
                tw.WriteLine(s);
        }
    }

}
