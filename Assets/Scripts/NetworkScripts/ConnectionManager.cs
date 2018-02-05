using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;


public class ConnectionManager : MonoBehaviour {

    /// <summary>
    ///     The NetworkManager component to use for hosting and clients
    /// </summary>
    NetworkManager networkManager;

    /// <summary>
    ///     The ConfigInitializer component containing all the global setup variables
    /// </summary>
    ConfigInitializer configInitializer;



    void Awake() {
        try {
            networkManager = GetComponent<NetworkManager>();
        }
        catch (Exception exception) {
            Debug.LogError("Error while looking for the NetworkManager. Exception raised : " + exception);
            Application.Quit();
        }

        try {
            configInitializer = FindObjectOfType<ConfigInitializer>();
        }
        catch (Exception exception) {
            Debug.LogError("Error while looking for the ConfigInitializer. Exception raised : " + exception);
            Application.Quit();
        }
    }


    void UpdateNetworkManager() {
        networkManager.networkAddress = configInitializer.GetServerIP();
        networkManager.networkPort = configInitializer.GetConnectionPort();
    }


    public bool Disconnect() {
        networkManager.StopHost();

        return true;
    }

    public bool StartAsHost() {
        try {
            UpdateNetworkManager();

            networkManager.StartHost();
            return true;
        }
        catch(Exception exception) {
            Debug.LogError("Error while launching application as host. Exception raised : " + exception);
            return false;
        }
    }


    public bool StartAsClient() {
        try {
            UpdateNetworkManager();

            networkManager.StartClient();
            return true;
        }
        catch (Exception exception) {
            Debug.LogError("Error while launching application as client. Exception raised : " + exception);
            return false;
        }
    }
}
