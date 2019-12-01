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

    private List<GameObject> tiles = new List<GameObject>();

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {       
            Collider hitCol = hit.collider;     
            if (hitCol != null)
            {                
                if (hitCol.tag == "Tile")
                {
                    Tile t = hitCol.gameObject.GetComponent<Tile>();
                    t.Select();

                    if (Input.GetMouseButton(0))
                    {
                        EventManager.instance.ClickTileCoord(t.tileCoord.x, t.tileCoord.y);
                    }
                }
            }
        }    
    }
}