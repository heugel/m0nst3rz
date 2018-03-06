using UnityEngine;
using System.Collections;

public class TextColorChange : MonoBehaviour
{

    //public GameObject minimap;
    public string curS;
    public string nexS;
    public TColorState[] states;
    private TColorState curstate, nextstate, nextstateU, nextstateD;

    public float transrate = 1f;

    private TextMesh spr;
    //private GameObject player;
    private Color tempcol;
    //private float y;
    private int stateref = 0;
    //public bool changebg = true;
    public float arb = 0;
    private bool arbmax = false;
    //private bool hasset = false;

    // Use this for initialization
    void Start()
    {
        curstate = states[2];
        stateref = 2;
        //nextstate = states[1];
        spr = GetComponent<TextMesh>();
        //spr.enabled = true;
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
        if (arb >= 200)
            arbmax = true;
        else if (arb <= -300)
            arbmax = false;

        if (arbmax) arb -= .5f;
        else arb += .5f;

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
    public class TColorState
    {
        public string name;
        public Color color;
        public float ylevel;

    }
}
