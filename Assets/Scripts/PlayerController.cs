using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class PlayerController : NetworkBehaviour {

    // Linking the script containing the network wide variables
    //public NetworkVariable networkVariable;

    // Linking test cube
    public GameObject testCube;
    private Renderer testCubeRenderer;



    void Start() {
        testCubeRenderer = testCube.GetComponent<Renderer>();
    }


    void Update() {
        // Handling the objects (other than the player avatar) moved by the other instances
        if (!isLocalPlayer) {
            // If the instance has the server role, updating the client avatar variables
            if (isServer) {
                
            } else {
                
            }

            return;
        }

        // Handling avatar movement
        // If the instance has the server role
        if (isServer) {
            // Handling movement and color (using alpha) in case of input in this direction
            if (Input.GetButton("Up")) {
                transform.Translate(0, Time.deltaTime, 0);
                
            // Handling the color reset when no input detected in this direction
            } else {

            }

            if (Input.GetButton("Down")) {
                transform.Translate(0, -Time.deltaTime, 0);
                
            } else {

            }

            if (Input.GetButton("Left")) {
                transform.Translate(-Time.deltaTime, 0, 0);
            } else {

            }

            if (Input.GetButton("Right")) {
                transform.Translate(Time.deltaTime, 0, 0);
            } else {

            }

        // If the instance has the client role
        } else {
            // Defining locally alpha values to export coloration on the network
            int upValue, downValue, rightValue, leftValue;

            // Handling movement and color (using alpha) in case of input in this direction
            if (Input.GetButton("Up")) {
                transform.Translate(0, Time.deltaTime, 0);
                upValue = 1;
            // Handling the color reset when no input detected in this direction
            } else {
                upValue = 0;               
            }

            if (Input.GetButton("Down")) {
                transform.Translate(0, -Time.deltaTime, 0);
                downValue = 1;
            } else {
                downValue = 0;
            }

            if (Input.GetButton("Left")) {
                transform.Translate(-Time.deltaTime, 0, 0);
                leftValue = 1;
            } else {
                leftValue = 0;
            }

            if (Input.GetButton("Right")) {
                transform.Translate(Time.deltaTime, 0, 0);
                rightValue = 1;
            } else {
                rightValue = 0;
            }

            // Calling the client callable method, to warn the server about the updated variables
            //networkVariable.CmdClient(upValue, downValue, rightValue, leftValue);

            /*
            // Updating the rendering using the changed variables
            redArrowUpColor.a = networkVariable.alphaValueUpClient;
            redArrowUp.GetComponent<SpriteRenderer>().color = redArrowUpColor;

            redArrowDownColor.a = networkVariable.alphaValueDownClient;
            redArrowDown.GetComponent<SpriteRenderer>().color = redArrowDownColor;

            redArrowLeftColor.a = networkVariable.alphaValueLeftClient;
            redArrowLeft.GetComponent<SpriteRenderer>().color = redArrowLeftColor;

            redArrowRightColor.a = networkVariable.alphaValueRightClient;
            redArrowRight.GetComponent<SpriteRenderer>().color = redArrowRightColor;
            */
        }
    }
}
