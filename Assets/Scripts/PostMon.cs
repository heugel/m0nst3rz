using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine;

public class PostMon : MonoBehaviour {

    public Text namedec;
    public Text moves;
    public Text health;
    public Text speed;
    public Text kills;

    public void Erase()
    {
        namedec.text = "";
        moves.text = "";
        health.text = "";
        speed.text = "";
        kills.text = "";
    }
    public void Set(Monster.SubMonster mon)
    {
        namedec.text = mon.Name + "---" + Types.TypePrinter(mon.Type) + " type";
        namedec.text += "\n\n";
        namedec.text += mon.desc;

        speed.text = "Attack: " + mon.ATK + "\n";
        speed.text += "Speed: " + mon.speed;

        kills.text = "Kills: " + mon.kills;

        moves.text = "Moves:\n";
        moves.text += mon.moves[0].Name + "---" + Types.TypePrinter(mon.moves[0].type) + "---" + mon.moves[0].curPP + "/" + mon.moves[0].PP + "---"+mon.moves[0].descr+"\n";
        moves.text += mon.moves[1].Name + "---" + Types.TypePrinter(mon.moves[1].type) + "---" + mon.moves[1].curPP + "/" + mon.moves[1].PP + "---" + mon.moves[1].descr + "\n";
        moves.text += mon.moves[2].Name + "---" + Types.TypePrinter(mon.moves[2].type) + "---" + mon.moves[2].curPP + "/" + mon.moves[2].PP + "---" + mon.moves[2].descr;

        health.text = "Health:\n\n";
        health.text += "" + Math.Round(mon.curhealth,2) + " / " + Math.Round(mon.maxhealth,2);

    }

}
