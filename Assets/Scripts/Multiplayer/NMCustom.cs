using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class NMCustom : NetworkManager {

    public GameObject hostbut, joinbut, disbut, jointxt, mainmenu;

    public void StartupHost()
    {
        SetPort();
        //hostbut.SetActive(false);
        //joinbut.SetActive(false);
        //jointxt.SetActive(false);
        //disbut.SetActive(true);
        //mainmenu.SetActive(false);
        NetworkManager.singleton.StartHost();

    }

    public void JoinGame()
    {
        //SetAddress();
        string ipAddress = GameObject.Find("InputFieldIP").transform.FindChild("Text").GetComponent<Text>().text;
        if (ipAddress == "") ipAddress = "" + 7777;
        NetworkManager.singleton.networkAddress = ipAddress;
        
        SetPort();

        //hostbut.SetActive(false);
        //joinbut.SetActive(false);
        //jointxt.SetActive(false);
        //disbut.SetActive(true);
        //mainmenu.SetActive(false);
        NetworkManager.singleton.StartClient();
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    void SetAddress()
    {
        string ipAddress = GameObject.Find("InputFieldIP").transform.FindChild("Text").GetComponent<Text>().text;
        if (ipAddress == "") ipAddress = "" + 7777;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    public void DisconBut()
    {
        //hostbut.SetActive(true);
        //joinbut.SetActive(true);
        //jointxt.SetActive(true);
        //disbut.SetActive(false);
        //mainmenu.SetActive(true);
    }
    /*void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            SetUpMenuSceneButtons();
        }
        else if (level==4)
        {
            SetUpOtherSceneButtons();
        }
    }

    void SetUpMenuSceneButtons()
    {
        GameObject.Find("Host").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Host").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("Join").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Join").GetComponent<Button>().onClick.AddListener(JoinGame);
    }

    void SetUpOtherSceneButtons()
    {
        GameObject.Find("Discon").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Discon").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }*/
}
