using UnityEngine;
using System;
using System.Collections.Generic;

//[System.Serializable]
//[System.NonSerialized]
public abstract class Monster : MonoBehaviour {

    public SubMonster themonster;
    
    //public Status curStatus;

    public abstract void ReceiveDam();

    public enum Status
    {
        none,
        poison,
        paralyze,
        burn,
        high,
        doubled,
        halved,
        annoyed,
        frozen,
        cold,
        random,
        evading,
        asleep,
        mixedup
    }
    //cure


    [System.Serializable]
    public class SubMonster
    {
        public string Name;
        public int Number;
        public Types.Type Type;
        public double avghealth;
        public double maxhealth;
        public double curhealth;
        public double speed;
        public double ATK;
        public Types.Type[] compatmovetypes;
        public string imagepath = "";
        public bool discovered = false;
        public Move.SubMove[] moves = new Move.SubMove[3];
        public string fingerprint;
        public List<Status> status = new List<Status>();
        public Quirk thequirk;
        private int statlen;
        public string desc;
        public int kills;

        private int poison=0;
        private int paralyze=0;
        private int burn=0;
        private int high=0;
        private int doubled=0;
        private int halved=0;
        private int annoyed=0;
        private int frozen=0;
        private int cold=0;
        private int evading=0;
        private int asleep=0;
        private int mixedup=0;


        public SubMonster()
        {
            Name = "";
            Number = 0;
            Type = Types.Type.none;
            avghealth = 0;
            maxhealth = 0;
            curhealth = 0;
            speed = 10;
            ATK = 10;
            compatmovetypes = new Types.Type[3];
            imagepath = "";
            discovered = false;
            moves = new Move.SubMove[3];
            fingerprint = "";
            status = new List<Status>();
            thequirk = new Quirk();
            desc = "";
            statlen = 0;
            kills = 0;

            poison = 0;
            paralyze = 0;
            burn = 0;
            high = 0;
            doubled = 0;
            halved = 0;
            annoyed = 0;
            frozen = 0;
            cold = 0;
            evading = 0;
            asleep = 0;
            mixedup = 0;
    }

        public SubMonster(SubMonster copy)
        {
            Name = copy.Name;
            Number = copy.Number;
            Type = copy.Type;
            avghealth = copy.avghealth;
            maxhealth = copy.maxhealth;
            curhealth = copy.curhealth;
            speed = copy.speed;
            ATK = copy.ATK;
            compatmovetypes = copy.compatmovetypes;
            imagepath = copy.imagepath;
            discovered = copy.discovered;
            moves = copy.moves;
            fingerprint = "";
            status = copy.status;
            thequirk = new Quirk(copy.thequirk);
            desc = copy.desc;
            statlen = 0;
            kills = copy.kills;

            poison = copy.poison;
            paralyze = copy.paralyze;
            burn = copy.burn;
            high = copy.high;
            doubled = copy.doubled;
            halved = copy.halved;
            annoyed = copy.annoyed;
            frozen = copy.frozen;
            cold = copy.cold;
            evading = copy.evading;
            asleep = copy.asleep;
            mixedup = copy.mixedup;
        }

        public double CurHealth() { return curhealth; }
        //public double MaxHealth() { return maxhealth; }
        public void GetKill()
        {
            kills++;

            if (maxhealth < 150)
            {
                maxhealth += .2;
                maxhealth = Math.Round(maxhealth, 2);
            }

            if (ATK < 200)
            {
                ATK += .02;
                ATK = Math.Round(ATK, 2);
            }
        }

        public void ClearStatus()
        {
            status = new List<Status>();
            poison = 0;
            paralyze = 0;
            burn = 0;
            high = 0;
            doubled = 0;
            halved = 0;
            annoyed = 0;
            frozen = 0;
            cold = 0;
            evading = 0;
            asleep = 0;
            mixedup = 0;
        }

        public void StatLenUpdate()
        {
            //add the switch from SetStatus to actually do the effects
            if (status.Count == 0)
            {
                return;
            }
            if (isDead())
            {
                ClearStatus();
                return;
            }

            if (status.Contains(Status.annoyed) && annoyed > 0)
            {
                annoyed--;
                if (annoyed == 0)
                {
                    status.Remove(Status.annoyed);
                    TheQueue.AddQueue(Name + " is no longer annoyed!");
                }
            }
            if (status.Contains(Status.burn) && burn > 0)
            {
                burn--;
                TheQueue.AddQueue(Name + "'s burn caused 2 damage!");//,true);
                Damage(2);
                if (burn == 0)
                {
                    status.Remove(Status.burn);
                    TheQueue.AddQueue(Name + " is no longer burned!");
                }
            }
            if (status.Contains(Status.cold) && cold > 0)
            {
                cold--;
                if (cold == 0)
                {
                    status.Remove(Status.cold);
                    TheQueue.AddQueue(Name + " is no longer cold!");
                }
            }
            if (status.Contains(Status.doubled) && doubled > 0)
            {
                doubled--;
                if (doubled == 0)
                {
                    status.Remove(Status.doubled);
                    TheQueue.AddQueue(Name + " is no longer POWERED UP!");
                }
            }
            if (status.Contains(Status.frozen) && frozen > 0)
            {
                frozen--;
                if (frozen == 0)
                {
                    status.Remove(Status.frozen);
                    TheQueue.AddQueue(Name + " is no longer frozen!");
                }
            }
            if (status.Contains(Status.halved) && halved > 0)
            {
                halved--;
                if (halved == 0)
                {
                    status.Remove(Status.halved);
                    TheQueue.AddQueue(Name + " is no longer dumbed down!");
                }
            }
            if (status.Contains(Status.high) && high > 0)
            {
                high--;
                if (high == 0)
                {
                    status.Remove(Status.high);
                    TheQueue.AddQueue(Name + " is no longer high!");
                }
            }
            if (status.Contains(Status.paralyze) && paralyze > 0)
            {
                paralyze--;
                if (paralyze == 0)
                {
                    status.Remove(Status.paralyze);
                    TheQueue.AddQueue(Name + " is no longer paralyzed!");
                }
            }
            if (status.Contains(Status.poison) && poison > 0)
            {
                poison--;
                TheQueue.AddQueue(Name + "'s poison caused " + Math.Round(CurHealth() / 10, 2) + " damage!");
                Damage(Math.Round(CurHealth() / 10, 2));
                if (poison == 0)
                {
                    status.Remove(Status.poison);
                    TheQueue.AddQueue(Name + " is no longer poisoned!");
                }
            }
            if (status.Contains(Status.evading) && evading > 0)
            {
                evading--;
                if (evading == 0)
                {
                    status.Remove(Status.evading);
                    TheQueue.AddQueue(Name + " is no longer evading!");
                }
            }
            if (status.Contains(Status.mixedup) && mixedup > 0)
            {
                mixedup--;
                if (mixedup == 0)
                {
                    status.Remove(Status.mixedup);
                    TheQueue.AddQueue(Name + " is no longer mIxEd uP!");
                }
            }
            if (status.Contains(Status.asleep) && asleep > 0)
            {
                asleep--;
                if (asleep == 0)
                {
                    status.Remove(Status.asleep);
                    TheQueue.AddQueue(Name + " is no longer asleep!");
                }
            }
            

            if (isDead())
            {
                ClearStatus();
                TheQueue.AddQueue(Name + " died from its status effect!");
                return;
            }

        }
        public void MaxHealthInit(bool avg)
        {
            /*if (!avg)
            {
                double randi = avghealth * (double)UnityEngine.Random.Range(.8f, 1.2f);
                maxhealth = randi;
            }
            else maxhealth = avghealth;

            curhealth = maxhealth;*/
            if (avg)
            {
                maxhealth = avghealth;
            }
            else
            {
                maxhealth = (int)UnityEngine.Random.Range(5, 60);
                if (Type == Types.Type.sad) maxhealth *= 1.5;
            }

            curhealth = maxhealth;


        }

        public void SpeedInit()
        {
            speed = (double)UnityEngine.Random.Range(.01f, 100);
            speed = System.Math.Round(speed, 2);
            ATK = (double)UnityEngine.Random.Range(0f, 100);
            ATK = System.Math.Round(ATK, 2);
        }

        public string Description()
        {
            return desc;
        }

        public bool KnowsMove(Move.SubMove test)
        {
            for(int i=0; i < 3; i++)
            {
                if (moves[i].Name == test.Name)
                    return true;
            }
            return false;
        }

        public void Damage(double dam)
        {
            curhealth -= dam;
            if (curhealth < 0) curhealth = 0;
        }

        public void Heal(double heal)
        {
            if (heal < 0) curhealth = maxhealth;
            else curhealth += heal;

            if (curhealth > maxhealth) curhealth = maxhealth;
        }

        public bool isDead() { return curhealth <= 0; }

        public void SetStatus(Status newstat, int len)
        {
            if(thequirk.quirk==QuirkType.statimmune && thequirk.givestatus == newstat)
            {
                //add switch for every status
                TheQueue.AddQueue(Name + " is immune to this status effect!");//,false);
                return;
            }

            if (status.Contains(newstat))
            {
                TheQueue.AddQueue(Name + " is already " + PrintStatus(newstat) + "!");
                return;
            }

            int randomboy;


            status.Add(newstat);
            
            switch (newstat)
            {
                case Status.annoyed:
                    annoyed = len;
                    TheQueue.AddQueue(Name + " is now annoyed!");//,false);
                    break;
                case Status.burn:
                    burn = len;
                    TheQueue.AddQueue(Name + " is now burned!");//,false);
                    break;
                case Status.cold:
                    cold = len;
                    TheQueue.AddQueue(Name + " is now cold!");//,false);
                    break;
                case Status.doubled:
                    doubled = len;
                    TheQueue.AddQueue(Name + " is now POWERED UP!");//,false);
                    break;
                case Status.frozen:
                    frozen = len;
                    TheQueue.AddQueue(Name + " is now frozen!");//,false);
                    break;
                case Status.halved:
                    halved = len;
                    TheQueue.AddQueue(Name + " is now dumbed down!");//,false);
                    break;
                case Status.high:
                    high = len;
                    TheQueue.AddQueue(Name + " is now high!");//,false);
                    break;
                case Status.paralyze:
                    paralyze = len;
                    TheQueue.AddQueue(Name + " is now paralyzed!");//,false);
                    break;
                case Status.poison:
                    poison = len;
                    TheQueue.AddQueue(Name + " is now poisoned!");//,false);
                    break;
                case Status.none:
                    ClearStatus();
                    TheQueue.AddQueue(Name + "'s status returned to normal!");//,false);
                    ClearStatus();
                    break;
                case Status.random:
                    randomboy = (int)UnityEngine.Random.Range((int)0, (int)11);
                    if (randomboy == 0) SetStatus(Status.annoyed, len);
                    else if (randomboy == 1) SetStatus(Status.burn, len);
                    else if (randomboy == 2) SetStatus(Status.cold, len);
                    else if (randomboy == 3) SetStatus(Status.doubled, len);
                    else if (randomboy == 4) SetStatus(Status.frozen, len);
                    else if (randomboy == 5) SetStatus(Status.halved, len);
                    else if (randomboy == 6) SetStatus(Status.high, len);
                    else if (randomboy == 7) SetStatus(Status.paralyze, len);
                    else if (randomboy == 8) SetStatus(Status.poison, len);
                    else if (randomboy == 9) SetStatus(Status.evading, len);
                    else if (randomboy == 10) SetStatus(Status.asleep, len);
                    else SetStatus(Status.evading, len); //to prevent setting status as "random" or "none"
                    break;
                case Status.evading:
                    evading = len;
                    TheQueue.AddQueue(Name + " is now evading!");//,false);
                    break;
                case Status.mixedup:
                    mixedup = len;
                    TheQueue.AddQueue(Name + " is now mIxEd uP!");//,false);
                    break;
                case Status.asleep:
                    asleep = len;
                    TheQueue.AddQueue(Name + " is now asleep!");//,false);
                    break;
            }
        }

        public void Discover()
        {
            discovered = true;
        }
        public string PrintStatus(int statusint)
        {
            if (statusint >= status.Count) return "";

            return PrintStatus(status[statusint]);
        }
        public string PrintStatus(Status teststatus)
        {
            switch (teststatus)
            {
                case Status.annoyed:
                    return "annoyed";
                case Status.burn:
                    return "burned";
                case Status.cold:
                    return "cold";
                case Status.doubled:
                    return "POWERED UP";
                case Status.frozen:
                    return "frozen";
                case Status.halved:
                    return "dumbed down";
                case Status.high:
                    return "high";
                case Status.paralyze:
                    return "paralyzed";
                case Status.poison:
                    return "poisoned";
                case Status.none:
                    return "none_error";
                case Status.random:
                    return "random_error";
                case Status.evading:
                    return "evading";
                case Status.mixedup:
                    return "mIxEd uP";
                case Status.asleep:
                    return "asleep";
            }

            return "";
        }


        //3 instakill
        //2 = super effective
        //1 = normal hit
        //0 = nothing
        //-1 = bounce back
        //-2 = heal
        //-3 = givestatus NOT ANYMORE
        //-4 = hurter NOTANYMORE

        //<-50 = givestatus
        //<-100 = hurter
        /*public void ReceiveDam(Monster.SubMonster atkr, Move.SubMove themove, double dam)
        {
            int typecheck = Types.CheckWeakness(themove.type, Type);
            bool statimmune = false;
            bool dodamage = themove.dodamage;
            bool movefailed = false;

            //if (themove.basedam == 0) dodamage = false;
            if (themove.self) statimmune = true;
            if (themove.status == Status.none) statimmune = true;


            switch (thequirk.quirk)
            {
                case Monster.QuirkType.none:
                    break;
                case Monster.QuirkType.bouncer:
                    if (themove.mt == thequirk.afmt && themove.dodamage)
                    {
                        TheQueue.AddQueue(Name+"'s "+thequirk.Name+" caused the move to bounce back!");//,false);

                        atkr.Damage(dam/2);
                        dodamage = false;
                        movefailed = true;
                        TheQueue.AddQueue("It caused " + dam / 2 + " damage!");//,true);
                        if (!themove.hurtself)
                            return;
                    }
                    break;
                case Monster.QuirkType.dodger:
                    if (themove.mt == thequirk.afmt && themove.dodamage)
                    {
                        if ((int)UnityEngine.Random.Range(0, 2) == 0) break;
                        else
                        {
                            dodamage = false;
                            movefailed = true;
                            TheQueue.AddQueue(Name + "'s " + thequirk.Name + " caused them to dodge!");//,false);
                            if (!themove.hurtself)
                                return;
                        }
                    }
                    break;
                case Monster.QuirkType.givestatus: 
                    if (themove.mt == thequirk.afmt && themove.dodamage&&atkr.status==Status.none) 
                    {
                        if (atkr.thequirk.quirk == QuirkType.statimmune)
                        {
                            if (atkr.thequirk.givestatus == thequirk.givestatus)
                            {
                                break;
                            }
                            else if(atkr.status == Status.none)
                            {
                                TheQueue.AddQueue("The move caused a status rebound!");
                                atkr.SetStatus(thequirk.givestatus, thequirk.statlen);
                            }
                        }
                        else if (atkr.status == Status.none)
                        {
                            TheQueue.AddQueue("The move caused a status rebound!");
                            atkr.SetStatus(thequirk.givestatus, thequirk.statlen);
                            //unless the attacker is immune to this status effect
                        }
                    }
                    
                    break;
                case Monster.QuirkType.healer:
                    if (thequirk.aftype == themove.type && themove.dodamage)
                    {
                        dodamage = false;
                        movefailed = true;
                        TheQueue.AddQueue("The moved caused " + Name + " to heal!");
                        Heal(thequirk.healhurt);
                        if (!themove.hurtself)
                            return;
                    }
                    break;
                case Monster.QuirkType.healerMT:
                    if (thequirk.afmt == themove.mt && themove.dodamage)
                    {
                        movefailed = true;
                        dodamage = false;
                        //add quirk name
                        TheQueue.AddQueue("The moved caused " + Name + " to heal!");
                        Heal(thequirk.healhurt);
                        if (!themove.hurtself)
                            return;
                    }
                    break;
                case Monster.QuirkType.hurter:
                    if (thequirk.afmt == themove.mt && themove.dodamage)
                    {
                        //add quirk name
                        TheQueue.AddQueue("The moved caused " + atkr.Name + " to take " + thequirk.healhurt+ " damage!");
                        atkr.Damage(thequirk.healhurt);
                        //if (atkr.isDead()) return;
                    }
                    break;
                case Monster.QuirkType.hurterT:
                    if (thequirk.aftype == themove.type && themove.dodamage)
                    {
                        //add quirk name
                        TheQueue.AddQueue("The moved caused " + atkr.Name + " to take damage!");
                        atkr.Damage(thequirk.healhurt);
                        //if (atkr.isDead()) return;
                    }
                    break;
                case Monster.QuirkType.mtimmune:
                    if(themove.mt == thequirk.afmt && themove.dodamage)
                    {
                        movefailed = true;
                        //add quirk name
                        dodamage = false;
                        TheQueue.AddQueue(Name + " is immune to this type of move!");
                        if (!themove.hurtself)
                            return;
                    }
                    break;
                case Monster.QuirkType.statimmune:
                    if (themove.status == thequirk.givestatus && !themove.self)
                    {
                        //add quirk name
                        statimmune = true;
                        TheQueue.AddQueue(Name + " is immune to " + themove.Name + "'s status effect!");
                    }
                    break;
                case Monster.QuirkType.typeimmune:
                    if(themove.type == thequirk.aftype && themove.dodamage)
                    {
                        //add quirk name
                        dodamage = false;
                        movefailed = true;
                        TheQueue.AddQueue(Name + " is immune to this type!");
                        if (!themove.hurtself)
                            return;
                        //return 0;
                    }
                    break;
            }
            if (dodamage)
            {
                if (typecheck == 2)
                {
                    TheQueue.AddQueue("MEGA WEAKNESS! INSTAKILL!");
                    Damage(maxhealth + 1);
                    //if (!themove.hurtself)
                        //return;
                }
                else if (typecheck == 1)
                {
                    TheQueue.AddQueue("Super effective!");
                    dam *= 2;
                    if (themove.Crit())
                    {
                        dam *= 2;
                        TheQueue.AddQueue("Critical hit!");
                    }
                    Damage(dam);
                    TheQueue.AddQueue("It did " + dam + " damage!");
                    //if (!isDead() && !statimmune && themove.status != Status.none && status == Status.none) SetStatus(themove.status, themove.statlen);

                    //if (!themove.hurtself)
                        //return;
                }
                else if (typecheck == 0)
                {
                    if (themove.Crit())
                    {
                        dam *= 2;
                        TheQueue.AddQueue("Critical hit!");
                    }
                    Damage(dam);
                    TheQueue.AddQueue("It did " + dam + " damage!");
                    //if (!isDead() && !statimmune && themove.status != Status.none && status == Status.none) SetStatus(themove.status, themove.statlen);
                    //if (!themove.hurtself)
                        //return;
                }
                else if (typecheck == -1)
                {
                    TheQueue.AddQueue("Not very effective");
                    dam /= 2;
                    Damage(dam);
                    TheQueue.AddQueue("It did " + dam + " damage!");
                    //if (!isDead() && !statimmune && themove.status != Status.none && status == Status.none) SetStatus(themove.status, themove.statlen);
                    //if (!themove.hurtself)
                        //return;
                }
                else if (typecheck == -2)
                {
                    movefailed = true;
                    statimmune = true;
                    TheQueue.AddQueue("The move had no effect!");
                    //if (!themove.hurtself)
                        //return;
                }
                else if (typecheck == -3)
                {
                    movefailed = true;
                    TheQueue.AddQueue("The move caused " + dam + " points of healing!");
                    Heal(dam);
                    statimmune = true;
                    //if (!themove.hurtself)
                        //return;
                }
            }

            if (!isDead() && !statimmune && themove.status != Status.none && status == Status.none)
                SetStatus(themove.status, themove.statlen);
            else if (!isDead() && !statimmune && themove.status != Status.none && status != Status.none && themove.statoverride)
                SetStatus(themove.status, themove.statlen);
            else if (!isDead() && themove.status != Status.none && !themove.self && (statimmune || status != Status.none))
                TheQueue.AddQueue(themove.Name + "'s status effect failed!");

            if (themove.hurtself)
            {

                    TheQueue.AddQueue(atkr.Name + " hurt itself!");
                    TheQueue.AddQueue("It caused " + themove.hurtselfdam + " damage!");
                    //health bar animation
                    atkr.Damage(themove.hurtselfdam);
                    if (atkr.isDead())
                        return;
                
            }

            if (themove.healself && !movefailed)
            {
                TheQueue.AddQueue(themove.Name + " caused " + atkr.Name + " to heal!");
                TheQueue.AddQueue(atkr.Name + " healed " + themove.hurtselfdam + " health!");
                //health bar animation
                atkr.Heal(themove.hurtselfdam);
                
            }

            if (themove.self && atkr.status==Status.none)
            {
                TheQueue.AddQueue(themove.Name + " gave " + atkr.Name + " a status effect!");
                atkr.SetStatus(themove.status, themove.statlen);
                statimmune = true;
                //if(themove.healself)
                //return;
            }
            else if (themove.self && atkr.status != Status.none && themove.statoverride)
            {
                TheQueue.AddQueue(themove.Name + " gave " + atkr.Name + " a status effect!");
                atkr.SetStatus(themove.status, themove.statlen);
                statimmune = true;
            }
            else if (themove.self && atkr.status != Status.none && !themove.statoverride)
            {
                TheQueue.AddQueue(atkr.Name+" failed to give itself a status effect!");

            }

            //if (!isDead() && !statimmune && themove.status != Status.none) SetStatus(themove.status, themove.statlen);
            //else if (isDead()) MonInv.RemoveMonster(this);




            //return 0;
        }*/

    }

    [System.Serializable]
    public class Quirk
    {
        public string Name;
        public QuirkType quirk;
        public Move.MoveType afmt;
        public Types.Type aftype;
        public double healhurt;
        public Status givestatus;
        public int statlen;
        public string descr;
        //dodge percent change? maybe 50 for affected move type

        public Quirk()
        {
            Name = "";
            quirk = QuirkType.none;
            afmt = Move.MoveType.none;
            aftype = Types.Type.none;
            healhurt = 0;
            givestatus = Status.none;
            statlen = 0;
            descr = "";
        }

        public Quirk(Quirk copy)
        {
            Name = copy.Name;
            quirk = copy.quirk;
            afmt = copy.afmt;
            aftype = copy.aftype;
            healhurt = copy.healhurt;
            givestatus = copy.givestatus;
            statlen = copy.statlen;
            descr = copy.descr;
        }

    }


    public enum QuirkType
    {
        none,
        dodger,
        givestatus,
        healer,
        hurter,
        healerMT,
        hurterT,
        mtimmune,
        typeimmune,
        statimmune,
        bouncer
    }
    //public string Description+quirks

    //public abstract void ReceiveDam();
    /*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/
}
