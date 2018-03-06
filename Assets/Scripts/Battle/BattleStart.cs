using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart : MonoBehaviour {

    private bool initmon = false;
    private Monster.SubMonster randopon = new Monster.SubMonster();
    //private Monster.SubMonster you = new Monster.SubMonster();
    public Monster test, testopon;

	// Use this for initialization
	void Start () {
        //MonInv.AddMonster(MonInit.MakeFromPrefab(test));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!initmon)
        {
            if (Lists.IsActive() && MonInit.IsActive() && BattleMain3.IsActive())
            {
                //MonInv.AddMonster(MonInit.MakeFromPrefab(test));
                //MonInv.AddMonster(MonInit.MakeFromPrefab(test));

                initmon = true;
                randopon = Lists.RandMon();
                randopon = MonInit.RandomFromPrefab(randopon);
                //randopon = MonInit.RandomFromPrefab(testopon);
                BattleMain3.SetMon(randopon);
                //you = MonInv.FirstActive();
                //you = MonInit.MakeFromPrefab(test);
                //you = new Monster.SubMonster(MonInv.FirstActive());

                //BattleMain3.SetYou(you);
                BattleMain3.TurnSet(true);
            }
        }
        else if (initmon)
        {
            gameObject.SetActive(false);
            //animation, activate BattleMain3, close this
        }
    }
}
