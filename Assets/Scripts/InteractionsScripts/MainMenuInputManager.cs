using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInputManager : MonoBehaviour {

    [SerializeField]
    MainMenuManager mainMenuManager;

    ConfigInitializer configInitializer;

    [SerializeField]
    HapticManager hapticManager;

    [SerializeField]
    [Tooltip("The valid range for the raycast")]
    float raycastRange = 100.0f;

    [SerializeField]
    GameObject laser;

    [SerializeField]
    float hapticArmMotionCoefficient = 5.0f;


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

        // Handle the laser's rotation
        Quaternion hapticRotation = hapticManager.HandRotation;

        laser.transform.localRotation = Quaternion.EulerAngles(0, hapticRotation.ToEulerAngles().y, -hapticRotation.ToEulerAngles().z);

        // Handle the laser's position
        Vector3 hapticPosition = hapticManager.HandPosition;

        laser.transform.localPosition = new Vector3(-hapticPosition.x, hapticPosition.y, -hapticPosition.z) * hapticArmMotionCoefficient;

        // Fire a raycast if the haptic arm's button is pressed, and transmit the menu's hit button to the main menu manager
        if (hapticManager.GetButtonDown(1)) {
            rayFired = new Ray(laser.transform.position, laser.transform.right);

            if (Physics.Raycast(rayFired, out raycastHit, raycastRange))
            {
                mainMenuManager.OnHitButton(raycastHit.collider.gameObject);
            }
        }
    }


    void HandleRemoteInputs() {
        // For now, handled by MiddleVR, edit if needed
        // Deactivate the cube used for the haptic arm
        laser.SetActive(false);
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
