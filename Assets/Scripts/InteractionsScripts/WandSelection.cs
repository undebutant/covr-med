﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// TODO use ifdef maybe ?
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

    bool isClicked = false;

    // Prefab of the local player
    GameObject prefabPlayer;

    // The remote position
    GameObject wand;

    // The hand script of the local player avatar prefab
    Hand avatarsHand;


    /// <summary>
    ///     Finds the local prefab player
    /// </summary>
    IEnumerator FindPrefabPlayer() {
        List<GameObject> playerPrefabs = new List<GameObject>();

        while (prefabPlayer == null) {
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject gameObject in gameObjects) {
                // Player prefab are the only one having a NetworkAvatarSetup script, hence they are the only one kept
                if (gameObject.GetComponent<NetworkAvatarSetup>() != null) {
                    // If this player prefab is local
                    if(gameObject.GetComponent<NetworkAvatarSetup>().isLocalPlayer) {
                        prefabPlayer = gameObject;
                    }
                }
            }

            // Do the operation over and over until the player prefabs are found
            if (prefabPlayer == null)
                yield return new WaitForSeconds(1);
        }
        avatarsHand = prefabPlayer.GetComponent<Hand>();

        yield return null;
    }


    /// <summary>
    ///     Updates the prefab player's hand transform with the wand's transform
    /// </summary>
    void UpdatePrefabPlayerHand() {
        if (prefabPlayer != null) {
            avatarsHand.SetHandTransform(wand.transform.position, wand.transform.rotation);
        }
    }

	// Use this for initialization
	void Start () {
        prefabPlayer = null;

        wand = GameObject.Find("HandNode");

        selectableObjectsLayer =  LayerMask.NameToLayer(selectableObjectsLayerName);
        buttonObjectsLayer = LayerMask.NameToLayer(buttonObjectsLayerName);

        // Looking for the local player prefab avatar
        if (SceneManager.GetActiveScene().name == mainSceneName) {
            StartCoroutine(FindPrefabPlayer());
        }
    }

    // TODO see TODO above, need workaround for non MiddleVR devices
    /*
    void Update () {
        // Update the prefab player hand's transform
        UpdatePrefabPlayerHand();

        Vector3 laserForward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (MiddleVR.VRDeviceMgr != null) {
            // Getting state of primary wand button
            bool isWandButtonPressed0 = MiddleVR.VRDeviceMgr.IsWandButtonPressed(0);

            // The laser forward raycast
            if (Physics.Raycast(transform.position, laserForward, out hit)) {
                if (hit.collider.gameObject.layer == selectableObjectsLayer) {
                    // Set the color of the wand's ray
                    GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);
                    if (isWandButtonPressed0) {
                        // TODO call the drag and drop
                    }
                } else if (hit.collider.gameObject.layer == buttonObjectsLayer) {
                    // Set the color of the wand's ray
                    GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);
                    // Click on the button only once
                    if (isWandButtonPressed0 && !isClicked) {
                        isClicked = true;
                        mainMenuManager.OnHitButton(hit.collider.gameObject);
                    }
                }
            }
        }

        if (!isWandButtonPressed0)
            isClicked = false;
    }
    */
}