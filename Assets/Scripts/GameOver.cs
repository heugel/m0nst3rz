using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void OnEnable ()
    {
        //Debug.Log(0);
        //print(0);
        StartCoroutine("GameOverCo");
        //print(1);
        SaveLoad.DeleteGame();
        //print(2);
	}
	
    IEnumerator GameOverCo()
    {
        while (!(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) && !Input.GetKey(KeyCode.C))
        {
            //print("f");
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene("MainMenu");
        yield break;
    }
}
