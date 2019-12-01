using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Board board;
    public Board Board
    {
        get 
        {
            return board;
        }
    }

    public GameObject player;
    public GameObject crosshair;
    public Vector2 playerStartTileCoord = new Vector2(0, 0);

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(gameObject);            
        }

        DontDestroyOnLoad(gameObject);

        board = GetComponent<Board>();
        board.MakeBoard();
    }

    private void Start() 
    {
        player = Instantiate(player, board.GetTilePositionFromTileCoord(0, 0), Quaternion.identity) as GameObject;
        crosshair = Instantiate(crosshair, board.GetTilePositionFromTileCoord(0, 0), Quaternion.identity) as GameObject;
    }
}
