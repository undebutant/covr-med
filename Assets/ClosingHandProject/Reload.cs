using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {

    [SerializeField]
    KeyCode _reloadKey = KeyCode.KeypadEnter;

    void Update() {
        if (Input.GetKeyDown(_reloadKey)) {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
