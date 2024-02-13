using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileStatus
{
    UNVISITED,
    OPEN,
    CLOSED,
    IMPASSABLE,
    GOAL,
    START
};

public enum NeighbourTile
{
    TOP_TILE,
    RIGHT_TILE,
    BOTTOM_TILE,
    LEFT_TILE,
    NUM_OF_NEIGHBOUR_TILES
};

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Color[] colors;
    [SerializeField] GameObject minePrefab;

    private GameObject[,] grid;
    private List<GameObject> mines = new List<GameObject>();
    private int rows = 12;
    private int columns = 16;

    public static GridManager Instance { get; private set; } // Static object of the class.

    void Awake()
    {
        if (Instance == null) // If the object/instance doesn't exist yet.
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances.
        }
    }

    private void Initialize()
    {
        BuildGrid();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            Vector2 gridPosition = GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            GameObject mineInst = GameObject.Instantiate(minePrefab, new Vector3(gridPosition.x, gridPosition.y), Quaternion.identity);

            mines.Add(mineInst);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            foreach(GameObject mine in mines)
            {
                Destroy(mine);
            }
            mines.Clear();
        }
    }

    private void BuildGrid()
    {
        grid = new GameObject[rows, columns];
        int count = 0;
        float rowPos = 5.5f;

        for (int row = 0; row <rows; row++, rowPos--)
        {
            float colPos = -7.5f;
            for (int col = 0; col <columns; col++, colPos++)
            {
                GameObject tileInst = GameObject.Instantiate(tilePrefab, new Vector3(colPos, rowPos, 0),Quaternion.identity);
                tileInst.GetComponent<TileScript>().SetColor(colors[System.Convert.ToInt32((count++ % 2 == 0))]);

                tileInst.transform.parent = transform;
                grid[row, col] = tileInst;
            }
            count--;
        }
    }

    private void ConnectGrid()
    {
        for(int row = 0; row<rows; row++)
        {
            for(int col = 0; col<columns; col++)
            {
                TileScript tileScript = grid[row, col].GetComponent<TileScript>();
                if (row > 0)
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.TOP_TILE, grid[row - 1, col]);
                }
                if (col < columns - 1)
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.RIGHT_TILE, grid[row, col+1]);
                }
                if (row > rows -1)
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.BOTTOM_TILE, grid[row + 1, col]);
                }
                if (col > 0)
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.LEFT_TILE, grid[row, col - 1]);
                }
            }
        }
    }

    public GameObject[,] GetGrid()
    {
        // Fix for Lab 4 Part 1.
        //
        // return grid;
        return null;
    }

    // The following utility function creates the snapping to the center of a tile.
    public Vector2 GetGridPosition(Vector2 worldPosition)
    {
        float xPos = Mathf.Floor(worldPosition.x) + 0.5f;
        float yPos = Mathf.Floor(worldPosition.y) + 0.5f;
        return new Vector2(xPos, yPos);
    }
}
