using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour {
    //TODO changer
    public Boolean controllerOn;

    public Camera avatarCamera;

    //Variables du raycast
    [SerializeField]
    int range=1000;
    int layerSelectionable;
    RaycastHit shootHit;

    //Variable du deplacement
    bool dragPossible;
    Vector3 offset;
    Vector3 objectScreenPoint;

    //Variables de snap
    GameObject zone;
    public float closeDistance = 1.0f;
    Color closeColor = new Color(0, 1, 0);
    private Color normalColor = new Color();


    // Use this for initialization
    void Start () {
        zone = GameObject.FindGameObjectWithTag("Zone");
        layerSelectionable = LayerMask.NameToLayer("selectionable");
        dragPossible = false;
        normalColor = zone.GetComponent<Renderer>().material.color;
    }
	

    void callRayCast(Vector3 handScreenPoint) {
        Ray ray = avatarCamera.ScreenPointToRay(avatarCamera.WorldToScreenPoint(transform.position));
        if (Physics.Raycast(ray, out shootHit, range)) {
            if (shootHit.collider.gameObject.layer == layerSelectionable) {
                dragPossible = true;
                objectScreenPoint = avatarCamera.WorldToScreenPoint(shootHit.collider.gameObject.transform.position);
                offset = shootHit.collider.gameObject.transform.position - avatarCamera.ScreenToWorldPoint(new Vector3(handScreenPoint.x, handScreenPoint.y, objectScreenPoint.z));
                shootHit.collider.gameObject.transform.rotation = Quaternion.identity;
            }
        }
    }

    void moveObject(Vector3 handScreenPoint) {
        if (dragPossible) {

            Vector3 curScreenPoint = new Vector3(handScreenPoint.x, handScreenPoint.y, objectScreenPoint.z);
            Vector3 curPosition = avatarCamera.ScreenToWorldPoint(curScreenPoint) + offset;
            Vector3 zonePosition = zone.transform.position;

            shootHit.collider.gameObject.transform.position = curPosition;

            float distance = Vector3.Distance(zonePosition, shootHit.collider.gameObject.transform.position);
            zone.GetComponent<Renderer>().material.color = (distance < closeDistance) ? closeColor : normalColor;

        }
    }

    void releaseObject() {
        if (dragPossible) {
            Vector3 zonePosition = zone.transform.position;
            float distance = Vector3.Distance(zonePosition, shootHit.collider.gameObject.transform.position);
            if (distance < closeDistance) {
                shootHit.collider.gameObject.transform.position = zone.transform.position + new Vector3(0, shootHit.collider.gameObject.transform.localScale.y/2.0f, 0);
                shootHit.collider.gameObject.transform.rotation = zone.transform.rotation;
                zone.GetComponent<Renderer>().material.color = normalColor;
            } else {
                zone.GetComponent<Renderer>().material.color = normalColor; //le bug fix avant le bug
            }
        }
        dragPossible = false;
    }

	// Update is called once per frame
	void Update () {
        Vector3 handScreenPoint = avatarCamera.WorldToScreenPoint(transform.position);
        if(controllerOn) {
            if (Input.GetButtonDown("Fire1")) {
                if(!dragPossible) {
                    callRayCast(handScreenPoint);
                } else {
                    releaseObject();
                }
            }

            moveObject(handScreenPoint);
        } else {
            if (Input.GetButtonDown("Fire1")) {
                callRayCast(handScreenPoint);
            }

            if (Input.GetButtonUp("Fire1")) {
                releaseObject();
            }

            moveObject(handScreenPoint);
        }
    } 
}
