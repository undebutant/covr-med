using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;


public class InputManager : NetworkBehaviour {

    [Tooltip("Indicates if the player is using a controller")]
    //public bool controllerOn;

    // Transform of the camera and Rigibody of the parent
    public Transform playerTransformCamera;
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

    // The config for the local instance
    ConfigInitializer config;

    NetworkManager networkManager;


    private void Start() {
        config = GameObject.FindObjectOfType<ConfigInitializer>();
        try {
            networkManager = GameObject.FindObjectOfType<NetworkManager>();
        } catch (Exception exception) {
            Debug.LogError("Error while looking for the NetworkManager. Exception raised : " + exception);
            Application.Quit();
        }
    }


    void Update () {
        if (isLocalPlayer) {
            float rotationX;

            // Handling inputs according to the boolean controllerOn
            if (config.GetInputDevice() == InputDevice.Controller) {
                rotationX = playerTransformCamera.localEulerAngles.y + Input.GetAxis("Horizontal2") * sensitivityXAxis;
                rotationY += -Input.GetAxis("Vertical2") * sensitivityYAxis;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            } else {
                rotationX = playerTransformCamera.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityXAxis;
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

            float xMove;
            float zMove;


            // Calculating movements in the current plan
            xMove = Input.GetAxis("Horizontal") * Mathf.Cos(playerTransformCamera.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical") * Mathf.Sin(playerTransformCamera.rotation.eulerAngles.y * Mathf.PI / 180);
            zMove = -Input.GetAxis("Horizontal") * Mathf.Sin(playerTransformCamera.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical") * Mathf.Cos(playerTransformCamera.rotation.eulerAngles.y * Mathf.PI / 180);


            // Applying rotations
            RotatePlayer(new Vector3(-rotationY, rotationX, 0));

            // Applying movements
            // No movement if the player is the surgeon
            if (config.GetPlayerRole() != PlayerRole.Surgeon) {
                MovePlayer(new Vector3(xMove, yMove, zMove) * speed);
            }

            // Button Echap for disconnecting from the current session
            if(Input.GetButtonDown("Cancel")) {
                if(isServer) {
                    // If we are the host we use the StopHost function
                    networkManager.StopHost();
                } else {
                    // If we are just a clien we use the StopClient function
                    networkManager.StopClient();
                }
                
            }
        }
    }


    void RotatePlayer(Vector3 rotation) {
        // Cancelling angular velocity on the rotation
        playerRigidbody.angularVelocity = new Vector3(0,0,0);
        // Use the transform because we don't want to use collider for the rotation
        playerTransformCamera.localEulerAngles = rotation;
    }


    void MovePlayer(Vector3 move) {
        // Use the rigidbody to use collider
        playerRigidbody.velocity = move;
    }
}
