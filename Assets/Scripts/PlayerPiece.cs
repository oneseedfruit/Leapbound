using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPiece : MovingPiece, IShootable, ITurn
{
    [SerializeField]
    private TileCoord startTileCoord = new TileCoord(5, 3);

    private Board board;

    public int hpCount = 100;

    private Text hpLabel;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        board = GameManager.instance.Board;

        MoveToTileCoord(startTileCoord.x, startTileCoord.y);
        EventManager.instance.onClickTileCoord += MoveToTileCoord;

        hpLabel = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        Color legalTilesColor = HasMovedThisTurn ? new Color(0.6f, 0.6f, 0.8f) : new Color(0.2f, 0.2f, 0.8f);
        ShowLegalTilesFromHere(legalTilesColor);
        hpLabel.rectTransform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, -0.8f, 0));
        hpLabel.text = hpCount >= 0 ? hpCount.ToString() : "You're Dead!";

        if (!HasAttackedThisTurn && HasMovedThisTurn)
        {
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE     
            //Check if Input has registered more than zero touches
            if (Input.touchCount > 0)
            {
                //Store the first touch detected.
                Touch myTouch = Input.touches[0];
                
                //Check if the phase of that touch equals Began
                if (myTouch.phase == TouchPhase.Began)            
                {
                    //If so, set touchOrigin to the position of that touch
                    touchOrigin = myTouch.position;
                }
                
                //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
                // else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                // {
                //     //Set touchEnd to equal the position of this touch
                //     Vector2 touchEnd = myTouch.position;
                    
                //     //Calculate the difference between the beginning and end of the touch on the x axis.
                //     float x = touchEnd.x - touchOrigin.x;
                    
                //     //Calculate the difference between the beginning and end of the touch on the y axis.
                //     float y = touchEnd.y - touchOrigin.y;
                    
                //     //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
                //     touchOrigin.x = -1;
                    
                //     //Check if the difference along the x axis is greater than the difference along the y axis.
                //     if (Mathf.Abs(x) > Mathf.Abs(y))
                //         //If x is greater than zero, set horizontal to 1, otherwise set it to -1
                //         horizontal = x > 0 ? 1 : -1;
                //     else
                //         //If y is greater than zero, set horizontal to 1, otherwise set it to -1
                //         vertical = y > 0 ? 1 : -1;
                // }
            }        
        #endif
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            ray = Camera.main.ScreenPointToRay(touchOrigin);
        #endif

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {       
                Collider hitCol = hit.collider;     
                if (hitCol != null)
                {
                    GameObject crosshair = GameManager.instance.crosshair;

                #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
                    if (Input.touchCount > 0)        
                    {                        
                        Touch myTouch = Input.touches[0];                        
                        
                        if (myTouch.phase == TouchPhase.Begin)                                    
                        {
                            Aim(crosshair.transform.position.x, crosshair.transform.position.y);
                        }

                        if (myTouch.phase == TouchPhase.Ended)
                        {
                            Fire();
                        }
                    }
                #endif

                    if (HasMovedThisTurn)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            Aim(crosshair.transform.position.x, crosshair.transform.position.y);
                        }

                        if (Input.GetMouseButtonUp(0))
                        {
                            Fire();
                        }
                    }
                }
            }    
        }
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

        int xDiff = Mathf.Abs(x - TileCoord.x);
        int yDiff = Mathf.Abs(y - TileCoord.y);

        if (!((xDiff == 2 && yDiff == 1) || (xDiff == 1 && yDiff == 2)))
        {
            isLegalMove = false;
        }

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
            TileCoord t = new TileCoord(-111, -111);

            if (IsMoveToTileCoordLegal(x, y))
            {
                t = GetRelativeTileCoordFrom(x, y);

                Vector3 target = GameManager.instance.Board.GetTilePositionFromTileCoord(x, y);
                Vector3 targetDir = target - transform.position;

                if (!GameManager.instance.CheckIfTileCoordIsOccupied(x, y))
                {
                    if (AttemptMove<EnemyPiece>(targetDir.x, targetDir.y))
                    {
                        TileCoord = new TileCoord(x, y);
                    }
                }
            }

            t = TileCoord + new Vector3(t.x, t.y, 0);

            if (t.x != -111 && t.y != -111)
            {
                GameObject tile = GameManager.instance.Board.GetTileFromTileCoord(t.x, t.y);
                
                if (tile != null)
                {
                    GameManager.instance.NextEnemySpawnTileCoord = t;
                    EventManager.instance.SpawnNextEnemy(t.x, t.y);
                }
            }
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        
    }

    private float bulletForce = 130f;

    [SerializeField]
    private GameObject projectile;
    public GameObject Projectile 
    { 
        get
        {
            return projectile;
        } 
        
        set
        {
            projectile = value;
        } 
    }

    private int projectilesAllowed = 3;

    private int projectileCount = 3;
    public int ProjectileCount
    {
        get
        {
            return projectileCount;
        }

        set
        {
            projectileCount = value;
        }
    }

    private bool hasAttackedThisTurn = true;    
    public bool HasAttackedThisTurn 
    { 
        get 
        {
            return hasAttackedThisTurn;
        } 

        set 
        {
            hasAttackedThisTurn = value;
        }
    }

    private Vector2 attackTarget = new Vector2(0, 0);
    public Vector2 AttackTarget 
    { 
        get
        {
            return attackTarget;
        }

        set
        {
            attackTarget = value;
        }
    }

    public void Aim(float x, float y)
    {
        if (HasMovedThisTurn && !HasAttackedThisTurn)
        {
            attackTarget = new Vector3(x, y, 0) - new Vector3(transform.position.x, transform.position.y, 0);            
        }
    }
    
    private IEnumerator ShootBullet()
    {
        for (int i = projectilesAllowed; i > 0; i--)
        {
            Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y, 0);            
            GameObject bullet = Instantiate(projectile, bulletPos, Quaternion.identity) as GameObject;
            bullet.GetComponent<Bullet>().spawnedBy = this.gameObject;    
            Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
            rbBullet.AddForce(attackTarget * bulletForce);
            
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void ExpireBullet()
    {        
        if (projectileCount > 1)
        {
            projectileCount--;
        }
        else
        {
            EndTurn();
        }
    }

    public void Fire()
    {        
        BoxCollider col = GetComponent<BoxCollider>();
        col.enabled = false;
        StartCoroutine(ShootBullet());
        col.enabled = true;
        hasAttackedThisTurn = true;        
    }

    public void ResetAttackTurn()
    {
        projectileCount = projectilesAllowed;
        hasAttackedThisTurn = false;
    }

    private bool isTurn = false;
    public bool IsTurn 
    { 
        get
        {
            return isTurn;
        }
    }

    public void EndTurn()
    {
        if (HasMovedThisTurn && HasAttackedThisTurn)
        {
            isTurn = false;
            projectileCount = projectilesAllowed;
        }
    }
    
    public void ResetTurn()
    {
        ResetMoveTurn();
        ResetAttackTurn();
        isTurn = true;
    }

    private void GetHurt(int attackedPower)
    {
        hpCount -= attackedPower;

        if (hpCount <= 0)
        {            
            
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.collider.tag == "Bullet")
        {
            Bullet b = other.collider.GetComponent<Bullet>();
            if (b.spawnedBy.tag == "Enemy")
            {
                GetHurt(b.attackPower);
            }
        }
    }
}