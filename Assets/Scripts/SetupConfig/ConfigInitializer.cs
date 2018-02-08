using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;


public class ConfigInitializer : MonoBehaviour {

    /// <summary>
    ///     The singleton instance of this script, to prevent multiple config setup
    /// </summary>
    public static ConfigInitializer singletonInstance;

    /// <summary>
    ///     The C# class containing all of the useful data for setup
    /// </summary>
    StartingConfig startingConfig;

    /// <summary>
    ///     Boolean to know if we are connected to a server
    /// </summary>
    bool isConnected;

    [SerializeField]
    [Tooltip("The name of the JSON configuration file")]
    string nameOfJSON = "config.json";

    [SerializeField]
    [Tooltip("Name of the main menu's scene")]
    string mainMenuScene;


    /// <summary>
    ///     Making sure the singleton pattern is respected
    /// </summary>
    void Awake() {
        if (singletonInstance == null) {
            singletonInstance = this;
        } else if (singletonInstance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    void Start() {
        // Creating StartingConfig object to store global setup variables in
        startingConfig = new StartingConfig();

        // Making sure the file is here
        if(File.Exists(nameOfJSON)) {
            try {
                startingConfig = JsonUtility.FromJson<StartingConfig>(File.ReadAllText(nameOfJSON));

                // Checking if we need to disable the VR
                ToggleVR();

                SceneManager.LoadScene(mainMenuScene);
            }
            // Catching error while parsing
            catch (ArgumentException exception) {
                Debug.LogError("Error while parsing the specified JSON file. Exception raised : " + exception);
                Application.Quit();
            }
            catch (Exception exception) {
                Debug.LogError("Unexpected exception raised : " + exception);
                Application.Quit();
            }
        } else {
            Debug.LogError("File not found (was looking at " + Path.GetFullPath(".") + "\\" + nameOfJSON + ")");

            // Creating a sample JSON file
            CreateBasicJSON(nameOfJSON, true);

            // Checking if we need to disable the VR
            ToggleVR();

            SceneManager.LoadScene(mainMenuScene);
        }

        isConnected = false;
    }


    /// <summary>
    ///     Setting up a basic JSON file as sample
    /// </summary>
    /// <param name="storingPath">The path in which we want the JSON to be saved</param>
    /// <param name="isSampleConfigUsed">Choosing if value inside the startingConfig object should be overriden as sample or not</param>
    void CreateBasicJSON(string storingPath, bool isSampleConfigUsed) {
        if (isSampleConfigUsed) {
            startingConfig.serverIP = "localhost";
            startingConfig.connectionPort = 7777;

            startingConfig.displayDevice = DisplayDevice.Cave;
            startingConfig.inputDevice = InputDevice.Controller;

            startingConfig.playerRole = PlayerRole.Nurse;
        }

        // Saving the sample JSON
        File.WriteAllText(nameOfJSON, JsonUtility.ToJson(startingConfig));
    }


    /// <summary>
    ///     Toggle on the Oculus SDK and VR setting on startup if we use it, according to the config loaded
    /// </summary>
    void ToggleVR() {
        if(startingConfig.displayDevice == DisplayDevice.Oculus) {
            StartCoroutine(LoadDevice("Oculus"));
        }
    }

    /// <summary>
    ///     Coroutine loading the VR device inserted as argument
    ///     Please note that you need to add the target device in Unity editor (see Edit > Project Settings > Player > Virtual Reality SDKs)
    ///     Disclaimer : it HAS to be a coroutine since it starts loading the SDK right after the call
    /// </summary>
    /// <param name="newDevice">The name of the VR device to load, as string</param>
    /// <returns></returns>
    IEnumerator LoadDevice(string newDevice) {
        VRSettings.LoadDeviceByName(newDevice);
        yield return null;
        VRSettings.enabled = true;
    }


    //////////////////////////////////////////////////////////////////////////
    //             Getter for enum length to cycle through them             //
    //////////////////////////////////////////////////////////////////////////

    public int GetPlayerRoleEnumLength() {
        return System.Enum.GetValues(typeof(PlayerRole)).Length;
    }

    public int GetDisplayDeviceEnumLength() {
        return System.Enum.GetValues(typeof(DisplayDevice)).Length;
    }

    public int GetInputDeviceEnumLength() {
        return System.Enum.GetValues(typeof(InputDevice)).Length;
    }



    //////////////////////////////////////////////////////////////////////////
    //              Getter and setters for the setup variables              //
    //////////////////////////////////////////////////////////////////////////

    public string GetServerIP() {
        return startingConfig.serverIP;
    }

    public int GetConnectionPort() {
        return startingConfig.connectionPort;
    }

    public DisplayDevice GetDisplayDevice() {
        return startingConfig.displayDevice;
    }

    public InputDevice GetInputDevice() {
        return startingConfig.inputDevice;
    }

    public PlayerRole GetPlayerRole() {
        return startingConfig.playerRole;
    }

    public bool GetIsConnected() {
        return isConnected;
    }

    public void SetIsConnected(bool value) {
        isConnected = value;
    }

    public void SetPlayerRole (PlayerRole newPlayerRole) {
        startingConfig.playerRole = newPlayerRole;

        // Updating JSON accordingly
        CreateBasicJSON(nameOfJSON, false);
    }

    public void SetDisplayDevice(DisplayDevice newDisplayDevice) {
        startingConfig.displayDevice = newDisplayDevice;

        // Updating JSON accordingly
        CreateBasicJSON(nameOfJSON, false);
    }

    public void SetInputDevice(InputDevice newInputDevice) {
        startingConfig.inputDevice = newInputDevice;

        // Updating JSON accordingly
        CreateBasicJSON(nameOfJSON, false);
    }
}
