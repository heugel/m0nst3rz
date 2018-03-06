using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{


    public GameObject backbut,tradebut,battlebut;
    //public GameObject close;

    public static bool IsActive() { return instance != null; }
    private static Portal curInstance;
    private static Portal instance { get { return curInstance; } }


    public static void SetActive(bool set)
    {
        instance.backbut.SetActive(set);
        instance.tradebut.SetActive(set);
        instance.battlebut.SetActive(set);

    }
    public static bool isActive()
    {
        return instance.backbut.activeSelf;
    }
    // Use this for initialization
    void Start()
    {
        if (curInstance == null) { curInstance = this; }
    }
    public void LoadBattle()
    {

    }
    public void LoadTrade()
    {

    }
    public void ShutUp()
    {
        instance.backbut.SetActive(false);
        instance.tradebut.SetActive(false);
        instance.battlebut.SetActive(false);
    }
    // Update is called once per frame
    //void Update () {}
}
