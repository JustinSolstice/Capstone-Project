using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : Unit
{
    public enum PursuitType
    {
        Normal, //Normal pursuit mode
        InfRange, //Target can be seen no matter the distance between them 
        InfArc, //Target can be seen no matter where they are, unless there is a wall between them
        RaycastOnly,//The enemy will pursuit the target and wont stop until there is a collider between them and target
        Inescapeable //The enemy will pursuit the target and wont stop until pursuitTarget is set to null
    }

    float speed = 3.5f;
    float arc = 40f; //IN DEGREES
    public float detectionRange = 5;
    public float degreesShift = 45;
    public bool showLines = true;
    [SerializeField] float facingDegrees = 0; //IN DEGREES
    [SerializeField] bool randomFacingDegrees = true; 

    public GameObject pursuitTarget;
    public PursuitType pursuitType = PursuitType.Normal;
    Vector2 targetLastSeenPos = Vector2.zero;
    Vector2 runDirection = Vector2.zero;

    LineRenderer[] lines = new LineRenderer[2];

    protected override void Awake() {
        if (randomFacingDegrees)
        {
            facingDegrees = UnityEngine.Random.Range(0, 359);
        }
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        lines = GetComponentsInChildren<LineRenderer>(true);
    }

    // Update is called once per frame
    protected void Update()
    {
        SetLinePositions(math.radians(facingDegrees));
        if (degreesShift != 0 && targetLastSeenPos == Vector2.zero) {
            facingDegrees += degreesShift * Time.deltaTime;
        }
        if (facingDegrees < 0) facingDegrees += 360;
        else if (facingDegrees >= 360) facingDegrees -= 360;

        if (checkForDetection(GameManager.Instance.Player)) pursuitTarget = GameManager.Instance.Player;

        foreach (LineRenderer item in lines)
        {
            if (showLines) 
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
                continue;
            }
            item.startColor = checkForDetection(GameManager.Instance.Player) ? Color.red : Color.white;
            item.endColor = new Color(item.startColor.r, item.startColor.g, item.startColor.b, 0);
        }

    }


    protected override void FixedUpdate() {
        if (checkForDetection(GameManager.Instance.Player)) pursuitTarget = GameManager.Instance.Player;

        if (pursuitTarget != null && pursuitTarget.activeInHierarchy) {
            Vector3 direction = pursuitTarget.transform.position - transform.position;
            facingDegrees = math.degrees(math.atan2(direction.y, direction.x));
            
            bool targetVisible = false;
            switch (pursuitType)
            {
                case PursuitType.Normal:
                    targetVisible = checkForDetection(pursuitTarget);
                    break;
                case PursuitType.InfArc:
                    targetVisible = canSeeTarget(pursuitTarget) && targetInRange(pursuitTarget);
                    break;
                case PursuitType.InfRange:
                    targetVisible = canSeeTarget(pursuitTarget) && targetInArc(pursuitTarget);
                    break;
                case PursuitType.RaycastOnly:
                    targetVisible = canSeeTarget(pursuitTarget);
                    break;
                case PursuitType.Inescapeable:
                    targetVisible = true;
                    break;
                default:
                    break;
            }

            if (targetVisible) 
            {
                targetLastSeenPos = pursuitTarget.transform.position;
            }
            else
            {
                print(gameObject.name + " lost sight");
                pursuitTarget = null;
            }
        }
        else
        {
            pursuitTarget = null;
        }

        if (targetLastSeenPos != Vector2.zero) {
            runDirection = targetLastSeenPos - (Vector2)transform.position;
            if (runDirection.magnitude > Time.fixedDeltaTime)
            {
                runDirection.Normalize();
            }
            if (runDirection != Vector2.zero)
            {
                rb.MovePosition(rb.position + Time.fixedDeltaTime * runDirection * new Vector2(1, 0.5f) * speed);
                facingDegrees = math.degrees(math.atan2(runDirection.y, runDirection.x));
                movementAnimations = true;
                if (rb.position == targetLastSeenPos)
                {
                    runDirection = Vector2.zero;
                    targetLastSeenPos = Vector2.zero;
                }
            }
        }
        else {
            movementAnimations = false;
            Vector2 dir = new Vector2
            {
                y = (facingDegrees > 180) ? 1 : -1,
                x = (facingDegrees >= 0 && facingDegrees < 90) || (facingDegrees < 360 && facingDegrees >= 270) ? -1 : 1
            };
            updateAnim(dir);
        }
        base.FixedUpdate();
    }

    private bool radiansInArc(float radian, float arcMidpoint, float arcSize) {
        if (radian < 0) radian += math.PI*2;
        else if (radian >= math.PI*2) radian -= math.PI * 2;
        if (arcMidpoint < 0) arcMidpoint += math.PI * 2;
        else if (arcMidpoint >= math.PI*2) arcMidpoint -= math.PI * 2;

        return radian >= arcMidpoint - arcSize / 2 && radian <= arcMidpoint + arcSize / 2;
    }

    protected bool canSeeTarget(GameObject target) {
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, (target.transform.position - transform.position).normalized, (target.transform.position - transform.position).magnitude))
        {
            if (hit.transform == transform || hit.transform == target.transform) continue;
            return false;
        }
        return true;
    }

    protected bool targetInArc(GameObject target)
    {
        Vector2 dir = (target.transform.position - transform.position).normalized;
        float playerRadians = math.atan2(dir.y, dir.x);

        return radiansInArc(playerRadians, math.radians(facingDegrees), math.radians(arc));
    }

    protected bool targetInRange(GameObject target) {
        float magnitude = (target.transform.position - transform.position).magnitude;
        return magnitude <= detectionRange;
    }

    protected bool checkForDetection(GameObject target = null) {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player");
        if (target == null || !target.activeInHierarchy) return false;
        return targetInRange(target) && targetInArc(target) && canSeeTarget(target);
    }

    protected void SetLinePositions(float aimRads) {
        Vector2[] directions = new Vector2[2] {
            new Vector2(math.cos(aimRads + math.radians(-arc/2)), math.sin(aimRads + math.radians(-arc/2))),
            new Vector2(math.cos(aimRads + math.radians(arc/2)), math.sin(aimRads + math.radians(arc/2)))
        };
        //then for the two linerenderers we set two positions, one at the transform and one at  the transform PLUS one of the direction vectors fron earlier
        for (int i = 0; i < 2; i++)
        {
            Vector2 end = transform.position + new Vector3(directions[i].x, directions[i].y, 0) * detectionRange;
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, directions[i], detectionRange))
            {
                if (hit.transform == transform) continue;
                end = hit.point;
                break;
            }
            lines[i].SetPositions(new Vector3[2] { transform.position, end});
        }
    }

    protected void SetLinePositions(Vector2 dir) {
        dir = dir.normalized;
        //this converts the vector3 to radians
        float aimRads = math.atan2(dir.y, dir.x);
        SetLinePositions(aimRads);
    }
}
