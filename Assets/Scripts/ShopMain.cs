using UnityEngine;
using System.Collections;

public class ShopMain : MonoBehaviour {

    public GameObject shop;


    public static bool IsActive() { return instance != null; }
    private static ShopMain curInstance;
    private static ShopMain instance { get { return curInstance; } }
    //private bool activated = false;

    public static void ShopSet(bool set) { instance.shop.SetActive(set);}
    public static bool isActive() { return instance.shop.activeSelf; }


	// Use this for initialization
	void Start ()
    {
        if (curInstance == null) { curInstance = this; }
    }
	
	/*// Update is called once per frame
	void Update ()
    {
        if (activated)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
	}*/
}
