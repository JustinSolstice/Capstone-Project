using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float arc = 45f;
    LineRenderer[] lines = new LineRenderer[2];
    // Start is called before the first frame update
    void Start()
    {
        lines = GetComponentsInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void SetLinePositionsByDirection(Vector3 dir)
    {
        //breaking this down so future me dosent have a stroke reading this
        //gets vector3 that points to the direction starting from the gameobject to the mouse position, .normalized causes the vector to have a max of 1
        dir = dir.normalized;
        //this converts the vector3 to radians
        float aimRads = math.atan2(dir.y, dir.x);
        //after which two directions vectors are made, using math shit we get two positions which corrospond to the vector being rotated by the amount of
        //degrees from the arc value
        Vector2[] directions = new Vector2[2] {
            new Vector2(math.cos(aimRads + math.radians(-arc/2)), math.sin(aimRads + math.radians(-arc/2))),
            new Vector2(math.cos(aimRads + math.radians(arc/2)), math.sin(aimRads + math.radians(arc/2)))
        };
        //then for the two linerenderers we set two positions, one at the transform and one at  the transform PLUS one of the direction vectors fron earlier
        for (int i = 0; i < 2; i++)
        {
            lines[i].SetPositions(new Vector3[2] { transform.position, transform.position + new Vector3(directions[i].x, directions[i].y, 0) * 7 });
        }
    }
}
