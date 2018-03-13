using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleToGUI : MonoBehaviour
{

	public UnityEngine.UI.Text ConsoleText;

    string myLog;
    Queue myLogQueue = new Queue();

    void Start()
    {

    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        //if (type == LogType.Exception)
        //{
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        //}
        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
        }
    }

    void Update () {
				
	    //UnityEngine.UI.Text instruction = ConsoleText.GetComponent<Text>();
        ConsoleText.text = myLog;
				
    }
}