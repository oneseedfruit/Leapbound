using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPiece : MovingPiece, ITurn
{
    public int hpCount = 50;

    private Text hpLabel;

    private TileCoord moveToThisTile = new TileCoord(0, 0);
    protected override void Start()
    {
        base.Start();
        hpLabel = GetComponentInChildren<Text>();
    }

    private void Update() 
    {           
        MoveToTileCoord(moveToThisTile.x, moveToThisTile.y);
        EndTurn();
        hpLabel.rectTransform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, -0.8f, 0));
        hpLabel.text = hpCount.ToString();
    }

    private void ShowLegalTilesFromHere(Color color)
    {
        List<GameObject> legalTiles = GetLegalTilesFromHere();
        
        for (int i = 0; i < legalTiles.Count; i++)
        {
            Tile tile = legalTiles[i].GetComponent<Tile>();
            if (!tile.IsHovered)
            {
                tile.Select(color);
            }
        }
    }

    private List<GameObject> GetLegalTilesFromHere()
    {
        List<GameObject> legalTiles = new List<GameObject>();
        
        for (int i = 0; i < GameManager.instance.Board.tiles.Count; i++)
        {
            GameObject tile = GameManager.instance.Board.tiles[i];
            Tile t = tile.GetComponent<Tile>();
            if (IsMoveToTileCoordLegal(t.tileCoord.x, t.tileCoord.y) && 
                !GameManager.instance.CheckIfTileCoordIsOccupied(t.tileCoord.x, t.tileCoord.y))
            {
                legalTiles.Add(GameManager.instance.Board.tiles[i]);
            }
        }

        return legalTiles;
    }

    private bool IsMoveToTileCoordLegal(int x, int y)
    {
        bool isLegalMove = true;

        if (x > 7 || x < 0 || y > 7 || y < 0)
            isLegalMove = false;
        
        return isLegalMove;
    }

    private TileCoord GetRelativeTileCoordFrom(int x, int y)
    {
        int xDiff = x - TileCoord.x;
        int yDiff = y - TileCoord.y;

        return new TileCoord(xDiff, yDiff);
    }

    protected void MoveToTileCoord(int x, int y)
    {   
        if (!HasMovedThisTurn)
        {
            if (GameManager.instance.CheckIfTileCoordIsOccupied(x, y) || !IsMoveToTileCoordLegal(x, y))
            {
                HasMovedThisTurn = true;
                return;
            }

            if (IsTurn)
                Debug.Log("Want to move to " + x + ", " + y);
            
            Vector3 target = GameManager.instance.Board.GetTilePositionFromTileCoord(x, y);
            Vector3 targetDir = target - transform.position;

            if (AttemptMove<EnemyPiece>(targetDir.x, targetDir.y))
            {
                TileCoord = new TileCoord(x, y);
            }
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        
    }

    private void OnCollisionStay(Collision other) 
    {
        if (other != null)
        {
            if (other.collider.tag == "Enemy")
            {
                GameManager.instance.enemies.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private bool isTurn = false;
    public bool IsTurn 
    { 
        get
        {
            return isTurn;
        }
    }

    public void NewRandomDirection()
    {
        if (HasMovedThisTurn && !isTurn)
        {
            if (Random.value > 0.75f)
            {
                moveToThisTile = new TileCoord(TileCoord.x + 1, TileCoord.y);
            }
            else if (Random.value > 0.50f)
            {
                moveToThisTile = new TileCoord(TileCoord.x - 1, TileCoord.y);
            } 
            else if (Random.value > 0.25f)
            {
                moveToThisTile = new TileCoord(TileCoord.x, TileCoord.y + 1);
            }
            else
            {
                moveToThisTile = new TileCoord(TileCoord.x, TileCoord.y - 1);
            }
        }
    }

    public void EndTurn()
    {
        if (HasMovedThisTurn)
        {            
            isTurn = false;
        }
    }

    public void ResetTurn()
    {
        NewRandomDirection();
        ResetMoveTurn();
        isTurn = true;
    }

    private void GetHurt(int attackedPower)
    {
        hpCount -= attackedPower;

        if (hpCount <= 0)
        {
            GameManager.instance.enemies.Remove(gameObject);
            GameManager.instance.turnParticipants.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.collider.tag == "Bullet")
        {
            Bullet b = other.collider.GetComponent<Bullet>();
            if (b.spawnedBy.tag == "Player")
            {
                GetHurt(b.attackPower);
            }
        }
    }
}
