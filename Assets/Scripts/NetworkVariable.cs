using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkVariable : NetworkBehaviour {

    [SyncVar]
    public int colorUpRedA1;
    
    public int colorUpRedA2;

    [SyncVar]
    public int colorDownRedA1;
   
    public int colorDownRedA2;
    [SyncVar]
    public int colorLeftRedA1;
    
    public int colorLeftRedA2;
    [SyncVar]
    public int colorRightRedA1;
    
    public int colorRightRedA2;


    [Command]
    public void CmdClient(int up,int down,int right, int left) {
        RpcUp(up, down, right, left);
    }

    [ClientRpc]
    void RpcUp(int up, int down, int right, int left) {
        colorUpRedA2=up;
        colorDownRedA2 = down;
        colorLeftRedA2 = left;
        colorRightRedA2 = right;
    }




    // Use this for initialization
    void Start() {
        colorUpRedA1 = 0;
        colorUpRedA2 = 0;
        colorDownRedA1 = 0;
        colorDownRedA2 = 0;
        colorLeftRedA1 = 0;
        colorLeftRedA2 = 0;
        colorRightRedA1 = 0;
        colorRightRedA2 = 0;
    }
	
	
}
