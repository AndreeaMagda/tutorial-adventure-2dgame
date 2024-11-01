using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;

    // Spawns the player at the specified cell position
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell;

        // Move to the correct world position based on the cell
        transform.position = m_Board.CellToWorld(m_CellPosition);
    }
}