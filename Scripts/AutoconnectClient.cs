using System;
using System.IO;
using UnityEngine;
using Mirror;
using kcp2k;


[Serializable]
public class ClientConfig
{
    public string url = "localhost";
    public int port = 7777;
}

[RequireComponent(typeof(NetworkManager))]
public class AutoconnectClient : MonoBehaviour
{
    NetworkManager networkManager;
    KcpTransport transport;
    public ClientConfig config;
    void Start()
    {

        networkManager = GetComponent<NetworkManager>();
        transport = GetComponent<KcpTransport>();
        string configFile = Path.Combine(Application.streamingAssetsPath, "config.json");
        string configText = File.ReadAllText(configFile);
        config = JsonUtility.FromJson<ClientConfig>(configText);
     
        networkManager.networkAddress = config.url;
        transport.Port = (ushort)config.port;
        networkManager.StartClient();
    }

   
}
