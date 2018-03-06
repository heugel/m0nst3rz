using UnityEngine;
using System.Collections;

public abstract class Move : MonoBehaviour {

    public SubMove themove;

    [System.Serializable]
    public class SubMove
    {
        public string Name;
        public Types.Type type;
        public double basedam;
        public double hurtselfdam;
        public bool healself;
        public bool hurtself;
        public bool dodamage;
        public bool multihit;
        public int maxhits;
        public double MHP;
        public int PP;
        public int curPP;
        public double critp;
        public double hitp;
        public MoveType mt;
        public Monster.Status status;
        public int statlen;
        public bool self;
        public bool statoverride;
        public string descr;

        public SubMove()
        {
            Name = "";
            type = Types.Type.none;
            basedam = 0;
            hurtselfdam = 0;
            healself = false;
            hurtself = false;
            dodamage = true;
            multihit = false;
            maxhits = 1;
            MHP = 0;
            PP = 1;
            curPP = 0;
            critp = 0;
            hitp = 0;
            mt = MoveType.none;
            status = Monster.Status.none;
            statoverride = false;
            statlen = 0;
            self = false;
            descr = "";
        }
        public SubMove(SubMove copy)
        {
            Name = copy.Name;
            type = copy.type;
            basedam = copy.basedam;
            hurtselfdam = copy.hurtselfdam;
            healself = copy.healself;
            hurtself = copy.hurtself;
            dodamage = copy.dodamage;
            multihit = copy.multihit;
            maxhits = copy.maxhits;
            MHP = copy.MHP;
            PP = copy.PP;
            curPP = copy.PP;
            critp = copy.critp;
            hitp = copy.hitp;
            mt = copy.mt;
            status = copy.status;
            statoverride = copy.statoverride;
            statlen = copy.statlen;
            self = copy.self;
            descr = copy.descr;
        }

        public bool Use()
        {
            curPP--;
            if (curPP < 0)
            {
                curPP = 0;
                return false;
            }
            return true;
        }

        public double GiveDam()
        {
            return basedam * (double)Random.Range(.7f, 1.3f);
        }


        public void Replenish() { curPP = PP; }

        public bool Hit()
        {
            int test = (int)Random.value * 100;
            if (test <= hitp) return true;
            else return false;
        }

        public bool Crit()
        {
            int test = (int)(Random.value * 100);
            if (test <= critp) return true;
            else return false;
        }

    }

    public enum MoveType
    {
        none,
        proj,
        phys,
        beam,
        nrg
    }

    public abstract double GiveDam();

}
