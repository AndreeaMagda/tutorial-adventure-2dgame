using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public GameObject ContainedObject;
    }

    private CellData[,] m_BoardData;
    private Tilemap m_Tilemap;
    private Grid m_Grid;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] BlockingTiles;

    public GameObject FoodPrefab;
    public List<Vector2Int> m_EmptyCellsList;

    public void Init()
    {
        m_EmptyCellsList = new List<Vector2Int>();
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();

        m_BoardData = new CellData[Width, Height];

        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();

                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    tile = BlockingTiles[Random.Range(0, BlockingTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;
                    m_EmptyCellsList.Add(new Vector2Int(x, y)); // Add passable cells to the list
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        // Ensure the player's starting position is removed
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));
        GenerateFood();
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        

        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }

        return m_BoardData[cellIndex.x, cellIndex.y];
    }

    void GenerateFood()
    {
        int foodCount = 5;
        for (int i = 0; i < foodCount; ++i)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            CellData data = m_BoardData[coord.x, coord.y];
            GameObject newFood = Instantiate(FoodPrefab);
            newFood.transform.position = CellToWorld(coord);
            data.ContainedObject = newFood;
        }
    }
}