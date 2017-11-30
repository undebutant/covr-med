using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Selectable : SceneObject {

    bool isSelected;

    //------Getters/Setters----------
    public bool IsSelected {
        get {
            return this.isSelected;
        }
        set {
            this.isSelected = value;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Selectable(int id, string name, string description) : base(id, name, description) { }
}
