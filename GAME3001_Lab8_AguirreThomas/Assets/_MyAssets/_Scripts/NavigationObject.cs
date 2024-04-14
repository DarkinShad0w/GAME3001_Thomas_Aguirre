using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationObject : MonoBehaviour
{

    [Header("Torpedo Properties")]
    private bool readyToFire = true;
    [SerializeField] float torpedoCooldown;
    [SerializeField] float torpedoLifespan;
    [SerializeField] GameObject torpedoPrefab;
    [SerializeField] float combatRange;

    public Vector2 gridIndex;

    private float timeSinceLastFire;

    void Awake()
    {
        gridIndex = new Vector2();
        SetGridIndex();
    }

    void Update()
    {

        if (!readyToFire)
        {
            timeSinceLastFire += Time.deltaTime;
            if (timeSinceLastFire >= torpedoCooldown)
            {
                readyToFire = true;
                timeSinceLastFire = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && readyToFire)
        {
            FireTorpedo();
            Game.Instance.SOMA.PlaySound("Torpedo");
        }
    }

    void FireTorpedo()
    {
        // Calculate the direction from the player to the mouse cursor
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Instantiate the torpedo
        GameObject torpedo = Instantiate(torpedoPrefab, transform.position, Quaternion.identity);
        PlayerTorpedoScript torpedoScript = torpedo.GetComponent<PlayerTorpedoScript>();
        if (torpedoScript != null)
        {
            // Set the direction of the torpedo
            torpedoScript.LockOnTarget(direction);
        }

        // Set the torpedo to destroy itself after its lifespan
        Destroy(torpedo, torpedoLifespan);

        readyToFire = false;
    }

    public Vector2 GetGridIndex()
    {
        return gridIndex;
    }

    public void SetGridIndex() 
    {
        float originalX = Mathf.Floor(transform.position.x) + 0.5f;
        gridIndex.x = (int)Mathf.Floor((originalX + 7.5f) / 15 * 15);
        float originalY = Mathf.Floor(transform.position.y) + 0.5f;
        gridIndex.y = 11 - (int)Mathf.Floor(originalY + 5.5f);
    }

    public bool HasLOS(GameObject source, string targetTag, Vector2 whiskerDirection, float whiskerLength)
    {
        // Set the layer of the source to Ignore Linecast.
        source.layer = 3;

        // Create the layermask for the ship.
        int layerMask = ~(1 << LayerMask.NameToLayer("Ignore Linecast"));

        // Cast a ray in the whisker direction.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, whiskerDirection, whiskerLength, layerMask);

        // Reset the source's layer.
        source.layer = 0;

        if (hit.collider != null && hit.collider.CompareTag(targetTag))
        {
            return true;
        }
        return false;

    }
}
