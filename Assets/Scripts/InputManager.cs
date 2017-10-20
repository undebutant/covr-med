using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public Transform cameraTransform;
    public Rigidbody cameraRigidbody;

    public float sensitivityX;
    public float sensitivityY;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    public float speed;

    float rotationY = 0F;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        

        float rotationX = cameraTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);


        float yMove = 0;
        if (Input.GetButton("Up")) {
            
            yMove += 1;
        }
        if (Input.GetButton("Down")) {
            
            yMove -= 1;
        }

        rotateCamera(new Vector3(-rotationY, rotationX, 0));
        
        moveCamera(new Vector3(Input.GetAxis("Horizontal") * speed* Mathf.Cos(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical") * speed * Mathf.Sin(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180), 1* yMove*speed, 
            -Input.GetAxis("Horizontal") * speed* Mathf.Sin(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical") * speed * Mathf.Cos(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180)));
        



    }
    

    

    void rotateCamera(Vector3 rotation) {
        cameraRigidbody.angularVelocity = new Vector3(0,0,0);
        cameraTransform.localEulerAngles = rotation;
    }


    void moveCamera(Vector3 move) {
        cameraRigidbody.velocity = move;
        //cameraTransform.Translate(move);
    }


}
