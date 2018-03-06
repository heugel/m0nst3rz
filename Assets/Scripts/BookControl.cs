using UnityEngine;
using System.Collections;

public class BookControl : MonoBehaviour
{

    public GameObject book;


    public static bool IsActive() { return instance != null; }
    private static BookControl curInstance;
    private static BookControl instance { get { return curInstance; } }
    //private bool activated = false;

    public static void BookSet(bool set) { instance.book.SetActive(set); }
    public static bool isActive() { return instance.book.activeSelf; }


    // Use this for initialization
    void Start()
    {
        if (curInstance == null) { curInstance = this; }
    }

}
