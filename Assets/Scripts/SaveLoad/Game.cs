using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Game
{

    private float gameTime;

    public float GetTime() { return gameTime; }
    public void SetTime(float time) { gameTime = time; }

    private ItemInv.InvSpot[] inventory;
    private Monster.SubMonster[] monsters;
    //private Monster.SubMonster[] actives;
    private List<Monster.SubMonster> disc;
    private List<string> discnames;
    private int coins;
    //moves?
    //private PlayerStats.STAT[] stats;

    public void SetCoins(int savecoins) { coins = savecoins; }
    public void SetInventory(ItemInv.InvSpot[] newInventory) { inventory = newInventory; }
    public void SetMonsters(Monster.SubMonster[] newmonsters) { monsters = newmonsters; }
    //public void SetActives(Monster.SubMonster[] newmonsters) { actives = newmonsters; }
    public void SetDisc(List<Monster.SubMonster> newdisc) { disc = newdisc; }
    public void SetNames(List<string> newnames) { discnames = newnames; }


    public int GetCoins() { return coins; }
    public ItemInv.InvSpot[] GetInventory() { return inventory; }
    public Monster.SubMonster[] GetMonsters() { return monsters; }
    //public Monster.SubMonster[] GetActives() { return actives; }
    public List<Monster.SubMonster> GetDisc() { return disc; }
    public List<string> GetNames() { return discnames; }


    //public Checkpoint GetCheckpoint() { return curcheckpoint; }


    public Game()
    {
        gameTime = 0.0f;
    }
}
