using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
///     Setup script used at the creation of the local player avatar.
///     Heritates from NetworkBehaviour
/// </summary>
public class NetworkAvatarSetup : NetworkBehaviour {

    [SerializeField]
    [Tooltip("The camera of the player avatar prefab, to set active on creation of the avatar")]
    GameObject playerCamera;


    void Start() {
        if(isLocalPlayer) {
            // Enabling the avatar's camera
            playerCamera.SetActive(true);

            // Disabling the global camera of the scene
            GameObject.FindWithTag("SceneCamera").SetActive(false);
        }
    }
}
