using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Class for storing saved variables.
 *  - Doesn't delete when you change scenes.
 *  - Also saves user data to a file in all platforms except the web.
 *  - Must put this class in each scene so it is a PREFAB.
 */

public class GameControl : MonoBehaviour {

    public static GameControl control;

    /* Variables for saving */
    // Coins and highest height
    public int coins;
    public float highestHeight;

    // Equipment
    public bool isSpaceBalloons; // Allows you do enter space
    public bool isSpikeBalloons; // Kills anything living that you collide with

    // Characters (they have different types of motion)
    public bool isNormalCharacter = true;
    public bool boughtSantaCharacter = false;
    public bool isSantaCharacter = false;

    // Keeping track of current height
    public float currentHeight;

    // Booleans for drawing which GUI
    private bool isMainMenu = false;
    private bool isGamePlay = false;
    private bool isGameOver = false;
    private bool isUpgradeMenu = false;
    private bool isSelectCharMenu = false;
    private bool isBeatHightestHeight = false;

    

	// Awake happens before Start()
	void Awake () {

        /*
         * So there can only be 1 control.(singleton)
         */
        // If control doesn't exist
        if(control == null)
        {
            // Doesn't destroy the game object when you change scenes
            DontDestroyOnLoad(gameObject);

            // Set control to this
            control = this;
        }
        // If control does exist but it is not this
        else if(control != this)
        {
            // Destroy gameobject because one already exists
            Destroy(gameObject);
        }
        
	}

    // Dat update
    void Update()
    {
        DetermineIfBeatHighestHeight();
    }

    

    /*
     * Add 1 coin to your total amount of coins!
     */
    public void AddOneCoin()
    {
        coins += 1;
    }

    /*
     * Get total amount of coins!
     */
    public int GetTotalCoins()
    {
        return coins;
    }

    /*
     * Set the boolean of isSpaceBalloons.
     */
    public void SetSpaceBalloon(bool b)
    {
        isSpaceBalloons = b;
    }

    /*
     * Get the boolean of isSpaceBalloons.
     */
    public bool GetSpaceBalloon()
    {
        return isSpaceBalloons;
    }


    /*
     * Set the boolean of isSpikeBalloons.
     */
    public void SetSpikeBalloon(bool b)
    {
        isSpikeBalloons = b;
    }

    /*
     * Get the boolean of isSpikeBalloons.
     */
    public bool GetSpikeBalloon()
    {
        return isSpikeBalloons;
    }


    /*
     * Turns on Game over display.
     */
    public void SetGameOverDisplay()
    {
        isGameOver = true;
    }


    /*
    * Calculates the height traveled
    */
    public void CalculateHeight(float posY)
    {
         currentHeight = posY + 3.8f;
    }
    

    /*
     * Determine if you beat your record for highest height.
     */
    public void DetermineIfBeatHighestHeight()
    {
        
        if(currentHeight > highestHeight)
        {
            isBeatHightestHeight = true;
            highestHeight = currentHeight;
        }
        
    }
    

    /*
     * Get the boolean isBeatHighestHeight.
     */
    public bool GetBeatHighestHeight()
    {
        return isBeatHightestHeight;
    }


    /*
     * Set the boolean isBeatHighestHeight.
     */
    public void SetBeatHighestHeight(bool b)
    {
        isBeatHightestHeight = b;
    }


    /*
     * Gets boolean to see if you are normal character.
     */
    public bool GetIsNormalCharacter()
    {
        return isNormalCharacter;
    }

    /*
     * Gets boolean to see if you are santa character.
     */
    public bool GetIsSantaCharacter()
    {
        return isSantaCharacter;
    }

    /*
     * Sets boolean to make you santa character.
     */
    public void SetIsSantaCharacter(bool b)
    {
        isSantaCharacter = b;
    }

    /*
     * Gets boolean to see if you have santa character.
     */
    public bool GetBoughtSantaCharacter()
    {
        return boughtSantaCharacter;
    }

    /*
     * Sets boolean to show you bought santa character.
     */
    public void SetBoughtSantaCharacter(bool b)
    {
        boughtSantaCharacter = b;
    }
	
	
    /*
     * Saves data out into a file. This works on all platforms except the web.
     * You could save file as playerInfo.anything or just playerInfo
     */
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        // Create the file.
        // Application.persistentDataPath is the folder its going to and
        // "/playerInfo.dat" is the file name.
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        // Set the variables
        PlayerData data = new PlayerData();
        data.coins = coins;
        data.highestHeight = highestHeight;
        data.isSpaceBalloons = isSpaceBalloons;
        data.isSpikeBalloons = isSpikeBalloons;
        data.boughtSantaCharacter = boughtSantaCharacter;

        // Save the data to the file
        bf.Serialize(file, data);

        file.Close();
    }


    /*
     * Loads data from a file.  Make sure to check if file exist.
     * This works on all platforms except the web.
     */
    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            // Need to cast meaning specify that this is a player data object.
            // So it pulls the PlayerData data out of the file
            PlayerData data = (PlayerData)bf.Deserialize(file);

            file.Close();

            // Set your variables to the loaded data
            coins = data.coins;
            highestHeight = data.highestHeight;
            isSpaceBalloons = data.isSpaceBalloons;
            isSpikeBalloons = data.isSpikeBalloons;
            boughtSantaCharacter = data.boughtSantaCharacter;
        }
    }

}

/* 
 * This class needs to be serializable... meaning this data can be written to a file.
 */
[Serializable]
class PlayerData
{
    public int coins;
    public float highestHeight;
    public bool isSpaceBalloons;
    public bool isSpikeBalloons;
    public bool boughtSantaCharacter;
}
