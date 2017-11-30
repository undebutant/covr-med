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

    [SerializeField]
    [Tooltip("The collider of the player avatar prefab, to set active on creation of the avatar")]
    Collider playerCollider;

    [SerializeField]
    [Tooltip("The input manager of the player avatar prefab, to set active on creation of the avatar")]
    InputManager playerInputManager;


    void Start() {
        if(isLocalPlayer) {
            // Enabling the avatar's camera
            playerCamera.SetActive(true);

            // Enabling the player's collider
            playerCollider.enabled = true;

            // Enabling the player's input manager
            playerInputManager.enabled = true;


            // Disabling the global camera of the scene
            GameObject.FindWithTag("SceneCamera").SetActive(false);
        }
    }
}
