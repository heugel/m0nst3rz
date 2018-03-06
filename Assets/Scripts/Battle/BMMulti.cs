using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class BMMulti : MonoBehaviour
{
    //[RPC]
    /*public static void SetOtherPlayer(PlayerMain pmtemp)
    {
        //pmsendtemp = pmtemp;
        PlayerSet(pmtemp, 2);
    }

    public static void SetLocal(PlayerMain pmtemp)
    {
        instance.PMLOCAL = pmtemp;
    }*/
    /*[RPC]
    public PlayerMain SendPlayer()
    {
        return GetComponent<PlayerMain>();
    }*/

    public PlayerMain player1m, player2m;//, PMLOCAL;
    public PlayerMain.Player player1, player2;
    private bool turn = false;
    public Monster.SubMonster p2mon = new Monster.SubMonster();
    public Monster.SubMonster p1mon = new Monster.SubMonster();


    public GameObject bar1, bar2;
    public TextMesh health1, health2;


    //need ???
    public static bool IsActive() { return instance != null; }
    private static BMMulti curInstance;
    private static BMMulti instance { get { return curInstance; } }

    public static Monster.SubMonster GetYou(int player)
    {
        if (player == 1) return instance.p1mon;
        else if (player == 2) return instance.p2mon;
        else return new Monster.SubMonster();
    }

    public static void SetYou(Monster.SubMonster newmon,int player)
    {
        if (player == 1) instance.p1mon = newmon;
        else if (player == 2) instance.p2mon = newmon;
        else newmon = new Monster.SubMonster();
    }

    public static void TurnSet(bool set)
    {
        instance.turn = set;
    }
    public static bool TurnGet() { return instance.turn; }

    public static void PlayerSet(PlayerMain pm, int i)
    {
        if (i == 1)
        {
            instance.player1m = pm;
            instance.player1 = pm.GetPlayer();
            instance.p1mon = instance.player1.moninv.FirstActive();
        }
        else if (i == 2)
        {
            instance.player2m = pm;
            instance.player2 = pm.GetPlayer();
            instance.p2mon = instance.player2.moninv.FirstActive();
        }
    }

    public static PlayerMain.Player PlayerPick(int i)
    {
        if (i == 1) return instance.player1;
        else if (i == 2) return instance.player2;
        else return null;
    }
    public static PlayerMain MPlayerPick(int i)
    {
        if (i == 1) return instance.player1m;
        else if (i == 2) return instance.player2m;
        else return null;
    }

    public enum TurnType
    {
        none,
        attack,
        change
    }

    public class AtkTurn
    {
        public Monster.SubMonster atk;
        public Monster.SubMonster def;
        public Move.SubMove atkmove;

        public AtkTurn()
        {
            atk = new Monster.SubMonster();
            def = new Monster.SubMonster();
            atkmove = new Move.SubMove();
        }
    }
    private AtkTurn atkturn = new AtkTurn();
    private Move.SubMove turnmove1 = new Move.SubMove();
    private Move.SubMove turnmove2 = new Move.SubMove();
    private Monster.SubMonster changeto1 = new Monster.SubMonster();
    private Monster.SubMonster changeto2 = new Monster.SubMonster();
    public static void SetMove(Move.SubMove newmove, int player)
    {
        if (player == 1)
            instance.turnmove1 = newmove;
        else if (player == 2)
            instance.turnmove2 = newmove;
    }
    public static void SetNewMon(Monster.SubMonster newmon, int player)
    {
        if (player == 1)
            instance.changeto1 = newmon;
        else if (player == 2)
            instance.changeto2 = newmon;
    }

    private int CheckFirst()
    {
        if (p1mon.speed > p2mon.speed)
        {
            return 1;
        }
        else if (p1mon.speed == p2mon.speed)
        {
            if ((int)UnityEngine.Random.Range((int)0, (int)2) == 0)
            {
                return 1;
            }
        }

        return 2;
    }

    private bool turn1 = false;
    private bool turn2 = false;
    private bool playerdied1 = false;
    private bool playerdied2 = false;
    IEnumerator CoDoTurn(TurnType[] tt)
    {
        int playerfirst = CheckFirst();

        if (playerfirst==1)
        {
            turn1 = true;
            StartCoroutine("YouTurn", tt[0]);
            while (turn1)
            {
                yield return new WaitForEndOfFrame();
            }
            //SaveLoad.Save(instance.SaveCallBack);
            turn2 = true;
            StartCoroutine("MonTurn",tt[1]);
            while (turn2)
            {
                yield return new WaitForEndOfFrame();
            }
            //SaveLoad.Save(instance.SaveCallBack);
        }
        else if (playerfirst ==2)
        {
            turn2 = true;
            StartCoroutine("MonTurn",tt[1]);
            while (turn2)
            {
                yield return new WaitForEndOfFrame();
            }

            turn1 = true;
            StartCoroutine("YouTurn", tt[0]);
            while (turn1)
            {
                yield return new WaitForEndOfFrame();
            }
            //SaveLoad.Save(instance.SaveCallBack);
            /*if (!playerdied)
            {
                youturn = true;
                StartCoroutine("YouTurn", tt);
                while (youturn)
                {
                    yield return new WaitForEndOfFrame();
                }
            }*/
            //playerdied = false;
            //SaveLoad.Save(instance.SaveCallBack);
        }
    }
    IEnumerator YouTurn(TurnType tt)
    {
        switch (tt)
        {
            case TurnType.none:
                break;
            case TurnType.attack:
                atkturn.atk = p1mon;
                atkturn.def = p2mon;
                atkturn.atkmove = turnmove1;
                usingmove = true;
                StartCoroutine("UseMoveCo", atkturn);
                //UseMove(p1mon, p2mon, instance.turnmove1);
                while (usingmove)
                {
                    yield return new WaitForEndOfFrame();
                }
                UpHealth();
                break;
            case TurnType.change:
                p1mon = changeto1;
                changeto1 = new Monster.SubMonster();
                UpHealth();
                TheQueue.AddQueue("P1 has changed m0nst3rz to " + p1mon.Name + "!");
                break;
        }

        while (TheQueue.QCount() > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        if (!p1mon.isDead())
        {
            p1mon.StatLenUpdate();
            while (TheQueue.QCount() > 0)
            {
                yield return new WaitForEndOfFrame();
            }
            UpHealth();
        }

        if (p1mon.isDead() && p2mon.isDead())
        {

            TheQueue.AddQueue("Both m0nst3rz have died!");
            player1.moninv.RemoveMonster(p1mon);
            p1mon = new Monster.SubMonster();
            player2.moninv.RemoveMonster(p2mon);
            p2mon = new Monster.SubMonster();
            if (player1.moninv.HasAnActive()&&player2.moninv.HasAnActive())
            {
                TheQueue.curdead = true;
                while (p1mon.Name == ""||p2mon.Name=="")
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            else if (!player1.moninv.HasAnActive() && !player2.moninv.HasAnActive())
            {
                TheQueue.AddQueue("It's a tie!!!");
            }
            else if (!player2.moninv.HasAnActive())
            {
                TheQueue.AddQueue("P1 out of m0nst3rz!");
                TheQueue.AddQueue("P2 wins!!!");

            }
            else if (!player1.moninv.HasAnActive())
            {
                TheQueue.AddQueue("P2 out of m0nst3rz!");
                TheQueue.AddQueue("P1 wins!!!");

            }
            yield break;
        }

        else if (p1mon.isDead())
        {
            player1.moninv.RemoveMonster(p1mon);
            if (player1.moninv.HasAnActive())
            {
                //player1.moninv.RemoveMonster(p1mon);
                p1mon = new Monster.SubMonster();
                TheQueue.curdead = true;
                while (p1mon.Name == "")
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                //player1.moninv.RemoveMonster(p1mon);
                p1mon = new Monster.SubMonster();
                TheQueue.AddQueue("P1 is out of m0nst3rz!!!");
                TheQueue.AddQueue("P2 Wins!");
                //TheQueue.gohome = true; //DISCONNECT
                yield break;
            }
        }

        else if (p2mon.isDead())
        {
            player2.moninv.RemoveMonster(p2mon);
            if (player2.moninv.HasAnActive())
            {
                //player2.moninv.RemoveMonster(p2mon);
                p2mon = new Monster.SubMonster();
                TheQueue.curdead = true;
                while (p2mon.Name == "")
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                //player1.moninv.RemoveMonster(p1mon);
                p2mon = new Monster.SubMonster();
                TheQueue.AddQueue("P2 is out of m0nst3rz!!!");
                TheQueue.AddQueue("P1 Wins!");
                //TheQueue.gohome = true; //DISCONNECT
                yield break;
            }
        }

        if (TheQueue.QCount() == 0)
            turn1 = false;
    }


    IEnumerator MonTurn(TurnType tt)
    {
        switch (tt)
        {
            case TurnType.none:
                break;
            case TurnType.attack:
                atkturn.atk = p2mon;
                atkturn.def = p1mon;
                atkturn.atkmove = turnmove2;
                usingmove = true;
                StartCoroutine("UseMoveCo", atkturn);
                while (TheQueue.QCount() > 0)
                {
                    yield return new WaitForEndOfFrame();
                }
                UpHealth();
                break;
            case TurnType.change:
                p2mon = changeto2;
                changeto2 = new Monster.SubMonster();
                UpHealth();
                TheQueue.AddQueue("P2 has changed m0nst3rz to " + p2mon.Name + "!");
                break;
        }

        while (TheQueue.QCount() > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        if (!p2mon.isDead())
        {
            p2mon.StatLenUpdate();
            while (TheQueue.QCount() > 0)
            {
                yield return new WaitForEndOfFrame();
            }
            UpHealth();
        }

        if (p1mon.isDead() && p2mon.isDead())
        {

            TheQueue.AddQueue("Both m0nst3rz have died!");
            player1.moninv.RemoveMonster(p1mon);
            p1mon = new Monster.SubMonster();
            player2.moninv.RemoveMonster(p2mon);
            p2mon = new Monster.SubMonster();
            if (player1.moninv.HasAnActive() && player2.moninv.HasAnActive())
            {
                TheQueue.curdead = true;
                while (p1mon.Name == "" || p2mon.Name == "")
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            else if (!player1.moninv.HasAnActive() && !player2.moninv.HasAnActive())
            {
                TheQueue.AddQueue("It's a tie!!!");
            }
            else if (!player2.moninv.HasAnActive())
            {
                TheQueue.AddQueue("P1 out of m0nst3rz!");
                TheQueue.AddQueue("P2 wins!!!");

            }
            else if (!player1.moninv.HasAnActive())
            {
                TheQueue.AddQueue("P2 out of m0nst3rz!");
                TheQueue.AddQueue("P1 wins!!!");

            }
            yield break;
        }

        else if (p1mon.isDead())
        {
            player1.moninv.RemoveMonster(p1mon);
            if (player1.moninv.HasAnActive())
            {
                //player1.moninv.RemoveMonster(p1mon);
                p1mon = new Monster.SubMonster();
                TheQueue.curdead = true;
                while (p1mon.Name == "")
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                //player1.moninv.RemoveMonster(p1mon);
                p1mon = new Monster.SubMonster();
                TheQueue.AddQueue("P1 is out of m0nst3rz!!!");
                TheQueue.AddQueue("P2 Wins!");
                //TheQueue.gohome = true; //DISCONNECT
                yield break;
            }
        }

        else if (p2mon.isDead())
        {
            player2.moninv.RemoveMonster(p2mon);
            if (player2.moninv.HasAnActive())
            {
                //player2.moninv.RemoveMonster(p2mon);
                p2mon = new Monster.SubMonster();
                TheQueue.curdead = true;
                while (p2mon.Name == "")
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                //player1.moninv.RemoveMonster(p1mon);
                p2mon = new Monster.SubMonster();
                TheQueue.AddQueue("P2 is out of m0nst3rz!!!");
                TheQueue.AddQueue("P1 Wins!");
                //TheQueue.gohome = true; //DISCONNECT
                yield break;
            }
        }

        if (TheQueue.QCount() == 0)
            turn2 = false;
    }

    private bool usingmove = false;
    IEnumerator UseMoveCo(AtkTurn theturn)
    {
        double dam = 0;
        double origdam = 0;
        int paracheck = 100;
        int numhits = 0;
        bool multihit = false;
        bool mixedupfail = false;

        Monster.SubMonster atk = theturn.atk;
        Monster.SubMonster def = theturn.def;
        Move.SubMove themove = theturn.atkmove;

        int typecheck = Types.CheckWeakness(themove.type, def.Type);
        bool statimmune = false;
        bool dodamage = themove.dodamage;
        bool movefailed = false;

        if (themove.self) statimmune = true;
        if (themove.status == Monster.Status.none) statimmune = true;

        TheQueue.AddQueue(atk.Name + " used " + themove.Name + "!");

        //separate
        if (atk.status.Contains(Monster.Status.paralyze) || atk.status.Contains(Monster.Status.frozen))
        {
            paracheck = (int)UnityEngine.Random.Range((int)0, (int)3);
            if (paracheck != 0)
            {

                TheQueue.AddQueue(atk.Name + "'s status prevented it from moving!");
                usingmove = false;
                yield break;
            }
        }

        else if (atk.status.Contains(Monster.Status.asleep))
        {
            if ((int)UnityEngine.Random.Range((int)0, (int)5) == 0)
            {
                TheQueue.AddQueue(atk.Name + " is still sleeping!");
                TheQueue.AddQueue(atk.Name + " just woke up");
                atk.SetStatus(Monster.Status.none, 1);
                UpStats();
            }
            else
            {
                TheQueue.AddQueue(atk.Name + " is still sleeping!");
                usingmove = false;
                yield break;
            }
        }

        themove.Use();

        if (def.status.Contains(Monster.Status.evading))
        {
            if ((themove.dodamage || (!themove.self && themove.status != Monster.Status.none)))
            {
                if ((int)UnityEngine.Random.Range((int)0, (int)4) > 0)
                {
                    TheQueue.AddQueue(def.Name + " evaded the move!");
                    if (themove.hurtself)
                    {

                        TheQueue.AddQueue(atk.Name + " hurt itself!");
                        while (TheQueue.QCount() > 0)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        TheQueue.AddQueue("It caused " + themove.hurtselfdam + " damage!");
                        UpHealth();
                        atk.Damage(themove.hurtselfdam);
                        if (atk.isDead())
                            TheQueue.AddQueue(atk.Name + " died!!!");
                    }
                    usingmove = false;
                    yield break;
                }
            }
        }
        switch (def.thequirk.quirk)
        {
            case Monster.QuirkType.none:
                break;
            case Monster.QuirkType.bouncer:
                if (themove.mt == def.thequirk.afmt && themove.dodamage)
                {
                    TheQueue.AddQueue(def.Name + "'s " + def.thequirk.Name + " caused the move to bounce back!");//,false);

                    atk.Damage(dam / 2);
                    while (TheQueue.QCount() > 0)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    UpHealth();
                    dodamage = false;
                    movefailed = true;
                    while (TheQueue.QCount() > 0)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    UpHealth();
                    TheQueue.AddQueue("It caused " + dam / 2 + " damage!");//,true);
                    if (!themove.hurtself)
                    {
                        usingmove = false;
                        yield break;
                    }
                }
                break;
            case Monster.QuirkType.dodger:
                if (themove.mt == def.thequirk.afmt && themove.dodamage)
                {
                    if ((int)UnityEngine.Random.Range(0, 2) == 0) break;
                    else
                    {
                        dodamage = false;
                        movefailed = true;
                        TheQueue.AddQueue(def.Name + "'s " + def.thequirk.Name + " caused them to dodge!");//,false);
                        if (!themove.hurtself)
                        {
                            usingmove = false;
                            yield break;
                        }
                    }
                }
                break;
            case Monster.QuirkType.givestatus:
                if (themove.mt == def.thequirk.afmt && themove.dodamage && !atk.status.Contains(def.thequirk.givestatus))
                {
                    if (atk.thequirk.quirk == Monster.QuirkType.statimmune)
                    {
                        if (atk.thequirk.givestatus == def.thequirk.givestatus)
                        {
                            break;
                        }
                        else
                        {
                            TheQueue.AddQueue("The move caused a status rebound!");
                            atk.SetStatus(def.thequirk.givestatus, def.thequirk.statlen);
                            UpStats();
                        }
                    }
                    else
                    {
                        TheQueue.AddQueue("The move caused a status rebound!");
                        atk.SetStatus(def.thequirk.givestatus, def.thequirk.statlen);
                        //unless the attacker is immune to this status effect
                    }
                }

                break;
            case Monster.QuirkType.healer:
                if (def.thequirk.aftype == themove.type && themove.dodamage)
                {
                    dodamage = false;
                    movefailed = true;
                    TheQueue.AddQueue("The moved caused " + def.Name + " to heal!");
                    def.Heal(def.thequirk.healhurt);
                    if (!themove.hurtself)
                    {
                        usingmove = false;
                        yield break;
                    }
                }
                break;
            case Monster.QuirkType.healerMT:
                if (def.thequirk.afmt == themove.mt && themove.dodamage)
                {
                    movefailed = true;
                    dodamage = false;
                    //add quirk name
                    TheQueue.AddQueue("The moved caused " + def.Name + " to heal!");
                    def.Heal(def.thequirk.healhurt);
                    if (!themove.hurtself)
                    {
                        usingmove = false;
                        yield break;
                    }
                }
                break;
            case Monster.QuirkType.hurter:
                if (def.thequirk.afmt == themove.mt && themove.dodamage)
                {
                    //add quirk name
                    TheQueue.AddQueue("The moved caused " + atk.Name + " to take " + def.thequirk.healhurt + " damage!");
                    atk.Damage(def.thequirk.healhurt);
                    while (TheQueue.QCount() > 0)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    UpHealth();
                    //if (atkr.isDead()) return;
                }
                break;
            case Monster.QuirkType.hurterT:
                if (def.thequirk.aftype == themove.type && themove.dodamage)
                {
                    //add quirk name
                    TheQueue.AddQueue("The moved caused " + atk.Name + " to take damage!");
                    atk.Damage(def.thequirk.healhurt);
                    while (TheQueue.QCount() > 0)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    UpHealth();
                    //if (atkr.isDead()) return;
                }
                break;
            case Monster.QuirkType.mtimmune:
                if (themove.mt == def.thequirk.afmt && themove.dodamage)
                {
                    movefailed = true;
                    //add quirk name
                    dodamage = false;
                    TheQueue.AddQueue(def.Name + " is immune to this type of move!");
                    if (!themove.hurtself)
                    {
                        usingmove = false;
                        yield break;
                    }
                }
                break;
            case Monster.QuirkType.statimmune:
                if (themove.status == def.thequirk.givestatus && !themove.self)
                {
                    //add quirk name
                    statimmune = true;
                    TheQueue.AddQueue(def.Name + " is immune to " + themove.Name + "'s status effect!");
                }
                break;
            case Monster.QuirkType.typeimmune:
                if (themove.type == def.thequirk.aftype && themove.dodamage)
                {
                    //add quirk name
                    dodamage = false;
                    movefailed = true;
                    TheQueue.AddQueue(def.Name + " is immune to this type!");
                    if (!themove.hurtself)
                    {
                        usingmove = false;
                        yield break;
                    }
                }
                break;
        }


        if (themove.Hit())
        {
            if (dodamage)
            {
                dam = themove.GiveDam();
                dam *= (1 + (atk.ATK / 200));
                //dam = System.Math.Round(dam, 2);
                if (atk.status.Contains(Monster.Status.doubled))
                {
                    dam *= 2;
                    TheQueue.AddQueue(atk.Name + "'s status caused double damage!");
                }
                if (atk.status.Contains(Monster.Status.halved))
                {
                    dam *= .5;
                    TheQueue.AddQueue(atk.Name + "'s status caused half damage!");
                }
                if (atk.status.Contains(Monster.Status.high))
                {
                    dam *= .7;
                    TheQueue.AddQueue(atk.Name + "'s status caused reduced damage!");
                }
                if (atk.status.Contains(Monster.Status.mixedup))
                {
                    if ((int)UnityEngine.Random.Range((int)0, (int)2) == 0)
                    {
                        TheQueue.AddQueue(atk.Name + " hurt itself instead!");
                        atk.Damage(dam / 2);
                        while (TheQueue.QCount() > 0)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        UpHealth();
                        TheQueue.AddQueue("It did " + dam / 2 + " damage!");
                        mixedupfail = true;
                        dodamage = false;
                    }
                }

                if (def.status.Contains(Monster.Status.annoyed) && !mixedupfail)
                {
                    //print("poopoo");
                    dam *= 1.4;
                    TheQueue.AddQueue(def.Name + "'s status caused increased damage!");
                }
                if (def.status.Contains(Monster.Status.cold) && !mixedupfail)
                {
                    dam *= 1.4;
                    TheQueue.AddQueue(def.Name + "'s status caused increased damage!");
                }

                if (dodamage)
                {
                    if (typecheck == 2)
                    {
                        TheQueue.AddQueue("MEGA WEAKNESS! INSTAKILL!");
                        dam *= 3000;
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
                        //def.Damage(dam);
                        //TheQueue.AddQueue("It did " + dam + " damage!");
                    }
                    else if (typecheck == 0)
                    {
                        if (themove.Crit())
                        {
                            dam *= 1.5;
                            TheQueue.AddQueue("Critical hit!");
                        }
                        //Damage(dam);
                        //TheQueue.AddQueue("It did " + dam + " damage!");
                    }
                    else if (typecheck == -1)
                    {
                        TheQueue.AddQueue("Not very effective");
                        dam /= 1.5;
                        //Damage(dam);
                        //TheQueue.AddQueue("It did " + dam + " damage!");
                    }
                    else if (typecheck == -2)
                    {
                        movefailed = true;
                        statimmune = true;
                        dodamage = false;
                        mixedupfail = true;
                        TheQueue.AddQueue("The move had no effect!");
                    }
                    else if (typecheck == -3)
                    {
                        movefailed = true;
                        dodamage = false;
                        mixedupfail = true;
                        TheQueue.AddQueue("The move caused " + dam + " points of healing!");
                        def.Heal(dam);
                        statimmune = true;
                    }

                    if (!mixedupfail)
                    {
                        dam = Math.Round(dam, 2);
                        origdam = dam;

                        if (themove.multihit && UnityEngine.Random.Range(0, 100f) < themove.MHP)
                        {
                            multihit = true;
                            TheQueue.AddQueue("Multi hit move!");
                        }

                        if (!multihit)
                        {
                            TheQueue.AddQueue("It did " + dam + " damage!");
                            def.Damage(dam);
                            while (TheQueue.QCount() > 0)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                            UpHealth();
                        }
                        else if (multihit)
                        {
                            TheQueue.AddQueue("First hit did " + dam + " damage!");
                            def.Damage(dam);
                            while (TheQueue.QCount() > 0)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                            UpHealth();

                            numhits = 2;
                            dam = origdam * (double)UnityEngine.Random.Range(.9f, 1.1f);
                            dam = Math.Round(dam, 2);

                            TheQueue.AddQueue("Second hit did " + dam + " damage!");
                            def.Damage(dam);
                            while (TheQueue.QCount() > 0)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                            UpHealth();
                            if (themove.maxhits > 2)
                            {
                                for (int i = 3; i <= themove.maxhits; i++)
                                {
                                    if (UnityEngine.Random.Range(0, 100f) < themove.MHP)
                                    {
                                        dam = origdam * (double)UnityEngine.Random.Range(.8f, 1.2f);
                                        dam = Math.Round(dam, 2);
                                        def.Damage(dam);
                                        while (TheQueue.QCount() > 0)
                                        {
                                            yield return new WaitForEndOfFrame();
                                        }
                                        UpHealth();
                                        numhits = i;
                                        TheQueue.AddQueue("Hit " + i + " did " + dam + " damage!");
                                    }
                                    else break;
                                }
                            }
                            TheQueue.AddQueue(numhits + " hits!");
                        }
                    }
                }


            }



            if (!def.isDead() && !statimmune && themove.status != Monster.Status.none)
                def.SetStatus(themove.status, themove.statlen);
            //else if (!def.isDead() && !statimmune && themove.status != Monster.Status.none && def.status != Monster.Status.none && themove.statoverride)
            //def.SetStatus(themove.status, themove.statlen);
            else if (!def.isDead() && themove.status != Monster.Status.none && !themove.self && statimmune)
                TheQueue.AddQueue(themove.Name + "'s status effect failed!");

            UpStats();

            if (themove.hurtself)
            {

                TheQueue.AddQueue(atk.Name + " hurt itself!");
                TheQueue.AddQueue("It caused " + themove.hurtselfdam + " damage!");
                //health bar animation
                atk.Damage(themove.hurtselfdam);
                while (TheQueue.QCount() > 0)
                {
                    yield return new WaitForEndOfFrame();
                }
                UpHealth();
            }

            else if (themove.healself && !movefailed)
            {
                TheQueue.AddQueue(themove.Name + " caused " + atk.Name + " to heal!");
                TheQueue.AddQueue(atk.Name + " healed " + themove.hurtselfdam + " health!");
                //health bar animation
                atk.Heal(themove.hurtselfdam);

            }

            if (themove.self && !atk.status.Contains(themove.status) && !atk.isDead())
            {
                TheQueue.AddQueue(themove.Name + " gave " + atk.Name + " a status effect!");
                atk.SetStatus(themove.status, themove.statlen);
                statimmune = true;
                //if(themove.healself)
                //return;
            }
            /*else if (themove.self && atk.status != Monster.Status.none && themove.statoverride && !atk.isDead())
            {
                TheQueue.AddQueue(themove.Name + " gave " + atk.Name + " a status effect!");
                atk.SetStatus(themove.status, themove.statlen);
                statimmune = true;
            }*/
            else if (themove.self && atk.status.Contains(themove.status) && !atk.isDead())
            {
                TheQueue.AddQueue(atk.Name + " failed to give itself a status effect!");

            }

            if (def.isDead())
            {
                TheQueue.AddQueue(def.Name + " died!!!");

            }
            if (atk.isDead())
            {
                TheQueue.AddQueue(atk.Name + " died!!!");
            }

        }
        else
        {
            TheQueue.AddQueue(atk.Name + "'s attack missed!");
        }

        UpStats();

        usingmove = false;
    }
    /*public static void UseMove(Monster.SubMonster atk, Monster.SubMonster def, Move.SubMove themove)
    {
        double dam = 0;
        double origdam = 0;
        int paracheck = 100;
        int numhits = 0;
        bool multihit = false;
        bool movefailed = false;

        
        TheQueue.AddQueue(atk.Name + " used " + themove.Name + "!");

        if (atk.status.Contains(Monster.Status.paralyze) || atk.status.Contains(Monster.Status.frozen))
        {
            paracheck = (int)UnityEngine.Random.Range((int)0, (int)3);
            if (paracheck != 0)
            {
                TheQueue.AddQueue(atk.Name + "'s status prevented it from moving!");
                return;
            }
        }

        else if (atk.status.Contains(Monster.Status.asleep))
        {
            if ((int)UnityEngine.Random.Range((int)0, (int)5) == 0)
            {
                TheQueue.AddQueue(atk.Name + " is still sleeping!");
                TheQueue.AddQueue(atk.Name + " just woke up");
                atk.SetStatus(Monster.Status.none, 1);

            }
            else
            {
                TheQueue.AddQueue(atk.Name + " is still sleeping!");
                return;
            }
        }

        themove.Use();

        if (def.status.Contains(Monster.Status.evading))
        {
            if ((themove.dodamage || (!themove.self && themove.status != Monster.Status.none)))
            {
                if ((int)UnityEngine.Random.Range((int)0, (int)4) > 0)
                {
                    TheQueue.AddQueue(def.Name + " evaded the move!");
                    if (themove.hurtself)
                    {

                        TheQueue.AddQueue(atk.Name + " hurt itself!");
                        TheQueue.AddQueue("It caused " + themove.hurtselfdam + " damage!");
                        //health bar animation
                        atk.Damage(themove.hurtselfdam);
                        if (atk.isDead())
                            TheQueue.AddQueue(atk.Name + " died!!!");
                    }
                    return;
                }
            }
        }

        if (themove.multihit && UnityEngine.Random.Range(0, 100f) < themove.MHP)
        {
            multihit = true;
            TheQueue.AddQueue("Multi hit move!");
        }

        if (themove.Hit())
        {
            if (themove.dodamage)
            {
                dam = themove.GiveDam();
                dam *= (1 + (atk.ATK / 200));
                //dam = System.Math.Round(dam, 2);
                if (atk.status.Contains(Monster.Status.doubled))
                {
                    dam *= 2;
                    TheQueue.AddQueue(atk.Name + "'s status caused double damage!");
                }
                else if (atk.status.Contains(Monster.Status.halved))
                {
                    dam *= .5;
                    TheQueue.AddQueue(atk.Name + "'s status caused half damage!");
                }
                else if (atk.status.Contains(Monster.Status.high))
                {
                    dam *= .7;
                    TheQueue.AddQueue(atk.Name + "'s status caused reduced damage!");
                }
                else if (atk.status.Contains(Monster.Status.mixedup))
                {
                    if ((int)UnityEngine.Random.Range((int)0, (int)2) == 0)
                    {
                        TheQueue.AddQueue(atk.Name + " hurt itself instead!");
                        atk.Damage(dam);
                        TheQueue.AddQueue("It did " + dam + " damage!");
                        movefailed = true;
                    }
                }

                if (def.status.Contains(Monster.Status.annoyed)&&!movefailed)
                {
                    //print("poopoo");
                    dam *= 1.4;
                    TheQueue.AddQueue(def.Name + "'s status caused increased damage!");
                }
                else if (def.status.Contains(Monster.Status.cold)&&!movefailed)
                {
                    dam *= 1.4;
                    TheQueue.AddQueue(def.Name + "'s status caused increased damage!");
                }
            }

            if (!movefailed)
            {
                dam = Math.Round(dam, 2);
                origdam = dam;
                //def.ReceiveDam(atk, themove, dam);

                if (multihit)
                {
                    numhits = 2;
                    dam = origdam * (double)UnityEngine.Random.Range(.9f, 1.1f);
                    dam = Math.Round(dam, 2);
                    //def.ReceiveDam(atk, themove, dam);
                    if (themove.maxhits > 2)
                    {
                        for (int i = 3; i <= themove.maxhits; i++)
                        {
                            if (UnityEngine.Random.Range(0, 100f) < themove.MHP)
                            {
                                dam = origdam * (double)UnityEngine.Random.Range(.8f, 1.2f);
                                dam = Math.Round(dam, 2);
                                //def.ReceiveDam(atk, themove, dam);
                                numhits = i;
                            }
                            else break;
                        }
                    }
                    TheQueue.AddQueue(numhits + " hits!");
                }
            }

            if (def.isDead())
            {
                TheQueue.AddQueue(def.Name + " died!!!");

            }
            if (atk.isDead())
            {
                TheQueue.AddQueue(atk.Name + " died!!!");
            }

        }
        else
        {
            TheQueue.AddQueue(atk.Name + "'s attack missed!");
        }
    }*/
    //need ???
    void Awake()
    {
        if (curInstance == null) { curInstance = this; }
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        p1ready = false;
        p2ready = false;
        //player1 = player1m.GetPlayer();
        //player2 = player2m.GetPlayer();
    }


    public static bool p1ready = false;
    public static bool p2ready = false;
    public static TurnType[] TTs = new TurnType[2];
    public static void InitTurn(TurnType temp,int player)
    {
        if (player == 1)
        {
            p1ready = true;
            TTs[0] = temp;
        }
        else if (player == 2)
        {
            p2ready = true;
            TTs[1] = temp;
        }

        if (p1ready && p2ready)
        {
            instance.StartCoroutine("CoDoTurn", TTs);
            p1ready = false;
            p2ready = false;
        }
    }


    public static void UpHealth()
    {
        if (instance.p1mon != null)
        {
            if (instance.p1mon.Name != "")
            {
                instance.bar1.transform.localScale = new Vector3(.7472217f * (float)(instance.p1mon.curhealth / instance.p1mon.maxhealth), .2752157f, 1);
                instance.health1.text = instance.p1mon.curhealth + "/" + instance.p1mon.maxhealth;
            }
            else
            {
                instance.bar1.transform.localScale = new Vector3(0, 0, 1);
                instance.health1.text = "";
            }

        }
        else
        {
            instance.bar1.transform.localScale = new Vector3(0, 0, 1);
            instance.health1.text = "";
        }

        if (instance.p2mon != null)
        {
            if (instance.p2mon.Name != "")
            {
                instance.bar2.transform.localScale = new Vector3(.7472217f * (float)(instance.p2mon.curhealth / instance.p2mon.maxhealth), .2752157f, 1);
                instance.health1.text = instance.p2mon.curhealth + "/" + instance.p2mon.maxhealth;
            }
            else
            {
                instance.bar2.transform.localScale = new Vector3(0, 0, 1);
                instance.health1.text = "";
            }

        }
        else
        {
            instance.bar2.transform.localScale = new Vector3(0, 0, 1);
            instance.health1.text = "";
        }
    }

    public GameObject p1monstats, p2monstats;

    private void UpStats()
    {
        if (p1mon != null)
        {
            if (p1mon.Name != "")
            {
                if (p1mon.status.Count == 0)
                {
                    p1monstats.GetComponent<TextMesh>().text = "";
                    p1monstats.SetActive(false);
                }
                else
                {
                    p1monstats.SetActive(true);
                    p1monstats.GetComponent<TextMesh>().text = "";
                    for (int i = 0; i < p1mon.status.Count; i++)
                    {
                        p1monstats.GetComponent<TextMesh>().text += p1mon.PrintStatus(i);
                        p1monstats.GetComponent<TextMesh>().text += "\n";
                    }
                }
            }
            else
            {
                p1monstats.GetComponent<TextMesh>().text = "";
                p1monstats.SetActive(false);
            }
        }
        else
        {
            p1monstats.GetComponent<TextMesh>().text = "";
            p1monstats.SetActive(false);
        }

        if (p2mon != null)
        {
            if (p2mon.Name != "")
            {
                if (p2mon.status.Count == 0)
                {
                    p2monstats.GetComponent<TextMesh>().text = "";
                    p2monstats.SetActive(false);
                }
                else
                {
                    p2monstats.SetActive(true);
                    p2monstats.GetComponent<TextMesh>().text = "";
                    for (int i = 0; i < p2mon.status.Count; i++)
                    {
                        p2monstats.GetComponent<TextMesh>().text += p2mon.PrintStatus(i);
                        p2monstats.GetComponent<TextMesh>().text += "\n";
                    }
                }
            }
            else
            {
                p2monstats.GetComponent<TextMesh>().text = "";
                p2monstats.SetActive(false);
            }
        }
        else
        {
            p2monstats.GetComponent<TextMesh>().text = "";
            p2monstats.SetActive(false);
        }

    }

    public SpriteRenderer left, right;
    private bool p1init = false;
    private bool p2init = false;
    void Update()
    {

        /*if (!doneyet)
        {
            you = player1.moninv.FirstActive();
            if (you != null)
            {
                if (you.Name != "")
                    doneyet = true;
            }

            if (opon == null) doneyet = false;
            else if (opon.Name == "") doneyet = false;
            else doneyet = true;

            UpHealth();
        }*/
        //print(player1m == null);
        if (player1m != null && !p1init)
        {
            p1init = true;
        }
        else if (player1m == null)
        {
            p1init = false;
            SetYou(null, 1);
        }

        if (player2m != null && !p2init)
        {
            p2init = true;
        }
        else if (player2m == null)
        {
            p2init = false;
            SetYou(null, 2);
        }

        if (p1init && p2init)
        {
            TurnSet(true);
        }
        else
        {
            TurnSet(false);
            //Waiting for players...
            
        }
        

        if (p1mon != null)
            left.sprite = Resources.Load("MonsterSprites/" + p1mon.imagepath, typeof(Sprite)) as Sprite;
        else if (p1mon == null)
            left.sprite = null;

        if (p2mon != null)
            right.sprite = Resources.Load("MonsterSprites/" + p2mon.imagepath, typeof(Sprite)) as Sprite;
        else if (p2mon == null)
            right.sprite = null;

    }


    private void SaveCallBack()
    {

    }
}