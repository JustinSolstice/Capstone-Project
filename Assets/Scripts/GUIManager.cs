using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [HideInInspector] public static GUIManager Instance;

    [HideInInspector] public GameObject transitionScreen;
    [HideInInspector] public Textbox textbox;

    private Dictionary<KeyCode, PromptData> prompts = new Dictionary<KeyCode, PromptData>();
    [SerializeField] GameObject promptUITemplate;

    void Awake() {
        if (Instance != null)
        {
            Debug.LogError("A" + this.GetType() + "MonoBehaviour already exists");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        transitionScreen = gameObject.transform.Find("Transition").gameObject;
        textbox = gameObject.GetOrAddComponent<Textbox>();
    }

    private void Update() {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("ProximityPrompt"))
        {
            ProximityPrompt proxPrompt = item.GetComponent<ProximityPrompt>();
            bool keyExists = prompts.ContainsKey(proxPrompt.interactKey);
            if (!proxPrompt.active)
            {
                if (keyExists && prompts[proxPrompt.interactKey].gameObject == item)
                {
                    proxPrompt.showUi = false;
                    prompts.Remove(proxPrompt.interactKey);
                }
                continue;
            };

            Vector2 diff = GameManager.Instance.Player.transform.position - item.transform.position;
            float magnitude = diff.magnitude;

            if (magnitude <= proxPrompt.radius)
            {
                if (keyExists && prompts[proxPrompt.interactKey].gameObject != item)
                {
                    if (prompts[proxPrompt.interactKey].magnitude < magnitude) continue;
                    else prompts[proxPrompt.interactKey].prompt.showUi = false;
                };


                prompts[proxPrompt.interactKey] = new PromptData(item, magnitude, proxPrompt);
                if (proxPrompt.UI == null)
                {
                    proxPrompt.UI = Instantiate(promptUITemplate, gameObject.transform);
                }
                proxPrompt.showUi = true;
            }
            else if (keyExists && prompts[proxPrompt.interactKey].gameObject == item)
            {
                proxPrompt.showUi = false;
                prompts.Remove(proxPrompt.interactKey);
            }
        }

        foreach (KeyValuePair<KeyCode, PromptData> kvp in prompts)
        {
            if (Input.GetKeyDown(kvp.Key) && kvp.Value.prompt.active) kvp.Value.prompt.OnInteract();
        }
    }
}
class PromptData
{ //used to simplify comparing prompts and such

    public GameObject gameObject;
    public float magnitude;
    public ProximityPrompt prompt;

    public PromptData(GameObject g, float m, ProximityPrompt p)
    {
        gameObject = g;
        magnitude = m;
        prompt = p;
    }
}