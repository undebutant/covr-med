using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadNodeManager : MonoBehaviour {

    GameObject headNode;

    // The VRWand object is need, as it is the only one knowing the player's prefab via the WandSelection script
    [SerializeField]
    GameObject vrWand;

    WandSelection wandSelection;

    // The local prefab player
    GameObject prefabPlayer;

	// Use this for initialization
	void Start () {
        headNode = null;
        // Find the head node 
        while (headNode == null) {
            headNode = GameObject.Find("HeadNode");
        }
        wandSelection = vrWand.GetComponent<WandSelection>();
	}
	
	// Update is called once per frame
	void Update () {

        // Wait for the WandSelection to find the local prefab player
        if (prefabPlayer == null)
            prefabPlayer = wandSelection.prefabPlayer;

        // Update the position of the prefab player
        Vector3 newPosition = new Vector3(headNode.transform.position.x, prefabPlayer.transform.position.y, headNode.transform.position.z);
        Quaternion newRotation = headNode.transform.rotation;
        prefabPlayer.transform.position = newPosition;

        // Set the prefab's camera transform
        GameObject prefabPlayerCamera = prefabPlayer.GetComponent<NetworkAvatarSetup>().GetPlayerCamera();
        prefabPlayerCamera.transform.rotation = newRotation;
    }
}
