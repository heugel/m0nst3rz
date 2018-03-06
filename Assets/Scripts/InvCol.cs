using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvCol : MonoBehaviour {

    public GameObject itemmenu;
    //public GameObject close;

    public static bool IsActive() { return instance != null; }
    private static InvCol curInstance;
    private static InvCol instance { get { return curInstance; } }


    public static void SetActive(bool set)
    {
        instance.itemmenu.SetActive(set);
        if (set)
            instance.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        else
            instance.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    public static bool isActive()
    {
        return instance.itemmenu.activeSelf;
    }
    // Use this for initialization
    void Start()
    {
        if (curInstance == null) { curInstance = this; }
    }
}
