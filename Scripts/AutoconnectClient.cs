using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AutoconnectClient : MonoBehaviour
{
    public NetworkManager networkManager;
    void Start()
    {
        networkManager.StartClient();
    }

   
}
