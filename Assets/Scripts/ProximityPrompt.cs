using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public abstract class ProximityPrompt : MonoBehaviour
{
    public float radius;
    public KeyCode interactKey;
    public GameObject UI;

    public bool active = true;
    public bool showUi = false;
    /*
    private void TweenUI(bool show) {
        print(show);
        
        if (UiTween != null) {
            LeanTween.cancel(UiTween.id);
            UiTween = null;
        }

        UI.SetActive(true);
        if (show) UiTween = LeanTween.size(UI.GetComponent<RectTransform>(), new Vector3(30, 30, 1), 0.2f).setEase(LeanTweenType.easeOutQuad);
        else UiTween = LeanTween.size(UI.GetComponent<RectTransform>(), Vector3.zero, 0.2f);
        
        UiTween.setOnComplete(OnTweenFinished);
        UiTween.setEase(LeanTweenType.easeInQuad);
    }

    private void OnTweenFinished() {
        UI.SetActive(math.round(UI.GetComponent<RectTransform>().sizeDelta.x) > 0);
    }
    */

    private void LateUpdate() {
        if (UI == null) return;
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 viewportSize = GUIManager.Instance.gameObject.GetComponent<RectTransform>().sizeDelta;
        UI.GetComponent<RectTransform>().anchoredPosition = new Vector3((screenPos.x-0.5f)* viewportSize.x, (screenPos.y - 0.5f) * viewportSize.y, 0);

        UI.GetComponentInChildren<TextMeshProUGUI>().text = interactKey.ToString();

        UI.SetActive(showUi && active);
    }

    public abstract void OnInteract();

    private void OnDestroy() {
        Destroy(UI);
    }
}
