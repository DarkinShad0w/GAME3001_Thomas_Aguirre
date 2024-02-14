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
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private GameObject tilePanelPrefab;
    [SerializeField] private GameObject panelParent;
    [SerializeField] Color[] colors;

    [SerializeField] private bool useManhattanHeuristic = true;
    [SerializeField] private float baseTileCost = 1;

    private GameObject[,] grid;
    private List<GameObject> mines = new();
    private int rows = 12;
    private int cols = 16;

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
        ConnectGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
            panelParent.SetActive(!panelParent.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector2 gridPosition = GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            GameObject mineInst = Instantiate(minePrefab, new Vector3(gridPosition.x, gridPosition.y), Quaternion.identity);
            Vector2 mineIndex = mineInst.GetComponent<NavigationObject>().GetGridIndex();
            grid[(int)mineIndex.y, (int)mineIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.IMPASSABLE);

            mines.Add(mineInst);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var mine in mines)
            {
                Vector2 mineIndex = mine.GetComponent<NavigationObject>().GetGridIndex();
                grid[(int)mineIndex.y, (int)mineIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
            }

            mines.Clear();
        }
    }

    private void BuildGrid()
    {
        grid = new GameObject[rows, cols];

        int count = 0;
        float rowPos = 5.5f;
        for (int row = 0; row < rows; row++, rowPos--)
        {
            float colPos = -7.5f;
            for (int col = 0; col < cols; col++, colPos++)
            {
                GameObject tileInst = Instantiate(tilePrefab, new(colPos, rowPos, 0), Quaternion.identity);
                TileScript tileScript = tileInst.GetComponent<TileScript>();
                tileScript.SetColor(colors[System.Convert.ToInt32(count++ % 2 == 0)]);
                tileInst.transform.SetParent(transform);
                grid[row, col] = tileInst;

                GameObject panelInst = Instantiate(tilePanelPrefab, tileInst.transform.position, Quaternion.identity);
                RectTransform panelTransform = panelInst.GetComponent<RectTransform>();
                panelInst.transform.SetParent(panelParent.transform);
                panelTransform.localScale = Vector3.one;
                panelTransform.anchoredPosition = new Vector3(64 * col, -64 * row);
                tileScript.tilePanel = panelInst.GetComponent<TilePanel>();
            }
            count--;
        }

        GameObject ship = GameObject.FindGameObjectWithTag("ship");
        Vector2 shipIndices = ship.GetComponent<NavigationObject>().GetGridIndex();
        grid[(int)shipIndices.y, (int)shipIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.START);

        GameObject planet = GameObject.FindGameObjectWithTag("Planet");
        Vector2 planetIndices = planet.GetComponent<NavigationObject>().GetGridIndex();

        grid[(int)planetIndices.y, (int)planetIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.GOAL);
        SetTileCosts(shipIndices);
    }

    private void ConnectGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                TileScript tileScript = grid[row, col].GetComponent<TileScript>();
                if (row > 0)
                    tileScript.SetNeighbourTile((int)NeighbourTile.TOP_TILE, grid[row - 1, col]);
                if (col < cols - 1)
                    tileScript.SetNeighbourTile((int)NeighbourTile.RIGHT_TILE, grid[row, col + 1]);
                if (row < rows - 1)
                    tileScript.SetNeighbourTile((int)NeighbourTile.BOTTOM_TILE, grid[row + 1, col]);
                if (col > 0)
                    tileScript.SetNeighbourTile((int)NeighbourTile.LEFT_TILE, grid[row, col - 1]);
            }
        }
    }

    public GameObject[,] GetGrid()
    {
        return grid;
    }

    // The following utility function creates the snapping to the center of a tile.
    public Vector2 GetGridPosition(Vector2 worldPosition)
    {
        float xPos = Mathf.Floor(worldPosition.x) + 0.5f;
        float yPos = Mathf.Floor(worldPosition.y) + 0.5f;
        return new Vector2(xPos, yPos);
    }

    public void SetTileCosts(Vector2 targetIndices)
    {
        float distance = 0;
        float dx = 0;
        float dy = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                TileScript tileScript = grid[row, col].GetComponent<TileScript>();
                if (useManhattanHeuristic)
                {
                    dx = Mathf.Abs(col - targetIndices.x);
                    dy = Mathf.Abs(row - targetIndices.y);

                    distance = dx + dy;
                }
                else // euclidean distance
                {
                    dx = targetIndices.x - col;
                    dy = targetIndices.y - row;

                    distance = Mathf.Sqrt(dx * dx + dy * dy);
                }

                float adjustedCost = distance * baseTileCost;

                tileScript.cost = adjustedCost;
                tileScript.tilePanel.costText.text = tileScript.cost.ToString("F1");
            }
        }
    }
}
