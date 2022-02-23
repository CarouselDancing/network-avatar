using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClientConfig
{
    public string url = "localhost";
    public int port = 7777;
    public string rpmURL = "https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb";

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



