using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    public Camera viewer;
   
    public Camera PlateCam;
    public Light fridgeLight;
    public Rigidbody DoorFridge;
    private bool op=false;
    Camera c;

    // Use this for initialization
    void Start()
    {


    }
   

    // Update is called once per frame

    void Update()
    {      
        if (Input.GetMouseButtonDown(0))
        {
            var clickPosition = Input.mousePosition;            
            RaycastHit hit = new RaycastHit();
            if (viewer.enabled)
                c = viewer;
            else if (PlateCam.enabled)
                c = PlateCam;
            else c = Camera.main;
            Ray ray = c.ScreenPointToRay(clickPosition);

            Vector2 oldPosition = new Vector2(transform.localPosition.x, transform.localPosition.z);
            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.collider.gameObject.name == "fridge_01" || hit.collider.gameObject.name == "Door_fridge")
                {                  
                        if (!op)
                        {
                            Open();                           
                        }
                        else
                        {
                            Close();                           
                        }                   
                }          
            }
        }
    }
	void Open () {
        op = true;
        viewer.enabled = true;
        fridgeLight.intensity  = 0.7f;
        DoorFridge.rotation = Quaternion.Euler(new Vector3(-90, 0, -60));
        c = viewer;
       

    }
    void Close()
    {
        op = false;
        viewer.enabled = false;
        fridgeLight.intensity = 0;
        DoorFridge.rotation = Quaternion.Euler(new Vector3(-90, 0, 90));
        c = Camera.main;
    }
    
}
