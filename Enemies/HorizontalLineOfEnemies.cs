using UnityEngine;
using System.Collections;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Creates a wall of 3 horizontal enemies.
 *  - It randomly leaves an opening just big enough for the main character to enter.
 */

public class HorizontalLineOfEnemies : MonoBehaviour {

    // Basket gameObject
    public GameObject basket;

    // These gameobjects will be created in this script
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    // Vectors for enemies
    private Vector3 enemy1Vector;
    private Vector3 enemy2Vector;
    private Vector3 enemy3Vector;

    // Random number for position
    private int randomNumber = 0;


	// Use this for initialization
	void Start () {

        // Just in case I forgot to set this
        if (basket == null)
        {
            basket = GameObject.FindGameObjectWithTag("Basket");
        }

        // Sets the random position
        RandomlySetPosition();

        // Create the 3 enemies
        Instantiate(enemy1, enemy1Vector, transform.rotation);
        Instantiate(enemy2, enemy2Vector, transform.rotation);
        Instantiate(enemy3, enemy3Vector, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
        RandomlySetPosition();
	}

    /*
     * Randomly sets x position of 3 enemies.
     * x means missing and * means there.
     * 0 = -2.20
     * 1 = -0.80
     * 2 = 0.60
     * 3 = 2.10
     */
    void RandomlySetPosition()
    {
        // Gets random number from 0 to n-1
        randomNumber = Random.Range(0, 4);

        // If random number is 0
        if (randomNumber == 0)
        {
            // x***
            enemy1Vector = new Vector3(-0.80f,
                transform.position.y, transform.position.z);
            enemy2Vector = new Vector3(0.60f,
                transform.position.y, transform.position.z);
            enemy3Vector = new Vector3(2.10f,
                transform.position.y, transform.position.z);
        }
        // If random number is 1
        else if(randomNumber == 1)
        {
            // *x**
            enemy1Vector = new Vector3(-2.35f,
                transform.position.y, transform.position.z);
            enemy2Vector = new Vector3(1.1f,
                transform.position.y, transform.position.z);
            enemy3Vector = new Vector3(2.40f,
                transform.position.y, transform.position.z);
        }
        // If random number is 2
        else if (randomNumber == 2)
        {
            // **x*
            enemy1Vector = new Vector3(-2.35f,
                transform.position.y, transform.position.z);
            enemy2Vector = new Vector3(-1.09f,
                transform.position.y, transform.position.z);
            enemy3Vector = new Vector3(2.40f,
                transform.position.y, transform.position.z);
        }
        // If random number is 3
        else if (randomNumber == 3)
        {
            // ***x
            enemy1Vector = new Vector3(-2.30f,
                transform.position.y, transform.position.z);
            enemy2Vector = new Vector3(-0.80f,
                transform.position.y, transform.position.z);
            enemy3Vector = new Vector3(0.70f,
                transform.position.y, transform.position.z);
        }
    }

}
