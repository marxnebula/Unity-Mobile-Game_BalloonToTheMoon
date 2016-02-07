using UnityEngine;
using System.Collections;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Enemy is triggered if main character comes within a y range of it.
 *  - The y range is basically the screen height.
 *  - Enemy can be given horizontal or veritcal speed.
 *  - It inherits Enemy class.
 */

public class TriggeredMovingEnemy : Enemy {

    // Horizontal speed
    public float horizontalSpeed = 0f;
    private float storeHorizontalSpeed = 0f;

    // Vertical speed
    public float verticalSpeed = 0f;
    private float storeVerticalSpeed = 0f;

    // Distance between basket and enemy
    private Vector2 distance;

    // Do something once
    private bool once = false;



    // Use this for initialization
    void Start()
    {

        // Call base Start()
        base.Start();

        // Store horizontal speed
        storeHorizontalSpeed = horizontalSpeed;
        horizontalSpeed = 0f;

        // Store vertical speed
        storeVerticalSpeed = verticalSpeed;
        verticalSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        // Triggers the movement
        TriggerMovement();

        // Set the velocity
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, verticalSpeed);

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
            if (distance.y < 11f)
            {
                // Set off vertical speed
                verticalSpeed = storeVerticalSpeed;
                horizontalSpeed = storeHorizontalSpeed;

                // Set once to true
                once = true;
            }
        }

    }

    /*
     * If collider on this game object is trigger and the other collider exists then
     * this function is called.
     */
    new void OnTriggerEnter2D(Collider2D other)
    {

        // Call the base function
        base.OnTriggerEnter2D(other);

        // If you touch left collider
        if (other.gameObject.tag == "LeftEdge")
        {
            horizontalSpeed = storeHorizontalSpeed;
        }
        // If you touch right collider
        else if (other.gameObject.tag == "RightEdge")
        {
            horizontalSpeed = -storeHorizontalSpeed;
        }

    }
}
