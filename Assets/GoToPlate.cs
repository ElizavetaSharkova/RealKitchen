using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToPlate : MonoBehaviour {

    public Camera plateCamera;
    public Camera FridgeCam;
    public Camera tableCamera;
    public GameObject[] Plates;
     GameObject[] tablePlace;
    public Button backButton;
    public Button plateButton;
    public Image Clock;
    int temperature = 0;
 
    public int NumbPlate;
    public GameObject ObjInPlate;
    Camera c;
    ClockUI MyClock;
    // Use this for initialization
    void Start () {
        backButton.onClick.AddListener(BackOnClick);
        plateButton.onClick.AddListener(KnobOnClick);
        tablePlace = new GameObject[21];
        for (int i = 0; i < GameObject.Find("Table").transform.childCount; i++)
        {
            tablePlace[i] = GameObject.Find("Table").transform.GetChild(i).gameObject;
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickPosition = Input.mousePosition;
            RaycastHit hit = new RaycastHit();
            if (FridgeCam.enabled)
                c = FridgeCam;
            else if (plateCamera.enabled)
                c = plateCamera;
            else if (tableCamera.enabled)
                c = tableCamera;
            else c = Camera.main;
            Ray ray = c.ScreenPointToRay(clickPosition);

            Vector2 oldPosition = new Vector2(transform.localPosition.x, transform.localPosition.z);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "hotPlates")
                {
                   
                    plateCamera.enabled = true;
                    backButton.gameObject.active = true;
                    GameObject.Find("sink").gameObject.GetComponent<BoxCollider>().enabled = false;
                    c = plateCamera;
                    for (int i = 0; i< Plates.Length; i++)
                    {
                        Plates[i].active = true;
                    }
                    if (temperature != 0)
                    {
                        plateButton.gameObject.transform.parent.gameObject.active = true;
                        for (int i = 0; i < Plates.Length-4; i++)
                        {
                            Plates[i + 4].active = false;
                        }
                    }
                    hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;

                }
                if (hit.collider.gameObject.name == "Table")
                {
                    tableCamera.enabled = true;
                    backButton.gameObject.active = true;
                    c = tableCamera;
                    hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                    for (int i = 0; i< tablePlace.Length; i++)
                    tablePlace[i].GetComponent<BoxCollider>().enabled = true;
                    
                }
                if (hit.collider.gameObject.tag == "knob" && c == plateCamera)
                {
                    
                    Plates[0].transform.parent.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("hot"+ hit.collider.gameObject.name) as Material ;
                    plateButton.gameObject.transform.parent.gameObject.active = true;
                    plateButton.GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    temperature = 1;                   
                    if (hit.collider.gameObject.name.Contains(NumbPlate.ToString()))                    
                    {
                        GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Включить плиту", true);
                        GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Выключить плиту",false);
                        //при включении плиты действие "выключить плиту" снова не выполненно 
                        Clock.gameObject.active = true;
                        Clock.gameObject.GetComponent<ClockUI>().Start();
                        if (ObjInPlate.gameObject.transform.Find("Water").gameObject.active)
                        {
                            StartCoroutine(boilingWater());
                        }
                    }
                    for (int i = 0; i < Plates.Length-4; i++)
                    {
                        Plates[i+4].active = false;
                    }
                   
                }



            }
          }
        }
    void BackOnClick()
    {
        plateCamera.enabled = false;
        tableCamera.enabled = false;
        backButton.gameObject.active = false;
        plateButton.gameObject.transform.parent.gameObject.active = false;
        c = Camera.main;
        for (int i = 0; i < Plates.Length; i++)
        {
            Plates[i].active = false;
        }
        Plates[0].transform.parent.gameObject.GetComponent<BoxCollider>().enabled = true;
        for (int i = 0; i < tablePlace.Length; i++)
            tablePlace[i].GetComponent<BoxCollider>().enabled = false;
       
        GameObject.Find("sink").gameObject.GetComponent<BoxCollider>().enabled = true;
        tablePlace[0].transform.parent.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    void KnobOnClick()
    {
        plateButton.GetComponent<Rigidbody2D>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45* temperature));
        if (temperature == 7)
        {
            temperature = 0;
            plateButton.gameObject.transform.parent.gameObject.active = false;
            Plates[0].transform.parent.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("hotPlates") as Material;
            Clock.gameObject.active = false;
            ObjInPlate.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.active = false;
            for (int i = 0; i < Plates.Length-4; i++)
            {
                Plates[i + 4].active = true;
            }
            GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Выключить плиту", true);
        }
        else temperature ++ ;
    }
    IEnumerator boilingWater()
    {
        float boilingTime = 60 * 90 / (8 + temperature);
        boilingTime = boilingTime / (GameObject.Find("Clock").GetComponent<ClockUI>().speed);
        for (float t = 0; t < boilingTime; t++)
        {
            if (temperature != 0  && NumbPlate !=0)
            {
                boilingTime = 60 * 90 / ((8 + temperature) * (GameObject.Find("Clock").GetComponent<ClockUI>().speed));
                yield return new WaitForSeconds(1f);
            }
            else yield break;
        }      
        ObjInPlate.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.active = true;
        GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Довести до кипения", true);
        if (ObjInPlate.gameObject.transform.childCount > 1)
        {
            GameObject.Find("hotPlates").GetComponent<Cooking>().StartCook = true;
        }
    }


}
