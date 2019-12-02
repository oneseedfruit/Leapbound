using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPiece : MovingPiece
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
}
