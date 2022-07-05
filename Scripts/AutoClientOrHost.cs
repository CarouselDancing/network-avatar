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

    // Start is called before the first frame update
    void Start()
    {
        var n = GetComponent<AppNetworkManager>();
        if(client){
            n.StartClient();
            SceneManager.LoadScene(ClientSceneName, LoadSceneMode.Additive);
        }
        if(host){
            n.StartHost();
            SceneManager.LoadScene(ClientSceneName, LoadSceneMode.Additive);
        }
        if(server){
            n.StartServer();
            SceneManager.LoadScene(ServerSceneName, LoadSceneMode.Additive);
            
        }
    }

}
