using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;


public class ConnectionManager : MonoBehaviour {



    /// <summary>
    ///     The NetworkManager component to use for hosting and clients
    /// </summary>
    [SerializeField]
    public static NetworkManager networkManager;

    /// <summary>
    ///     The ConfigInitializer component containing all the global setup variables
    /// </summary>
    ConfigInitializer configInitializer;


    float test;

    void Awake() {


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
        test = UnityEngine.Random.value;


    }


    void UpdateNetworkManager() {
        networkManager.networkAddress = configInitializer.GetServerIP();
        networkManager.networkPort = configInitializer.GetConnectionPort();
    }


    public bool Disconnect() {
        try {

            Debug.LogError("ClientDisconnecting");
            networkManager.StopClient();
            Debug.LogError(test);
            /*
            if (isHost) {
                Debug.LogError("ServerDisconnecting");
                networkManager.StopServer();
            }*/
            Destroy(gameObject);
            
            
            return true;
        } catch (Exception exception) {
            Debug.LogError("Error while stopping application as host. Exception raised : " + exception);
            return false;
        }
    }

    public bool StartAsHost() {
        /*
        if (networkManager == null) {
            networkManager = GameObject.FindObjectOfType<NetworkManager>();
            Debug.LogError("networkNull");
        }*/
        
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
        if (networkManager == null) {
            networkManager = GameObject.FindObjectOfType<NetworkManager>();
        }

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
