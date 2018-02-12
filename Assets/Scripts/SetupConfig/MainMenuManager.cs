using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    Text roleButtonText;

    [SerializeField]
    Text displayDeviceButtonText;

    [SerializeField]
    Text inputDeviceButtonText;

    [SerializeField]
    Text hostIpButtonText;

    [SerializeField]
    Text hostPortText;

    [SerializeField]
    ConnectionManager connectionManager;

    // The configuration class to get data from
    ConfigInitializer configInitializer;

    // Storing the length of each used enum in config, to cycle through them using the buttons
    int playerRoleEnumLength;
    int displayDeviceEnumLength;
    int inputDeviceEnumLength;


    // ========== Set the texts displayed on the main menu ============

    /// <summary>
    ///     Sets the role displayed on the button
    /// </summary>
    /// <param name="role">Role to display</param>
    void SetRoleButtonDisplay(string role) {
        roleButtonText.text = role;
    }

    /// <summary>
    ///     Sets the 'display device' displayed on the button
    /// </summary>
    /// <param name="displayDevice">Display device's name to display on the button in the menu</param>
    void SetDisplayDeviceButtonDisplay(string displayDevice) {
        displayDeviceButtonText.text = displayDevice;
    }

    /// <summary>
    ///     Sets the input device on the button
    /// </summary>
    /// <param name="inputDevice">Input device to display</param>
    void SetInputDeviceButtonDisplay(string inputDevice) {
        inputDeviceButtonText.text = inputDevice;
    }

    /// <summary>
    ///     Sets the host server IP to display on the button
    /// </summary>
    /// <param name="hostIp">Host server IP to display on the button in the main menu</param>
    void SetHostIPDisplay(string hostIp) {
        hostIpButtonText.text = hostIp;
    }

    /// <summary>
    ///     Sets the host server's port to display on the button in the main menu
    /// </summary>
    /// <param name="hostPort">host servers's port to display on the button in the main menu</param>
    void SetHostPortDisplay(string hostPort) {
        hostPortText.text = hostPort;
    }

    void Start() {
        // Read data thanks to the configuration loader
        configInitializer = GameObject.FindObjectOfType<ConfigInitializer>();

        SetRoleButtonDisplay(configInitializer.GetPlayerRole().ToString());
        SetDisplayDeviceButtonDisplay(configInitializer.GetDisplayDevice().ToString());
        SetInputDeviceButtonDisplay(configInitializer.GetInputDevice().ToString());
        SetHostIPDisplay(configInitializer.GetServerIP());
        SetHostPortDisplay(configInitializer.GetConnectionPort().ToString());

        playerRoleEnumLength = configInitializer.GetPlayerRoleEnumLength();
        displayDeviceEnumLength = configInitializer.GetDisplayDeviceEnumLength();
        inputDeviceEnumLength = configInitializer.GetInputDeviceEnumLength();
    }


    public void OnHitButton(GameObject buttonHit) {
        switch (buttonHit.name) {
            case "HostButton":
                connectionManager.StartAsHost();
                break;
            case "ClientHost":
                connectionManager.StartAsClient();
                break;
            case "RoleButton":
                // Cycling through the PlayerRole enum
                configInitializer.SetPlayerRole((PlayerRole)((((int)configInitializer.GetPlayerRole()) + 1) % playerRoleEnumLength));
                // Updating the button text
                SetRoleButtonDisplay(configInitializer.GetPlayerRole().ToString());
                break;
            case "DisplayDeviceButton":
                // Cycling through the DisplayDevice enum
                configInitializer.SetDisplayDevice((DisplayDevice)((((int)configInitializer.GetDisplayDevice()) + 1) % displayDeviceEnumLength));
                // Updating the button text
                SetDisplayDeviceButtonDisplay(configInitializer.GetDisplayDevice().ToString());
                break;
            case "InputDeviceButton":
                // Cycling through the InputDevice enum
                configInitializer.SetInputDevice((InputDevice)((((int)configInitializer.GetInputDevice()) + 1) % inputDeviceEnumLength));
                // Updating the button text
                SetInputDeviceButtonDisplay(configInitializer.GetInputDevice().ToString());
                break;
            default:
                Debug.LogError("Reaching unexpected button name in the MainMenuManager");
                break;
        }
    }
}
