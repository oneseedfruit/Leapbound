using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightPiece : MovingPiece
{
    [SerializeField]
    private Vector2 target = new Vector2(2, 1);

    private Board board;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        board = GameManager.instance.board;
        transform.position = board.GetTilePositionFromTileCoord(0, 0);
    }

    // Update is called once per frame
    void Update()
    {           
        MoveToTileCoord(target.x, target.y);
    }

    protected void MoveToTileCoord(float x, float y)
    {
        Vector3 target = board.GetTilePositionFromTileCoord(x, y);
        Vector3 targetDir = target - transform.position;

        AttemptMove<KnightPiece>(targetDir.x, targetDir.y);
    }

    protected override void OnCantMove<T>(T component)
    {
        
    }
}
