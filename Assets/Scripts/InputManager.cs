using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
    //Position et Rigidbody de la caméra
    public Transform cameraTransform;
    public Rigidbody cameraRigidbody;

    //Variables de sensibilité
    public float sensitivityX;
    public float sensitivityY;
    //Variable d'angle max
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    //Vitesse du déplacement
    public float speed;

    float rotationY = 0F;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        //Calcul des angles de déplacement en fonction de la position de la souris
        float rotationX = cameraTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        //Calcule des déplacements en hauteur
        float yMove = 0;
        if (Input.GetButton("Up")) {
            
            yMove += 1;
        }
        if (Input.GetButton("Down")) {
            
            yMove -= 1;
        }

        //Calcule des déplacements dans le plan
        float xMove = Input.GetAxis("Horizontal")  * Mathf.Cos(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical")  * Mathf.Sin(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180);
        float zMove = -Input.GetAxis("Horizontal") * Mathf.Sin(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180) + Input.GetAxis("Vertical") * Mathf.Cos(cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180);

        //Rotation
        rotateCamera(new Vector3(-rotationY, rotationX, 0));
        //Déplacement
        moveCamera(new Vector3(xMove, yMove, zMove)*speed);
        



    }
    

    

    void rotateCamera(Vector3 rotation) {
        //Anulation de la vélocité sur la rotation
        cameraRigidbody.angularVelocity = new Vector3(0,0,0);

        cameraTransform.localEulerAngles = rotation;
    }


    void moveCamera(Vector3 move) {
        cameraRigidbody.velocity = move;
        
    }


}
