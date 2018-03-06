using UnityEngine;
using System.Collections;

[System.Serializable]
public static/*?*/ class Types {//? : MonoBehaviour {

    public enum Type
    {
        none,
        fire,
        water,
        grass,
        germ,
        acid,
        earth,
        bouncy,
        flying,
        bomb,
        normal,
        ice,
        nerd,
        electric,
        magic,
        sad,
        tubby,
        laser,
        amyschumer,
        techno,
        lawyer,
        spooky
    }

    public static string TypePrinter(Type type)
    {
        if(type==Type.fire)
        {
            return "Fire";
        }
        if (type == Type.water)
        {
            return "Water";
        }
        if (type == Type.grass)
        {
            return "Grass";
        }
        if (type == Type.germ)
        {
            return "Germ";
        }
        if (type == Type.acid)
        {
            return "Acid";
        }
        if (type == Type.earth)
        {
            return "Earth";
        }
        if (type == Type.bouncy)
        {
            return "Bouncy";
        }
        if (type == Type.flying)
        {
            return "Flying";
        }
        if (type == Type.bomb)
        {
            return "Bomb";
        }
        if (type == Type.normal)
        {
            return "Normal";
        }
        if (type == Type.ice)
        {
            return "Ice";
        }
        if (type == Type.nerd)
        {
            return "Nerd";
        }
        if (type == Type.electric)
        {
            return "Electric";
        }
        if (type == Type.magic)
        {
            return "Magic";
        }
        if (type == Type.sad)
        {
            return "Sad";
        }
        if (type == Type.tubby)
        {
            return "Tubby";
        }
        if (type == Type.laser)
        {
            return "Laser";
        }
        if (type == Type.amyschumer)
        {
            return "Amy Schumer";
        }
        if (type == Type.techno)
        {
            return "Techno";
        }
        if (type == Type.lawyer)
        {
            return "Lawyer";
        }
        if (type == Type.spooky)
        {
            return "Spooky";
        }


        return "";
    }

    // =2 means instakill
    // =1 means super effective
    // =0 means normal
    //=-1 means weak
    //=-2 means no effect at all
    //=-3 means heal def
    public static int CheckWeakness(Type atk, Type def)
    {
        if(def==Type.sad)
        {
            if (atk == Type.normal)
                return -2;
            else return 1;
        }

        if (atk == Type.fire) //-1
        {
            if (def == Type.water || def == Type.laser || def==Type.fire)
            {
                return -1;
            }
            else if (def == Type.ice || def == Type.grass)
            {
                return 1;
            }
            else return 0;
        }
        else if (atk == Type.water) //-1
        {
            if (def == Type.water)
            {
                return -1;
            }
            else if (def == Type.grass)
                return -3;
            else if (def == Type.techno || def == Type.grass || def == Type.electric)
            {
                return 1;
            }
            else return 0;
        }
        else if (atk == Type.grass) //-2
        {
            if (def == Type.grass || def == Type.laser)
            {
                return -1;
            }
            else if (def == Type.fire)
                return -2;
            else if (def == Type.water || def == Type.lawyer)
            {
                return 1;
            }
            else return 0;
        }
        else if (atk == Type.germ) //-1
        {
            if (def == Type.acid)
            {
                return -1;
            }
            else if (def == Type.fire || def == Type.laser)
                return -2;
            else if (def == Type.nerd || def == Type.water)
            {
                return 1;
            }
            else if (def == Type.amyschumer)
                return 2;
            else return 0;
        }
        else if (atk == Type.acid) //-2
        {
            if (def == Type.water || def == Type.magic)
            {
                return -1;
            }
            else if (def == Type.electric)
                return -3;
            else if (def == Type.acid)
                return -2;
            else if (def == Type.germ || def == Type.grass || def == Type.fire || def==Type.earth || def==Type.lawyer)
            {
                return 1;
            }
            else return 0;
        }
        else if (atk == Type.earth)
        {
            if (def == Type.earth || def == Type.bouncy) //0
            {
                return -1;
            }
            else if (def == Type.flying)
                return -2;
            else if (def == Type.nerd || def == Type.fire || def == Type.electric || def==Type.tubby)
            {
                return 1;
            }
            else return 0;
        }
        else if (atk == Type.bouncy) //0
        {
            if (def == Type.earth || def == Type.bouncy || def == Type.flying)
                return 1;
            else if (def == Type.tubby || def == Type.bomb || def == Type.ice)
                return -1;
        }
        else if (atk == Type.flying)
        {
            if (def == Type.flying)
                return -1;
            else if (def == Type.earth)
                return 1;

        }
        else if (atk == Type.bomb)
        {
            
        }
        else if (atk == Type.normal)
        {
            
        }
        else if (atk == Type.ice)
        {
            if (def == Type.tubby || def == Type.fire)
                return -1;
            else if (def == Type.germ) return 1;
        }
        else if (atk == Type.nerd)
        {
            
        }
        else if (atk == Type.electric)
        {
            if (def == Type.water)
                return 1;
            else if (def == Type.earth || def == Type.nerd)
                return -1;
        }
        else if (atk == Type.magic)
        {
            
        }
        else if (atk == Type.sad)
        {
            if (def == Type.normal)
                return 2;
            else return -1;
        }
        else if (atk == Type.tubby)
        {
            if (def == Type.tubby)
                return -1;
            else if (def == Type.earth)
                return 1;
        }
        else if (atk == Type.laser)
        {
            if (def == Type.bomb || def == Type.ice || def == Type.flying)
                return 1;
            else if (def == Type.laser || def == Type.techno || def == Type.electric)
                return -1;
        }
        else if (atk == Type.amyschumer)
        {
            if (def == Type.normal)
                return 1;
        }
        else if (atk == Type.techno)
        {
            if (def == Type.amyschumer)
                return 1;
        }
        else if (atk == Type.lawyer)
        {
            if (def == Type.lawyer || def == Type.nerd || def == Type.magic || def == Type.tubby)
                return 1;
            else if (def == Type.amyschumer) return -2;

        }
        else if (atk == Type.spooky)
        {

        }

        return 0;
    }

    public static string TypePrinter(int type)
    {
        if (type == 0)
        {
            return "None";
        }
        if (type == 1)
        {
            return "Fire";
        }
        if (type == 2)
        {
            return "Water";
        }
        if (type == 3)
        {
            return "Grass";
        }
        if (type == 4)
        {
            return "Germ";
        }
        if (type == 5)
        {
            return "Acid";
        }
        if (type == 6)
        {
            return "Earth";
        }
        if (type == 7)
        {
            return "Bouncy";
        }
        if (type == 8)
        {
            return "Flying";
        }
        if (type == 9)
        {
            return "Bomb";
        }
        if (type == 10)
        {
            return "Normal";
        }
        if (type == 11)
        {
            return "Ice";
        }
        if (type == 12)
        {
            return "Nerd";
        }
        if (type == 13)
        {
            return "Electric";
        }
        if (type == 14)
        {
            return "Magic";
        }
        if (type == 15)
        {
            return "Sad";
        }
        if (type == 16)
        {
            return "Tubby";
        }
        if (type == 17)
        {
            return "Laser";
        }
        if (type == 18)
        {
            return "Amy Schumer";
        }
        if (type == 19)
        {
            return "Techno";
        }
        if (type == 20)
        {
            return "Lawyer";
        }
        if (type == 21)
        {
            return "Spooky";
        }


        return "";
    }

    public static Type Int2Type(int type)
    {
        if (type == 0)
        {
            return Type.none;
        }
        if (type == 1)
        {
            return Type.fire;
        }
        if (type == 2)
        {
            return Type.water;
        }
        if (type == 3)
        {
            return Type.grass;
        }
        if (type == 4)
        {
            return Type.germ;
        }
        if (type == 5)
        {
            return Type.acid;
        }
        if (type == 6)
        {
            return Type.earth;
        }
        if (type == 7)
        {
            return Type.bouncy;
        }
        if (type == 8)
        {
            return Type.flying;
        }
        if (type == 9)
        {
            return Type.bomb;
        }
        if (type == 10)
        {
            return Type.normal;
        }
        if (type == 11)
        {
            return Type.ice;
        }
        if (type == 12)
        {
            return Type.nerd;
        }
        if (type == 13)
        {
            return Type.electric;
        }
        if (type == 14)
        {
            return Type.magic;
        }
        if (type == 15)
        {
            return Type.sad;
        }
        if (type == 16)
        {
            return Type.tubby;
        }
        if (type == 17)
        {
            return Type.laser;
        }
        if (type == 18)
        {
            return Type.amyschumer;
        }
        if (type == 19)
        {
            return Type.techno;
        }
        if (type == 20)
        {
            return Type.lawyer;
        }
        if (type == 21)
        {
            return Type.spooky;
        }


        return Type.none;
    }
}
