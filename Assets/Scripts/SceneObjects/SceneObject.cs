using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneObject {
    private int id;
    private string objectName;
    private string description;
    public GameObject associatedObject;

    //--------------Getters and setters-------------

    public int Id {
        get {
            return this.id;
        }
        set {
            this.id = value;
        }
    }

    public string ObjectName {
        get {
            return this.objectName;
        }
        set {
            this.objectName = value;
        }
    }

    public string Description {
        get {
            return this.description;
        }
        set {
            this.description = value;
        }
    }

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="idParam">Id of the object</param>
    /// <param name="objectNameParam">Name of the object</param>
    /// <param name="descriptionParam">Small description of the object</param>
    
    public SceneObject(int idParam, string objectNameParam, string descriptionParam) {
        id = idParam;
        objectName = objectNameParam;
        description = descriptionParam;
    }
}
