using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Statistics : MonoBehaviour {

    public TerrainHandler terrainHandler;
    public UnityEngine.UI.Text statsText;

    private float PreviousParseADTRootSpeed;
    private float PreviousParseADTTexSpeed;
    private float PreviousParseADTObjSpeed;
    private float PreviousAssembleHTMeshSpeed;

    private float AverageADTRootParseSpeed = 0f;
    private float AverageHTMeshCreateSpeed = 0f;

    private List<float> ADTRootParseTimes;
    private List<float> HTMeshCreateTimes;

    private string RemainingHTerrainBlocks = "";

    // Use this for initialization
    void Start () {

        ADTRootParseTimes = new List<float>();
        HTMeshCreateTimes = new List<float>();
        PreviousParseADTRootSpeed = 0;
        PreviousParseADTTexSpeed = 0;
        PreviousParseADTObjSpeed = 0;
        PreviousAssembleHTMeshSpeed = 0;

    }
	
	// Update is called once per frame
	void Update () {

        UpdateTerrainMeshStats();

        /*
        float ParseADTTexSpeed= ADT.finishedTimeTerrainTextures;
        float ParseADTObjSpeed = ADT.finishedTimeTerrainModels;
        float AverageParseSpeed = 0f;
        float AverageCreateSpeed = 0f;


        if (BlockParseSpeed != PreviousBlockParse || BlockCreateSpeed != PreviousBlockCreate)
        {

            int numberInParseQueue = terrainHandler.ADTThreadQueue.Count;
            int numberInCreateQueue = ADT.AllBlockData.Count;

            string RemainingParseBlocks = Tabs(numberInParseQueue, "□");
            string RemainingCreateBlocks = Tabs(numberInCreateQueue, "■");

            if (BlockParseSpeed != 0)
            {
                BlockParseTimes.Add(BlockParseSpeed);
            }
            if (BlockCreateSpeed != 0)
            {
                BlockCreateTimes.Add(BlockCreateSpeed);
            }
            AverageParseSpeed = Truncate(CalculateAverage(BlockParseTimes),2);
            AverageCreateSpeed = Truncate(CalculateAverage(BlockCreateTimes),2);
            statsText.text = "\n" + "[Last Block]" +
                             "\n" + "Parse Speed: " + BlockParseSpeed + "s" +
                             "\n" + "Create Speed: " + BlockCreateSpeed + "s" +
                             "\n" +
                             "\n" + "[Average]" +
                             "\n" + "Parse Speed: " + AverageParseSpeed + "s" +
                             "\n" + "Create Speed: " + AverageCreateSpeed + "s" +
                             "\n" +
                             "\n" + "[Remaining Blocks]" +
                             "\n" + RemainingParseBlocks +
                             "\n" + RemainingCreateBlocks +
                             "\n";
        }
        */
    }

    private void UpdateTerrainMeshStats()
    {
        float ParseADTRootSpeed = ADT.finishedTimeTerrainMesh;
        float AssembleHTMeshSpeed = Truncate(terrainHandler.finishedTimeAssembleHT, 2);

        if (PreviousParseADTRootSpeed != ParseADTRootSpeed)
        {
            PreviousParseADTRootSpeed = ParseADTRootSpeed;
            if (ParseADTRootSpeed != 0)
            {
                ADTRootParseTimes.Add(ParseADTRootSpeed);
            }
            AverageADTRootParseSpeed = Truncate(CalculateAverage(ADTRootParseTimes), 2);

            int numberInParseQueue = terrainHandler.ADTRootQueue.Count;
            RemainingHTerrainBlocks = Tabs(numberInParseQueue, "□");
        }

        if (PreviousAssembleHTMeshSpeed != AssembleHTMeshSpeed)
        { 
            PreviousAssembleHTMeshSpeed = AssembleHTMeshSpeed;
            if (AssembleHTMeshSpeed != 0)
            {
                HTMeshCreateTimes.Add(ParseADTRootSpeed);
            }
            AverageHTMeshCreateSpeed = Truncate(CalculateAverage(HTMeshCreateTimes), 2);
        }

        // setup string //
        statsText.text = "\n" + "[HTerrain Mesh Speed]" +
                 "\n" + "Last Parse: " + ParseADTRootSpeed + "s" +
                 "\n" + "Last Create: " + AssembleHTMeshSpeed + "s" +
                 "\n" + "Average Parse: " + AverageADTRootParseSpeed + "s" +
                 "\n" + "Average Create: " + AverageHTMeshCreateSpeed + "s" +
                 "\n" + "[Remaining HTerrain Blocks]" +
                 "\n" + RemainingHTerrainBlocks;

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