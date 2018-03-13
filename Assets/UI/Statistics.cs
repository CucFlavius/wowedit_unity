using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Statistics : MonoBehaviour {

    public TerrainHandler terrainHandler;
    public UnityEngine.UI.Text statsText;

    private float PreviousBlockParse;
    private float PreviousBlockCreate;

    private List<float> BlockParseTimes;
    private List<float> BlockCreateTimes;

	// Use this for initialization
	void Start () {
        BlockParseTimes = new List<float>();
        BlockCreateTimes = new List<float>();
        PreviousBlockParse = 0;
        PreviousBlockCreate = 0;
    }
	
	// Update is called once per frame
	void Update () {

        float BlockParseSpeed = ADT.finishedTime;
        float BlockCreateSpeed = Truncate(terrainHandler.finishedTime, 2);
        float AverageParseSpeed = 0f;
        float AverageCreateSpeed = 0f;


        if (BlockParseSpeed != PreviousBlockParse || BlockCreateSpeed != PreviousBlockCreate)
        {
            PreviousBlockParse = BlockParseSpeed;
            PreviousBlockCreate = BlockCreateSpeed;

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
    }

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

}