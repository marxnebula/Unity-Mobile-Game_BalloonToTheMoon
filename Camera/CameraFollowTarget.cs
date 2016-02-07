using UnityEngine;
using System.Collections;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Camera follows main character.
 *  - Doesn't start following until main char position is > 0.
 *  - It also sets the aspect and camera size based on what platform game is running on.
 */


public class CameraFollowTarget : MonoBehaviour {

    // Transform of what the camera follows
    public Transform player;

    // Offset for the camera
    public Vector3 offset;

    // Aspect
    public float aspect;
    
    // Current aspect
    public float currentAspect;


    void Start()
    {
        // Set camera aspect
        DetermineCameraAspectAndSize();
    }
    
	
	// Update is called once per frame
	void Update () {

        // Camera follows the player with specified offset position
        transform.position = new Vector3(offset.x, player.position.y + offset.y, -10);

        // Doesn't start following player until a certain point
        if(transform.position.y < 0)
        {
            transform.position = new Vector3(0f, 0f, -10);
        }

	}


    /*
     * Determine what the camera aspect is based on current device.
     * Then set the aspect and adjust the size to show entire screen.
     * The camera size was determined by trial and error.
     */
    void DetermineCameraAspectAndSize()
    {
        
        // Get the current aspect
        currentAspect = (float)Screen.width / (float)Screen.height;

        
        // 3:4 = 0.75
        if (currentAspect > 0.70)
        {
            // Set aspect
            GetComponent<Camera>().aspect = 3f / 4f;

            // Set size
            Camera.main.orthographicSize = 4.30f;
        }
        // 2:3 = 0.6666666667
        else if (currentAspect > 0.64)
        {
            // Set Aspect
            GetComponent<Camera>().aspect = 2f / 3f;

            // Set size
            Camera.main.orthographicSize = 4.82f;
        }
        // 10:16 = 0.625
        else if (currentAspect > 0.60)
        {
            // Set aspect
            GetComponent<Camera>().aspect = 10f / 16f;

            // Set size
            Camera.main.orthographicSize = 5.15f;
        }
        // 10:17 = 0.5882
        else if (currentAspect > 0.57)
        {
            // Set aspect
            GetComponent<Camera>().aspect = 10f / 17f;

            // Set size
            Camera.main.orthographicSize = 5.72f;
        }
        // 9:16 = 0.5625
        else if (currentAspect > 0.50)
        {
            // Set aspect
            GetComponent<Camera>().aspect = 9f / 16f;

            // Set size
            Camera.main.orthographicSize = 5.7f;
        }
        // Just incase
        else
        {
            // Set aspect
            GetComponent<Camera>().aspect = 9f / 16f;

            // Set size
            Camera.main.orthographicSize = 5.7f;
        }
         

    }
}
