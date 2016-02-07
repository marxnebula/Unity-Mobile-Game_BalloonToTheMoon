using UnityEngine;
using System.Collections;


/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Class for the bubble item.
 *  - If user collides with the bubble item then delete bubble item, play a sound and
 *    equal a bubble around the main character.
 */

public class BubbleItem : MonoBehaviour {

    // Get the bubble game object
    public GameObject bubble;


    /*
     * If trigger collider has balloon or basket collider enter it then destroy the bubble item and
     * equal a bubble to the main character.
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        // If other is a balloon or basket
        if (other.gameObject.tag == "Balloon" || other.gameObject.tag == "Basket")
        {
            // Play the audio
            GetComponent<AudioSource>().Play();

            // Destroy the collider
            Destroy(GetComponent<CircleCollider2D>());

            // Unrender the bubble item
            GetComponent<SpriteRenderer>().enabled = false;

            // Render the bubble
            bubble.GetComponent<SpriteRenderer>().enabled = true;

            // Turn bubble collider on
            bubble.GetComponent<CircleCollider2D>().enabled = true;
           
            // Destroy the bubble after the sound has played
            Destroy(gameObject, GetComponent<AudioSource>().clip.length);

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
