using UnityEngine;
using System.Collections;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Enemy is triggered is within short y distance to enemy.
 *  - If moves horizontally based on where you are.
 *  - It inherits Enemy class.
 */

public class TriggeredHorizontalEnemy : Enemy {

    // Horizontal speed
    public float horizontalSpeed = 0f;
    private float storeHorizontalSpeed;

    // Distance between basket and enemy
    private Vector2 distance;

    // Do something once
    private bool once = false;



	// Use this for initialization
	void Start () {

        // Call base Start()
        base.Start();

        // Store horizontal speed
        storeHorizontalSpeed = horizontalSpeed;
        horizontalSpeed = 0f;
	}
	
	// Update is called once per frame
	void Update () {

        // Triggers the movement
        TriggerMovement();
	
        // Set the velocity
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, 0f);

	}


    /*
     * If basket is within a range in y position then set off the enemy.
     * Determine which side the basket is on to set horizontal speed.
     */
    void TriggerMovement()
    {
        // So you only do this once after you are within range
        if (!once)
        {
            // Y distance between basket and enemy
            distance = transform.position - basket.GetComponent<Transform>().position;

            // distance is less than 1
            if (distance.y < 1.5f)
            {
                
                // If to the left of the object
                if (distance.x > 0)
                {
                    horizontalSpeed = -storeHorizontalSpeed;
                }
                // If to the right of the object
                else
                {
                    horizontalSpeed = storeHorizontalSpeed;
                }

                // Set once to true
                once = true;
            }
        }

    }


}
