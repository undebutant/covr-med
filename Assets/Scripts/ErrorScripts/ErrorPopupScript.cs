using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///     This script is used to display a popup with an error message
/// </summary>
public class ErrorPopupScript : MonoBehaviour {

    [SerializeField]
    GameObject popupGameObject;
    [SerializeField]
    Text errorText;

    bool isActiveStart = false;


	void Start () {
        // At the start we deactivate the popup box, unless a popup was asked during the awakening process
        popupGameObject.SetActive(isActiveStart);
    }


    // Function to call to display a popup with an error message
    public void NewPopup(string errorMessage) {
        // Activate the box
        popupGameObject.SetActive(true);
        // Prevent the Start function from deactivating the display when another script asked for a popup (when loading the main menu)
        isActiveStart = true;
        // Set the error message
        errorText.text = errorMessage;
 
        StartCoroutine(Popup());
    }


    // A coroutine that waits 5 seconds before deactivating the popup
    IEnumerator Popup() {
        yield return new WaitForSeconds(5);
        popupGameObject.SetActive(false);
    }
}
