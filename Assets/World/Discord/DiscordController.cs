using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DiscordController : MonoBehaviour
{
    public static DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
    public string applicationId;
    public string optionalSteamId;
    public int callbackCalls;
    public static string mapName;
    public static long secondsSinceEpoch;
    public DiscordRpc.DiscordUser joinRequest;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;
    public UnityEngine.Events.UnityEvent hasResponded;

    DiscordRpc.EventHandlers handlers;


    public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser)
    {
        ++callbackCalls;
        //Debug.Log(string.Format("Discord: connected to {0}#{1}", connectedUser.username, connectedUser.discriminator));
        onConnect.Invoke();
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        ++callbackCalls;
        //Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        ++callbackCalls;
        //Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (mapName == null)
        {
            presence.state = "Nothing";
            DiscordRpc.UpdatePresence(presence);
        }
        else
        {
            presence.state = string.Format("{0}", mapName);
            DiscordRpc.UpdatePresence(presence);
        }
        DiscordRpc.RunCallbacks();            
    }

    void OnEnable()
    {
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        secondsSinceEpoch = (int)t.TotalSeconds;

        //Debug.Log("Discord: init");
        callbackCalls = 0;

        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback = ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;

        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);

        presence.details = "Editing:";
        presence.smallImageKey = "wow_editor_small";
        presence.smallImageText = "Editing";
        presence.largeImageKey = "wow_editor_large";
        presence.largeImageText = "Working in WoW Editor";
        presence.startTimestamp = secondsSinceEpoch;

        DiscordRpc.UpdatePresence(presence);
    }

    void OnDisable()
    {
        //Debug.Log("Discord: shutdown");
        DiscordRpc.Shutdown();
    }

    void OnDestroy()
    {
        DiscordRpc.Shutdown();
    }
}