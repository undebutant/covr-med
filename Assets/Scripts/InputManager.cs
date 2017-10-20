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

        cameraTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        cameraTransform.Translate(Input.GetAxis("Horizontal")*Time.deltaTime*speed, 0, Input.GetAxis("Vertical")*Time.deltaTime*speed);

    }
}
