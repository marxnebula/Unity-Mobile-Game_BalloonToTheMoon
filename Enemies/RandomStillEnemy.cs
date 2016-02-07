using UnityEngine;
using System.Collections;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Randomly positions a standing still enemy.
 *  - In the unity interface you select which random positions are acceptable.
 *  - It inherits Enemy class
 */

public class RandomStillEnemy : Enemy {

    // Random numbers
    public int randomPositionNumber = 0;
    public int randomIndex = 0;

    // Booleans for where the enemy can go
    public bool pos0 = false;
    public bool pos1 = false;
    public bool pos2 = false;
    public bool pos3 = false;
    public bool pos4 = false;
    public bool pos5 = false;
    public bool pos6 = false;

    // Position that corresponds with the pos booleans
    private float pos0Number = -2.25f;
    private float pos1Number = -1.50f;
    private float pos2Number = -0.75f;
    private float pos3Number = 0f;
    private float pos4Number = 0.75f;
    private float pos5Number = 1.50f;
    private float pos6Number = 2.25f;

    // Array to store the pos numbers
    public float[] numberPositions = new float[7];




	// Use this for initialization
	void Start () {

        // Base function
        base.Start();

        // Randomly sets position
        RandomlySetPosition();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
    * Randomly sets the position.
    * 0 positon in world space means it is in the center of the screen.
    * 0 = -2.25
    * 1 = -1.50
    * 2 = -0.75
    * 3 = 0
    * 4 = 0.75
    * 5 = 1.50
    * 6 = 2.25
    */
    void RandomlySetPosition()
    {

        // If pos0 is true
        if(pos0)
        {
            // Store number
            numberPositions[randomIndex] = pos0Number;

            // Add 1 to index
            randomIndex++;
        }
        // If pos1 is true then add 1 to index
        if (pos1)
        {
            // Store number
            numberPositions[randomIndex] = pos1Number;

            // Add 1 to index
            randomIndex++;
        }
        // If pos2 is true then add 1 to index
        if (pos2)
        {
            // Store number
            numberPositions[randomIndex] = pos2Number;

            // Add 1 to index
            randomIndex++;
        }
        // If pos3 is true then add 1 to index
        if (pos3)
        {
            // Store number
            numberPositions[randomIndex] = pos3Number;

            // Add 1 to index
            randomIndex++;
        }
        // If pos4 is true then add 1 to index
        if (pos4)
        {
            // Store number
            numberPositions[randomIndex] = pos4Number;

            // Add 1 to index
            randomIndex++;
        }
        // If pos5 is true then add 1 to index
        if (pos5)
        {
            // Store number
            numberPositions[randomIndex] = pos5Number;

            // Add 1 to index
            randomIndex++;
        }
        // If pos6 is true then add 1 to index
        if (pos6)
        {
            // Store number
            numberPositions[randomIndex] = pos6Number;

            // Add 1 to index
            randomIndex++;
        }



        // Gets random number from 0 to n-1
        randomPositionNumber = Random.Range(0, randomIndex);


        // Loop to check what random position number it is and sets position
        for (int i = 0; i < randomIndex; i++ )
        {
            // If random number is i
            if(randomPositionNumber == i)
            {
                // Set the position
                transform.position = new Vector3(numberPositions[i],
                    transform.position.y, transform.position.z);
            }
        }

    }


    /*
     * If some enemies overlap then re-randomize position.
     */
    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            RandomlySetPosition();
        }

    }
}
