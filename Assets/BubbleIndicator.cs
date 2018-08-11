using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleIndicator : MonoBehaviour {
    public Player player;
    public AudioSource alarmSFX;
	void Update () {
		if (player.bubblePercentage < 26 || player.bubblePercentage > 124) {
            GetComponent<Image>().color = Color.red;
            if (alarmSFX && !alarmSFX.isPlaying) alarmSFX.Play();
        } else {
            GetComponent<Image>().color = Color.white;
            if (alarmSFX.isPlaying) alarmSFX.Stop();
        }
	}
}
