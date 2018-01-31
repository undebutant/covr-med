using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    [SerializeField]
    Hand handScript;

    // Stock the height of the last tissue encountered in collision
    float lastTissueY;

    public float GetLastTissueY() {
        return lastTissueY;
    }

    // Variable to know if we are in the tissue
    bool isContactTissue = false;

    public bool GetIsContactTissue() {
        return isContactTissue;
    }

    public void InTissue() {
        isContactTissue = true;
    }

    public void LeaveTissue() {
        isContactTissue = false;
    }

    // Stock the height of the last table entered in collision
    float lastTableY;

    public float GetLastTableY() {
        return lastTableY;
    }

    // Variable to know if we are in the table
    bool isContactTable = false;

    public bool GetIsContactTable() {
        return isContactTable;
    }

    public void ContactTable() {
        isContactTable = true;
    }

    public void LeaveTable() {
        isContactTable = false;
    }

    // The int value of the layer masks
    int layerSelectable;
    int layerPatient;
    int layerTable;


    void Start() {
        // Initiate the value of the layers
        layerSelectable = LayerMask.NameToLayer("Selectable");
        layerPatient = LayerMask.NameToLayer("Patient");
        layerTable = LayerMask.NameToLayer("Table");

        isContactTissue = false;
        isContactTable = false;

        lastTableY = 0;
        lastTissueY = 0;
    }


    private void OnTriggerEnter(Collider other) {
        // If the object that collides with the hand is a selectable object...
        if (other.gameObject.layer == layerSelectable) {
            // ... tell the handScript that he can grab this object
            handScript.ObjectToSelect = other.gameObject;
        }

        // If the object that collides with the hand is a patient object
        if (other.gameObject.layer == layerPatient) {
            InTissue();
            lastTissueY = other.bounds.max.y;
        }

        // If the object that collides with the hand is a table object
        if (other.gameObject.layer == layerTable) {
            ContactTable();
            lastTableY = other.bounds.max.y;
        }
    }


    private void OnTriggerExit(Collider other) {
        // If the object in the selectable layer is no longer colliding with the hand...
        if (other.gameObject.layer == layerSelectable)
        {
            // ... tell the handscript that he can no longer grab it
            handScript.ObjectToSelect = null;
        }

        // If the object in the patient layer is no longer colliding with the hand
        if (other.gameObject.layer == layerPatient) {
            LeaveTissue();
        }

        // If the object in the table layer is no longer colliding with the hand
        if (other.gameObject.layer == layerTable) {
            LeaveTable();
        }
    }
}
