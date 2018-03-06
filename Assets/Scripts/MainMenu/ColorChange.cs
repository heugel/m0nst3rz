using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour
{

    //public GameObject minimap;
    public string curS;
    public string nexS;
    public ColorState[] states;
    private ColorState curstate, nextstate, nextstateU, nextstateD;

    public float transrate = 1f;

    private SpriteRenderer spr;
    //private GameObject player;
    private Color tempcol;
    //private float y;
    private int stateref=0;
    //public bool changebg = true;
    public float arb = 0;
    private bool arbmax = false;
    //private bool hasset = false;

    // Use this for initialization
    void Start()
    {
        curstate = states[5];
        stateref = 5;
        //nextstate = states[1];
        spr = GetComponent<SpriteRenderer>();
        spr.enabled = true;
        //player = gameObject;
    }

    void CheckState()
    {

        float y = arb;
        if (y < nextstateD.ylevel && stateref < states.Length - 1) { curstate = nextstate; stateref++; /*hasset = false;*/ }
        else if (y > nextstateU.ylevel && stateref > 0) { curstate = nextstate; stateref--; /*hasset = false;*/ }
        


    }
    // Update is called once per frame
    void Update()
    {
        if (arb >= 500)
            arbmax = true;
        else if (arb <= -500)
            arbmax = false;

        if (arbmax) arb -= .5f;
        else arb += .5f;
            /*if (Input.GetKeyDown(KeyCode.M)) //maybe make it so you can't change if paused or dead
            {
                minimap.SetActive(!minimap.activeSelf);
            }
            if (changebg)
                minimap.GetComponent<Camera>().backgroundColor = tempcol;*/
            /*if (player == null)
            {
                player = gameObject;
                float y = player.transform.position.y;
                float curmin = Mathf.Abs(states[0].ylevel - y);
                curstate = states[0];
                stateref = 0;
                for (int i = 1; i < states.Length; i++)
                {
                    if (Mathf.Abs(states[i].ylevel - y) < curmin)
                    {
                        curmin = Mathf.Abs(states[i].ylevel - y);
                        curstate = states[i];
                        stateref = i;
                    }
                }
            }*/
            //if (!hasset)
            //{
            if (stateref > 0)
            {
                nextstateU = states[stateref - 1];
            }
            else nextstateU = curstate;
        if (stateref < states.Length - 1)
        {
            nextstateD = states[stateref + 1];
        }
        else nextstateD = curstate;

        if (arb > curstate.ylevel)
            nextstate = nextstateU;
        else nextstate = nextstateD;

        //hasset = true;
        //}


        CheckState();
        curS = curstate.name;
        nexS = nextstate.name;
        tempcol = Color.Lerp(curstate.color, nextstate.color, ((arb - curstate.ylevel)) / ((nextstate.ylevel - curstate.ylevel)));
        //tempcol = curstate.color;
        tempcol.a = 1f;
        //if (!hasset)
        //{
        //hasset = true;
        spr.color = tempcol;
        //}

    }


    [System.Serializable]
    public class ColorState
    {
        public string name;
        public Color color;
        public float ylevel;

    }
}
