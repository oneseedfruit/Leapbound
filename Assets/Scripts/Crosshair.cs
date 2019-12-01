using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private PlayerPiece player;

    private SpriteRenderer sprCrosshair;
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE 
    private Vector2 touchOrigin = -Vector2.one;
#endif
    // Start is called before the first frame update
    void Start()
    {   
        player = GameManager.instance.player.GetComponent<PlayerPiece>();
        sprCrosshair = GetComponentInChildren<SpriteRenderer>();
    }   

    private void Update() 
    {
        sprCrosshair.enabled = false;

        if (!player.HasAttackedThisTurn && player.HasMovedThisTurn)
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
                #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
                    if (Input.touchCount > 0)        
                    {                        
                        Touch myTouch = Input.touches[0];
                        
                        //Check if the phase of that touch equals Began
                        if (myTouch.phase == TouchPhase.Moved)            
                        {
                            sprCrosshair.enabled = true;
                            transform.position = Camera.main.ScreenToWorldPoint(myTouch.position);
                            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                        }
                    }
                #endif

                    if (Input.GetMouseButton(0))
                    {
                        sprCrosshair.enabled = true;
                        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                    }                    
                }
            }    
        }
    }
}
