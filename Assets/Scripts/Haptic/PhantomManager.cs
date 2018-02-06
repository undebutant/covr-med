using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagedPhantom;

public class PhantomManager : MonoBehaviour {
    // This script is use to make the SimplePhantomUnity a singleton
    // Because when the haptic is used in two differents scenes, the phantom have troubles to initialized twice
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

        // Changin behaviour if we are in Unity debug mode or in a build
#if UNITY_EDITOR
        if (phantom == null || !phantom.IsRunning)
            return false;

        while (!phantom.IsAvailable) ;

        // Exit the use of PHANTOM
        phantom.Close();
        phantom = null;
#else
        // Bugfix very dirty to make sure both the haptic and the application are stopped
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif

        return true;
    }
}
