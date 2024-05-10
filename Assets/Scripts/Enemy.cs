using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : Unit
{
    float arc = 40f; //IN DEGREES
    float detectionRange = 5;
    public float facingDegrees = 0; //IN DEGREES

    LineRenderer[] lines = new LineRenderer[2];
    // Start is called before the first frame update
    void Start()
    {
        lines = GetComponentsInChildren<LineRenderer>();
    }

    // Update is called once per frame
    protected void Update()
    {
        SetLinePositions(math.radians(facingDegrees));
        if (CheckForDetection()) {
            print("detected");
        }
    }

    protected bool CheckForDetection() {
        foreach (Collider2D item in Physics2D.OverlapCircleAll(transform.position, detectionRange)) {
            if (item.gameObject.CompareTag("Player")) {
                Vector2 dir = (item.transform.position - transform.position).normalized;
                float rads = math.atan2(dir.y, dir.x);
                print(rads + " | " + math.radians(facingDegrees));
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
            lines[i].SetPositions(new Vector3[2] { transform.position, transform.position + new Vector3(directions[i].x, directions[i].y, 0) * detectionRange });
        }
    }

    protected void SetLinePositions(Vector2 dir) {
        dir = dir.normalized;
        //this converts the vector3 to radians
        float aimRads = math.atan2(dir.y, dir.x);
        SetLinePositions(aimRads);
    }
}
