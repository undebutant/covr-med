using UnityEngine;
using System.Collections;

public class SelectAndMove : MonoBehaviour {
    float maxSpeed;
    bool isMovable = false;
    bool mouseExit = false;
    private Shader shaderNormal;
    private Shader shaderHighlighted;
  

    void Start() {
        maxSpeed = this.GetComponent<Engines>().MaxSpeed;
        shaderNormal = Shader.Find("VertexLit"); // Normal texture
        shaderHighlighted = Shader.Find("Diffuse"); // Selected texture
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) { // Select with the right click of the mouse (left click is for drag and drop)
            mouseExit = !isMovable ? false : true;
            isMovable = !isMovable;
        }
    }

    void OnMouseExit() {
        mouseExit = true;
    }

    /// <summary>
    ///     F and B are used to bring the object forward or backward
    ///     Use the directional keys on the keyboard to move
    /// </summary>
    void Update() {
        if (isMovable) {
            this.GetComponent<Engines>().Speed = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            if(Input.GetKey(KeyCode.F))
                this.GetComponent<Engines>().Speed= new Vector3(0, 0, transform.position.z + maxSpeed);
            if (Input.GetKey(KeyCode.B))
                this.GetComponent<Engines>().Speed = new Vector3(0, 0, -transform.position.z - maxSpeed);
            GetComponent<Renderer>().material.shader = shaderHighlighted;//selection effect
        }

        //if right button clicked outside the gameobject or on the gameobject when it's selected 
        if (Input.GetMouseButtonDown(1) && (mouseExit ||! isMovable)) {
            isMovable = false;
            GetComponent<Renderer>().material.shader = shaderNormal;
        }
    }
}