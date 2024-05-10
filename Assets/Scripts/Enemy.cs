using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float arc = 40f;
    float detectionRange = 5;
    float facingDegrees = 0;

    LineRenderer[] lines = new LineRenderer[2];
    // Start is called before the first frame update
    void Start()
    {
        lines = GetComponentsInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SetLinePositions(math.radians(facingDegrees))
        if (CheckForDetection()) {
            print("detected")
        }
    }

    protected bool CheckForDetection() {
        List<Collider2D> results;
        Physics2D.OverlapCircle(transform.position, detectionRange, null, results);
        foreach (Collider2D item in results) {
            if (item.gameObject.CompareTag("Player")) {
                Vector2 dir = (Vector2)(transform.position - item.transform.position).normalized;
                float rads = math.atan2(dir.y, dir.x);
                if (rads >= math.radians(facingDegrees-arc/2) && rads <= math.radians(facingDegrees+arc/2)) {
                    return true;
                }
                break;
            }
        }
        return false;
    }

    protected void SetLinePositions(float aimRads) {
        Vector2[] directions = new Vector2[2] {
            new Vector2(math.cos(aimRads + math.radians(-arc/2)), math.sin(aimRads + math.radians(-arc/2))),
            new Vector2(math.cos(aimRads + math.radians(arc/2)), math.sin(aimRads + math.radians(arc/2)))
        };
        //then for the two linerenderers we set two positions, one at the transform and one at  the transform PLUS one of the direction vectors fron earlier
        for (int i = 0; i < 2; i++)
        {
            lines[i].SetPositions(new Vector3[2] { transform.position, transform.position + new Vector3(directions[i].x, directions[i].y, 0) * detectionRange })
        }
    }

    protected void SetLinePositions(Vector2 dir) {
        dir = dir.normalized;
        //this converts the vector3 to radians
        float aimRads = math.atan2(dir.y, dir.x);
        SetLinePositions(aimRads);
    }
}
