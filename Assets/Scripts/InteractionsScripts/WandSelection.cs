using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// TODO use ifdef maybe ?
//using MiddleVR_Unity3D;


public class WandSelection : MonoBehaviour {

    [SerializeField]
    private string selectableObjectsLayerName = "Selectable";

    [SerializeField]
    private string buttonObjectsLayerName = "Button";

    [SerializeField]
    MainMenuManager mainMenuManager;

    [SerializeField]
    string mainSceneName = "OR_Room";

    GameObject systemCenterNode;

    int selectableObjectsLayer;
    int buttonObjectsLayer;

    bool isClicked = false;

    bool isHoveringSelectableObject = false;

    bool isObjectSelected = false;
    GameObject selectedObject;

    // Prefab of the local player
    public GameObject prefabPlayer;

    // The remote position
    GameObject wand;

    // The hand script of the local player avatar prefab
    Hand avatarsHand;

    // Object drag's script of the prefab player
    ObjectDrag objectDrag;

    SoundManager soundManager;
    ZonesNavigation zonesNavigation;


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
        objectDrag = prefabPlayer.GetComponentInChildren<ObjectDrag>();
        zonesNavigation = prefabPlayer.GetComponent<ZonesNavigation>();

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
        selectedObject = null;

        // Looking for the local player prefab avatar
        if (SceneManager.GetActiveScene().name == mainSceneName) {
            StartCoroutine(FindPrefabPlayer());
        }

        soundManager = GameObject.FindObjectOfType<SoundManager>();

        // Initialize system center node
        systemCenterNode = GameObject.Find("VRManager").GetComponent<VRManagerScript>().VRSystemCenterNode;
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
            if (Physics.Raycast(transform.position, laserForward , out hit)) {

                // Select an object to drag and drop
                if (hit.collider.gameObject.layer == selectableObjectsLayer) {
                    // Set the color of the wand's ray
                    GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);

                    // Plays the hover sound 
                    if(!isHoveringSelectableObject)
                        soundManager.PlayHoverSound(new Vector3(0, 0, 0));

                    isHoveringSelectableObject = true;

                    if (isWandButtonPressed0 && !isClicked) {
                        isClicked = true;
                        isObjectSelected = true;
                        selectedObject = hit.collider.gameObject;
                        objectDrag.SelectObject(wand, selectedObject, Vector3.Distance(wand.transform.position, selectedObject.transform.position));

                        // Playing the selection sound effect
                        soundManager.PlaySelectionSound(hit.collider.gameObject.transform.position);
                    }

                    // Click on a menu's button
                } else if (hit.collider.gameObject.layer == buttonObjectsLayer) {
                    // Set the color of the wand's ray
                    GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);
                    // Click on the button only once
                    if (isWandButtonPressed0 && !isClicked) {
                        isClicked = true;
                        mainMenuManager.OnHitButton(hit.collider.gameObject);
                    }

                    // Release object
                } else if (isWandButtonPressed0 && !isClicked && isObjectSelected) {
                        isClicked = true;
                        isObjectSelected = false;
                        selectedObject = null;
                        objectDrag.ReleaseObject();

                        // Playing the selection sound effect
                        soundManager.PlayDropSound(hit.collider.gameObject.transform.position);
                }

                // Set isHoveringSelectableObject to false if not hovering
                if (hit.collider.gameObject.layer != selectableObjectsLayer && !isObjectSelected)
                    isHoveringSelectableObject = false;

                // If a navigation zone was selected
                if (hit.collider.gameObject.tag == "NavigationZone") {
                    GameObject zone = hit.collider.gameObject;
                    if (isWandButtonPressed0 && !isClicked)
                        systemCenterNode.transform.position = new Vector3(zone.transform.position.x, systemCenterNode.transform.position.y, zone.transform.position.z);
                }
            }
            if (!isWandButtonPressed0)
                isClicked = false;
        }
    }
    */
}
