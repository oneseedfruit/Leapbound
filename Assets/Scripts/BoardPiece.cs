using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece : MovingPiece
{
    [SerializeField]
    private TileCoord startTileCoord = new TileCoord(0, 0);

    private Board board;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        board = GameManager.instance.Board;
        transform.position = board.GetTilePositionFromTileCoord(startTileCoord.x, startTileCoord.y);

        EventManager.instance.onClickTileCoord += MoveToTileCoord;
    }

    protected void MoveToTileCoord(int x, int y)
    {
        Vector3 target = board.GetTilePositionFromTileCoord(x, y);
        Vector3 targetDir = target - transform.position;

        AttemptMove<BoardPiece>(targetDir.x, targetDir.y);
    }

    protected override void OnCantMove<T>(T component)
    {
        
    }
}
