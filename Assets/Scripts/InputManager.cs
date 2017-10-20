using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public Transform cameraTransform;

    public float sensitivityX;
    public float sensitivityY;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        
        if (mouseY == 0) {
            cameraTransform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        } else if (mouseX == 0) {

        } else {
            float rotationX = cameraTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            cameraTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }*/

        float rotationX = cameraTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        cameraTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
}
