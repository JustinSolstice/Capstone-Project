using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEnemyBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (collision.GetComponent<Enemy>()) {
            enemy.detectionRange = 5;
            enemy.showLines = true;
            enemy.degreesShift = 45;
            enemy.pursuitType = Enemy.PursuitType.InfRange;
        }
    }

}
