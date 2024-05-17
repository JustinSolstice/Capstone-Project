using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Vector3 newPosition;
    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(teleportTransition());
        }
    }

    IEnumerator teleportTransition()
    {
        active = false;
        Animator animator = GUIManager.Instance.Transition(false);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("TransOut") ||
        !(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
        {
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.Player.transform.position = newPosition;
        GUIManager.Instance.Transition(true);
        active = true;
    }
}
