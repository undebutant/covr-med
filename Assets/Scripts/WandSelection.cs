using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using MiddleVR_Unity3D;

public class WandSelection : MonoBehaviour {

    [SerializeField]
    private string selectableObjectsLayerName = "selectionable";

    [SerializeField]
    private string buttonObjectsLayerName = "button";

    [SerializeField]
    MainMenuManager mainMenuManager;

    [SerializeField]
    string mainSceneName = "OR_Room";

    int selectableObjectsLayer;
    int buttonObjectsLayer;

    bool clicked = false;

    // Prefab of the local player
    GameObject prefabPlayer;

    /// <summary>
    ///     Finds the local prefab player
    /// </summary>
    /// <returns></returns>
    IEnumerator FindPrefabPlayer() {
        List<GameObject> playerPrefabs = new List<GameObject>();
        while (playerPrefabs.Count == 0) {
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject gameObject in gameObjects) {
                if (gameObject.GetComponent<NetworkAvatarSetup>() != null) {
                    playerPrefabs.Add(gameObject);
                }
            }
            // Do the operation over and over until the player prefabs are found
            if(playerPrefabs.Count == 0)
                yield return new WaitForSeconds(1);
        }
        // Find the right player prefab among the player prefabs
        foreach (GameObject playerPrefab in playerPrefabs) {
            if (playerPrefab.GetComponent<NetworkAvatarSetup>().isLocalPlayer)
                prefabPlayer = playerPrefab;
        }
        yield return null;
    }

    /// <summary>
    ///     Updates the prefab player's hand transform with the wand's transform
    /// </summary>
    /// <returns></returns>
    void UpdatePrefabPlayerHand() {
        if (prefabPlayer != null) {
            GameObject wand = GameObject.Find("HandNode");
            prefabPlayer.GetComponent<Hand>().SetHandTransform(wand.transform.position, wand.transform.rotation);
        }
    }

	// Use this for initialization
	void Start () {
        selectableObjectsLayer =  LayerMask.NameToLayer(selectableObjectsLayerName);
        buttonObjectsLayer = LayerMask.NameToLayer(buttonObjectsLayerName);
        if (SceneManager.GetActiveScene().name == mainSceneName) {
            StartCoroutine(FindPrefabPlayer());
        }
    }
	
	// Update is called once per frame
    //void Update () {
    //    // Update the prefab player hand's transform
    //    UpdatePrefabPlayerHand();

    //    Vector3 fwd = transform.TransformDirection(Vector3.forward);
    //    RaycastHit hit;
    //    if (MiddleVR.VRDeviceMgr != null) {
    //        // Getting state of primary wand button
    //        bool wandButtonPressed0 = MiddleVR.VRDeviceMgr.IsWandButtonPressed(0);

    //        // Getting toggled state of primary wand button
    //        // bool wandButtonToggled0 = MiddleVR.VRDeviceMgr.IsWandButtonToggled(0);

    //        if (Physics.Raycast(transform.position, fwd, out hit)) {
    //            if (hit.collider.gameObject.layer == selectableObjectsLayer) {
    //                // Set the color of the wand's ray
    //                GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);
    //                if (wandButtonPressed0) {
    //                    // Here, call the drag and drop

    //                }
    //            } else if (hit.collider.gameObject.layer == buttonObjectsLayer) {
    //                // Set the color of the wand's ray
    //                GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);
    //                // Click on the button only once
    //                if (wandButtonPressed0 && !clicked) {
    //                    Debug.Log("Button clicked");
    //                    clicked = true;
    //                    mainMenuManager.OnHitButton(hit.collider.gameObject);
    //                }
    //            }
    //        }
    //    }
    //}
}
