using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{    
    public GameObject spawnedBy;

    private int stamina = 6;

    private void OnCollisionEnter(Collision other) 
    {
        stamina--;

        if (stamina <= 0)
        {
            if (spawnedBy != null)
            {
                spawnedBy.GetComponent<PlayerPiece>().ExpireBullet();
            }
            Destroy(gameObject);
        }
    }
}