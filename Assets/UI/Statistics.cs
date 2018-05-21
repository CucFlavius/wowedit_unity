using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    public TerrainHandler terrainHandler;
    public WMOhandler wmoHandler;
    public M2handler m2Handler;
    public UnityEngine.UI.Text statsText;

    private float PreviousParseADTRootSpeed;
    private float PreviousParseADTTexSpeed;
    private float PreviousParseADTObjSpeed;
    private float PreviousAssembleHTMeshSpeed;
    private float PreviousAssembleHTexSpeed;

    private float AverageADTRootParseSpeed = 0f;
    private float AverageADTTexParseSpeed = 0f;
    private float AverageHTMeshCreateSpeed = 0f;
    private float AverageHTexCreateSpeed = 0f;

    private List<float> ADTRootParseTimes;
    private List<float> ADTTexParseTimes;
    private List<float> HTMeshCreateTimes;
    private List<float> HTexCreateTimes;

    private string RemainingHTerrainBlocks = "";
    private string RemainingHTexBlocks = "";

    // Use this for initialization
    void Start () {

        ADTRootParseTimes = new List<float>();
        ADTTexParseTimes = new List<float>();
        HTMeshCreateTimes = new List<float>();
        HTexCreateTimes = new List<float>();
        PreviousParseADTRootSpeed = 0;
        PreviousParseADTTexSpeed = 0;
        PreviousParseADTObjSpeed = 0;
        PreviousAssembleHTMeshSpeed = 0;
        PreviousAssembleHTexSpeed = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateTerrainMeshStats();
    }

    private void UpdateTerrainMeshStats()
    {
        float ParseADTRootSpeed = ADT.finishedTimeTerrainMesh;
        float ParseADTTexSpeed = ADT.finishedTimeTerrainTextures;
        float AssembleHTMeshSpeed = Truncate(terrainHandler.finishedTimeAssembleHT, 2);
        float AssembleHTexSpeed = Truncate(terrainHandler.finishedTimeAssembleHTextures, 2);

        // root parse //
        PreviousParseADTRootSpeed = ParseADTRootSpeed;
        if (ParseADTRootSpeed != 0)
        {
            ADTRootParseTimes.Add(ParseADTRootSpeed);
        }
        AverageADTRootParseSpeed = Truncate(CalculateAverage(ADTRootParseTimes), 2);

        int numberInParseQueue = ADTRootData.MeshBlockDataQueue.Count;
        RemainingHTerrainBlocks = Tabs(numberInParseQueue, "□");

        // tex parse //
        PreviousParseADTTexSpeed = ParseADTTexSpeed;
        if (ParseADTTexSpeed != 0)
        {
            ADTTexParseTimes.Add(ParseADTTexSpeed);
        }
        AverageADTTexParseSpeed = Truncate(CalculateAverage(ADTTexParseTimes), 2);

        int numberInParseQueue1 = terrainHandler.ADTTexQueue.Count;
        RemainingHTexBlocks = Tabs(numberInParseQueue1, "■");

        // root assemble //
        PreviousAssembleHTMeshSpeed = AssembleHTMeshSpeed;
        if (AssembleHTMeshSpeed != 0)
        {
            HTMeshCreateTimes.Add(AssembleHTMeshSpeed);
        }
        AverageHTMeshCreateSpeed = Truncate(CalculateAverage(HTMeshCreateTimes), 2);

        // tex assemble //
        PreviousAssembleHTexSpeed = AssembleHTexSpeed;
        if (AssembleHTMeshSpeed != 0)
        {
            HTexCreateTimes.Add(AssembleHTexSpeed);
        }
        AverageHTexCreateSpeed = Truncate(CalculateAverage(HTexCreateTimes), 2);

        // setup string //
        statsText.text = "\n" + "<b><color=silver>[HTerrain Mesh Speed]</color></b>" +
                 "\n" + "<b>Last Parse: </b>" + ParseADTRootSpeed + "s" +
                 "\n" + "<b>Last Create: </b>" + AssembleHTMeshSpeed + "s" +
                 "\n" + "<b>Average Parse: </b>" + AverageADTRootParseSpeed + "s" +
                 "\n" + "<b>Average Create: </b>" + AverageHTMeshCreateSpeed + "s" +
                 "\n" + "<b>Remaining HTerrain Blocks: </b>" + ADTRootData.MeshBlockDataQueue.Count + 
                 //"\n" + RemainingHTerrainBlocks +
                 "\n" + "<b><color=silver>[HTextures Speed]</color></b>" +
                 "\n" + "<b>Last Parse: </b>" + ParseADTTexSpeed + "s" +
                 "\n" + "<b>Last Create: </b>" + AssembleHTexSpeed + "s" +
                 "\n" + "<b>Average Parse: </b>" + AverageADTTexParseSpeed + "s" +
                 "\n" + "<b>Average Create: </b>" + AverageHTexCreateSpeed + "s" +
                 "\n" + "<b>Remaining HTexture Blocks: </b>" + terrainHandler.ADTTexQueue.Count +
                 //"\n" + RemainingHTexBlocks +
                 "\n" + "<b><color=silver>[WMO Queues]</color></b>" +
                 "\n" + "<b>ADTObj: </b>" + ADTObjData.ModelBlockDataQueue.Count +
                 "\n" + "<b>Remaining WMO: </b>" + wmoHandler.WMOThreadQueue.Count +
                 "\n" + "<b>Remaining M2: </b>" + m2Handler.M2ThreadQueue.Count;
    }

    //////////////////////
    #region Helpers

    private float CalculateAverage(List<float> Values)
    {
        if (Values.Count > 0)
        {
            float total = 0;
            foreach (float value in Values)
            {
                total = total + value;
            }
            return total / Values.Count;
        }
        return 0f;
    }

    private float Truncate(float value, int digits)
    {
        double mult = System.Math.Pow(10.0f, digits);
        double result = System.Math.Truncate(mult * value) / mult;
        return (float)result;
    }

    private string Tabs(int numTabs, string tabType)
    {
        IEnumerable<string> tabs = Enumerable.Repeat(tabType, (int)numTabs);
        return (numTabs > 0) ? tabs.Aggregate((sum, next) => sum + next) : "";
    }

    #endregion
    //////////////////////

}