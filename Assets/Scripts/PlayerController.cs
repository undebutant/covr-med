using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {



    


    Vector3 initPos;

    public NetworkVariable networkVariable;

    public GameObject RedUp;
    Color RedUpColor;

    public GameObject RedDown;
    Color RedDownColor;

    public GameObject RedLeft;
    Color RedLeftColor;

    public GameObject RedRight;
    Color RedRightColor;



    private NetworkIdentity objNetId;


    // Use this for initialization
    void Start() {
        RedUpColor= RedUp.GetComponent<SpriteRenderer>().color;
        RedDownColor = RedDown.GetComponent<SpriteRenderer>().color;
        RedLeftColor = RedLeft.GetComponent<SpriteRenderer>().color;
        RedRightColor = RedRight.GetComponent<SpriteRenderer>().color;
        //networkVariable = GameObject.FindGameObjectWithTag("NetworkVariable").GetComponent<NetworkVariable>();
        if (!isLocalPlayer) {
            return;
        }

    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) {
            
            if (isServer) {
                RedUpColor.a = networkVariable.colorUpRedA2;
            } else {
                RedUpColor.a = networkVariable.colorUpRedA1;
            }
            RedUp.GetComponent<SpriteRenderer>().color = RedUpColor;

            if (isServer) {
                RedDownColor.a = networkVariable.colorDownRedA2;
            } else {
                RedDownColor.a = networkVariable.colorDownRedA1;
            }
            RedDown.GetComponent<SpriteRenderer>().color = RedDownColor;

            if (isServer) {
                RedLeftColor.a = networkVariable.colorLeftRedA2;
            } else {
                RedLeftColor.a = networkVariable.colorLeftRedA1;
            }
            RedLeft.GetComponent<SpriteRenderer>().color = RedLeftColor;

            if (isServer) {
                RedRightColor.a = networkVariable.colorRightRedA2;
            } else {
                RedRightColor.a = networkVariable.colorRightRedA1;
            }
            RedRight.GetComponent<SpriteRenderer>().color = RedRightColor;



            return;
        }


        if (isServer) {
            if (Input.GetButton("Up")) {
                transform.Translate(0, Time.deltaTime, 0);
                
                networkVariable.colorUpRedA1 = 1;
                RedUpColor.a = networkVariable.colorUpRedA1;
                

                RedUp.GetComponent<SpriteRenderer>().color = RedUpColor;

            } else {
                
                networkVariable.colorUpRedA1 = 0;
                RedUpColor.a = networkVariable.colorUpRedA1;
                
                RedUp.GetComponent<SpriteRenderer>().color = RedUpColor;
            }

            if (Input.GetButton("Down")) {
                transform.Translate(0, -Time.deltaTime, 0);
                
                networkVariable.colorDownRedA1 = 1;
                RedDownColor.a = networkVariable.colorDownRedA1;
                
                RedDown.GetComponent<SpriteRenderer>().color = RedDownColor;
            } else {
                
                networkVariable.colorDownRedA1 = 0;
                RedDownColor.a = networkVariable.colorDownRedA1;
                
                RedDown.GetComponent<SpriteRenderer>().color = RedDownColor;
            }

            if (Input.GetButton("Left")) {
                transform.Translate(-Time.deltaTime, 0, 0);
                
                networkVariable.colorLeftRedA1 = 1;
                RedLeftColor.a = networkVariable.colorLeftRedA1;
                
                RedLeft.GetComponent<SpriteRenderer>().color = RedLeftColor;
            } else {
                
                networkVariable.colorLeftRedA1 = 0;
                RedLeftColor.a = networkVariable.colorLeftRedA1;
                
                RedLeft.GetComponent<SpriteRenderer>().color = RedLeftColor;

            }

            if (Input.GetButton("Right")) {
                transform.Translate(Time.deltaTime, 0, 0);
                
                networkVariable.colorRightRedA1 = 1;
                RedRightColor.a = networkVariable.colorRightRedA1;
                
                RedRight.GetComponent<SpriteRenderer>().color = RedRightColor;
            } else {
                
                networkVariable.colorRightRedA1 = 0;
                RedRightColor.a = networkVariable.colorRightRedA1;
                
                RedRight.GetComponent<SpriteRenderer>().color = RedRightColor;
            }


        } else {
            int up;
            int down;
            int right;
            int left;
            if (Input.GetButton("Up")) {
                transform.Translate(0, Time.deltaTime, 0);
                

                up=1;
                

            } else {

                up = 0;
               
            }

            if (Input.GetButton("Down")) {
                transform.Translate(0, -Time.deltaTime, 0);

                down = 1;
                
            } else {

                down = 0;
                
            }

            if (Input.GetButton("Left")) {
                transform.Translate(-Time.deltaTime, 0, 0);

                left = 1;
                
            } else {

                left = 0;
                

            }

            if (Input.GetButton("Right")) {
                transform.Translate(Time.deltaTime, 0, 0);
                right = 1;
                
            } else {

                right = 0;
                
            }

            networkVariable.CmdClient(up, down, right, left);

            RedUpColor.a = networkVariable.colorUpRedA2;
            RedUp.GetComponent<SpriteRenderer>().color = RedUpColor;

            RedDownColor.a = networkVariable.colorDownRedA2;
            RedDown.GetComponent<SpriteRenderer>().color = RedDownColor;

            RedLeftColor.a = networkVariable.colorLeftRedA2;
            RedLeft.GetComponent<SpriteRenderer>().color = RedLeftColor;

            RedRightColor.a = networkVariable.colorRightRedA2;
            RedRight.GetComponent<SpriteRenderer>().color = RedRightColor;
        }


        

        

       

    }

    

}
