using UnityEngine;
using System.Collections;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - If main character is inside trigger collider of air stream, then
 *    add velocity to main character.
 */

public class AirStream : MonoBehaviour {

    // Basket gameObject
    public GameObject basket;

    // Horizontal and vertical speeds
    public float horizontalSpeed = 0f;
    public float verticalSpeed = 0f;

    // Boolean for if user can have input while in air stream
    public bool dontAllowMovement = false;


	// Use this for initialization
	void Start () {

        // Just in case I forgot to set this
        if (basket == null)
        {
            basket = GameObject.FindGameObjectWithTag("Basket");
        }

	}


    /*
     * If balloon or dead balloon enters trigger then set off air stream velocity.
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        // If other is a balloon (I might add the tag basket)
        if (other.gameObject.tag == "Balloon" ||
            other.gameObject.tag == "DeadBalloon")
        {

            // If this is true then you can't move inside air stream
            if(dontAllowMovement)
            {
                // Turn off your initial entering speed
                basket.GetComponent<ControlBalloonMovement>().SetZeroHorizontalSpeed();
            }

            

            // Set air stream boolean to true
            basket.GetComponent<ControlBalloonMovement>().SetAirStreamBoolean(true);

            // Set the air stream velocity
            basket.GetComponent<ControlBalloonMovement>()
                .SetAirStreamVelocity(horizontalSpeed, verticalSpeed);
        }
    }


    /*
     * If balloon or dead boolean leaves trigger then set air stream boolean to false.
     * That boolean slowly increments the added velocity to 0.
     */
    void OnTriggerExit2D(Collider2D other)
    {
        // If other is a balloon(I might add basket)
        if (other.gameObject.tag == "Balloon" ||
            other.gameObject.tag == "DeadBalloon")
        {
            // Set air stream boolean to false
            basket.GetComponent<ControlBalloonMovement>().SetAirStreamBoolean(false);

            // If this is true then don't allow movement inside of air stream
            if(dontAllowMovement)
            {
                basket.GetComponent<ControlBalloonMovement>().SetZeroHorizontalSpeed();
            }
        }
    }
    

    /*
     * The object is considered visible when it needs to be rendered in the scene.
     * So when off camera it is invisible.
     */
    void OnBecameInvisible()
    {
        enabled = false;
    }


    /*
     * The object is considered visible when it needs to be rendered in the scene.
     * So when on camera it becomes visible.
     */
    void OnBecameVisible()
    {
        enabled = true;
    }
}
