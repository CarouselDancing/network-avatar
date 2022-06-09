using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AutoClientOrHost : MonoBehaviour
{
    public bool host;
    public bool server;
    public bool client;
    // Start is called before the first frame update
    void Start()
    {
        var n = GetComponent<NetworkManager>();
        if(client)n.StartClient();
        if(host)n.StartHost();
        if(server)n.StartServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
