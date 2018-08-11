using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreUpdate : MonoBehaviour {
	void Start () {
        GetComponent<UnityEngine.UI.Text>().text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0);
	}
}
