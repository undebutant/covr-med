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
    //c'était private on verra apres

    public Vector3 HandPosition = Vector3.zero;
    public Quaternion HandRotation = Quaternion.identity;


    public bool InitHaptics()
    {

        // Initializes variables
        Init();

        Phantom = new SimplePhantomUnity();    
        Phantom.AddSchedule(PhantomUpdate,Hd.Priority.HD_RENDER_EFFECT_FORCE_PRIORITY);

        //while (!Phantom.IsAvailable) ;

        Phantom.Start();

        return true;

    }

    private void Init()
    {
        // Initialization of hand position and orientation
        HandPosition = Vector3.zero;
        HandRotation = Quaternion.identity;

    }


    /// Process when disabling the application
    private void OnDisable()
    {
        Debug.Log(_tag + "Haptic go out on disable");
        StopHaptics();
    }

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
        // ...
        InitHaptics();
    }

    // Update is called once per frame
    void Update()
    {
        if (Phantom != null) 
        Debug.Log(HandPosition);

    }
     private bool PhantomUpdate()
    {
        HandPosition = Phantom.GetPosition();
        HandRotation = Phantom.GetRotation();

        return true;
    }
}
