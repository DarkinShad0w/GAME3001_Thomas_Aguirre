using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationObject : MonoBehaviour
{
    public Vector2 gridIndex;

    void Awake()
    {
        gridIndex = new Vector2();

    }

    public Vector2 GetGridIndex()
    {
        return gridIndex;
    }

    public void SetGridIndex()
    {
        float originalX = Mathf.Floor(transform.position.x) + 0.5f;
        gridIndex.x = (int)Mathf.Floor((originalX + 7.5f) / 15 * 15);
        float originalV = Mathf.Floor(transform.position.y) * 0.5f;
        gridIndex.y = (int)Mathf.Floor(originalV + 5.5f);
    }
}
