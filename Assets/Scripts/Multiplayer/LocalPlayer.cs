using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class LocalPlayer : NetworkBehaviour {

    [SerializeField]
    GameObject buttons, menus;

    /*private void Start()
    {
        SaveLoad.Load(SaveCallBack);
    }*/

    private bool doneyet = false;

    [RPC]
    public void GetOtherPlayer(PlayerMain pmtemp)
    {
        BMMulti.PlayerSet(pmtemp, 2);
    }

    // Use this for initialization
    void Start ()
    {
        //GetComponent<PlayerMain>().SetPlayer(GameObject.Find("SinglePlayer").GetComponent<PlayerMain>().GetPlayer());

        //SaveLoad.Load(SaveCallBack);
        if (isLocalPlayer)
            {
                GetComponent<PlayerMain>().SetPlayer(GameObject.Find("SinglePlayer").GetComponent<PlayerMain>().GetPlayer());
                GetComponent<MBattlePlayer>().enabled = true;
                BMMulti.PlayerSet(GetComponent<PlayerMain>(), 1);

                //BMMulti.SetLocal(GetComponent<PlayerMain>());

                doneyet = true;
            }
            else
            {
            //BMMulti.PlayerSet(GetComponent<PlayerMain>(), 2);
                GetComponent<NetworkView>().RPC("GetOtherPlayer", RPCMode.AllBuffered, GameObject.Find("SinglePlayer").GetComponent<PlayerMain>());
                
            
                buttons.layer = 2;
                menus.layer = 2;

                foreach (Transform child in buttons.transform)
                {
                    child.gameObject.layer = 2;
                }
                foreach (Transform child in menus.transform)
                {
                    child.gameObject.layer = 2;
                    foreach (Transform child2 in child)
                        child2.gameObject.layer = 2;
                }
                doneyet = true;
            }
	}

    public void SaveCallBack()
    {

    }

}
