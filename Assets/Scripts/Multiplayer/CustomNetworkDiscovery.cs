using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class CustomNetworkDiscovery : NetworkDiscovery
{

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("Client discovery received broadcast " + data + " from " + fromAddress);

        var items = data.Split(':');
        if (items.Length == 3 && items[0] == "NetworkManager")
        {
            if (NetworkManager.singleton != null && NetworkManager.singleton.client == null)
            {
                NetworkManager.singleton.networkAddress = fromAddress; //items[1];
                NetworkManager.singleton.networkPort = Convert.ToInt32(items[2]);
                NetworkManager.singleton.StartClient();
            }
        }
    }

}