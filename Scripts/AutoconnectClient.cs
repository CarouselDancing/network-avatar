
using UnityEngine;
using Mirror;
using kcp2k;



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

        config = ClientConfig.GetInstance();
        networkManager.networkAddress = config.url;
        transport.Port = (ushort)config.port;
        networkManager.StartClient();
    }

   
}
