using UnityEngine;
using System.Collections;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Code for trigger collision with balloon and/or basket.
 *  - If trigger then plays sound and adds downward velocity while changing the tag to deadballoon.
 *  - Also if trigger is bubble then it destroys the bubble.
 *  - This class is inherited by almost all enemies.
 */

public class Enemy : MonoBehaviour {

    // Basket gameObject
    public GameObject basket;


	// Use this for initialization
	public void Start () {

        // Just in case I forgot to set this
        if (basket == null)
        {
            basket = GameObject.FindGameObjectWithTag("Basket");
        }

	}
	


    /*
     * If collider on this game object is trigger and a balloon or bubble hits it then
     * "Destroy" the object and set off its downward velocity.
     */
    protected void OnTriggerEnter2D(Collider2D other)
    {

        // If other is a balloon
        if (other.gameObject.tag == "Balloon" ||
            other.gameObject.tag == "Bubble")
        {
            // Play the audio
            GetComponent<AudioSource>().Play();

            // Add velocity downward to the balloon
            basket.GetComponent<ControlBalloonMovement>().SetOffDownwardVelocityWhenBalloonHitsEnemy();


            // If it's a balloon
            if (other.gameObject.tag == "Balloon")
            {
                // Change the tag
                other.tag = "DeadBalloon";

                // Stop rendering
                other.GetComponent<SpriteRenderer>().enabled = false;
            }
            // If it's a bubble
            else
            {
                // Stop rendering
                other.GetComponent<SpriteRenderer>().enabled = false;

                // Turn off collider
                other.enabled = false;
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
