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

        // Searching for the config and networkmanager Scripts
        try {
            configInitializer = FindObjectOfType<ConfigInitializer>();
        } catch (Exception exception) {
            Debug.LogError("Error while looking for the ConfigInitializer. Exception raised : " + exception);
            Application.Quit();
        }
        try {
            networkManager = GameObject.FindObjectOfType<NetworkManager>();
        } catch (Exception exception) {
            Debug.LogError("Error while looking for the NetworkManager. Exception raised : " + exception);
            Application.Quit();
        }
        // Test if we have been disconnected from the host 
        if(configInitializer.GetIsConnected()) {
            Debug.LogError("Lost connection from the host");
            // In this case, a popup will inform the user
            GameObject.FindObjectOfType<ErrorPopupScript>().NewPopup("Lost connection from the host");
            configInitializer.SetIsConnected(false);
        }
    }


    void UpdateNetworkManager() {
        networkManager.networkAddress = configInitializer.GetServerIP();
        networkManager.networkPort = configInitializer.GetConnectionPort();
    }


    public bool StartAsHost() {
        
        try {
            // Use the IP and Port from the Json for the NetworkManager
            UpdateNetworkManager();
            configInitializer.SetIsConnected(true);
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
            // Use the IP and Port from the Json for the NetworkManager
            UpdateNetworkManager();
            configInitializer.SetIsConnected(true);
            networkManager.StartClient();
            return true;
        }
        catch (Exception exception) {
            Debug.LogError("Error while launching application as client. Exception raised : " + exception);
            return false;
        }
    }


}
