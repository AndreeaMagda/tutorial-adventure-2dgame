using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;
    private Grid m_Grid;  // Added Grid reference

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    public PlayerController Player;  // Player reference

    public class CellData
    {
        public bool Passable;
    }

    private CellData[,] m_BoardData;

    // Start is called before the first frame update
    void Start()
    {
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
                    tile = WallTiles.Length > 0 ? WallTiles[Random.Range(0, WallTiles.Length)] : null;
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles.Length > 0 ? GroundTiles[Random.Range(0, GroundTiles.Length)] : null;
                    m_BoardData[x, y].Passable = true;
                }

                if (tile != null)
                {
                    m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

        // Spawning the player at the specified position
        Player.Spawn(this, new Vector2Int(1, 1));
    }

    // Converts a grid cell position to world position
    public Vector3 CellToWorld(Vector2Int cellPosition)
    {
        return m_Grid.CellToWorld(new Vector3Int(cellPosition.x, cellPosition.y, 0));
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width
                            || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }

        return m_BoardData[cellIndex.x, cellIndex.y];
    }
}