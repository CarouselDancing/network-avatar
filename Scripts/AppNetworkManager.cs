using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class AppNetworkManager : NetworkManager
{
    public string ClientSceneName;
    public string ServerSceneName;

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        SceneManager.LoadScene(ClientSceneName, LoadSceneMode.Additive);
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        SceneManager.LoadScene(ServerSceneName, LoadSceneMode.Additive);
    }
}
