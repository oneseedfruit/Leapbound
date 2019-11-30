using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingPiece : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;
            
    private BoxCollider boxCollider;
    private Rigidbody rb;
    private float inverseMoveTime;
    
    protected virtual void Start()
    {			
        boxCollider = GetComponent<BoxCollider>();		
        rb = GetComponent<Rigidbody>();
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(float xDir, float yDir, out RaycastHit hit)
    {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(xDir, yDir, 0);
        boxCollider.enabled = false;
        Physics.Linecast(start, end, out hit, blockingLayer);
        boxCollider.enabled = true;
        
        if(hit.transform == null)
        {				
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        
        return false;
    }
    
    protected IEnumerator SmoothMovement(Vector3 end)
    {		
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
    
        while(sqrRemainingDistance > float.Epsilon)
        {				
            Vector3 newPostion = Vector3.MoveTowards(rb.position, end, inverseMoveTime * Time.deltaTime);
            rb.MovePosition (newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }
    
    protected virtual void AttemptMove<T>(float xDir, float yDir) where T : Component
    {
        RaycastHit hit;
        bool canMove = Move(xDir, yDir, out hit);

        if(hit.transform == null)
            return;
                    
        T hitComponent = hit.transform.GetComponent<T>();

        if(!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }
    
    protected abstract void OnCantMove<T>(T component) where T : Component;
}
