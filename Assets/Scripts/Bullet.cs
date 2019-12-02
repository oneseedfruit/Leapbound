using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{    
    public GameObject spawnedBy;

    private Transform tfSprBullet;
    private SpriteRenderer sprBullet;

    private int stamina = 5;
    public int attackPower = 1;

    private void Start() 
    {
        tfSprBullet = GetComponentInChildren<Transform>();
        sprBullet = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        attackPower++;

        stamina--;
        tfSprBullet.localScale = new Vector3(tfSprBullet.localScale.x * 0.95f, tfSprBullet.localScale.y * 0.95f, 1);
        sprBullet.color = new Color(sprBullet.color.r, sprBullet.color.g, sprBullet.color.b, sprBullet.color.a * 0.95f);
        if (stamina <= 0)
        {
            if (spawnedBy != null)
            {
                PlayerPiece p = spawnedBy.GetComponent<PlayerPiece>();
                EnemyPiece e = spawnedBy.GetComponent<EnemyPiece>();
                if (p != null)
                {
                    p.ExpireBullet();
                }

                if (e != null)
                {
                    e.ExpireBullet();
                }                    
            }
            Destroy(gameObject);
        }
    }
}