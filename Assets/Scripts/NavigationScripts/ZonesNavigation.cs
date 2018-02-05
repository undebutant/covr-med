using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ZonesNavigation : NetworkBehaviour {

    GameObject prefabPlayer;

    void Start() {
        prefabPlayer = this.gameObject;
    }

    /// <summary>
    ///     Sets the new position of the avatar. Only horizontal translations are operated
    /// </summary>
    /// <param name="zone">The zone to head to</param>
    public void SetDestination(GameObject zone){
        if (isLocalPlayer) {
            prefabPlayer.transform.position = new Vector3(zone.transform.position.x, prefabPlayer.transform.position.y, zone.transform.position.z);
        }
    }
}
