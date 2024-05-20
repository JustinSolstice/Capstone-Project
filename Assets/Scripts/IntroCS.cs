using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCS : MonoBehaviour
{
    [SerializeField] string cutscene;
    // Start is called before the first frame update
    void Start()
    {
        CutsceneManager.Instance.StartCutscne(cutscene);
        GUIManager.Instance.transitionScreen.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
