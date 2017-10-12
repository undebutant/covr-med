using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class NetworkVariable : NetworkBehaviour {

    [SyncVar]
    public int alphaValueUpServer;
    
    public int alphaValueUpClient;


    [SyncVar]
    public int alphaValueDownServer;
   
    public int alphaValueDownClient;


    [SyncVar]
    public int alphaValueLeftServer;
    
    public int alphaValueLeftClient;


    [SyncVar]
    public int alphaValueRightServer;
    
    public int alphaValueRightClient;



    void Start() {
        alphaValueUpServer = 0;
        alphaValueUpClient = 0;

        alphaValueDownServer = 0;
        alphaValueDownClient = 0;

        alphaValueLeftServer = 0;
        alphaValueLeftClient = 0;

        alphaValueRightServer = 0;
        alphaValueRightClient = 0;
    }



    // Defining a method callable by the client, to update synced variables on the server
    [Command]
    public void CmdClient(int upValue, int downValue, int rightValue, int leftValue) {
        // Calling the sync method server side, to make other instances aware of the updated variables
        RpcUp(upValue, downValue, rightValue, leftValue);
    }


    // Remote Procedure Calls (RPC) callable by the server, to update the variables in all known clients
    [ClientRpc]
    void RpcUp(int upValue, int downValue, int rightValue, int leftValue) {
        alphaValueUpClient = upValue;
        alphaValueDownClient = downValue;
        alphaValueLeftClient = leftValue;
        alphaValueRightClient = rightValue;
    }
}
