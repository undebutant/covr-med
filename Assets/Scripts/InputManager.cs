using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class InputManager : NetworkBehaviour {
    public ObjectDrag objectDrag;

    // Transform and Rigibody of the main camera
    public Transform playerTransform;
    public Rigidbody playerRigidbody;

    // Sensitivity variables
    public float sensitivityXAxis;
    public float sensitivityYAxis;

    // Clamping the allowed angle for the camera
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;

    // Moving speed
    public float speed;

    // Current Y rotation of the camera
    float rotationY = 0F;


	void Update () {
        float rotationX;
        if (objectDrag.controllerOn) {
            rotationX = playerTransform.localEulerAngles.y + Input.GetAxis("Horizontal2") * sensitivityXAxis;
            rotationY += -Input.GetAxis("Vertical2") * sensitivityYAxis;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        } else {
            rotationX = playerTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityXAxis;
            // Calculating camera rotations given the mouse movements
            rotationY += Input.GetAxis("Mouse Y") * sensitivityYAxis;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        }
        
        

        

        // Calculating up and down movements
        float yMove = 0;
        if (Input.GetButton("Up")) {
            yMove += 1;
        }
        if (Input.GetButton("Down")) {
            yMove -= 1;
        }

        // Calculating movements in the current plan
        float xMove = Input.GetAxis("Horizontal") * Mathf.Cos(playerTransform.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical") * Mathf.Sin(playerTransform.rotation.eulerAngles.y * Mathf.PI / 180);
        float zMove = -Input.GetAxis("Horizontal") * Mathf.Sin(playerTransform.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical") * Mathf.Cos(playerTransform.rotation.eulerAngles.y * Mathf.PI / 180);

        // Applying rotations
        rotatePlayer(new Vector3(-rotationY, rotationX, 0));

        // Applying movements
		if (!isServer) {
		    movePlayer(new Vector3(xMove, yMove, zMove) * speed);
		}
        
    }


    void rotatePlayer(Vector3 rotation) {
        // Cancelling angular velocity on the rotation
        playerRigidbody.angularVelocity = new Vector3(0,0,0);

        playerTransform.localEulerAngles = rotation;
    }


    void movePlayer(Vector3 move) {
        playerRigidbody.velocity = move;
    }
}
