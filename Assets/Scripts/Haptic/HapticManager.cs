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

    Vector3 handPosition;
    Quaternion handRotation;

    
    Vector3 offsetPosition;
    Quaternion offsetRotation;

    [SerializeField]
    GameObject hand;

    //Last position to calculate friction forces
    Vector3 lastPosition;

    //Boolean and state to recreate the GetButtonDown from the haptic

    bool waitForButton1ToBePressed;
    bool waitForButton2ToBePressed;
    bool isButton1Pressed;
    bool isButton2Pressed;

    private void Update() {
        //Debug.LogError(handPosition);
    }
   

    // Variable to reduce range of Haptic movement in Unity scale
    [SerializeField]
    int downScale = 150;

    // Variable to deactivate some rotations from the hand so that the syringe follow the haptic arm correctly
    bool isSyringeSelected = false;

    public void SelectSyringe() {
        isSyringeSelected = true;
    }

    public void ReleaseSyringe() {
        isSyringeSelected = false;
    }

    public SimplePhantomUnity Phantom {
        get
        {
            return phantom;
        }
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

        phantom = new SimplePhantomUnity();    
        phantom.AddSchedule(PhantomUpdate, Hd.Priority.HD_RENDER_EFFECT_FORCE_PRIORITY);
        phantom.Start();

        return true;
    }


    // Initialization of the manager
    private void Init() {
        // Initialization of hand position and orientation
        handPosition = Vector3.zero;
        handRotation = Quaternion.identity;
        lastPosition = Vector3.zero;
    }


    // Process when disabling the application
    private void OnDisable() {
        Debug.Log(_tag + "Haptic go out on disable");
        StopHaptics();
    }


    // Stop device communication
    public bool StopHaptics() {

        // Bugfix very dirty for the Build Only
#if UNITY_EDITOR
        if (phantom == null || !phantom.IsRunning)
            return false;

        while (!Phantom.IsAvailable) ;


        // Exit the use of PHANTOM
        phantom.Close();
        phantom = null;
#else
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif



        return true;
    }


    void Start () {
        offsetPosition = hand.transform.localPosition;
        offsetRotation = hand.transform.localRotation;
      
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



        
        // TEST for forces 
        Vector3 force = new Vector3(0,0,0);

        // Test for the friction of the tissues
        if (handPosition.y < 0) {
            force.y = (lastPosition.y - haptPosition.y)*1500;
            if(force.y>1.5) {
                force.y = 1.5f;
            }
        }


        // Test for the table, because Brian is on the table
        if (handPosition.y<-0.2) {
            force.y = 300 * (-0.2f - handPosition.y);
        }

        Phantom.SetForce(force);


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
