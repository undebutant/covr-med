using UnityEngine;
using System.Collections;


public class SelectAndMove : MonoBehaviour {

    // Taken from the Engine script
    float maxSpeed;

    bool isSelected = false;
    bool isMouseExiting = false;

    Shader shaderNormal;
    Shader shaderHighlighted;


    void Start() {
        maxSpeed = this.GetComponent<Engines>().maxSpeed;

        shaderNormal = Shader.Find("VertexLit");                // Basic texture
        shaderHighlighted = Shader.Find("Diffuse");             // Special texture when the object is selected
    }


    void OnMouseOver() {
        // Select with the right click of the mouse (left click is dedicated to drag and drop)
        if (Input.GetMouseButtonDown(1)) {
            // Knowing that the player just right clicked, guessing if the mouse is exiting or not
            // depending on the selected (or not) state of the object
            isMouseExiting = !isSelected ? false : true;

            isSelected = !isSelected;
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
        if (isSelected) {
            this.GetComponent<Engines>().InputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

            if (Input.GetKey(KeyCode.F)) {
                this.GetComponent<Engines>().InputDirection = new Vector3(0, 0, transform.position.z + maxSpeed);
            }

            if (Input.GetKey(KeyCode.B)) {
                this.GetComponent<Engines>().InputDirection = new Vector3(0, 0, -transform.position.z - maxSpeed);
            }

            GetComponent<Renderer>().material.shader = shaderHighlighted;           // Applying selected effect
        }

        // If the right button is clicked outside the gameobject or on the gameobject when it is selected 
        if (Input.GetMouseButtonDown(1) && (isMouseExiting || !isSelected)) {
            isSelected = false;

            GetComponent<Renderer>().material.shader = shaderNormal;                // Rendering the object back to normal
        }
    }
}
