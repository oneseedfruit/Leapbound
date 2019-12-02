using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private TileCoord nextEnemySpawnTileCoord;
    public TileCoord NextEnemySpawnTileCoord
    {
        get
        {
            return nextEnemySpawnTileCoord;
        }

        set
        {
            nextEnemySpawnTileCoord = value;
        }
    }    

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
    private PlayerPiece playerPiece;
    public GameObject enemy;
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> turnParticipants = new List<GameObject>();
    public GameObject crosshair;
    public Vector2 playerStartTileCoord = new Vector2(0, 0);

    [SerializeField]
    private int turn = 0;

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
        playerPiece = player.GetComponent<PlayerPiece>();
        turnParticipants.Add(player);

        crosshair = Instantiate(crosshair, board.GetTilePositionFromTileCoord(0, 0), Quaternion.identity) as GameObject;
        EventManager.instance.onSpawnNextEnemy += SpawnNextEnemy;
    }

    public void SpawnNextEnemy(int x, int y)
    {
        bool alreadySpawned = false;

        for (int i = 0; i < enemies.Count && !alreadySpawned; i++)
        {
            EnemyPiece t = enemies[i].GetComponent<EnemyPiece>();
            if (t.TileCoord.x.Equals(x) && t.TileCoord.y.Equals(y))
            {
                alreadySpawned = true;
            }
        }

        if (!alreadySpawned)
        {
            Vector3 spawnPos = board.GetTilePositionFromTileCoord(x, y);
            GameObject e = Instantiate(enemy, spawnPos, Quaternion.identity) as GameObject;
            e.GetComponent<EnemyPiece>().TileCoord = new TileCoord(x, y);
            enemies.Add(e);
        }
    }

    private void Update()
    {
        StartCoroutine(UpdateTurn());
    }

    protected IEnumerator UpdateTurn()
    {	        
        int turnParticipantCount = turnParticipants.Count;
        int currentParticipant = 0;

        while(currentParticipant < turnParticipantCount)
        {   
            ITurn current = turnParticipants[currentParticipant].GetComponent<ITurn>();
            if (!current.IsTurn)
            {
                currentParticipant++;
            }
            yield return null;
        }

        turn++;

        for (int i = 0; i < turnParticipants.Count; i++)
        {
            turnParticipants[i].GetComponent<ITurn>().ResetTurn();
        }
    }
}