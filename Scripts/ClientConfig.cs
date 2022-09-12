using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ServerRegistryConfig{
    public string protocol = "https";
    public int port = 8888;
    public string url = "motion.dfki.de";
    public bool usePort = true;
    public bool usePortWorkAround = true;
}

[Serializable]
public class RPMAvatarEntry
{
    public string name;
    public string url;

}

[Serializable]
public class TrackerConfig
{
    public float[] posOffset;
    public float[] rotOffset;

}


[Serializable]
public class ClientConfig
{
    public string url = "localhost";
    public int port = 7777;
    public int userAvatar = 0;
    public List<RPMAvatarEntry> rpmAvatars = new List<RPMAvatarEntry>(){ new RPMAvatarEntry(){name = "female", url = "https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb"}};
    public string protocol = "kcp";
    public bool activateHipTracker;
    public bool activateFootTrackers;
    public TrackerConfig hipTracker;
    public TrackerConfig leftFootTracker;
    public TrackerConfig rightFootTracker;
    public TrackerConfig leftControllerTracker;
    public TrackerConfig rightControllerTracker;
    public bool registerServerOnline = true;
    public ServerRegistryConfig serverRegistryConfig;
    protected static ClientConfig instance;

    override public string ToString()
    {
        var s = "activate hip:" + activateHipTracker + "\n";
        s += "activate feet:" + activateFootTrackers +"\n";
        
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
