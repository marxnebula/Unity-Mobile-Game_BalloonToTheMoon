using UnityEngine;
using System.Collections;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Input horizontal and/or vertical speed for enemy.
 *  - If it hits the wall then it changes direction.
 *  - It inherits the Enemy class.
 */

public class MovingEnemy : Enemy {

    // Horizontal speed
    public float horizontalSpeed = 0f;
    private float storeHorizontalSpeed;

    // Vertical speed
    public float verticalSpeed = 0f;
    private float storeVerticalSpeed;

    // Random numbers
    private float randomHorizontalNumber = 0f;
    private float randomVerticalNumber = 0f;

    // Bool for if random
    public bool isRandomHorizontalSpeed = false;
    public bool isRandomVerticalSpeed = false;



	// Use this for initialization
	void Start () {

        // Call base Start()
        base.Start();
         

        if(isRandomHorizontalSpeed)
        {
            RandomlySetHorizontalSpeed();
        }

        // If random vertical speed boolean is true
        if (isRandomVerticalSpeed)
        {
            // Randomly sets vertical speed
            RandomlySetVerticalSpeed();
        }

        // Store the horizontal speed
        storeHorizontalSpeed = horizontalSpeed;

        // Store the vertical speed
        storeVerticalSpeed = verticalSpeed;

	}
	
	// Update is called once per frame
	void Update () {

        // Set the velocity
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, verticalSpeed);

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
            // If it's the random speed then set the correct speed
            if (isRandomHorizontalSpeed)
            {
                horizontalSpeed = randomHorizontalNumber;
            }
            else
            {
                horizontalSpeed = storeHorizontalSpeed;
            }

        }
        // If you touch right collider
        else if (other.gameObject.tag == "RightEdge")
        {
            // If it's the random speed then set the correct speed
            if (isRandomHorizontalSpeed)
            {
                horizontalSpeed = -randomHorizontalNumber;
            }
            else
            {
                horizontalSpeed = -storeHorizontalSpeed;
            }

        }
        
    }


    /*
     * Randomlys sets the horizontal speed from 0.5 to 2
     */
    void RandomlySetHorizontalSpeed()
    {

        // Gets random float number from 0 to n-1
        randomHorizontalNumber = Random.Range(0.5f, 2f);

        // Set the horizontal speed to the random number
        horizontalSpeed = randomHorizontalNumber;

    }

    /*
    * Randomlys sets the vertical speed from 0.5 to 2
    */
    void RandomlySetVerticalSpeed()
    {

        // Gets random float number from 0 to n-1
        randomVerticalNumber = -Random.Range(0.5f, 2f);

        // Set the horizontal speed to the random number
        verticalSpeed = randomVerticalNumber;

    }
}
