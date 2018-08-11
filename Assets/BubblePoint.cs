using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubblePoint : MonoBehaviour {
    public Button relocateButton;
    private WaitForSeconds wait = new WaitForSeconds(10);
    public void Start() {
        FindNewPosition();
    }
    public void FindNewPosition() {
        transform.position = Random.insideUnitCircle * 30;
        relocateButton.interactable = false;
        StopAllCoroutines();
        StartCoroutine(ChargerRelocate());
    }
    public IEnumerator ChargerRelocate() {
        yield return wait;
        relocateButton.interactable = true;
    }
}
