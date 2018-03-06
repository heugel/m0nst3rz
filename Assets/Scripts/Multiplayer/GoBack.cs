using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GoBack : MonoBehaviour {

    public NetworkManager manager;
    public NetworkDiscovery netdis;

    public void GoMenu()
    {
        manager.StopHost();
        //manager.StopClient();
        if(netdis.running)
            netdis.StopBroadcast();

        SceneManager.LoadScene("MainMenu");
    }
}
