using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInputManager : MonoBehaviour {

    [SerializeField]
    MainMenuManager mainMenuManager;

    [SerializeField]
    ConfigInitializer configInitializer;

    [SerializeField]
    [Tooltip("The valid range for the raycast")]
    float raycastRange;


    // The raycast used for selection
    Ray rayFired;
    RaycastHit raycastHit;


    private void Start() {
        configInitializer = FindObjectOfType<ConfigInitializer>();
    }


    void Update () {
        switch(configInitializer.GetInputDevice()) {
            case InputDevice.Controller:
                HandleControllerInputs();
                break;
            case InputDevice.Haptic:
                HandleHapticInputs();
                break;
            case InputDevice.Remote:
                HandleRemoteInputs();
                break;
            default:
                Debug.LogError("Input device not recognised in the MainMenuInputManager script");
                Application.Quit();
                break;
        }

        // Mouse compatibility if needed
        HandleMouseInputs();
    }


    void HandleControllerInputs() {
        // TODO
    }


    void HandleHapticInputs() {
        // TODO
    }


    void HandleRemoteInputs() {
        // For now, handled by MiddleVR, edit if needed
    }


    void HandleMouseInputs() {
        if (Input.GetButtonDown("Fire1")) {
            rayFired = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayFired, out raycastHit, raycastRange)) {
                mainMenuManager.OnHitButton(raycastHit.collider.gameObject);
            }
        }
    }
}
