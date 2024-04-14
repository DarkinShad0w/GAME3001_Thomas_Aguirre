using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorpedoScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private Vector2 direction;
    private Vector2 vectorToTarget;

    void Update()
    {
        transform.Translate(vectorToTarget.x, vectorToTarget.y, 0f);   
    }

    public void LockOnTarget(Vector2 targetDirection)
    {
        direction = targetDirection.normalized;
    }
}
