using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class AutoClientOrHost : MonoBehaviour
{
    public bool host;
    public bool server;
    public bool client;
    
    public string ClientSceneName;
    public string ServerSceneName;
    public bool loadAdditiveScene = true;

    // Start is called before the first frame update
    void Start()
    {
        var n = GetComponent<AppNetworkManager>();
        if(client){
            n.StartClient();
            if(loadAdditiveScene)SceneManager.LoadScene(ClientSceneName, LoadSceneMode.Additive);
        }
        if(host){
            n.StartHost();
            if(loadAdditiveScene)SceneManager.LoadScene(ClientSceneName, LoadSceneMode.Additive);
        }
        if(server){
            n.StartServer();
           if(loadAdditiveScene)SceneManager.LoadScene(ServerSceneName, LoadSceneMode.Additive);
            
        }
    }

}
