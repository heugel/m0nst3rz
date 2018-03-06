using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MUpMenu : MonoBehaviour
{

    public GameObject monmenu, atkmenu;

    public curMenu upnow = curMenu.none;

    public void SetMenu(curMenu newmen)
    {
        upnow = newmen;
    }


    public enum curMenu
    {
        none,
        mon,
        atk
    }

    // Update is called once per frame
    void Update()
    {
        switch (upnow)
        {
            case curMenu.none:
                monmenu.SetActive(false);
                atkmenu.SetActive(false);
                break;
            case curMenu.atk:
                monmenu.SetActive(false);
                atkmenu.SetActive(true);
                break;
            case curMenu.mon:
                monmenu.SetActive(true);
                atkmenu.SetActive(false);
                break;
        }
    }
}
