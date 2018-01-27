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

    private SimplePhantomUnity Phantom = null;

    Vector3 handPosition;
    Quaternion handRotation;

    
    Vector3 offset;

    Quaternion offsetRotation;

    [SerializeField]
    GameObject hand;

    [SerializeField]
    GameObject player;

    //Variaple to reduce plage of Haptic movement
    [SerializeField]
    int downScale = 150;

    public  Vector3 HandPosition
    {
        get
        {
            return handPosition;
        }
    }


    public Quaternion HandRotation
    {
        get
        {
            return handRotation;
        }
    }


    // Initializes communication with Phantom device
    public bool InitHaptics()
    {
        // Initializes variables
        Init();

        Phantom = new SimplePhantomUnity();    
        Phantom.AddSchedule(PhantomUpdate,Hd.Priority.HD_RENDER_EFFECT_FORCE_PRIORITY);
        Phantom.Start();

        return true;
    }


    // Initialization of the manager
    private void Init()
    {
        // Initialization of hand position and orientation
        handPosition = Vector3.zero;
        handRotation = Quaternion.identity;
    }


    // Process when disabling the application
    private void OnDisable()
    {
        Debug.Log(_tag + "Haptic go out on disable");
        StopHaptics();
    }


    // Stops device communication
    public bool StopHaptics()
    {
        if (Phantom == null || !Phantom.IsRunning)
            return false;

        while (!Phantom.IsAvailable) Debug.Log(_tag + "...");

        // Exit the use of PHANTOM
        Phantom.Close();
        Phantom = null;

        return true;
    }

    // Use this for initialization
    void Start () {
        offset = hand.transform.localPosition;
        offsetRotation = hand.transform.localRotation;
        Debug.LogError(offsetRotation);
      
        InitHaptics();

    }


    // Update is called once per frame
    void Update()
    {
        if (Phantom != null) 
        Debug.Log(handPosition);

    }


    // Function to be executed asynchronously from the haptic device
    // Responsable of all the haptic force feedback during simulation
    private bool PhantomUpdate()
    {

        Vector3 haptPosition = Phantom.GetPosition() / downScale;
        //adapting haptic axes to unity axes 
        handPosition.x = - haptPosition.z;
        handPosition.z = haptPosition.x;
        handPosition.y = haptPosition.y;
        handPosition = handPosition + offset;

        Quaternion haptRotation = Phantom.GetRotation();
        //get euler vector from quaternion to be aple to apply changes
        Vector3 eulerVector = haptRotation.eulerAngles;

        float temp = eulerVector.x;
        eulerVector.x = -eulerVector.z;
        eulerVector.z = temp;

        haptRotation = Quaternion.Euler(eulerVector.x, eulerVector.y, eulerVector.z);
        handRotation = haptRotation * offsetRotation;

        return true;
    }
}
