using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileCoord tileCoord = new TileCoord(0, 0);
    public TileShade tileShade = TileShade.light;        
    public Sprite[] tileSprites = new Sprite[2];    
    public SpriteRenderer sprTile;
    private bool isHovered = false;
    public bool IsHovered
    {
        get
        {
            return isHovered;
        }
    }
    
    private Color hoverColor = new Color(1.0f, 0.5f, 0, 1);

    // Start is called before the first frame update
    private void Awake()
    {
        sprTile = GetComponent<SpriteRenderer>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        sprTile.sprite = tileSprites[ (int) tileShade ];  

        if (isHovered)
        {
            sprTile.color = hoverColor;
            isHovered = false;
        }
        else
        {
            sprTile.color = Color.white;
        }
    }

    public void Select(Color color)
    {
        hoverColor = color;
        isHovered = true;
    }
}
