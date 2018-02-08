using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

    [SerializeField] KeyCode _key = KeyCode.Escape;

	void Update() {
        if (Input.GetKeyDown(_key)) {
            // Application.Quit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
	}
}
