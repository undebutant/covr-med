using UnityEngine;
using System.Collections;


public class SelectAndMove : MonoBehaviour {

    //ModeManager
    public ModeManager modeManager;

    // Transform of the main camera
    public Transform cameraTransform;

    // Taken from the Engine script
    float maxSpeed;

    bool isSelected = false;
    bool isMouseExiting = false;

    Shader shaderNormal;
    Shader shaderHighlighted;


    void Start() {
        maxSpeed = this.GetComponent<Engines>().maxSpeed;

        shaderNormal = Shader.Find("Standard");                // Basic texture
        shaderHighlighted = Shader.Find("VertexLit");             // Special texture when the object is selected
    }


    void OnMouseOver() {
        if(modeManager.isSelectModeOn()) {
            // Select with the right click of the mouse (left click is dedicated to drag and drop)
            if (Input.GetMouseButtonDown(1)) {
                // Knowing that the player just right clicked, guessing if the mouse is exiting or not
                // depending on the selected (or not) state of the object
                isMouseExiting = !isSelected ? false : true;

                isSelected = !isSelected;
                if(isSelected) {
                    GetComponent<Renderer>().material.shader = shaderHighlighted;
                } else {
                    GetComponent<Renderer>().material.shader = shaderNormal;
                }
            }
        }
    }

    void OnMouseExit() {
        isMouseExiting = true;
    }


    /// <summary>
    ///     F and B keys are used to bring the object forward or backward
    ///     Use the directional keys on the keyboard to move
    /// </summary>
    void Update() {
        if (modeManager.isSelectModeOn()) {
            if (isSelected) {


                float zInput = 0;

                if (Input.GetKey(KeyCode.F)) {
                    zInput--;
                }

                if (Input.GetKey(KeyCode.B)) {
                    zInput++;
                }

                float rotationY = cameraTransform.rotation.eulerAngles.y;
                rotationY -= 35;

                // Calculating movements in the current plan
                float xMove = Input.GetAxis("Horizontal") * Mathf.Cos(rotationY * Mathf.PI / 180) + zInput * Mathf.Sin(rotationY * Mathf.PI / 180);
                float zMove = -Input.GetAxis("Horizontal") * Mathf.Sin(rotationY * Mathf.PI / 180) + zInput * Mathf.Cos(rotationY * Mathf.PI / 180);

                float yMove = Input.GetAxisRaw("Vertical");

                this.GetComponent<Engines>().InputDirection = new Vector3(xMove, yMove, zMove);

                

                
            }

            // If the right button is clicked outside the gameobject or on the gameobject when it is selected 
            if (Input.GetMouseButtonDown(1) && (isMouseExiting || !isSelected)) {
                isSelected = false;

                GetComponent<Renderer>().material.shader = shaderNormal;                // Rendering the object back to normal
            }
        }
    }
}
