using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;

    [SerializeField] private GameObject player;

    public GameObject Player { 
        get {
            if (player == null || !player.activeInHierarchy) {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            return player;
        } 
    }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("A" + this.GetType() + "MonoBehaviour already exists");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //GameObject.Find("Debug").GetComponent<TextMeshProUGUI>().text = "" + math.round(player.GetComponent<Unit>().arc);
    }

    private void LateUpdate() {
        if (player == null) return;
        Camera.main.transform.position = Player.transform.position + Vector3.back;
    }

    public void TransitionToScene(string scene) {
        StartCoroutine(sceneTransition(scene));
    }

    IEnumerator sceneTransition(string scene) {
        Animator animator = GUIManager.Instance.Transition(false);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("TransOut") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return new WaitForEndOfFrame();
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f) {yield return new WaitForEndOfFrame();}
        asyncOperation.allowSceneActivation = true;
        do { yield return new WaitForEndOfFrame();} while (!asyncOperation.isDone);

        GUIManager.Instance.Transition(true);
        Time.timeScale = 1;
    }

    IEnumerator quitTransition()
    {
        Animator animator = GUIManager.Instance.Transition(false);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("TransOut") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return new WaitForEndOfFrame();
        }
        Application.Quit();
    }
    public void Quit()
    {
        StopAllCoroutines();
        StartCoroutine(quitTransition());
    }

    public void Lose() {
        player.GetComponent<Player>().allowControl = false;
        Time.timeScale = 0;
        GUIManager.Instance.ShowGameOverUI(true);
    }

    public void Restart() {
        TransitionToScene(SceneManager.GetActiveScene().name);
        GUIManager.Instance.ShowGameOverUI(false);
    }
}