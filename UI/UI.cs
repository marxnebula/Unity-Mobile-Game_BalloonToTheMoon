using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Displays all UI buttons including Play, Upgrade, Select Char.
 *  - Also displays the pop up menus.
 *  - If game over it changes play to replay.
 *  - Replay button will load the game play screen.
 */

public class UI : MonoBehaviour {

    // Text for UI
    public Text coinText;
    public Text heightText;
    public Text beatRecordText;

    // Images for UI
    public Image playButton;
    public Image upgradeButton;
    public Image selectCharacterButton;
    public Image backButton;
    public Image upgradePopUp;
    public Image selectCharacterPopUp;

    // Replay sprite
    public Sprite replaySprite;

    // Booleans for drawing UI
    private bool isMainMenu = false;
    private bool isGamePlay = false;
    private bool isGameOver = false;
    public bool isUpgradePopUp = false;
    private bool isSelectCharacterPopUp = false;
    private bool isBeatHightestHeight = false;

    // String for scene name
    private string mainMenuScene = "FirstScene";
    private string replayScene = "ReplayScene";

    // Basket
    public GameObject basket;


    void Start()
    {
        // If the scene is main menu
        if (SceneManager.GetActiveScene().name == mainMenuScene)
        {
            // Set game play to false
            isGamePlay = false;
        }
        // If the scene is not main menu(this game only has 2 scenes so must be game play scene)
        else
        {
            // Set the game play to true
            isGamePlay = true;
        }
    }
	
	// Update is called once per frame
	void Update () {

        // Display users stats
        DisplayUserStats();

        // Check the users input
        CheckUserInput();

        // Displays main menu buttons
        DisplayMainMenu();

        // Display pop up menus
        DisplayPopUpMenus();
        
        // Displays game over buttons
        DisplayGameOver();
	}


    /*
     * Checks users input based on which platform they are on.
     * It then calls a function to check if a certain button was pressed.
     */
    void CheckUserInput()
    {
        // If user is running on android
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                // TouchPhase.Began means a finger touched the screen
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    CheckWhichButtonTouched(Input.GetTouch(0).position);
                }
            }
        }

        // If user is running the editor
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // If left mouse button is up
            if (Input.GetMouseButtonUp(0))
            {
                CheckWhichButtonTouched(Input.mousePosition);
            }
        }
    }


    /*
     * Checks which button was pressed if any.
     */
    void CheckWhichButtonTouched(Vector3 pos)
    {
        
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = pos;

        // Gets a list of raycast
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);


        if (raycastResults.Count > 0)
        {
            // If it's play button
            if (raycastResults[0].gameObject.name == "PlayButton")
            {
                print("Play button hit");
                isGamePlay = true;

                // Load variables if changed character
                basket.GetComponent<ControlBalloonMovement>().DetermineVariablesBasedOnCharacter();

                // If this is a replay then load new level
                if(isGameOver)
                {
                    // Reset the boolean
                    GameControl.control.SetBeatHighestHeight(false);

                    // Load the replay level
                    SceneManager.LoadScene(replayScene);
                }
            }
            // If it's upgrade button
            else if (raycastResults[0].gameObject.name == "UpgradeButton")
            {
                print("Upgrade button hit");
                isUpgradePopUp = true;
            }
            // If it's select character button
            else if (raycastResults[0].gameObject.name == "SelectCharacterButton")
            {
                print("Select Character button hit");
                isSelectCharacterPopUp = true;
            }
            // If it's select character button
            else if (raycastResults[0].gameObject.name == "BackButton")
            {
                print("Back button hit");
                
                // Determine what the back button does
                if(isUpgradePopUp)
                {
                    isUpgradePopUp = false;
                }
                else if(isSelectCharacterPopUp)
                {
                    isSelectCharacterPopUp = false;
                }

            }
        }
    }


    /*
     * Displays the Play, Upgrade, and Select Character buttons at the start.
     * Once the user has clicked replay then it no longer displays these.
     */
    void DisplayMainMenu()
    {
        // If it is scene mainMenuScene and game play hasn't started
        if (SceneManager.GetActiveScene().name == mainMenuScene &&
            !isGamePlay)
        {
            // Upgrade, select character or game over is false then turn buttons on
            if (!isUpgradePopUp && !isSelectCharacterPopUp && !isGameOver)
            {
                // Enable the play button
                playButton.enabled = true;

                // Enable the upgrade button
                upgradeButton.enabled = true;

                // Enable the select char button
                selectCharacterButton.enabled = true;
            }
            else
            {
                // Disable the play button
                playButton.enabled = false;

                // Disable the upgrade button
                upgradeButton.enabled = false;

                // Disable the select char button
                selectCharacterButton.enabled = false;
            }
        }
        else
        {
            // Diable the play button
            playButton.enabled = false;

            // Disable the upgrade button
            upgradeButton.enabled = false;

            // Disable the select char button
            selectCharacterButton.enabled = false;
        }
    }


    /*
     * If it's game over then display game over buttons.
     * Also change the play button to replay button.
     */
    void DisplayGameOver()
    {
        // If it is game over
        if (isGameOver)
        {
            // Upgrade or select character pop ups are false
            if (!isUpgradePopUp && !isSelectCharacterPopUp)
            {
                // Set the plat button sprite to the replay sprite
                playButton.sprite = replaySprite;

                // Enable the play button
                playButton.enabled = true;

                // Enable the upgrade button
                upgradeButton.enabled = true;

                // Enable the select char button
                selectCharacterButton.enabled = true;
            }
            else
            {
                // Disable the play button
                playButton.enabled = false;

                // Disable the upgrade button
                upgradeButton.enabled = false;

                // Disable the select char button
                selectCharacterButton.enabled = false;
            }


            // If you beat your highest score
            if(GameControl.control.GetBeatHighestHeight())
            {
                beatRecordText.enabled = true;

                // Display best record text
                beatRecordText.text = "NEW RECORD: " + (int)GameControl.control.highestHeight;
                print("Beat Record");
            }
        }
    }


    /*
     * Display pop up menus based on booleans.
     */
    void DisplayPopUpMenus()
    {
        // If one of these booleans is true
        if (isUpgradePopUp || isSelectCharacterPopUp)
        {
            // If upgrade button is clicked
            if (isUpgradePopUp)
            {
                // Enable the upgrade pop up button
                upgradePopUp.enabled = true;

                // Enable the back button
                backButton.enabled = true;
            }

            // If selected character is clicked
            if (isSelectCharacterPopUp)
            {
                // Enable the select char pop up
                selectCharacterPopUp.enabled = true;

                // Eneable the back button
                backButton.enabled = true;

                // Need code for selecting character
                // And set variables once char selected
              //  basket.GetComponent<ControlBalloonMovement>().DetermineVariablesBasedOnCharacter();
            }
        }
        // Else make sure pop ups and back button is off
        else
        {
            // Disable upgrade pop up
            upgradePopUp.enabled = false;

            // Disabel select char pop up
            selectCharacterPopUp.enabled = false;

            // Disable back button
            backButton.enabled = false;
        }
    }


    /*
     * Displays the users stats which are in GameControl.
     */
    void DisplayUserStats()
    {
        coinText.text = "Coins: " + GameControl.control.coins;
        heightText.text = "Height: " + (int)GameControl.control.currentHeight;
    }


    /*
     * Sets isGameOver boolean to true.
     */
    public void SetGameOver()
    {
        isGameOver = true;
    }


    /*
     * Sets isGameOver boolean to true.
     */
    public bool GetGamePlay()
    {
        return isGamePlay;
    }
}
