using UnityEngine;
using System.Collections;

public class MICollider : MonoBehaviour {


    public GameObject monmenu;
    //public GameObject close;

    public static bool IsActive() { return instance != null; }
    private static MICollider curInstance;
    private static MICollider instance { get { return curInstance; } }


    public static void SetActive(bool set)
    {
        instance.monmenu.SetActive(set);
        if (set)
            instance.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        else
            instance.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

    }
    public static bool isActive()
    {
        return instance.monmenu.activeSelf;
    }
        // Use this for initialization
     void Start () {
        if (curInstance == null) { curInstance = this; }
    }
	
	// Update is called once per frame
	//void Update () {}
}
