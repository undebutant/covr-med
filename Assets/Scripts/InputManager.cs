using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    // Transform and Rigibody of the main camera
    public Transform cameraTransform;
    public Rigidbody cameraRigidbody;

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
        // Calculating camera rotations given the mouse movements
        float rotationX = cameraTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityXAxis;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityYAxis;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        // Calculating up and down movements
        float yMove = 0;
        if (Input.GetButton("Up")) {
            yMove += 1;
        }
        if (Input.GetButton("Down")) {
            yMove -= 1;
        }

        // Calculating movements in the current plan
        float xMove = Input.GetAxis("Horizontal")  * Mathf.Cos(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical") * Mathf.Sin(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180);
        float zMove = -Input.GetAxis("Horizontal") * Mathf.Sin(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical") * Mathf.Cos(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180);

        // Applying rotations
        rotateCamera(new Vector3(-rotationY, rotationX, 0));

        // Applying movements
        moveCamera(new Vector3(xMove, yMove, zMove) * speed);
    }


    void rotateCamera(Vector3 rotation) {
        // Cancelling angular velocity on the rotation
        cameraRigidbody.angularVelocity = new Vector3(0,0,0);

        cameraTransform.localEulerAngles = rotation;
    }


    void moveCamera(Vector3 move) {
        cameraRigidbody.velocity = move;
    }
}
