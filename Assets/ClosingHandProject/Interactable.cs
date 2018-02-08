using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {
    [SerializeField] Material _material;
    GameObject _copyForOutline;

    void Start() {
        // TODO warning static
        // TODO warning rigidbody

        gameObject.tag = "Manipulable";
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        // create a child for outline
        _copyForOutline = Instantiate(gameObject);

        _copyForOutline.transform.SetParent(transform);
        _copyForOutline.transform.localPosition = Vector3.zero;
        _copyForOutline.transform.localRotation = Quaternion.identity;
        _copyForOutline.transform.localScale = Vector3.one;

        foreach (var component in _copyForOutline.GetComponents<Component>()) {
            if (!(component is Transform) && !(component is MeshFilter) && !(component is Renderer)) {
                Destroy(component);
            }
        }

        Material[] materialsForOutline = new Material[GetComponent<Renderer>().materials.Length];
        for (int i = 0; i < GetComponent<Renderer>().materials.Length; ++i) {
            materialsForOutline[i] = _material;
        }
        _copyForOutline.GetComponent<Renderer>().materials = materialsForOutline;

        if (GetComponent<Collider>() == null) {
            gameObject.AddComponent<BoxCollider>();
        }

        // add a rigidbody for interaction
        if (GetComponent<Rigidbody>() == null) {
            gameObject.AddComponent<Rigidbody>();
        }
        gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

        _copyForOutline.SetActive(false);
    }

    public void Outline(bool b) {
        _copyForOutline.SetActive(b);
    }
}
