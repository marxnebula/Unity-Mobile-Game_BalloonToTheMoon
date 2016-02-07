using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Controls all user input movement with the balloons.
 *  - If 2 balloons remain then it draws 1 unrendered balloon on top of basket.
 *  - If 1 balloon remains then it draws 2 balloons to the left and right side of the
 *    1 balloon.  Only the 1 balloon is rendered though.  This is done to simulate
 *    the same movement with 3 balloons but only shows 1 balloon.
 *  - If a balloon hits an enemy there is a downward velocity.
 *  - There is fake wind forces when the balloon/basket is attached to the anchor.
 */

public class ControlBalloonMovement : MonoBehaviour {

    // The balloons!
    public GameObject leftBalloon;
    public GameObject middleBalloon;
    public GameObject rightBalloon;

    // Booleans for if these balloons are alive
    private bool isLeftBalloon = true;
    private bool isMiddleBalloon = true;
    private bool isRightBalloon = true;

    // Booleans for which way you are moving
    private bool isMovingRight = false;
    private bool isMovingLeft = false;

    // Boolean for if anchored
    private bool isAnchored = true;

    // Boolean for doing something once
    private bool moveBalloonsToSide = true;

    // Boolean for if all balloons ar dead
    private bool allBalloonsAreDead = false;

    // Boolean for air stream
    private bool isAirStream = false;
    private bool turnOffInitialVelocity = false;

    // Number of balloons alive
    public int numberOfBalloons;

    // Storing horizontal speed
    private float storeHorizontalSpeed = 0f;

    // Starting variables for 3 balloons alive
    public float horizontalSpeed = 1.5f;
    public float verticalSpeed = 2f;
    public float increment = 0.007f;

    // Velocity added to normal velocity
    public float balloonPopVelocityChange = 0f;
    public float balloonPopIncrement = 0f;
    public float airStreamHorizontalVelocity = 0f;
    public float airStreamVerticalVelocity = 0f;

    // Store the spring joints from the middle baloon
    private SpringJoint2D[] middleBalloonSpringJoints;

    // Store the spring joints from the basket
    private SpringJoint2D[] basketSpringJoints;

    // Edge bool colliders
    public bool isLeftEdgeCollider = false;
    public bool isRightEdgeCollider = false;

    // Timer for balloon movements when anchored
    private float timer = 0f;

    // Boolean for setting off force if anchored
    private bool isForce = false;
    private float forceSpeedX = 10f;
    private float forceSpeedY = 5f;

    // Script for setting off gameover
    public UI uiScript;


	// Use this for initialization
	void Start () {

        // Incase I forgot to set variables
        if(leftBalloon == null)
        {
            // Set left balloon
            leftBalloon = GameObject.Find("LeftBalloon");
        }

        if (middleBalloon == null)
        {
            // Set middle balloon
            middleBalloon = GameObject.Find("MiddleBalloon");

            // Get the spring joints
            middleBalloonSpringJoints = middleBalloon.GetComponents<SpringJoint2D>();
        }
        else
        {
            // Get the spring joints
            middleBalloonSpringJoints = middleBalloon.GetComponents<SpringJoint2D>();
        }

        if (rightBalloon == null)
        {
            // Set the right balloon
            rightBalloon = GameObject.Find("RightBalloon");
        }
        
        // Always start with 3 balloons
        numberOfBalloons = 3;

        // Set the variables
        DetermineVariablesBasedOnCharacter();

        // Store spring joints from basket
        basketSpringJoints = GetComponents<SpringJoint2D>();
	}
	

	// Update is called once per frame
	void Update () {

        // If there is at least 1 balloon
        if (numberOfBalloons > 0)
        {
            // Calculate how many balloons are alive
            CalculateHowManyBalloonsExist();

            // Don't allow movement updating if on main menu
            if (uiScript.GetGamePlay() == true)
            {
                // Get user input and control horizontal speed
                UpdateUserInputAndLimitHorizontalSpeed();
            }

            // Turns off balloon joints and sets distance for distance joint
            TurnOffSpringJointsAndSetDistanceJoint();

            // Update 1 balloon stuff
            UpdateIfOnlyOneBalloon();

            // Updates the added velocity to the balloons if they hit an object
            UpdateDownwardVelocitySetOffFromBalloonHitsEnemy();

            // Updates the air stream velocity
            UpdateAirStreamVelocity();

            // Update the velocity
            UpdateBalloonsMovement();

            // Update if they all die
            UpdateIfAllBalloonsAreDead();

        }


        // If anchored then call anchor method
        if (isAnchored)
        {
            UpdateMainCharacterAnchors();
        }

	}


    /*
     * Sets variables based on what character you have selected.
     * Variables include horizontal speed, vertical speed, and increments.
     */
    public void DetermineVariablesBasedOnCharacter()
    {
        if(GameControl.control.GetIsNormalCharacter())
        {
            // Store the horizontal speed
            storeHorizontalSpeed = 1.5f;

            // Set initial speed to 0
            horizontalSpeed = 0f;
        }
        else if(GameControl.control.GetIsSantaCharacter())
        {
            // Store the horizontal speed
            storeHorizontalSpeed = 1.5f;

            // Set initial speed to 0
            horizontalSpeed = 0f;
        }
    }


    /*
     * Calculates how many balloons exist.
     * numberofBalloons start at 3 and you minus 1 if a balloons render is false.
     */
    void CalculateHowManyBalloonsExist()
    {

        // If left balloons render is false
        if(leftBalloon.GetComponent<SpriteRenderer>().enabled == false && isLeftBalloon)
        {
            numberOfBalloons -= 1;
            isLeftBalloon = false;
        }

        // If middle balloons render is false
        if (middleBalloon.GetComponent<SpriteRenderer>().enabled == false && isMiddleBalloon)
        {
            numberOfBalloons -= 1;
            isMiddleBalloon = false;
        }

        // If right baloons render is false
        if (rightBalloon.GetComponent<SpriteRenderer>().enabled == false && isRightBalloon)
        {
            numberOfBalloons -= 1;
            isRightBalloon = false;
        }

        // Set off boolean of all balloons being deaad
        if(numberOfBalloons == 0)
        {
            allBalloonsAreDead = true;
        }
        
    }


    /*
     * Gets the users input and sets variables based on if moving left or right.
     * The increment will keep increasing or decreasing(depending if moving left or right) and
     * will stop at 0.
     */
    void UpdateUserInputAndLimitHorizontalSpeed()
    {

        // If user is running on android
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                // TouchPhase.Began means a finger touched the screen
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    UpdateMainCharacterBasedOnTouchedScreen(Input.GetTouch(0).position);
                }
            }
        }

        // If user is running the editor
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // If left click mouse is up
            if (Input.GetMouseButtonUp(0))
            {
                UpdateMainCharacterBasedOnTouchedScreen(Input.mousePosition);
            }

            // If pressed left arrow and you are not colliding with the left wall
            if (Input.GetKey(KeyCode.LeftArrow) && !isLeftEdgeCollider)
            {
                isMovingLeft = true;
                isMovingRight = false;
                horizontalSpeed = -storeHorizontalSpeed;
            }
            // If pressed right arrow and you are not colliding with right wall
            else if (Input.GetKey(KeyCode.RightArrow) && !isRightEdgeCollider)
            {
                isMovingRight = true;
                isMovingLeft = false;
                horizontalSpeed = storeHorizontalSpeed;
            }

        }

        

        // Determine the increment
        if(isMovingLeft)
        {
            // Increment should be positive
            if (increment < 0)
            {
                increment = -increment;
            }
        }
        else if(isMovingRight)
        {
            // Increment should be negative
            if (increment > 0)
            {
                increment = -increment;
            }
        }


        // This is here so you don't initially start moving horizontal
        if (isMovingLeft || isMovingRight)
        {
            // Add the increment to the horizontal speed to slowly slow it down
            horizontalSpeed += increment;
        }

        // If you are moving left
        if (isMovingLeft)
        {
            //  If horizontal speed becomes greater than 0 then set it to 0
            if (horizontalSpeed >= 0)
            {
                horizontalSpeed = 0;
            }
        }
        //  If you are moving right
        else if (isMovingRight)
        {
            // If horizontal speed becomes less than 0 then set it to 0
            if (horizontalSpeed <= 0)
            {
                horizontalSpeed = 0;
            }
        }
    }


    /*
     * Function for when there are 3 or 2 balloons alive.
     * The function called after this(UpdateIfOneBalloon) will modify distance joint for 1 balloon alive.
     * Turns off spring joints of middle balloon based on what balloon is not being rendered.
     */
    void TurnOffSpringJointsAndSetDistanceJoint()
    {
        
        /*
         * These 3 if statments are replaced below if there is only 1 balloon alive.
         */
        // Turn off spring joint middle balloon connected to left balloon
        if (leftBalloon.GetComponent<SpriteRenderer>().enabled == false)
        {
            middleBalloonSpringJoints[0].enabled = false;
            middleBalloonSpringJoints[1].enabled = false;
            leftBalloon.GetComponent<DistanceJoint2D>().distance = 0.1f;

            // Set distance of remaining two
            middleBalloon.GetComponent<DistanceJoint2D>().distance = 1f;
            rightBalloon.GetComponent<DistanceJoint2D>().distance = 1f;

            // Set the layer to be DeadBalloon so it doesn't interfere with enemies
            leftBalloon.layer = 8;

        }
        // Turn off spring joint middle balloon connected to right balloon
        if (rightBalloon.GetComponent<SpriteRenderer>().enabled == false)
        {
            middleBalloonSpringJoints[0].enabled = false;
            middleBalloonSpringJoints[1].enabled = false;
            rightBalloon.GetComponent<DistanceJoint2D>().distance = 0.1f;

            // Set distance of remaining two
            middleBalloon.GetComponent<DistanceJoint2D>().distance = 1f;
            leftBalloon.GetComponent<DistanceJoint2D>().distance = 1f;

            // Set the layer to be DeadBalloon so it doesn't interfere with enemies
            rightBalloon.layer = 8;
        }

        // Turn off spring joint middle balloon connected to right balloon and left balloon
        if (middleBalloon.GetComponent<SpriteRenderer>().enabled == false)
        {
            middleBalloonSpringJoints[0].enabled = false;
            middleBalloonSpringJoints[1].enabled = false;
            middleBalloon.GetComponent<DistanceJoint2D>().distance = 0.1f;

            // Set distance of remaining two
            leftBalloon.GetComponent<DistanceJoint2D>().distance = 1f;
            rightBalloon.GetComponent<DistanceJoint2D>().distance = 1f;

            // Set the layer to be DeadBalloon so it doesn't interfere with enemies
            middleBalloon.layer = 8;
        }
    
               
    }


    /*
     * If only 1 balloon is alive then bring the other two back to where they were when there were
     * 3 balloons.  They won't be able to collide with any enemies.  This is so the physics of the 
     * balloon and basket are the same... The physics of just 1 balloon is goofy by itself.
     */
    void UpdateIfOnlyOneBalloon()
    {

        // If only 1 balloon is alive :(
        if (numberOfBalloons == 1)
        {
            // If left balloon is alive then set distance of other balloons to 1
            if (leftBalloon.GetComponent<SpriteRenderer>().enabled == true)
            {
                middleBalloon.GetComponent<DistanceJoint2D>().distance = 1f;
                rightBalloon.GetComponent<DistanceJoint2D>().distance = 1f;
            }
            // If middle balloon is alive then set distance of other balloons to 1
            else if (middleBalloon.GetComponent<SpriteRenderer>().enabled == true)
            {
                leftBalloon.GetComponent<DistanceJoint2D>().distance = 1f;
                rightBalloon.GetComponent<DistanceJoint2D>().distance = 1f;
            }
            // If right balloon is alive then set distance of other balloons to 1
            else if (rightBalloon.GetComponent<SpriteRenderer>().enabled == true)
            {
                middleBalloon.GetComponent<DistanceJoint2D>().distance = 1f;
                leftBalloon.GetComponent<DistanceJoint2D>().distance = 1f;
            }

            // Do this only once
            if (moveBalloonsToSide)
            {
                // If left balloon alive then shift other balloons to the side of it so
                // you get the same behavior as 3 balloons alive
                if (leftBalloon.GetComponent<SpriteRenderer>().enabled == true)
                {

                    // Shift the other balloons to different sides of the left balloon
                    middleBalloon.GetComponent<Transform>().position = new Vector3(
                        leftBalloon.GetComponent<Transform>().position.x + 5,
                        leftBalloon.GetComponent<Transform>().position.y,
                        leftBalloon.GetComponent<Transform>().position.z);

                    rightBalloon.GetComponent<Transform>().position = new Vector3(
                        leftBalloon.GetComponent<Transform>().position.x - 5,
                        leftBalloon.GetComponent<Transform>().position.y,
                        leftBalloon.GetComponent<Transform>().position.z);

                }
                // If middle balloon alive then shift other balloons to the side of it so
                // you get the same behavior as 3 balloons alive
                else if (middleBalloon.GetComponent<SpriteRenderer>().enabled == true)
                {

                    // Shift the other balloons to different sides of the middle balloon
                    rightBalloon.GetComponent<Transform>().position = new Vector3(
                        middleBalloon.GetComponent<Transform>().position.x + 5,
                        middleBalloon.GetComponent<Transform>().position.y,
                        middleBalloon.GetComponent<Transform>().position.z);

                    leftBalloon.GetComponent<Transform>().position = new Vector3(
                        middleBalloon.GetComponent<Transform>().position.x - 5,
                        middleBalloon.GetComponent<Transform>().position.y,
                        middleBalloon.GetComponent<Transform>().position.z);


                }
                // If right balloon alive then shift other balloons to the side of it so
                // you get the same behavior as 3 balloons alive
                else if (rightBalloon.GetComponent<SpriteRenderer>().enabled == true)
                {

                    // Shift the other balloons to different sides of the right balloon
                    middleBalloon.GetComponent<Transform>().position = new Vector3(
                        rightBalloon.GetComponent<Transform>().position.x + 5,
                        rightBalloon.GetComponent<Transform>().position.y,
                        rightBalloon.GetComponent<Transform>().position.z);

                    leftBalloon.GetComponent<Transform>().position = new Vector3(
                        rightBalloon.GetComponent<Transform>().position.x - 5,
                        rightBalloon.GetComponent<Transform>().position.y,
                        rightBalloon.GetComponent<Transform>().position.z);

                }

                // So you only do this once
                moveBalloonsToSide = false;
            }
        }
    }
    


    /*
     * Update the balloons velcities.
     */
    void UpdateBalloonsMovement()
    {
        // This bool is set to true when you enter/stay in air stream
        if(turnOffInitialVelocity)
        {
            horizontalSpeed = 0f;
            turnOffInitialVelocity = false;
        }

        // Set the velocities
        leftBalloon.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed + airStreamHorizontalVelocity, verticalSpeed + balloonPopVelocityChange + airStreamVerticalVelocity);
        middleBalloon.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed + airStreamHorizontalVelocity, verticalSpeed + balloonPopVelocityChange + airStreamVerticalVelocity);
        rightBalloon.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed + airStreamHorizontalVelocity, verticalSpeed + balloonPopVelocityChange + airStreamVerticalVelocity);
    }


    /*
     * Change the velocity of when balloon hits an object.
     * Also change its increment number for setting it back to 0.
     */
    public void SetOffDownwardVelocityWhenBalloonHitsEnemy()
    {
        balloonPopVelocityChange = -4f;
        balloonPopIncrement = 0.05f;
    }


    /*
     * Decrease or increase the increment to balloonPopVelocityChange until it is 0.
     */
    void UpdateDownwardVelocitySetOffFromBalloonHitsEnemy()
    {

        if(balloonPopVelocityChange < 0)
        {
            balloonPopVelocityChange += balloonPopIncrement;
        }
        else
        {
            balloonPopVelocityChange = 0f;
        }

    }


    /*
     * If all the balloons are dead then destroy the balloons
     */
    void UpdateIfAllBalloonsAreDead()
    {
        if(numberOfBalloons == 0)
        {

            // Set the boolean of isGameOver to true
            GameControl.control.SetGameOverDisplay();
            uiScript.SetGameOver();

            // Destroy the balloons
            Destroy(leftBalloon);
            Destroy(middleBalloon);
            Destroy(rightBalloon);
        }
    }


    /*
     * Returns boolean of allBalloonsAreDead.
     */
    public bool GetAllBalloonsDead()
    {
        return allBalloonsAreDead;
    }


    /*
     * Checks where the user has clicked on the screen.
     * If it is to the right of the main character, then move him right.
     * If it is to the left of the main character, then move him left.
    */
    void UpdateMainCharacterBasedOnTouchedScreen(Vector3 pos)
    {

        // Gets the point where user touched screen and converts it to world position
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(pos);

        // The spot where the user touched in world coordinates
        Vector2 touchPosition = new Vector2(worldPoint.x, worldPoint.y);

        // Check if a collider overlaps a point in space.
        Collider2D hit = Physics2D.OverlapPoint(touchPosition);


        
        // If user clicks on a collider2d make sure it isn't the main character.
        // If it is the basket then don't move
        if(hit)
        {
            if (hit.gameObject.name != "Basket")
            {
                // If user clicks to the right of the main character and
                // isn't colliding with right edge then move him right
                if ((touchPosition.x > GetComponent<Transform>().position.x) &&
                    !isRightEdgeCollider)
                {
                    isMovingRight = true;
                    isMovingLeft = false;
                    horizontalSpeed = storeHorizontalSpeed;
                }
                // If the user clicks to the left of the main chracter and
                // isn't colliding with left edge then move him left
                else if ((touchPosition.x < GetComponent<Transform>().position.x) &&
                !isLeftEdgeCollider)
                {
                    isMovingLeft = true;
                    isMovingRight = false;
                    horizontalSpeed = -storeHorizontalSpeed;
                }
            }
        }
        // Else if user has not clicked on a collider2d update movement logic
        else
        {
            // If user clicks to the right of the main character and
            // isn't colliding with right edge then move him right
            if ((touchPosition.x > GetComponent<Transform>().position.x) &&
                !isRightEdgeCollider)
            {
                isMovingRight = true;
                isMovingLeft = false;
                horizontalSpeed = storeHorizontalSpeed;
            }
            // If the user clicks to the left of the main chracter and
            // isn't colliding with left edge then move him left
            else if ((touchPosition.x < GetComponent<Transform>().position.x) &&
                !isLeftEdgeCollider)
            {
                isMovingLeft = true;
                isMovingRight = false;
                horizontalSpeed = -storeHorizontalSpeed;
            }
        }
        
        
    }


    /*
     * Sets the boolean isLeftEdgeCollider.
     * Also stops the balloons in their horizontal track!
     */
    public void SetLeftEdgeBooleanAndStopHorizontalMovement(bool b)
    {
        isLeftEdgeCollider = b;
        horizontalSpeed = 0f;
    }


    /*
     * Sets the boolean isRightEdgeCollider.
     * Also stops the balloons in their horizontal track!
     */
    public void SetRightEdgeBooleanAndStopHorizontalMovement(bool b)
    {
        isRightEdgeCollider = b;
        horizontalSpeed = 0f;
    }


    /*
     * Update movement of main character when attached to anchors.
     * If user touches anywhere on screen then release the main character from anchors.
     */
    void UpdateMainCharacterAnchors()
    {

        // Timer for switching force variables
        if(timer > 0 && timer < 3)
        {
            forceSpeedX = 6f;
            forceSpeedY = 1f;
        }
        else if(timer > 3 && timer < 8)
        {
            forceSpeedX = -6f;
            forceSpeedY = 0f;
        }
        else if (timer > 8 && timer < 11)
        {
            forceSpeedX = 6f;
            forceSpeedY = 1f;
        }
        else
        {
            timer = 0f;
        }


        // Apply the forces
        leftBalloon.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceSpeedX, forceSpeedY));
        middleBalloon.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceSpeedX, forceSpeedY));
        rightBalloon.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceSpeedX, forceSpeedY));


        // Don't allow breaking anchor if on main menu
        if (uiScript.GetGamePlay() == true)
        {
            // If user is running on android
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    // TouchPhase.Began means a finger touched the screen
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        // Turn off spring joints
                        basketSpringJoints[0].enabled = false;
                        //   basketSpringJoints[1].enabled = false;

                        // You are no longer anchored
                        isAnchored = false;

                        // So you don't burst out of anchor when released
                        verticalSpeed = 2f;
                        horizontalSpeed = 0f;
                    }
                }
            }


            // If user is running the editor
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    // Turn off spring joints
                    basketSpringJoints[0].enabled = false;
                    //   basketSpringJoints[1].enabled = false;

                    // You are no longer anchored
                    isAnchored = false;

                    // So you don't burst out of anchor when released
                    verticalSpeed = 2f;
                    horizontalSpeed = 0f;
                }
            }
        }

        // Update timer
        timer += Time.deltaTime;

    }


    /*
     * Sets off the speed for being in the air stream.
     */
    public void SetAirStreamVelocity(float xSpeed, float ySpeed)
    {
        airStreamHorizontalVelocity = xSpeed;
        airStreamVerticalVelocity = ySpeed;
    }


    /*
     * Sets the boolean isAirStream.
     */
    public void SetAirStreamBoolean(bool b)
    {
        isAirStream = b;
    }


    /*
     * Increments the air stream velocity once you leave it.
     */
    void UpdateAirStreamVelocity()
    {
        // If not in air stream
        if(!isAirStream)
        {

            // If air stream vertical velocity is greater than 0
            if (airStreamVerticalVelocity > 0)
            {
                airStreamVerticalVelocity -= 0.05f;
            }
            else
            {
                airStreamVerticalVelocity = 0f;
            }


            /*
             * I used 0.05 because it never actually hits 0.
             * If I used 0 instead then there would always remain some
             * added velocity after exiting the air stream.
             */
            // If air stream horizontal velocity is greater than 0.05
            if(airStreamHorizontalVelocity > 0.05)
            {
                airStreamHorizontalVelocity -= 0.05f;
            }
            // If air stream horizontal velocity is less than -0.05
            else if(airStreamHorizontalVelocity < -0.05)
            {
                airStreamHorizontalVelocity += 0.05f;
            }
            else
            {
                airStreamHorizontalVelocity = 0f;
            }

        }

    }


    /*
     * Sets turnOffInitialVelocity bool to true.
     * Used in Function UpdateBalloonsMovement().
     */
    public void SetZeroHorizontalSpeed()
    {
        turnOffInitialVelocity = true;
    }



    
}
