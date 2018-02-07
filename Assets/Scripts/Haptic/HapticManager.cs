using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagedPhantom;


/// <summary>
///     The input manager dedicated for the haptic arm
/// </summary>
public class HapticManager : MonoBehaviour {

    // Variables declarations

    /// Tag for logging information (Debug purpose only)
    private static string _tag = ".::. HapticManager .::. ";

    private SimplePhantomUnity phantom = null;

    [SerializeField]
    GameObject hand;

    Vector3 handPosition;
    Quaternion handRotation;

    
    Vector3 offsetPosition;
    Quaternion offsetRotation;
    Vector3 offsetGlobalPosition;

    // Last position to calculate friction forces
    Vector3 lastPosition;


    // Boolean and state to recreate the GetButtonDown from the haptic
    bool waitForButton1ToBePressed;
    bool waitForButton2ToBePressed;
    bool isButton1Pressed;
    bool isButton2Pressed;


    // Variable to reduce range of Haptic movement in Unity scale
    [SerializeField]
    int downScale = 150;


    [SerializeField]
    HandCollider handColliderScript;

    [SerializeField]
    Collider handCollider;


    // Variable to deactivate some rotations from the hand so that the syringe follow the haptic arm correctly
    bool isSyringeSelected = false;

    public void SelectSyringe() {
        isSyringeSelected = true;
    }

    public void ReleaseSyringe() {
        isSyringeSelected = false;
    }


    public  Vector3 HandPosition {
        get {
            return handPosition;
        }
    }

    public Quaternion HandRotation {
        get {
            return handRotation;
        }
    }


    // Initialize communication with Phantom device
    public bool InitHaptics() {
        // Initialize variables
        Init();
        // This script just get the phantom that is already existing from the PhantomManager ...
        PhantomManager phantomManager = GameObject.FindObjectOfType<PhantomManager>();
        phantom = phantomManager.GetPhantom();
        // ... and add his function to it
        phantom.AddSchedule(PhantomUpdate, Hd.Priority.HD_RENDER_EFFECT_FORCE_PRIORITY);
        phantom.Start();
        return true;
    }


    // Process when disabling the application
    private void OnDisable() {
        // Clear the phantom for the next script that will use the phantom
        phantom.Stop();
        phantom.ClearSchedule();
    }


    // Initialization of the manager
    private void Init() {
        // Initialization of hand position and orientation
        handPosition = Vector3.zero;
        handRotation = Quaternion.identity;
        lastPosition = Vector3.zero;
    }


    void Start () {
        offsetPosition = hand.transform.localPosition;
        offsetRotation = hand.transform.localRotation;
        offsetGlobalPosition = hand.transform.position;

        InitHaptics();

        isButton1Pressed = false;
        isButton2Pressed = false;
        waitForButton1ToBePressed = true;
        waitForButton2ToBePressed = true;
        isSyringeSelected = false;
    }


    // Function to be executed asynchronously from the haptic device
    private bool PhantomUpdate() {
        // Downscaling the movement range in Unity app for the user
        Vector3 haptPosition = phantom.GetPosition() / downScale;

        // Axes are swapped because unity has a xzy reference frame and the haptic has a zxy reference frame
        handPosition.x = - haptPosition.z;
        handPosition.z = haptPosition.x;
        handPosition.y = haptPosition.y;
        handPosition = handPosition + offsetPosition;

        Quaternion haptRotation = phantom.GetRotation();

        // Converting from quaternion to euler for axes swapping
        Vector3 eulerVector = haptRotation.eulerAngles;

        // Going from Phantom to Unity axes
        float temp = eulerVector.x;
        eulerVector.x = -eulerVector.z;
        eulerVector.z = temp;

        // Going back to quaternion
        haptRotation = Quaternion.Euler(eulerVector.x, eulerVector.y, eulerVector.z);


        // Deactivate some rotations from the hand to have the syringe follow the haptique correctly
        if (isSyringeSelected) {
            handRotation = haptRotation;
        } else {
            handRotation = haptRotation * offsetRotation;
        }

        // Test for forces 
        Vector3 force = new Vector3(0,0,0);

        float maxForceFriction = 1.5f;

        // Test for the friction of the tissues
        if (handColliderScript.GetIsContactTissue()) {
            if (isSyringeSelected) {
                // In the case we have to apply friction for the syringe...
                force.y = (lastPosition.y - haptPosition.y) * 1500;
                if (force.y > maxForceFriction) {
                    force.y = maxForceFriction;
                }


                if (force.y < -maxForceFriction) {
                    force.y = -maxForceFriction;
                }
            } else {
                // ... else we don't hold the syringe, then, we just hit the table, and we don't want to go through it
                force.y = 300 * (handColliderScript.GetLastTissueY() - offsetGlobalPosition.y - handPosition.y + offsetPosition.y);
                if (force.y < 0) {
                    force.y = 0;
                }
            }
            
        }

        // Test for the table, because Brian is on the table
        if (handColliderScript.GetIsContactTable() && isSyringeSelected) {
            force.y = 300 * (handColliderScript.GetLastTableY() - offsetGlobalPosition.y - handPosition.y + offsetPosition.y);
            if(force.y < 0) {
                force.y = 0;
            }
        }

        phantom.SetForce(force);


        // Test if the button 1 and 2 are pressed
        // If so, toggle on/off the boolean isButtonPressed
        if (phantom.GetButton() == Buttons.Button1 && waitForButton1ToBePressed) {
            waitForButton1ToBePressed = false;
            isButton1Pressed = true;
        }

        if (phantom.GetButton() != Buttons.Button1 && !waitForButton1ToBePressed) {
            waitForButton1ToBePressed = true;
            isButton1Pressed = false;
        }

        if (phantom.GetButton() == Buttons.Button2 && waitForButton2ToBePressed) {
            waitForButton2ToBePressed = false;
            isButton2Pressed = true;
        }

        if (phantom.GetButton() != Buttons.Button2 && !waitForButton2ToBePressed) {
            waitForButton2ToBePressed = true;
            isButton2Pressed = false;
        }

        lastPosition = haptPosition;

        return true;
    }


    /// <summary>
    ///     Public function to know if a button of the haptic arm is pressed 
    /// </summary>
    /// <param name="button">1 for Button1, 2 for Button2</param>
    public bool GetButtonDown (int button) {
        if(button == 1) {
            if(isButton1Pressed) {
                //When the function is called with a button pressed, set the buttonPressed at false to prevent multiple ON value from one Input
                isButton1Pressed = false;
                return true;
            } else {
                return false;
            }
        }
        if (button == 2) {
            if (isButton2Pressed) {
                isButton2Pressed = false;
                return true;
            } else {
                return false;
            }
        }
        return false;
    }
}
