using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MovingPiece
{
    [SerializeField]
    private TileCoord startTileCoord = new TileCoord(5, 3);

    private Board board;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        board = GameManager.instance.Board;

        MoveToTileCoord(startTileCoord.x, startTileCoord.y);
        EventManager.instance.onClickTileCoord += MoveToTileCoord;        
    }

    private void Update() 
    {
        ShowLegalTilesFromHere();
    }

    private void ShowLegalTilesFromHere()
    {
        List<GameObject> legalTiles = GetLegalTilesFromHere();
        
        for (int i = 0; i < legalTiles.Count; i++)
        {
            legalTiles[i].GetComponent<Tile>().Select(Color.blue);
        }
    }

    private List<GameObject> GetLegalTilesFromHere()
    {
        List<GameObject> legalTiles = new List<GameObject>();
        
        for (int i = 0; i < GameManager.instance.Board.tiles.Count; i++)
        {
            GameObject tile = GameManager.instance.Board.tiles[i];
            Tile t = tile.GetComponent<Tile>();
            if (IsMoveToTileCoordLegal(t.tileCoord.x, t.tileCoord.y))
            {
                legalTiles.Add(GameManager.instance.Board.tiles[i]);
            }
        }

        return legalTiles;
    }

    private bool IsMoveToTileCoordLegal(int x, int y)
    {
        bool isLegalMove = true;

        int xDiff = Mathf.Abs(x - TileCoord.x);
        int yDiff = Mathf.Abs(y - TileCoord.y);

        if (!((xDiff == 2 && yDiff == 1) || (xDiff == 1 && yDiff == 2)))
        {
            isLegalMove = false;
        }

        return isLegalMove;
    }

    protected override void MoveToTileCoord(int x, int y)
    {
        if (IsMoveToTileCoordLegal(x, y))
        {
            base.MoveToTileCoord(x, y);
        }
        // ResetMoveTurn();
    }

    protected override void OnCantMove<T>(T component)
    {
        
    }
}
