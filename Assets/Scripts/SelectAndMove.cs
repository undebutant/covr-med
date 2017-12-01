using UnityEngine;
using System.Collections;

public class SelectAndMove : MonoBehaviour
{
    float maxspeed;
    bool movable = false;
    bool mouseExit = false;
    private Shader shaderNormal;
    private Shader shaderHighlighted;
  

    void Start()
    {
        maxspeed = this.GetComponent<ObjectAvatar>().MaxSpeed;
        shaderNormal = Shader.Find("VertexLit");
        shaderHighlighted = Shader.Find("Diffuse");
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
            if (movable)
                movable = false;
            else
            {
                movable = true;
                mouseExit = false;
            }
                
      
    }

    void OnMouseExit()
    {
        mouseExit = true;
    }


    void Update()
    {
        if (movable)
        {
            this.GetComponent<Engines>().Speed = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            if(Input.GetKey(KeyCode.F))
                this.GetComponent<Engines>().Speed= new Vector3(0, 0, transform.position.z + maxspeed);
            if (Input.GetKey(KeyCode.B))
                this.GetComponent<Engines>().Speed = new Vector3(0, 0, -transform.position.z - maxspeed);
            GetComponent<Renderer>().material.shader = shaderHighlighted;//selection effect
    }
        if (Input.GetMouseButtonDown(1) && (mouseExit ||! movable)) //if right button clicked outside the gameobject or on the gameobject when it's selected 
        {
            movable = false;
            GetComponent<Renderer>().material.shader = shaderNormal;
        }
            


}
}