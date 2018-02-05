using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagedPhantom;

public class PhantomManager : MonoBehaviour {
    // This script is use for making the SimplePhantomUnity a singleton
    // Because when the haptic is use in two differents scenes, the phantom have troubles to initialized twice
    // in the same Build
    public static PhantomManager singletonInstance;

    private SimplePhantomUnity phantom = null;

    void Start() {
        // Initialize the SimplePhantomUnity

        phantom = new SimplePhantomUnity();
    }

    public SimplePhantomUnity GetPhantom () {
        return phantom;
    }

    void Awake() {
        if (singletonInstance == null) {
            singletonInstance = this;
        } else if (singletonInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    // Process when disabling the application
    private void OnDisable() {
        StopHaptics();

    }

    // Stop device communication
    public bool StopHaptics() {

        // Bugfix very dirty for the Build Only
#if UNITY_EDITOR
        if (phantom == null || !phantom.IsRunning)
            return false;

        while (!phantom.IsAvailable) ;


        // Exit the use of PHANTOM
        phantom.Close();
        phantom = null;
#else
        
        System.Diagnostics.Process.GetCurrentProcess().Kill();
        
#endif

        return true;
    }

}
