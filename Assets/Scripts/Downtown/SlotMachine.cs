using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class SlotMachine : MonoBehaviour {

    public GameObject slotpanel, winpanel, losepanel, minorwinpanel;
    public GameObject leavebut, startbut;
    public Image slot1, slot2, slot3;
    public Sprite[] sprites = new Sprite[5];

    private void OnEnable()
    {
        slot1.sprite = sprites[0];
        slot2.sprite = sprites[4];
        slot3.sprite = sprites[1];

        slotpanel.SetActive(true);
        winpanel.SetActive(false);
        losepanel.SetActive(false);
        minorwinpanel.SetActive(false);

        leavebut.SetActive(true);
        startbut.SetActive(true);
    }

    public void StartSlots()
    {
        if (DowntownControl.GetPlayer().iteminv.Coins() >= 50)
        {
            leavebut.SetActive(false);
            startbut.SetActive(false);
            DowntownControl.GetPlayer().iteminv.SubstractCoins(50);
            StartCoroutine("GoSlots");
        }
    }

    public void Win()
    {
        leavebut.SetActive(true);
        slotpanel.SetActive(false);
        winpanel.SetActive(true);
        losepanel.SetActive(false);
        minorwinpanel.SetActive(false);

        DowntownControl.GetPlayer().iteminv.AddCoins(1000);
    }

    public void MinorWin()
    {
        leavebut.SetActive(true);
        slotpanel.SetActive(false);
        winpanel.SetActive(false);
        losepanel.SetActive(false);
        minorwinpanel.SetActive(true);


        DowntownControl.GetPlayer().iteminv.AddCoins(30);
    }

    public void Lose()
    {
        leavebut.SetActive(true);
        slotpanel.SetActive(false);
        winpanel.SetActive(false);
        losepanel.SetActive(true);
        minorwinpanel.SetActive(false);

    }
    public void Quit()
    {
        gameObject.SetActive(false);
    }

    IEnumerator GoSlots()
    {
        int i = 0;
        int j = 4;
        int k = 1;
        

        while (!(Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began) && !Input.GetKey(KeyCode.C))
        {
            i++;
            if (i == 5) i = 0;

            j++;
            if (j == 5) j = 0;

            k++;
            if (k == 5) k = 0;

            slot1.sprite = sprites[i];
            slot2.sprite = sprites[j];
            slot3.sprite = sprites[k];
            //if ((Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKey(KeyCode.C)) break;
            yield return new WaitForEndOfFrame();
        }

        while (Input.touchCount >= 1)
        {
            j++;
            if (j == 5) j = 0;

            k++;
            if (k == 5) k = 0;

            slot2.sprite = sprites[j];
            slot3.sprite = sprites[k];
            yield return new WaitForEndOfFrame();
        }

        while (!(Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began) && !Input.GetKey(KeyCode.C))
        {

            j++;
            if (j == 5) j = 0;

            k++;
            if (k == 5) k = 0;

            slot2.sprite = sprites[j];
            slot3.sprite = sprites[k];
            //if ((Input.touchCount >= 1) || Input.GetKey(KeyCode.C)) break;
            yield return new WaitForEndOfFrame();
        }

        while (Input.touchCount >= 1)
        {
            k++;
            if (k == 5) k = 0;

            slot3.sprite = sprites[k];
            yield return new WaitForEndOfFrame();
        }

        while (!(Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began) && !Input.GetKey(KeyCode.C))
        {
            
            k++;
            if (k == 5) k = 0;

            slot3.sprite = sprites[k];

            //if ((Input.touchCount >= 1) || Input.GetKey(KeyCode.C)) break;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.5f);

        if(i==j && i == k)
        {
            
            Win();
        }
        else if(i==j || i==k || j == k)
        {
            MinorWin();
        }
        else
        {
            Lose();
        }

    }

}
