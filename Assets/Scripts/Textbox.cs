using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour
{
    public bool typing = false;
    public Dialouge[] textArrays;

    [SerializeField] private GameObject textbox;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private GameObject nameLabel;
    [SerializeField] private GameObject portrait;
    int textArrayIndex;

    private GameObject choiceBox;

    private Coroutine typingCoroutine;
    private Action onComplete;

    private Action<int> choiceAction;
    public bool choosing = false;

    void Start()
    {
        if (textbox == null) textbox = gameObject.transform.Find("Textbox").gameObject;
        if (textMesh == null) textMesh = textbox.GetComponentInChildren<TextMeshProUGUI>();
        if (nameLabel == null) nameLabel = textbox.transform.Find("Name").gameObject;
        if (portrait == null) portrait = textbox.transform.Find("Portrait").gameObject;
        if (choiceBox == null) choiceBox = textbox.transform.Find("Choice").gameObject;

        StopCurrentText(false);
    }

    void ResizeText()
    {
        //textMesh.fontSize = textbox.GetComponent<RectTransform>().sizeDelta.y / 3 - (textMesh.gameObject.GetComponent<RectTransform>().offsetMin.y + textMesh.gameObject.GetComponent<RectTransform>().offsetMax.y);
    }

    void Update()
    {
        choiceBox.SetActive(choosing);
        if (choosing) {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnChoose(1);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnChoose(2);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (typing) typing = false;
                else AdvanceText();
            }

            if (Input.GetKey(KeyCode.RightControl))
            {
                if (typing) typing = false;
                else AdvanceText();
            }
        }
    }

    void OnDisable()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
    }

    int textIndex = 0;
    void CreateText(Dialouge txt, float delay = 0.01f)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(drawTxt(txt.text, delay));
        if (txt.name != null) {
            nameLabel.GetComponentInChildren<TextMeshProUGUI>().text = txt.name;
        }
        nameLabel.gameObject.SetActive(txt.name != null);

        if (txt.portraitImg != null) {
            Texture2D tex = (Texture2D)Resources.Load("Images/Portraits/" + txt.portraitImg);
            portrait.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        }
        portrait.gameObject.SetActive(txt.portraitImg != null);
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
            textbox.GetComponent<AudioSource>().Play();
            float actualDelay = delay;
            if (char.IsPunctuation(txt[textIndex - 1])) actualDelay += 0.1f;
            yield return new WaitForSeconds(actualDelay);
        }
        textMesh.SetText(txt);
        typing = false;
        textIndex = txt.Length;
    }

    public void CreateTextSequence(Dialouge[] txt, Action complete = null)
    {
        if (textArrays != null) StopCurrentText(true);
        textArrayIndex = 0;
        textArrays = txt;
        onComplete = complete;
        CreateText(txt[textArrayIndex]);
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
        if (textArrayIndex < textArrays.Length) CreateText(textArrays[textArrayIndex]);
        else
        {
            StopCurrentText();
        }
    }

    public void Choice(string[] txt, Action<int> complete = null) {
        textbox.SetActive(true);
        choosing = true;
        for (int i = 0; i < 2; i++)
        {
            TextMeshProUGUI tmp = choiceBox.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            tmp.text = txt[i];
        }
        choiceAction = complete;
    }

    void OnChoose(int selected)
    {
        choosing = false;
        if (choiceAction != null)
        {
            choiceAction(selected);
        }
    }
}

[Serializable]
public class Dialouge {
    [SerializeField] public string text;
    [SerializeField] public string portraitImg;
    [SerializeField] public string name;
}