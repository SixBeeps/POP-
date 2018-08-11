using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFlip : MonoBehaviour {
    public Image fader;
    public void Awake() {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Game")
            StartCoroutine(FadeFromScene());
    }
    public void GoToScene(string name) {
        StartCoroutine(FadeToScene(name));
    }
    public void QuitGame() {
        Application.Quit();
    }
    public IEnumerator FadeToScene(string name) {
        fader.gameObject.SetActive(true);
        Time.timeScale = 1;
        for (float t = 0; t < 1.75f; t += Time.deltaTime) {
            fader.color = new Color(0, 0, 0, t/1.75f);
            foreach(AudioSource a in FindObjectsOfType<AudioSource>()) {
                a.volume = 1 - (t / 1.75f);
            }
            yield return new WaitForEndOfFrame();
        }
        // Make sure to finish fading!
        fader.color = new Color(0, 0, 0, 1);
        foreach (AudioSource a in FindObjectsOfType<AudioSource>()) {
            a.Stop();
        }
        yield return new WaitForEndOfFrame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
    public IEnumerator FadeFromScene() {
        fader.gameObject.SetActive(true);
        for (float t = 0; t < 1.75f; t += Time.deltaTime) {
            fader.color = new Color(0, 0, 0, 1 - (t / 1.75f));
            foreach (AudioSource a in FindObjectsOfType<AudioSource>()) {
                a.volume = t / 1.75f;
            }
            yield return new WaitForEndOfFrame();
        }
        fader.gameObject.SetActive(false);
    }
}
