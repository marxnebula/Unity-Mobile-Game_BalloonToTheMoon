using UnityEngine;
using System.Collections;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - If main character is in trigger collider then play a sound and add 1 coin to game control.
 *  - Unrender and destroy the coin once the audio ends.
 */

public class CollectCoin : MonoBehaviour {

    /*
     * If the trigger collider has another collider enter it.
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        // If other is a balloon or basket
        if (other.gameObject.tag == "Balloon" || other.gameObject.tag == "Basket")
        {
            // Add 1 coin to your total amount
            GameControl.control.AddOneCoin();

            // Play the audio
            GetComponent<AudioSource>().Play();

            // Destroy the collider
            Destroy(GetComponent<CircleCollider2D>());

            // Unrender the coin
            GetComponent<SpriteRenderer>().enabled = false;

            // Destroy the coin after the sound has played
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
