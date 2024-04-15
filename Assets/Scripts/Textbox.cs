using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour
{
    public bool typing = false;

    private GameObject textbox;
    private TextMeshProUGUI textMesh;
    string[] textArrays;
    int textArrayIndex;

    private Coroutine typingCoroutine;
    private Action onComplete;

    void Start()
    {
        if (textbox == null) textbox = gameObject.transform.Find("Textbox").gameObject;
        if (textMesh == null) textMesh = textbox.GetComponentInChildren<TextMeshProUGUI>();
        CreateText("testing text");
    }

    void ResizeText()
    {
        textMesh.fontSize = textbox.GetComponent<RectTransform>().sizeDelta.y / 3 - (textMesh.gameObject.GetComponent<RectTransform>().offsetMin.y + textMesh.gameObject.GetComponent<RectTransform>().offsetMax.y);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) CreateTextSequence(new string[] { "guys look the textbox is working.", "ajnfkdjgndsgldzs" });
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (typing) typing = false;
            else AdvanceText();
        };
        ResizeText();
    }

    void OnDisable()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
    }

    int textIndex = 0;
    void CreateText(string txt, float delay = 0.01f)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(drawTxt(txt, delay));
    }
    private IEnumerator drawTxt(string txt, float delay)
    {
        typing = true;
        textbox.SetActive(true);
        textIndex = 0;
        while (typing && textIndex < txt.Length)
        {
            textIndex += 1;
            textMesh.SetText(txt[..textIndex]);
            float actualDelay = delay;
            if (char.IsPunctuation(txt[textIndex - 1])) actualDelay += 0.1f;
            yield return new WaitForSeconds(actualDelay);
        }
        textMesh.SetText(txt);
        typing = false;
        textIndex = txt.Length;
    }

    public void CreateTextSequence(string[] txt, Action complete = null)
    {
        if (textArrays != null) StopCurrentText(true);
        textArrayIndex = 0;
        textArrays = txt;
        onComplete = complete;
        CreateText(textArrays[textArrayIndex]);
    }

    private void StopCurrentText(bool showBox = false)
    {
        textArrays = null;
        textArrayIndex = 0;
        typing = false;
        textMesh.SetText("");

        if (onComplete != null)
        {
            onComplete();
        }
        textbox.SetActive(showBox);
    }

    public void AdvanceText()
    {
        typing = false;
        if (textArrays == null)
        {
            StopCurrentText();
            return;
        };

        textArrayIndex += 1;
        print(textArrayIndex);
        if (textArrayIndex < textArrays.Length) CreateText(textArrays[textArrayIndex]);
        else
        {
            StopCurrentText();
        }
    }
}