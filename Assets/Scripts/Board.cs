using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab = null;

    [SerializeField]
    private Vector2 XTimesY = new Vector2(8, 8);

    public List<GameObject> tiles = new List<GameObject>();

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE 
    private Vector2 touchOrigin = -Vector2.one;
#endif

    public void MakeBoard()
    {  
        Vector3 pos = Vector3.zero;
        Vector3 tileOffset = new Vector3(-0.5f, -0.5f, 0);
        Vector3 gridOffset = -XTimesY * 0.5f;

        for (int i = 0; i < XTimesY.y; i++)
        {
            for (int j = 0; j < XTimesY.x; j++)
            {
                pos.x = j;                
                pos.y = i;
                pos -= tileOffset - gridOffset;

                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity) as GameObject;
                tile.transform.SetParent(transform);

                Tile t = tile.GetComponent<Tile>();
                t.tileCoord = pos + (tileOffset - gridOffset);
                
                t.tileShade = TileShade.dark;                
                if (i % 2 == 0 && j % 2 != 0)
                    t.tileShade = TileShade.light;
                if (i % 2 != 0 && j % 2 == 0)
                    t.tileShade = TileShade.light;

                tiles.Add(tile);
            }
        }
    }

    public GameObject GetTileFromTileCoord(int x, int y)
    {
        GameObject tile = null;
        TileCoord tileCoord = new TileCoord(x, y);

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].GetComponent<Tile>().tileCoord == tileCoord)
            {
                tile = tiles[i];
            }
        }

        return tile;
    }

    public Vector3 GetTilePositionFromTileCoord(int x, int y)
    {        
        Tile tile = GetTileFromTileCoord(x, y).GetComponent<Tile>();

        return tile.transform.position;
    }

    private void Update() 
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
                if (hitCol.tag == "Tile")
                {
                    Tile t = hitCol.gameObject.GetComponent<Tile>();
                    t.Select(new Color(1.0f, 0.5f, 0.5f));

                    if (Input.GetMouseButton(0))
                    {
                        t.Select(Color.white);
                        EventManager.instance.ClickTileCoord(t.tileCoord.x, t.tileCoord.y);
                    }
                }
            }
        }    
    }
}