using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagedPhantom;

//---------------------------------------------------------------------------
// HAPTIC MANAGER
//---------------------------------------------------------------------------


public class HapticManager : MonoBehaviour {

    //declarations

    /// Tag for logging information (Debug purpose only)
    private static string _tag = ".::. HapticManager .::. ";

    private SimplePhantomUnity phantom = null;

    Vector3 handPosition;
    Quaternion handRotation;

    
    Vector3 offset;

    Quaternion offsetRotation;

    [SerializeField]
    GameObject hand;


    //Boolean and state to recreate the GetButtonDown from the haptic

    bool waitForButton1ToBePressed;
    bool waitForButton2ToBePressed;
    bool isButton1Pressed;
    bool isButton2Pressed;

    //Variaple to reduce plage of Haptic movement
    [SerializeField]
    int downScale = 150;

    public SimplePhantomUnity Phantom
    {

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


    // Initializes communication with Phantom device
    public bool InitHaptics() {
        // Initializes variables
        Init();

        phantom = new SimplePhantomUnity();    
        phantom.AddSchedule(PhantomUpdate,Hd.Priority.HD_RENDER_EFFECT_FORCE_PRIORITY);
        phantom.Start();

        return true;
    }


    // Initialization of the manager
    private void Init() {
        // Initialization of hand position and orientation
        handPosition = Vector3.zero;
        handRotation = Quaternion.identity;
    }


    // Process when disabling the application
    private void OnDisable() {
        Debug.Log(_tag + "Haptic go out on disable");
        StopHaptics();
    }


    // Stops device communication
    public bool StopHaptics() {
        if (phantom == null || !phantom.IsRunning)
            return false;

        while (!phantom.IsAvailable) Debug.Log(_tag + "...");

        // Exit the use of PHANTOM
        phantom.Close();
        phantom = null;

        return true;
    }


    // Use this for initialization
    void Start () {

        offset = hand.transform.localPosition;
        offsetRotation = hand.transform.localRotation;
      
        InitHaptics();

        isButton1Pressed = false;
        isButton2Pressed = false;
        waitForButton1ToBePressed = true;
        waitForButton2ToBePressed = true;
    }


    // Update is called once per frame
    void Update() {

        if (phantom != null) 
        Debug.Log(handPosition);

    }


    // Function to be executed asynchronously from the haptic device
    // Responsable of all the haptic force feedback during simulation
    private bool PhantomUpdate() {
        
        //The changes are made because unity has a base with xzy axes and the haptic has a base with zxy

        Vector3 haptPosition = phantom.GetPosition() / downScale;
        //adapting haptic axes to unity axes 
        handPosition.x = - haptPosition.z;
        handPosition.z = haptPosition.x;
        handPosition.y = haptPosition.y;
        handPosition = handPosition + offset;

        Quaternion haptRotation = phantom.GetRotation();
        //get euler vector from quaternion to be aple to apply changes
        Vector3 eulerVector = haptRotation.eulerAngles;

        float temp = eulerVector.x;
        eulerVector.x = -eulerVector.z;
        eulerVector.z = temp;

        haptRotation = Quaternion.Euler(eulerVector.x, eulerVector.y, eulerVector.z);
        handRotation = haptRotation * offsetRotation;



        //Test if the button 1 and 2 are pressed
        if (phantom.GetButton() == Buttons.Button1 && waitForButton1ToBePressed)
        {
            waitForButton1ToBePressed = false;
            isButton1Pressed = true;
        }

        if (phantom.GetButton() != Buttons.Button1 && !waitForButton1ToBePressed)
        {
            waitForButton1ToBePressed = true;
            isButton1Pressed = false;
        }

        if (phantom.GetButton() == Buttons.Button2 && waitForButton2ToBePressed)
        {
            waitForButton2ToBePressed = false;
            isButton2Pressed = true;
        }

        if (phantom.GetButton() != Buttons.Button2 && !waitForButton2ToBePressed)
        {
            waitForButton2ToBePressed = true;
            isButton2Pressed = false;
        }

        return true;
    }

    /// <summary>
    ///     Public function to get if a button of the haptic is pressed 
    /// </summary>
    /// <param name="button">1 for Button1, 2 for Button2</param>
    /// <returns></returns>
    public bool GetButtonDown (int button)
    {

        if(button == 1)
        {
            if(isButton1Pressed)
            {
                //When the function is called with a button pressed, set the buttonPressed at false to prevent multiple ON value from one Input
                isButton1Pressed = false;
                return true;
            } else
            {
                return false;
            }
            
        }
        if (button == 2)
        {
            if (isButton2Pressed)
            {
                isButton2Pressed = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}
