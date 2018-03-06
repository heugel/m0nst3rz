using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpMenu : MonoBehaviour {

    public GameObject monmenu, itemmenu, atkmenu;

    public static curMenu upnow = curMenu.none;

    public static bool IsActive() { return instance != null; }
    private static UpMenu curInstance;
    private static UpMenu instance { get { return curInstance; } }

    public static void SetMenu(curMenu newmen)
    {
        upnow = newmen;
    }


    public enum curMenu
    {
        none,
        item,
        mon,
        atk
    }
	// Use this for initialization
	void Start ()
    {
        if (curInstance == null) { curInstance = this; }
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (upnow)
        {
            case curMenu.none:
                monmenu.SetActive(false);
                itemmenu.SetActive(false);
                atkmenu.SetActive(false);
                break;
            case curMenu.item:
                monmenu.SetActive(false);
                itemmenu.SetActive(true);
                atkmenu.SetActive(false);
                break;
            case curMenu.atk:
                monmenu.SetActive(false);
                itemmenu.SetActive(false);
                atkmenu.SetActive(true);
                break;
            case curMenu.mon:
                monmenu.SetActive(true);
                itemmenu.SetActive(false);
                atkmenu.SetActive(false);
                break;
        }
	}
}
