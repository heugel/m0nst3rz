using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class BattleMain3 : MonoBehaviour
{


    public PlayerMain player1m;//, player2m;
    public PlayerMain.Player player1;//, player2;
    private bool turn = false;
    public Monster.SubMonster opon = new Monster.SubMonster();
    public Monster.SubMonster you = new Monster.SubMonster();

    private AtkTurn atkturn = new AtkTurn();

    public GameObject youbar, oponbar;
    public TextMesh youhealth, oponhealth;
    public GameObject youstats, oponstats;


    //need ???
    public static bool IsActive() { return instance != null; }
    private static BattleMain3 curInstance;
    private static BattleMain3 instance { get { return curInstance; } }

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

    public static Monster.SubMonster GetYou()
    {
        return instance.you;
    }
    public static void SetMon(Monster.SubMonster newmon)
    {
        instance.opon = newmon;
    }
    public static void SetYou(Monster.SubMonster newmon)
    {
        instance.you = newmon;
    }

    public static void TurnSet(bool set)
    {
        instance.turn = set;
    }
    public static bool TurnGet() { return instance.turn; }

    public static PlayerMain.Player Player1() { return instance.player1; }
    //public static PlayerMain.Player Player2() { return instance.player2; }

    public enum TurnType
    {
        none,
        item,
        attack,
        esc,
        change
    }

    private Item turnitem = new Item();
    private Move.SubMove turnmove = new Move.SubMove();
    private Monster.SubMonster changeto = new Monster.SubMonster();
    public static void SetMove(Move.SubMove newmove) { instance.turnmove = newmove; }
    public static void SetItem(Item newitm) { instance.turnitem = newitm; }
    public static void SetNewMon(Monster.SubMonster newmon)
    {
        instance.changeto = newmon;
    }

    private bool CheckNet()
    {
        double var1 = opon.curhealth / (1.1 * opon.maxhealth);
        double var2 = -.004 * opon.maxhealth + 1.02;
        double perc = (1 - var1) * (var2);
        perc *= 100;

        if (perc > (double)UnityEngine.Random.Range(0f, 100f))
            return true;
        else return false;
    }
    private bool CheckYouFirst()
    {
        if (opon.speed > you.speed)
        {
            return false;
        }
        else if (instance.opon.speed == instance.you.speed)
        {
            if ((int)UnityEngine.Random.Range((int)0, (int)2) == 0)
            {
                return false;
            }
        }

        return true;
    }

    private bool youturn = false;
    private bool monturn = false;
    private bool playerdied = false;
    IEnumerator CoDoTurn(TurnType tt)
    {
        playerdied = false;
        bool playerfirst = CheckYouFirst();
        if (tt != TurnType.attack) playerfirst = true;

        if (playerfirst)
        {
            youturn = true;
            StartCoroutine("YouTurn", tt);
            while (youturn)
            {
                yield return new WaitForEndOfFrame();
            }
            SaveLoad.Save(instance.SaveCallBack);
            monturn = true;
            StartCoroutine("MonTurn");
            while (monturn)
            {
                yield return new WaitForEndOfFrame();
            }
            SaveLoad.Save(instance.SaveCallBack);
        }
        else
        {
            monturn = true;
            StartCoroutine("MonTurn");
            while (monturn)
            {
                yield return new WaitForEndOfFrame();
            }
            SaveLoad.Save(instance.SaveCallBack);
            if (!playerdied)
            {
                youturn = true;
                StartCoroutine("YouTurn", tt);
                while (youturn)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            else if (playerdied)
            {
                playerdied = false;
                //yield break;
            }
            
            SaveLoad.Save(instance.SaveCallBack);
        }
    }
    IEnumerator YouTurn(TurnType tt)
    {
        int esccheck;

        switch (tt)
        {
            case TurnType.none:
                break;
            case TurnType.item:
                if (instance.turnitem.IT == Item.ItemType.potion)
                {
                    //do animation here
                    //DoPotion function?
                    instance.you.Heal(-1);
                    TheQueue.AddQueue("You used a potion!");
                    while (TheQueue.QCount() > 0)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    UpHealth();
                    TheQueue.AddQueue(instance.you.Name + " is fully healed!");
                    if (instance.you.status.Count==0)
                        instance.you.SetStatus(Monster.Status.none, 1);
                }
                else if (instance.turnitem.IT == Item.ItemType.net)
                {

                    //DoNet
                    //animation
                    //if types match, catch
                    //else if net type is none, 50/50
                    //else if types dont match, dont catch
                    TheQueue.AddQueue("You launched a net!");
                    if (instance.turnitem.nettype == Types.Type.none)
                    {
                        if (!instance.CheckNet())
                        {
                            TheQueue.AddQueue("Your net missed!");
                        }
                        else
                        {
                            if (player1.moninv.IsFull())
                            {
                                TheQueue.AddQueue("You don't have room for this m0nst3r!");
                                TheQueue.goadventure = true;
                                yield break;
                            }
                            else
                            {
                                TheQueue.AddQueue("You've caught the " + instance.opon.Name + "!");
                                player1.moninv.AddMonster(instance.opon);
                                TheQueue.goadventure = true;
                                yield break;
                            }
                        }
                    }
                    else if (instance.turnitem.nettype == instance.opon.Type)
                    {
                        if (player1.moninv.IsFull())
                        {
                            TheQueue.AddQueue("You don't have room for this m0nst3r!");
                            TheQueue.goadventure = true;
                            yield break;
                        }
                        else
                        {
                            TheQueue.AddQueue("You've caught the " + instance.opon.Name + "!");
                            player1.moninv.AddMonster(instance.opon);
                            TheQueue.goadventure = true;
                            yield break;
                        }
                    }
                    else
                    {
                        TheQueue.AddQueue("Your net missed!");
                    }
                }
                break;
            case TurnType.attack:

                atkturn.atk = you;
                atkturn.def = opon;
                atkturn.atkmove = turnmove;

                usingmove = true;
                StartCoroutine("UseMoveCo", atkturn);
                while (usingmove)
                {
                    yield return new WaitForEndOfFrame();
                }
                UpHealth();
                break;
            case TurnType.change:
                you = changeto;
                changeto = new Monster.SubMonster();
                UpHealth();
                TheQueue.AddQueue("You've changed m0nst3rz to " + instance.you.Name + "!");
                break;
            case TurnType.esc:
                esccheck = UnityEngine.Random.Range((int)0, (int)3);
                if (esccheck == 0)
                {
                    TheQueue.AddQueue("You've escaped!");
                    TheQueue.goadventure = true;
                    yield break;
                }
                else
                {
                    TheQueue.AddQueue("Escape attempt failed!");
                }
                break;
        }

        while (TheQueue.QCount() > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        if (!instance.you.isDead())
        {
            instance.you.StatLenUpdate();
            UpStats();
            while (TheQueue.QCount() > 0)
            {
                yield return new WaitForEndOfFrame();
            }
            UpHealth();
        }

        if (instance.you.isDead() && instance.opon.isDead())
        {

            TheQueue.AddQueue("Both m0nst3rz have died!");
            player1.moninv.RemoveMonster(instance.you);
            you = new Monster.SubMonster();
            if (player1.moninv.HasAnActive())
                TheQueue.goadventure = true;
            else
            {
                TheQueue.gohome = true;
            }
            yield break;
        }

        else if (instance.you.isDead())
        {
            player1.moninv.RemoveMonster(instance.you);
            if (player1.moninv.HasAnActive())
            {
                player1.moninv.RemoveMonster(you);
                you = new Monster.SubMonster();
                //instance.deadoponturn = true;
                TheQueue.curdead = true;
                while (you.Name == "")
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                player1.moninv.RemoveMonster(you);
                you = new Monster.SubMonster();
                TheQueue.AddQueue("You're out of m0nst3rz!!!");
                TheQueue.gohome = true;
                yield break;
            }
        }

        else if (instance.opon.isDead())
        {
            you.GetKill();
            TheQueue.AddQueue("You killed the " + instance.opon.Name + "!!!");
            TheQueue.goadventure = true;
            yield break;
            //SceneManager.LoadScene("Adventure");
        }

        if (TheQueue.QCount() == 0)
            youturn = false;
    }


    IEnumerator MonTurn()
    {
        int opmove;
        //bool died = false;

        List<Move.SubMove> potmoves = new List<Move.SubMove>();

        for (int i = 0; i < 3; i++)
        {
            if (opon.moves[i].curPP > 0)
            {
                potmoves.Add(opon.moves[i]);
            }
        }

        if (potmoves.Count > 0)
        {
            opmove = (int)UnityEngine.Random.Range((int)0, (int)potmoves.Count);

            atkturn.atk = opon;
            atkturn.def = you;
            atkturn.atkmove = potmoves[opmove];

            usingmove = true;
            StartCoroutine("UseMoveCo", atkturn);
            while (usingmove)
            {
                yield return new WaitForEndOfFrame();
            }
            UpHealth();
        }
        else
        {
            TheQueue.AddQueue(instance.opon.Name + " can't use any moves!");
        }

        while (TheQueue.QCount() > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        if (instance.you.isDead() && instance.opon.isDead())
        {

            TheQueue.AddQueue("Both m0nst3rz have died!");
            player1.moninv.RemoveMonster(instance.you);
            you = new Monster.SubMonster();
            if (player1.moninv.HasAnActive())
                TheQueue.goadventure = true;
            else
            {
                TheQueue.gohome = true;
            }
            yield break;
        }
        else if (instance.you.isDead())
        {
            player1.moninv.RemoveMonster(instance.you);
            opon.GetKill();
            UpHealth();//check
            if (player1.moninv.HasAnActive())
            {
                //instance.deadoponturn = false;
                player1.moninv.RemoveMonster(you);
                you = new Monster.SubMonster();
                TheQueue.curdead = true;
                playerdied = true;
                while (you.Name == "")
                {
                    yield return new WaitForEndOfFrame();
                }
                //playerdied = false;
            }
            else
            {
                player1.moninv.RemoveMonster(you);
                you = new Monster.SubMonster();
                TheQueue.AddQueue("You're out of m0nst3rz!!!");
                TheQueue.gohome = true;
                yield break;
            }
        }

        while (TheQueue.QCount() > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        opon.StatLenUpdate();
        UpStats();
        while (TheQueue.QCount() > 0)
        {
            yield return new WaitForEndOfFrame();
        }
        UpHealth();

        if (instance.opon.isDead())
        {
            you.GetKill();
            TheQueue.AddQueue("You killed the " + instance.opon.Name + "!!!");
            TheQueue.goadventure = true;
            //deadoponturn = false;
        }

        if (TheQueue.QCount() == 0)
            monturn = false;
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
                while (TheQueue.QCount() > 0)
                {
                    yield return new WaitForEndOfFrame();
                }
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

        dam = themove.GiveDam();
        dam *= (1 + (atk.ATK / 200));
        dam = System.Math.Round(dam, 2);
        //maybe move this switch statement within themove.hit()
        if (themove.dodamage || !statimmune)
        {
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
                                while (TheQueue.QCount() > 0)
                                {
                                    yield return new WaitForEndOfFrame();
                                }
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
                        while (TheQueue.QCount() > 0)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        UpHealth();
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
                        while (TheQueue.QCount() > 0)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        UpHealth();
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
                        dam = System.Math.Round(dam, 2);
                        atk.Damage(dam / 2);
                        while (TheQueue.QCount() > 0)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        UpHealth();
                        TheQueue.AddQueue("It did " + dam / 2 + " damage!");
                        mixedupfail = true;
                        dodamage = false;
                        statimmune = true;
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
                        /*if (themove.Crit())
                        {
                            dam *= 2;
                            TheQueue.AddQueue("Critical hit!");
                        }*/
                        //def.Damage(dam);
                        //TheQueue.AddQueue("It did " + dam + " damage!");
                    }
                    else if (typecheck == 0)
                    {
                        /*if (themove.Crit())
                        {
                            dam *= 1.5;
                            TheQueue.AddQueue("Critical hit!");
                        }*/
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
                            if (themove.Crit())
                            {
                                dam *= 1.5;
                                TheQueue.AddQueue("Critical hit!");
                            }
                            TheQueue.AddQueue("It did " + dam + " damage!");
                            def.Damage(dam);
                            while (TheQueue.QCount() > 0)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
                            
                            UpHealth();
                        }
                        else if (multihit)
                        {
                            if (themove.Crit())
                            {
                                dam *= 1.5;
                                TheQueue.AddQueue("Critical hit!");
                            }
                            TheQueue.AddQueue("First hit did " + dam + " damage!");
                            def.Damage(dam);
                            while (TheQueue.QCount() > 0)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
                            UpHealth();

                            numhits = 2;
                            dam = origdam * (double)UnityEngine.Random.Range(.9f, 1.1f);
                            dam = Math.Round(dam, 2);
                            if (themove.Crit())
                            {
                                dam *= 1.5;
                                TheQueue.AddQueue("Critical hit!");
                            }
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
                                        if (themove.Crit())
                                        {
                                            dam *= 1.5;
                                            TheQueue.AddQueue("Critical hit!");
                                        }
                                        def.Damage(dam);
                                        while (TheQueue.QCount() > 0)
                                        {
                                            yield return new WaitForEndOfFrame();
                                        }
                                        UpHealth();
                                        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
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

            while (TheQueue.QCount() > 0)
            {
                yield return new WaitForEndOfFrame();
            }
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
                while (TheQueue.QCount() > 0)
                {
                    yield return new WaitForEndOfFrame();
                }
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


        //UpStats();

        usingmove = false;
    }

    //need ???
    void Awake()
    {
        if (curInstance == null) { curInstance = this; }
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        player1 = player1m.GetPlayer();
        //player2 = player2m.GetPlayer();
    }

    public static void InitTurn(TurnType temp)
    {
        instance.StartCoroutine("CoDoTurn", temp);
    }


    public static void UpHealth()
    {
        if (instance.you != null)
        {
            if (instance.you.Name != "")
            {
                instance.youbar.transform.localScale = new Vector3(.7472217f * (float)(instance.you.curhealth / instance.you.maxhealth), .2752157f, 1);
                instance.youhealth.text = instance.you.curhealth + "/" + instance.you.maxhealth;
            }
            else
            {
                instance.youbar.transform.localScale = new Vector3(0, 0, 1);
                instance.youhealth.text = "";
            }

        }
        else
        {
            instance.youbar.transform.localScale = new Vector3(0, 0, 1);
            instance.youhealth.text = "";
        }

        if (instance.opon != null)
        {
            if (instance.opon.Name != "")
            {
                instance.oponbar.transform.localScale = new Vector3(.7472217f * (float)(instance.opon.curhealth / instance.opon.maxhealth), .2752157f, 1);
                instance.oponhealth.text = instance.opon.curhealth + "/" + instance.opon.maxhealth;
            }
            else
            {
                instance.oponbar.transform.localScale = new Vector3(0, 0, 1);
                instance.oponhealth.text = "";
            }

        }
        else
        {
            instance.oponbar.transform.localScale = new Vector3(0, 0, 1);
            instance.oponhealth.text = "";
        }
    }

    private void UpStats()
    {
        if (you != null)
        {
            if (you.Name != "")
            {
                if (you.status.Count == 0)
                {
                    youstats.GetComponent<TextMesh>().text = "";
                    youstats.SetActive(false);
                }
                else
                {
                    youstats.SetActive(true);
                    youstats.GetComponent<TextMesh>().text = "";
                    for (int i = 0; i < you.status.Count; i++)
                    {
                        youstats.GetComponent<TextMesh>().text += you.PrintStatus(i);
                        youstats.GetComponent<TextMesh>().text += "\n";
                    }
                }
            }
            else
            {
                youstats.GetComponent<TextMesh>().text = "";
                youstats.SetActive(false);
            }
        }
        else
        {
            youstats.GetComponent<TextMesh>().text = "";
            youstats.SetActive(false);
        }

        if (opon != null)
        {
            if (opon.Name != "")
            {
                if (opon.status.Count == 0)
                {
                    oponstats.GetComponent<TextMesh>().text = "";
                    oponstats.SetActive(false);
                }
                else
                {
                    oponstats.SetActive(true);
                    oponstats.GetComponent<TextMesh>().text = "";
                    for (int i = 0; i < opon.status.Count; i++)
                    {
                        oponstats.GetComponent<TextMesh>().text += opon.PrintStatus(i);
                        oponstats.GetComponent<TextMesh>().text += "\n";
                    }
                }
            }
            else
            {
                oponstats.GetComponent<TextMesh>().text = "";
                oponstats.SetActive(false);
            }
        }
        else
        {
            oponstats.GetComponent<TextMesh>().text = "";
            oponstats.SetActive(false);
        }

    }

    public SpriteRenderer left, right;
    private bool doneyet = false;
    //private int i = 0; //testing
    void Update()
    {

        if (!doneyet)
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
        }
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("Adventure");
        /*if (Input.GetKeyDown("space")&&turn)
        {
            you = Lists.RandMon();
            int opmove = (int)UnityEngine.Random.Range((int)0, (int)3);
            UseMove(instance.opon, instance.you, instance.opon.moves[opmove]);
        }*/

        /*if (Input.GetKeyDown(KeyCode.Space) && i == 0)
        {
            UpMenu.SetMenu(UpMenu.curMenu.atk);
            i++;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && i == 1)
        {
            i = 0;            
            UpMenu.SetMenu(UpMenu.curMenu.none);
            SetMove(you.moves[0]);
            StartCoroutine("CoDoTurn",TurnType.attack);
        }*/

        //buttons.SetActive(turn);

        if (you != null)
            left.sprite = Resources.Load("MonsterSprites/" + you.imagepath, typeof(Sprite)) as Sprite;
        else if (you == null)
            left.sprite = null;

        if (opon != null)
            right.sprite = Resources.Load("MonsterSprites/" + opon.imagepath, typeof(Sprite)) as Sprite;







    }


    private void SaveCallBack()
    {

    }
}