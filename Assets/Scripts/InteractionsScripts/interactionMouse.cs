using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionMouse : MonoBehaviour {

    [SerializeField]
    MainMenuManager mainMenuManager;


	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1")) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f)) {
                Debug.Log("You clicked the " + hit.transform.name);
                mainMenuManager.OnHitButton(hit.collider.gameObject);
            }
        }
    }
}
