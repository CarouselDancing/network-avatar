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
    public string networkMode;
    protected static ClientConfig instance;

    override public string ToString()
    {
        var s = "activate hip:" + activateHipTracker +" tracker id: " + hipTracker.deviceID.ToString() + "\n";
        s += "activate feet:" + activateFootTrackers + " left id: " + leftFootTracker.deviceID.ToString() +" right id: "+ rightFootTracker.deviceID.ToString() + "\n";
        
        return s;
    }  
    
    public static ClientConfig GetInstance()
    {
        return instance;
    }
    public static ClientConfig InitInstance(string configText)
    {
        if (instance == null)
        {
            instance = JsonUtility.FromJson<ClientConfig>(configText);
        }
        return instance;
    }
}

public class GlobalGameState
{
    public ClientConfig config;
    protected static GlobalGameState instance;
    public static bool useResources = true;


    protected GlobalGameState()
    {
        Load();
    }


    protected void Load()
    {
        
        config = ClientConfig.GetInstance();
        if (config!=null)return;
        string configText = "";
        if(useResources){
            var configAsset = Resources.Load<TextAsset>("config");
            configText = configAsset.text;
        }else{
            string configFile = Path.Combine(Application.streamingAssetsPath, "config.json");
           configText = File.ReadAllText(configFile);
        }
        
        config = ClientConfig.InitInstance(configText);
        Debug.Log(config.ToString());

    }

    public static GlobalGameState GetInstance()
    {
        if (instance == null)
        {
            instance = new GlobalGameState();
        }
        return instance;
    }
}

