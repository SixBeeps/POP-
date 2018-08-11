using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float speed;
    public float rotateMultiplyer;
    public Text countdownTxt;
    public GameObject playerSprite;
    public float maxTimerDelta;
    public float bubblePercentage = 100;
    public Text bubbleIndicator;
    public Button levelUpButton;
    public Button relocateButton;
    public int coinCounter;
    public int level = 1;
    public AudioSource collectSound;
    public Text levelIndicator;
    public GameObject gameOverScreen;
    public Text gameOverSubtext;
    public GameObject highscoreObj;
    public GameObject pauseObj;

    private Vector3 axisPoints;
    private Vector2 lastFramePosition;
    private float velocity;
    private float timer = 1;
    private Vector2 lastCollectedPosition;
    private bool hasSetHS = false;

    private void Start() {
        Application.targetFrameRate = 60;
        Invoke("StartDeflate", 3);
        StartCoroutine(Countdown());
        if (PlayerPrefs.GetInt("Highscore", -1) == -1) PlayerPrefs.SetInt("Highscore", 1);
    }
    private void StartDeflate() {
        StartCoroutine(BubbleDeflate());
    }
    private IEnumerator Countdown() {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        countdownTxt.text = "2";
        yield return new WaitForSecondsRealtime(1);
        countdownTxt.text = "1";
        yield return new WaitForSecondsRealtime(1);
        countdownTxt.text = "GO";
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(1);
        countdownTxt.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();
    }
    void FixedUpdate () {
        transform.Translate(0, Input.GetAxis("Vertical")*speed, 0, Space.Self);
        transform.Rotate(0, 0, Input.GetAxis("Horizontal") * rotateMultiplyer);
	}

    public void Update() {
        if (Input.GetButtonDown("LeftBumper") && levelUpButton.interactable) Upgrade();
        if (Input.GetButtonDown("RightBumper") && relocateButton.interactable) FindObjectOfType<BubblePoint>().FindNewPosition();
        if (Input.GetButtonDown("Pause")) {
            if (!pauseObj.activeInHierarchy) Pause(); else Unpause();
        }
        bubbleIndicator.text = Mathf.RoundToInt(bubblePercentage) + "%";
        levelIndicator.text = "Level " + level;

        if (bubblePercentage < 1 || bubblePercentage > 149) {
            gameOverScreen.SetActive(true);
            if (hasSetHS) highscoreObj.SetActive(true);
            StopAllCoroutines();
            if (bubblePercentage < 2) gameOverSubtext.text += "deflation!"; else gameOverSubtext.text += "overinflation!";
            Destroy(FindObjectOfType<BubbleIndicator>().alarmSFX.gameObject);
            Time.timeScale = 0;
            Destroy(this);
        }
    }

    public void Pause() {
        Time.timeScale = 0f;
        pauseObj.SetActive(true);
    }

    public void Unpause() {
        Time.timeScale = 1f;
        pauseObj.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Arena") {
            transform.position = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Coin") {
            Pump();
            collectSound.Play();
            other.GetComponent<BubblePoint>().FindNewPosition();
        }
    }

    public void Pump() {
        bubblePercentage += Vector3.Distance(lastCollectedPosition, transform.position)/2f;
        coinCounter++;
        if (coinCounter > 7) {
            levelUpButton.interactable = true;
        }
        lastCollectedPosition = transform.position;
    }

    public void Upgrade() {
        level++;
        speed += 0.1f;
        if (rotateMultiplyer < 45f) rotateMultiplyer += 1;
        maxTimerDelta -= 0.02f;
        coinCounter = 0;
        bubblePercentage += 2;
        levelUpButton.interactable = false;
        if (PlayerPrefs.GetInt("Highscore") < level) {
            PlayerPrefs.SetInt("Highscore", level);
            hasSetHS = true;
        }
    }

    public IEnumerator BubbleDeflate() {
        yield return new WaitForSeconds(timer);
        bubblePercentage--;
        if (timer > maxTimerDelta) timer *= (9 / 10f);
        StartCoroutine(BubbleDeflate());
    }
}
