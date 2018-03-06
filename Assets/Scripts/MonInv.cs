using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[System.Serializable]
public class MonInv{

        public Monster.SubMonster[] monsters = new Monster.SubMonster[13];
        public List<Monster.SubMonster> discovered = new List<Monster.SubMonster>();
        public List<String> discnames = new List<string>();

        public MonInv()
        {
            discovered = new List<Monster.SubMonster>();
            discnames = new List<string>();

            for (int i = 0; i < 13; i++)
            {
                monsters[i] = null;
            }
        }


        public int HowManyActives()
        {
            int numactives = 0;
            for (int i = 0; i < 3; i++)
            {
                if (monsters[i] != null)
                {
                    if (monsters[i].Name != "")
                        numactives++;
                }
            }

            return numactives;
        }
        public bool HasAnActive()
        {
            for (int i = 0; i < 3; i++)
            {
                if (monsters[i] != null)
                {
                    if (monsters[i].Name != "")
                        return true;
                }
            }

            return false;
        }
        public Monster.SubMonster FirstActive()
        {
            for (int i = 0; i < 3; i++)
            {
                if (monsters[i] != null)
                {
                    if (monsters[i].Name != "")
                        return monsters[i];
                }
            }
            return null;

        }

        public string[] Trap(double dam)
        {
            List<string> deads = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                if (monsters[i] != null)
                {
                    monsters[i].Damage(dam);
                    if (monsters[i].isDead())
                    {
                        deads.Add(monsters[i].Name);
                        monsters[i] = null;
                    }
                }
            }

            string[] deads2 = deads.ToArray();
            return deads2;
        }


        public void AddMonster(Monster.SubMonster newmon)
        {
            if (!discnames.Contains(newmon.Name))
            {
                discnames.Add(newmon.Name);
                discovered.Add(newmon);
            }

            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] == null)
                {

                    monsters[i] = newmon;
                    return;

                }

            }


        }
        public int FindReference(Monster.SubMonster refitem)
        {
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] != null)
                {
                    if (monsters[i].fingerprint == refitem.fingerprint)
                    {
                        return i;
                    }
                }
            }
            return 100;
        }

        public void RemoveMonster(Monster.SubMonster rmitem)
        {
            int i = FindReference(rmitem);
            if (i == 100)
            {
                return;
            }
            else
            {
                monsters[i] = null;
            }
        }

        //revise
        public void SwapPos(Monster.SubMonster mon1, Monster.SubMonster mon2)
        {
            Monster.SubMonster temp;
            int i, j;
            i = FindReference(mon1);
            if (i == 100) return;
            else
            {
                j = FindReference(mon2);
                if (j == 100) return;
                else
                {
                    temp = mon1;
                    monsters[i] = monsters[j];
                    monsters[j] = temp;
                }
            }

        }

        public void Move2Empty(Monster.SubMonster mon, int slot)
        {
            Monster.SubMonster temp;
            int i;
            i = FindReference(mon);
            if (i == 100) return;
            else
            {

                temp = mon;
                monsters[i] = null;
                monsters[slot] = temp;
            }


        }


        public bool HasMonster(Monster.SubMonster testitem)
        {
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] != null)
                {
                    if (monsters[i].Name == testitem.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool IsFull()
        {
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsEmpty()
        {
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool OnlyOne()
    {
        int num=0;
        for (int i = 0; i < monsters.Length; i++)
        {
            if (monsters[i] != null)
            {
                num++;
            }
        }
        if (num == 1) return true;
        else return false;
    }

        public Monster.SubMonster[] ReturnMain()
        {
            Monster.SubMonster[] final = new Monster.SubMonster[10];
            for (int i = 3; i < 13; i++)
            {
                final[i - 3] = monsters[i];
            }
            return final;
        }
        public Monster.SubMonster[] ReturnActives()
        {
            Monster.SubMonster[] final = new Monster.SubMonster[3];
            for (int i = 0; i < 3; i++)
            {
                final[i] = monsters[i];
            }
            return final;
        }

        //fountain
        public void HealAll()
        {
            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] != null)
                {
                    monsters[i].Heal(-1);
                    monsters[i].ClearStatus();

                    for (int j = 0; j < 3; j++)
                    {
                        monsters[i].moves[j].Replenish();
                    }
                }
            }
        }
}





