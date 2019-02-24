using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TakeObject : MonoBehaviour {

    public Camera PlateCam;
    public Camera FridgeCam;
    public Camera tableCamera;
    public Text Tooltip;
    public Image Icon;
    public GameObject Steam;
    public Slider waterSlider;
    bool InventiryFull = false;
    public Button clearEgg;
    public GameObject ObjInInventory { get; set; }
    Camera c;
    RaycastHit hit;
    bool slader;
    Sprite UISprite;
    // Use this for initialization
    void Start () {
        clearEgg.onClick.AddListener(ClearEgg);
         slader = false;

    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickPosition = Input.mousePosition;
           
            if (FridgeCam.enabled)
                c = FridgeCam;
            else if (PlateCam.enabled)
                c = PlateCam;
            else if (tableCamera.enabled)
                c = tableCamera;
            else c = Camera.main;
            
            Ray ray = c.ScreenPointToRay(clickPosition);
            hit = new RaycastHit();
            Vector2 oldPosition = new Vector2(transform.localPosition.x, transform.localPosition.z);
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.gameObject.tag == "You_can_take" || hit.collider.gameObject.tag == "product" || hit.collider.gameObject.tag == "crockery" || hit.collider.gameObject.tag == "тарелка")
                {
                    if (!InventiryFull)
                    {
                        ObjInInventory = hit.collider.gameObject;
                        if (ObjInInventory.transform.parent.gameObject.tag == "table")
                        {
                            ObjInInventory.transform.parent = GameObject.Find("kitchen_creation_kit (main)").gameObject.transform;
                        }                       
                        Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + "_sprite") as Sprite;
                        if (ObjInInventory.name.Contains("вареное") && ObjInInventory.name.Contains("яйцо"))
                        {
                            Icon.sprite = Resources.Load<Sprite>("Яйцо_sprite") as Sprite;
                            clearEgg.gameObject.active = true;
                        }
                        Icon.enabled = true;
                        InventiryFull = true;
                        writeTooltip();

                        if (GameObject.Find("hotPlates").GetComponent<GoToPlate>().ObjInPlate == ObjInInventory)
                        {
                            GameObject.Find("hotPlates").GetComponent<GoToPlate>().ObjInPlate = null;
                            GameObject.Find("hotPlates").GetComponent<GoToPlate>().NumbPlate = 0;
                        }
                        if (hit.collider.gameObject.name == "Крышка")
                        {
                            hit.collider.gameObject.transform.parent = GameObject.Find("kitchen_creation_kit (main)").transform;
                        }
                        ObjInInventory.active = false;
                    }
                    else if (hit.collider.gameObject.tag == "crockery")
                    {
                        if (ObjInInventory.tag == "product")
                        {
                            //ObjInInventory.transform.parent = hit.collider.gameObject.transform;
                            GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Положить яйцо в ковшик", true);
                            GameObject.Find("hotPlates").GetComponent<Cooking>().AddedProduct(ObjInInventory.name);
                            clearInventory(new Vector3(0, 0.025f, 0));
                        }
                        else if (Tooltip.text.Contains("Стакан с"))
                        {
                            hit.collider.gameObject.transform.GetChild(0).gameObject.active = true;
                            if (Tooltip.text.Contains("водой"))
                            {
                                hit.collider.gameObject.transform.GetChild(0).gameObject.transform.position += new Vector3(0, -0.04f, 0);
                                GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Налить стакан воды в ковшик", true);
                                GameObject.Find("hotPlates").GetComponent<Cooking>().AddedProduct("Вода");
                            }
                            else if (Tooltip.text.Contains("молоком"))
                            {
                                hit.collider.gameObject.transform.GetChild(0).gameObject.name = "молоко";
                                hit.collider.gameObject.transform.GetChild(0).gameObject.transform.position += new Vector3(0, 0.02f, 0);
                                hit.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Milk_material") as Material;
                                GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Налить стакан молока в ковшик", true);
                                GameObject.Find("hotPlates").GetComponent<Cooking>().AddedProduct("Молоко");
                            }
                            else if (Tooltip.text.Contains("овсяными хлопьями"))
                            {
                                hit.collider.gameObject.transform.GetChild(0).gameObject.transform.position += new Vector3(0, 0.02f, 0);
                                hit.collider.gameObject.transform.GetChild(0).gameObject.name = "овсянка";
                                GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Добавить овсяные хлопья", true);
                                GameObject.Find("hotPlates").GetComponent<Cooking>().AddedProduct("Овсяные хлопья (среднего размера)");
                                if (hit.collider.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.active)
                                {
                                    GameObject.Find("hotPlates").GetComponent<Cooking>().StartCook = true;
                                }
                            }
                            ObjInInventory.active = true;
                            ObjInInventory.transform.GetChild(0).name = "Water";
                            ObjInInventory.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("back-foam_Diffuse") as Material;
                            ObjInInventory.transform.GetChild(0).gameObject.active = false;
                            writeTooltip();
                            Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + "_sprite") as Sprite;
                            ObjInInventory.active = false;


                        }
                        else if (ObjInInventory.name == "Молоко")
                        {
                            Steam = hit.collider.gameObject.transform.GetChild(1).gameObject;
                            waterSlider.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255);
                            //не забудь обратно вернусь это и цвет слайдера
                            Steam.active = true;
                            ObjInInventory.transform.position = hit.collider.gameObject.transform.position + new Vector3(0, 0.4f, 0.18f);                            
                            ObjInInventory.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                            ObjInInventory.active = true;
                            waterSlider.value = 0;
                            StartCoroutine(StartSlider(0.02f, 85, 100));
                            hit.collider.gameObject.transform.Find("Water").gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Milk_material") as Material;
                            hit.collider.gameObject.transform.Find("Water").gameObject.name = "молоко";
                        }
                        else if (ObjInInventory.name == "Овсяные хлопья")
                        {

                            waterSlider.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255);
                            UISprite = waterSlider.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
                            waterSlider.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Овсянка текстура") as Sprite;
                            //не забудь обратно вернусь это и цвет слайдера

                            ObjInInventory.transform.position = hit.collider.transform.position + new Vector3(-0.02f, 0.5f, 0f);
                            ObjInInventory.transform.rotation = Quaternion.Euler(114, -127, -142);
                            ObjInInventory.active = true;
                            waterSlider.value = 0;
                            slader = true;
                            StartCoroutine(StartSlider(0.03f, 55, 70));
                            hit.collider.gameObject.transform.Find("Water").gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Oves_material") as Material;
                            hit.collider.gameObject.transform.Find("Water").gameObject.name = "овес";
                        }
                        else if (ObjInInventory.name == "Соль")
                        {
                            Vector3 pos = hit.collider.gameObject.transform.position + new Vector3(0, 0.25f, 0.1f);
                            StartCoroutine(Salt(pos));
                        }
                        else if (ObjInInventory.name == "Столовая ложка")
                        {
                            Vector3 pos = hit.collider.gameObject.transform.position  + new Vector3(-0.02f, 0.125f, 0.05f);
                            StartCoroutine(Stir(pos));
                        }
                        else if (ObjInInventory.name == "Крышка")
                        {
                            ObjInInventory.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            ObjInInventory.transform.parent = hit.collider.gameObject.transform;
                            clearInventory(new Vector3(0,0.105f,0));
                            GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Накрыть крышкой", true);
                            GameObject.Find("hotPlates").GetComponent<GoToPlate>().Clock.gameObject.active = true;
                            //GameObject.Find("hotPlates").GetComponent<GoToPlate>().Clock.gameObject.GetComponent<ClockUI>().Start();
                            StartCoroutine(GameObject.Find("hotPlates").GetComponent<Cooking>().UnderCap(3));
                        }
                        else if (ObjInInventory.name == "Чайная ложка сахара")
                        {
                            ObjInInventory.name = "Чайная ложка";
                            writeTooltip();
                            Icon.sprite = Resources.Load<Sprite>("Чайная ложка_sprite") as Sprite;
                            GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Добавить сахар", true);
                            GameObject.Find("hotPlates").GetComponent<Cooking>().AddedProduct("Сахар");                          
                        }
                        else if (ObjInInventory.name == "Масло")
                        {
                            GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Добавить масло", true);
                            GameObject.Find("hotPlates").GetComponent<Cooking>().AddedProduct("Сливочное масто");
                            StartCoroutine(AddButter());
                        }
                    }
                    else if (hit.collider.gameObject.name == "Молоко" && ObjInInventory.name == "Молоко")
                    {
                        Steam.active = false;
                        ObjInInventory.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        ObjInInventory.active = false;
                        waterSlider.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(76, 158, 246);
                        //Steam = GameObject.Find("FX_Steam");

                    }
                    else if (hit.collider.gameObject.name == "Овсяные хлопья" && ObjInInventory.name == "Овсяные хлопья")
                    {
                        slader = false;
                        ObjInInventory.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        ObjInInventory.active = false;
                        waterSlider.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(76, 158, 246);
                        waterSlider.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = UISprite;
                        waterSlider.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Овсянка текстура") as Sprite;
                    }
                    else if (hit.collider.gameObject.tag == "тарелка" && (ObjInInventory.tag == "end_product" || ObjInInventory.transform.GetChild(0).tag == "end_product"))
                    {
                        // ObjInInventory.transform.parent = hit.collider.gameObject.transform;
                        if (ObjInInventory.transform.childCount > 0 && ObjInInventory.transform.GetChild(0).tag == "end_product")
                        {
                            ObjInInventory.transform.GetChild(0).gameObject.name = "Water";
                            ObjInInventory.transform.GetChild(0).gameObject.active = false;
                            Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + "_sprite") as Sprite;
                            writeTooltip();
                            hit.collider.gameObject.transform.GetChild(0).gameObject.active = true;

                        }
                        else clearInventory(new Vector3(0, 0.025f, 0));
                        if (GameObject.Find("myBook").GetComponent<Book>().leftOrRight == "RecipeRightText")
                        {
                            GameObject.Find("RecipeRightText").GetComponent<RecipentParse>().Scoring();
                        }
                        else GameObject.Find("RecipeLeftText").GetComponent<RecipentParse>().Scoring();
                    }


                }
                else if (hit.collider.gameObject.name == "Sugar" && ObjInInventory.name == "Чайная ложка")
                {
                    ObjInInventory.name = "Чайная ложка сахара";
                    writeTooltip();
                    Icon.sprite = Resources.Load<Sprite>("Ложка с сахаром_sprite") as Sprite;
                }
                else if (hit.collider.gameObject.name == "faucet")
                {
                    if (Steam.active)
                    {
                        Steam.active = false;
                        if (ObjInInventory.name == "Яйцо")
                        {
                            ObjInInventory.active = false;
                            Tooltip.text = ObjInInventory.name + " мытое";
                            GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Помыть яйцо", true);
                        }
                    }
                    else
                    {
                        Steam.active = true;
                        ObjInInventory.active = true;
                        if (ObjInInventory.tag == "crockery")
                        {
                            ObjInInventory.transform.position = Steam.transform.position + new Vector3(-0.0054f, -0.3f, 0);
                            
                                if (ObjInInventory.transform.GetChild(0).name == "Water" && ObjInInventory.transform.GetChild(0).gameObject.active)
                            {
                                
                                GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Подержать под холодной водой", true);
                            }
                            else if (ObjInInventory.name == "Стакан")
                            {
                                ObjInInventory.transform.position += new Vector3(0.13f, 0, 0.02f);
                                StartCoroutine(StartSlider(0.02f, 85, 100));
                            }
                                else  StartCoroutine(StartSlider(0.15f, 50, 75));
                        }
                        else if (Tooltip.text == "Яйцо")
                        {
                            ObjInInventory.transform.position = Steam.transform.position + new Vector3(-0.0054f, -0.2f, 0);
                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "plate" && ObjInInventory.tag == "crockery")
                {


                    switch (hit.collider.gameObject.name)
                    {
                        case "Plate1":
                            GameObject.Find("hotPlates").GetComponent<GoToPlate>().NumbPlate = 1;
                            break;
                        case "Plate2":
                            GameObject.Find("hotPlates").GetComponent<GoToPlate>().NumbPlate = 2;
                            break;
                        case "Plate3":
                            GameObject.Find("hotPlates").GetComponent<GoToPlate>().NumbPlate = 3;
                            break;
                        default:
                            GameObject.Find("hotPlates").GetComponent<GoToPlate>().NumbPlate = 4;
                            break;
                    }
                    GameObject.Find("hotPlates").GetComponent<GoToPlate>().ObjInPlate = ObjInInventory;
                    GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Поставить на плиту", true);
                    clearInventory(new Vector3(0, 0, 0));

                }
                else if (hit.collider.gameObject.tag == "table" && hit.collider.gameObject.transform.childCount==0 && (ObjInInventory.tag == "crockery"|| ObjInInventory.tag == "You_can_take" || ObjInInventory.tag == "тарелка"))
                {
                    ObjInInventory.transform.parent = hit.collider.gameObject.transform;
                    if (ObjInInventory.name.Contains("ложка"))
                    {
                        clearInventory(new Vector3(0.1f, 0.05f, 0));
                    }
                    else if (ObjInInventory.name.Contains("тарелка") || ObjInInventory.name == "Крышка")
                    {
                        clearInventory(new Vector3(0, 0.04f, 0));
                    }
                    else clearInventory(new Vector3(0, 0, 0));
                }
                else if (hit.collider.gameObject.name.Contains("sink")  && (Tooltip.text.Contains(ObjInInventory.name + " с водой") | Tooltip.text == ObjInInventory.name + " - набранно слишком много воды (попробуйте еще раз)"))
                {
                    ObjInInventory.transform.Find("Water").gameObject.active = false;
                    writeTooltip();
                    Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + "_sprite") as Sprite;
                    waterSlider.value = 0;
                    GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Слить воду", true);
                    ObjInInventory.active = false;
                }
                


            }
        }
    }
    public IEnumerator StartSlider(float time, int min, int max)
    {
        waterSlider.gameObject.active = true;
        for (int i = 0; i < waterSlider.maxValue; i++)
        {
            if (Steam.active || slader)
            {
                waterSlider.value++;
                yield return new WaitForSeconds(time);
            }
            else StartCoroutine(compareWater(min, max));
        }
        
        
    }
    IEnumerator compareWater(int min, int max)
    {
        if (waterSlider.value < min)
        {
           Tooltip.text = ObjInInventory.name + " - мало набранно";
            if (ObjInInventory.transform.Find("Water") != null)
                ObjInInventory.transform.Find("Water").gameObject.active = false;
        }
        else if (waterSlider.value >= min && waterSlider.value <= max)
        {
            waterSlider.gameObject.active = false;
            if (ObjInInventory.transform.Find("Water") != null)
            {
                ObjInInventory.transform.Find("Water").gameObject.active = true;
            }
            else GameObject.Find("Стакан").gameObject.transform.GetChild(0).gameObject.active = true;
            
            yield return new WaitForSeconds(1f);
            writeTooltip();
            
            yield return new WaitForSeconds(0.5f);
            ObjInInventory.active = false;
            if (ObjInInventory.name.Contains("Ковшик"))
            {
                GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Налить 0.5-0.75 холодной воды", true);
                GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Слить воду", false);
                //при набирании воды действие "Слить воду" снова не выполненно
            }
        }
        else if (waterSlider.value > max)
        {
            Tooltip.text = ObjInInventory.name + " - набранно слишком много (попробуйте еще раз)";
            if (ObjInInventory.transform.Find("Water") != null)
                ObjInInventory.transform.Find("Water").gameObject.active = true;
            Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + "_with_water_sprite") as Sprite;
            
            waterSlider.gameObject.active = false;          
        }
    }
    public IEnumerator Salt(Vector3 pos)
    {
        ObjInInventory.transform.position = pos;
        ObjInInventory.transform.rotation = Quaternion.Euler(140, 0, 0);
        ObjInInventory.active = true; 
        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(0.2f);
            ObjInInventory.transform.position += new Vector3(0, Convert.ToSingle(0.05*Math.Pow((-1),(i+1))), 0);           
        }      
        ObjInInventory.transform.rotation = Quaternion.Euler(-90, 0, 0);
        yield return new WaitForSeconds(0.5f);
        ObjInInventory.active = false;
        GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Посолить", true);
        GameObject.Find("hotPlates").GetComponent<Cooking>().AddedProduct("Соль");
    }

    public IEnumerator Stir(Vector3 pos)
    {
        ObjInInventory.transform.position = pos;       
        ObjInInventory.active = true;
        float time = 0.3f;
        for (int i = 0; i < 2; i++)
        {
            ObjInInventory.transform.rotation = Quaternion.Euler(-170, 0, 0);
            ObjInInventory.transform.position += new Vector3(0.02f, 0.02f, 0.05f);
            yield return new WaitForSeconds(time);
            ObjInInventory.transform.rotation = Quaternion.Euler(-160, 0, -20);
            ObjInInventory.transform.position += new Vector3(0.02f, -0.02f, -0.05f);
            yield return new WaitForSeconds(time);
            ObjInInventory.transform.rotation = Quaternion.Euler(-150, 0, 0);
            ObjInInventory.transform.position += new Vector3(-0.02f, -0.02f, -0.03f);
            yield return new WaitForSeconds(time);
            ObjInInventory.transform.rotation = Quaternion.Euler(-160, 0, 20);
            ObjInInventory.transform.position += new Vector3(-0.02f, 0.02f, 0.03f);
            yield return new WaitForSeconds(time);
        }
        ObjInInventory.transform.rotation = Quaternion.Euler(-170, 0, 0);
        ObjInInventory.transform.position += new Vector3(0.02f, 0.02f, 0.05f);
        yield return new WaitForSeconds(time);
        ObjInInventory.active = false;
        GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Постоянно помешивать", true);
        ObjInInventory.transform.rotation = Quaternion.Euler(-90, 180, -90);
    }
    public IEnumerator AddButter()
    {
        ObjInInventory.transform.position = hit.collider.gameObject.transform.position + new Vector3(-0.1f, 0.15f, 0);
        ObjInInventory.active = true;
        yield return new WaitForSeconds(0.5f);
        ObjInInventory.transform.GetChild(1).transform.position += new Vector3(0.05f,0,0);
        yield return new WaitForSeconds(0.08f);
        ObjInInventory.transform.GetChild(1).transform.position += new Vector3(0, -0.05f, 0);
        yield return new WaitForSeconds(0.08f);
        ObjInInventory.transform.GetChild(1).transform.position += new Vector3(0, -0.05f, 0);
        yield return new WaitForSeconds(0.08f);
        ObjInInventory.transform.GetChild(1).transform.position += new Vector3(0, -0.05f, 0);
        yield return new WaitForSeconds(0.7f);
        ObjInInventory.transform.GetChild(1).gameObject.active = false;
        ObjInInventory.active = false;
        ObjInInventory.transform.GetChild(1).transform.position += new Vector3(-0.05f, 0.15f, 0);
    }
    void clearInventory(Vector3 up)
    {
        ObjInInventory.transform.position = hit.collider.gameObject.transform.position + up;
        if (ObjInInventory.tag.Contains("product"))
        {
            ObjInInventory.transform.parent = hit.collider.gameObject.transform;
        }
        ObjInInventory.active = true;
        Tooltip.text = "";
        Icon.enabled = false;
        ObjInInventory = null;
        InventiryFull = false;
    }
    public void writeTooltip()
    {
        Tooltip.text = ObjInInventory.name;
        for (int i = 0; i < ObjInInventory.transform.childCount; i++)
        {
            if (ObjInInventory.transform.GetChild(i).gameObject.active)
            {
                if (ObjInInventory.transform.GetChild(i).name == "Water")
                {
                    Tooltip.text += " с водой";
                    Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + "_with_water_sprite") as Sprite;
                }

                else if (ObjInInventory.transform.GetChild(i).name == "молоко")
                {
                    Tooltip.text += " с молоком";
                    Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + " с молоком_sprite") as Sprite;
                }
                else if (ObjInInventory.transform.GetChild(i).name == "овес")
                {
                    Tooltip.text += " с овсяными хлопьями";
                    Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + " с овсяными хлопьями_sprite") as Sprite;
                }
                    if (ObjInInventory.transform.GetChild(i).tag == "product")
                {
                    Tooltip.text += " и " + ObjInInventory.transform.GetChild(i).name;
                }
                if (ObjInInventory.transform.GetChild(i).name == "Крышка")
                {
                    Tooltip.text += " с крышкой";
                    Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + " с крышкой_sprite") as Sprite;
                }
                if (ObjInInventory.transform.GetChild(i).name == "овсяная каша")
                {
                    Tooltip.text += " с овсяной кашей";
                    Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + "_с кашей_sprite") as Sprite;
                }

            }
        }
        
    }
    void ClearEgg()
    {
        ObjInInventory.name = "Очищенное яйцо";
        writeTooltip();
        ObjInInventory.GetComponent<Renderer>().material = Resources.Load<Material>("egg2") as Material;
        Icon.sprite = Resources.Load<Sprite>(ObjInInventory.name.ToString() + "_sprite") as Sprite;
        GameObject.Find("hotPlates").GetComponent<Cooking>().ComplitedAction("Очистить яйцо", true);
        clearEgg.gameObject.active = false;
        ObjInInventory.tag = "end_product";
        GameObject.Find("тарелка").tag = "тарелка";
        GameObject.Find("Глубокая тарелка").tag = "You_can_take";
    }
}

