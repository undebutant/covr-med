using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class PlayerController : NetworkBehaviour {

    // Linking the script containing the network wide variables
    public NetworkVariable networkVariable;

    // Setting up variables for the 4 directionnal red arrow sprites
    public GameObject redArrowUp;
    Color redArrowUpColor;

    public GameObject redArrowDown;
    Color redArrowDownColor;

    public GameObject redArrowLeft;
    Color redArrowLeftColor;

    public GameObject redArrowRight;
    Color redArrowRightColor;



    void Start() {
        redArrowUpColor = redArrowUp.GetComponent<SpriteRenderer>().color;
        redArrowDownColor = redArrowDown.GetComponent<SpriteRenderer>().color;
        redArrowLeftColor = redArrowLeft.GetComponent<SpriteRenderer>().color;
        redArrowRightColor = redArrowRight.GetComponent<SpriteRenderer>().color;
    }


    void Update() {
        // Handling the objects (other than the player avatar) moved by the other instances
        if (!isLocalPlayer) {
            // If the instance has the server role, updating the client avatar variables
            if (isServer) {
                redArrowUpColor.a = networkVariable.alphaValueUpClient;
                redArrowDownColor.a = networkVariable.alphaValueDownClient;
                redArrowLeftColor.a = networkVariable.alphaValueLeftClient;
                redArrowRightColor.a = networkVariable.alphaValueRightClient;
            } else {
                redArrowUpColor.a = networkVariable.alphaValueUpServer;
                redArrowDownColor.a = networkVariable.alphaValueDownServer;
                redArrowLeftColor.a = networkVariable.alphaValueLeftServer;
                redArrowRightColor.a = networkVariable.alphaValueRightServer;
            }

            // Using the changed variables to update the sprite renderer
            redArrowUp.GetComponent<SpriteRenderer>().color = redArrowUpColor;
            redArrowDown.GetComponent<SpriteRenderer>().color = redArrowDownColor;
            redArrowLeft.GetComponent<SpriteRenderer>().color = redArrowLeftColor;
            redArrowRight.GetComponent<SpriteRenderer>().color = redArrowRightColor;

            return;
        }

        // Handling avatar movement
        // If the instance has the server role
        if (isServer) {
            // Handling movement and color (using alpha) in case of input in this direction
            if (Input.GetButton("Up")) {
                transform.Translate(0, Time.deltaTime, 0);
                
                networkVariable.alphaValueUpServer = 1;
                redArrowUpColor.a = networkVariable.alphaValueUpServer;

                redArrowUp.GetComponent<SpriteRenderer>().color = redArrowUpColor;
                
            // Handling the color reset when no input detected in this direction
            } else {
                networkVariable.alphaValueUpServer = 0;
                redArrowUpColor.a = networkVariable.alphaValueUpServer;
                
                redArrowUp.GetComponent<SpriteRenderer>().color = redArrowUpColor;
            }

            if (Input.GetButton("Down")) {
                transform.Translate(0, -Time.deltaTime, 0);
                
                networkVariable.alphaValueDownServer = 1;
                redArrowDownColor.a = networkVariable.alphaValueDownServer;
                
                redArrowDown.GetComponent<SpriteRenderer>().color = redArrowDownColor;
                
            } else {
                networkVariable.alphaValueDownServer = 0;
                redArrowDownColor.a = networkVariable.alphaValueDownServer;
                
                redArrowDown.GetComponent<SpriteRenderer>().color = redArrowDownColor;
            }

            if (Input.GetButton("Left")) {
                transform.Translate(-Time.deltaTime, 0, 0);
                
                networkVariable.alphaValueLeftServer = 1;
                redArrowLeftColor.a = networkVariable.alphaValueLeftServer;
                
                redArrowLeft.GetComponent<SpriteRenderer>().color = redArrowLeftColor;

            } else {
                networkVariable.alphaValueLeftServer = 0;
                redArrowLeftColor.a = networkVariable.alphaValueLeftServer;
                
                redArrowLeft.GetComponent<SpriteRenderer>().color = redArrowLeftColor;
            }

            if (Input.GetButton("Right")) {
                transform.Translate(Time.deltaTime, 0, 0);
                
                networkVariable.alphaValueRightServer = 1;
                redArrowRightColor.a = networkVariable.alphaValueRightServer;
                
                redArrowRight.GetComponent<SpriteRenderer>().color = redArrowRightColor;
            } else {
                networkVariable.alphaValueRightServer = 0;
                redArrowRightColor.a = networkVariable.alphaValueRightServer;
                
                redArrowRight.GetComponent<SpriteRenderer>().color = redArrowRightColor;
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
            networkVariable.CmdClient(upValue, downValue, rightValue, leftValue);

            // Updating the rendering using the changed variables
            redArrowUpColor.a = networkVariable.alphaValueUpClient;
            redArrowUp.GetComponent<SpriteRenderer>().color = redArrowUpColor;

            redArrowDownColor.a = networkVariable.alphaValueDownClient;
            redArrowDown.GetComponent<SpriteRenderer>().color = redArrowDownColor;

            redArrowLeftColor.a = networkVariable.alphaValueLeftClient;
            redArrowLeft.GetComponent<SpriteRenderer>().color = redArrowLeftColor;

            redArrowRightColor.a = networkVariable.alphaValueRightClient;
            redArrowRight.GetComponent<SpriteRenderer>().color = redArrowRightColor;
        }
    }
}
