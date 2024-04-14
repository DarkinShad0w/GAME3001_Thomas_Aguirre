using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTorpedoScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private Vector2 directionTarget;
    private Vector2 vectorToTarget;

    void Update()
    {
        transform.Translate(vectorToTarget.x, vectorToTarget.y, 0f);   
    }

    public void LockOnTarget(Transform target)
    {
        directionTarget = (target.position - transform.position).normalized;
        vectorToTarget = directionTarget * moveSpeed * Time.deltaTime;
    }
}
