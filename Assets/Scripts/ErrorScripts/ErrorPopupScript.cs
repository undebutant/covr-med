using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This script is used for making a popup with a error message
/// </summary>
public class ErrorPopupScript : MonoBehaviour {

    [SerializeField]
    GameObject popupGameObject;
    [SerializeField]
    Text errorText;

    bool isActiveStart = false;

	// Use this for initialization
	void Start () {
        // At the start we desactivate the popup box, unless a popup have been ask during the awakenning process
        popupGameObject.SetActive(isActiveStart);

    }

    // Function to ask for a popup with a message
    public void NewPopup(string errorMessage) {
        // Activate the box
        popupGameObject.SetActive(true);
        // To prevent the Start function from deactivate the box whereas another script ask for a popup
        isActiveStart = true;
        // Set the error message
        errorText.text = errorMessage;
 
        StartCoroutine(Popup());
    }
    // A couroutine that wait 5 seconds before deactivate the popup
    IEnumerator Popup() {
        yield return new WaitForSeconds(5);
        popupGameObject.SetActive(false);
    }
}
