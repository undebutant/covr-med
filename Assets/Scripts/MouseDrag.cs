using UnityEngine;
using System.Collections;

/// <summary>
///     To be applied on the object that is dragged
/// </summary>

public class MouseDrag : MonoBehaviour {

    Vector3 offset;
    Vector3 screenPoint;

    /// <summary>
    ///     Moves the object depending on the camera
    /// </summary>
    void OnMouseDown() {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    /// <summary>
    ///     Drags the object with the mouse
    /// </summary>
    void OnMouseDrag() {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
}
