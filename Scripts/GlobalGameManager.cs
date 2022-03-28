using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrackerConfig
{
    public int deviceID;
    public float[] posOffset;
    public float[] rotOffset;

}

[Serializable]
public class ClientConfig
{
    public string url = "localhost";
    public int port = 7777;
    public string rpmURL = "https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb";
    public bool activateHipTracker;
    public bool activateFootTrackers;
    public TrackerConfig hipTracker;
    public TrackerConfig leftFootTracker;
    public TrackerConfig rightFootTracker;
    public bool activateDebugVis;

    override public string ToString()
    {
        var s = "activate hip:" + activateHipTracker +" tracker id: " + hipTracker.deviceID.ToString() + "\n";
        s += "activate feet:" + activateFootTrackers + " left id: " + leftFootTracker.deviceID.ToString() +" right id: "+ rightFootTracker.deviceID.ToString() + "\n";
        
        return s;
    }
}

public class GlobalGameManager
{
    public ClientConfig config;
    private static GlobalGameManager instance;

    private GlobalGameManager()
    {
        Load();
    }


    void Load()
    {
        string configFile = Path.Combine(Application.streamingAssetsPath, "config.json");
        string configText = File.ReadAllText(configFile);
        config = JsonUtility.FromJson<ClientConfig>(configText);
        Debug.Log(config.ToString());

    }

    public static GlobalGameManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GlobalGameManager();
        }
        return instance;
    }
}



