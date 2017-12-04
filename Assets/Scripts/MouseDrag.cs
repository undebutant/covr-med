using UnityEngine;
using System.Collections;

public class MouseDrag : MonoBehaviour
{
    public string placeName = "place";
    public string zoneName = "zone";
    public float scale = 1;
    public float closeDistance = 0.05f;
    public float moveSpeed = 40.0f;
    public float rotateSpeed = 90.0f;

    private float distance;

    private Color normalColor = new Color();
    private GameObject place;
    private GameObject zone;
    private GameObject hand;

    Color closeColor = new Color(0,1,0);

    Vector3 offset;
    Vector3 screenPoint;

    private Shader shaderNormal;
    private Shader shaderHighlighted;

    Collider objectCollider;
    Vector3 objectSize, objectMin, objectMax;
    Collider handCollider;
    Vector3 handSize, handMin, handMax;


    float zPosition;
    Vector3 cursorPosition;

    bool selected;

    int layerMask;

    private void Start()
    {
        selected = false;
        zone = GameObject.Find(zoneName);
        normalColor = zone.GetComponent<Renderer>().material.color;
        place = GameObject.Find(placeName);
        zone.transform.localScale += new Vector3(scale, 0, scale);
        hand = GameObject.Find("hand");
        shaderNormal = Shader.Find("VertexLit");
        shaderHighlighted = Shader.Find("Diffuse");

 
        objectCollider = GetComponent<Collider>();
     

        objectMin = objectCollider.bounds.min;
        objectMax = objectCollider.bounds.max;
        

        zPosition = transform.position.z;

        //Création du mask en prenant en compte tout sauf le layer de la main
        int layerMask = 1 << 8; //Le bit du layer 8 est à 1 et tous les autres à 0
        layerMask = ~layerMask; //On inverse et le mask
    }

    /* private void Update()
     {
         if ((objectMin.x <= hand.transform.position.x && objectMax.x >= hand.transform.position.x)
             && (objectMin.y <= hand.transform.position.y && objectMax.y >= hand.transform.position.y))
         {
             GetComponent<Renderer>().material.shader = shaderHighlighted;//selection effect
             if (Input.GetMouseButtonDown(0))
             {
                 selected = true;
             }
         }

         else
             GetComponent<Renderer>().material.shader = shaderNormal;//selection effect

         if (selected)
         {
             GetComponent<Renderer>().material.shader = shaderHighlighted;//selection effect
             transform.position = new Vector3(hand.transform.position.x, hand.transform.position.y, transform.position.z) ;
             Vector3 zonePosition = place.transform.position;

             distance = Vector3.Distance(zonePosition, transform.position);
             zone.GetComponent<Renderer>().material.color = (distance < closeDistance) ? closeColor : normalColor;
         }

         if(selected && Input.GetMouseButtonDown(0))
         {
             if (distance < closeDistance)
             {
                 transform.localPosition = Vector3.MoveTowards(place.transform.localPosition, Vector3.zero, Time.deltaTime * moveSpeed);
                 transform.localRotation = Quaternion.RotateTowards(place.transform.localRotation, Quaternion.identity, Time.deltaTime * rotateSpeed);
                 zone.GetComponent<Renderer>().material.color = normalColor;
                 selected = false;
             }
         }
     }

         */



    //Selection à la mainVirtuel
    private void Update() {
        Vector3 screenPointHand = Camera.main.WorldToScreenPoint(hand.transform.position);

        
        
    }


    //Selection à la souris
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }



    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        Vector3 zonePosition = place.transform.position;

        transform.position = curPosition;
     
        distance = Vector3.Distance(zonePosition, transform.position);
        zone.GetComponent<Renderer>().material.color = (distance < closeDistance) ? closeColor : normalColor;
    }



    void OnMouseUp()
    {
        if (distance < closeDistance)
        {
            transform.localPosition = Vector3.MoveTowards(place.transform.localPosition, Vector3.zero, Time.deltaTime * moveSpeed);
            transform.localRotation = Quaternion.RotateTowards(place.transform.localRotation, Quaternion.identity, Time.deltaTime * rotateSpeed);
            zone.GetComponent<Renderer>().material.color = normalColor;
        }
    }

}
