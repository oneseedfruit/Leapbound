using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{    
    private int stamina = 6;

    private void OnCollisionEnter(Collision other) 
    {
        stamina--;

        if (stamina <= 0)
        {
            Destroy(gameObject);
        }
    }
}