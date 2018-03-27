using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// A console to display Unity's debug logs in-game.
/// </summary>
public class ConsoleToGUI : MonoBehaviour
{
    public UnityEngine.UI.Text logText;
    private string fullText;

    struct Log
    {
        public string message;
        public string stackTrace;
        public LogType type;
    }

    private void Update()
    {
        logText.text = fullText;
    }

    static readonly Dictionary<LogType, string> logTypeColors = new Dictionary<LogType, string>()
    {
        { LogType.Assert, "white" },
        { LogType.Error, "#ff0000ff" },
        { LogType.Exception, "#ff0000ff" },
        { LogType.Log, "white" },
        { LogType.Warning, "#00ff00ff" },
    };
    
    public void OnEnable()
    {
        //Debug.Log("ExternalLoggerComponent -> OnEnable");

        Application.logMessageReceivedThreaded += HandleLog;
    }

    public void OnDisable()
    {
        //Debug.Log("ExternalLoggerComponent -> OnDisable");

        Application.logMessageReceivedThreaded -= HandleLog;
    }


    void HandleLog(string logString, string stackTrace, LogType type)
    {
        fullText += ("<color=" + logTypeColors[type] + ">" + logString + "</color>" + "\n");
    }


}