using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour {

    //public float CookingTime = 3;
    public bool StartCook ;
    public List<string> Type; // = new string[] { "Всмятку", "В мешочек", "Вкрутую" };
    public List<float> CookingTime;
    string side;
    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        if (StartCook)
        {
            StartCoroutine(boil());
            StartCook = false;
        }
        side = GameObject.Find("myBook").GetComponent<Book>().leftOrRight;
    }
    IEnumerator boil()
    {
        
        GameObject OnPlate = GameObject.Find("hotPlates").GetComponent<GoToPlate>().ObjInPlate;
        for (int i = 0; i < CookingTime.Count; i++)
        {
            float k;
            CookingTime[i] = CookingTime[i] * 60 / GameObject.Find("Clock").GetComponent<ClockUI>().speed;
            if (i == 0)
                k = 0;
            else k = CookingTime[i - 1]; 
            for (float t = 0; t < (CookingTime[i]-k); t++)
            {
                if (OnPlate.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.active)
                {
                    yield return new WaitForSeconds(1f);
                }
                else yield break;
            }
            if (OnPlate.gameObject.transform.childCount >1 && OnPlate.gameObject.transform.GetChild(1).name.Contains("йцо"))
            {
                OnPlate.gameObject.transform.GetChild(1).name = "вареное " + Type[i].ToLower() + " яйцо";
                CookingTime[i] = CookingTime[i] * GameObject.Find("Clock").GetComponent<ClockUI>().speed / 60;
                ComplitedAction(Type[i].ToString() + ": в кипящей воде варить " + CookingTime[i] + " минут", true);
            }
            else if (OnPlate.gameObject.transform.GetChild(0).name.Contains("овсянка"))
            {
                OnPlate.gameObject.transform.GetChild(0).name = "овсяная каша";
                OnPlate.gameObject.transform.GetChild(0).tag = "end_product";
                GameObject.Find("Глубокая тарелка").tag = "тарелка";
                GameObject.Find("тарелка").tag = "You_can_take";
                OnPlate.gameObject.transform.GetChild(0).GetComponent<Renderer>().material = Resources.Load<Material>("Ovsyanka_material") as Material;
                ComplitedAction("Варить 5 минут", true);

            }
        }
        
    }
    public IEnumerator UnderCap(float time)
    {
        time = time*60/ GameObject.Find("Clock").GetComponent<ClockUI>().speed;
        for (float t = 0; t < time; t++)
        {
            if ((GameObject.Find("Крышка") != null && GameObject.Find("Крышка").transform.parent.gameObject.tag == "crockery") || (GameObject.Find("kitchen_03").GetComponent<TakeObject>().ObjInInventory != null && GameObject.Find("kitchen_03").GetComponent<TakeObject>().ObjInInventory.transform.Find("Крышка") != null))
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                GameObject.Find("hotPlates").GetComponent<GoToPlate>().Clock.gameObject.active = false;
                yield break;
            }
        }
        time = time * GameObject.Find("Clock").GetComponent<ClockUI>().speed / 60;
        ComplitedAction("Дать постоять "+ time + " минуты", true);

        yield return new WaitUntil (() => (GameObject.Find("kitchen_03").GetComponent<TakeObject>().ObjInInventory != null && GameObject.Find("kitchen_03").GetComponent<TakeObject>().ObjInInventory.name == "Крышка"));
        GameObject.Find("hotPlates").GetComponent<GoToPlate>().Clock.gameObject.active = false;
    }
    public void ComplitedAction(string action, bool tr)
    {
        if (side == "RecipeRightText")
        {
            int i = GameObject.Find("RecipeRightText").GetComponent<RecipentParse>().actions.IndexOf(action);
            if (i != -1)
            {
                GameObject.Find("RecipeRightText").GetComponent<RecipentParse>().isCompletedActions[i] = tr;
            }
        }
        else if (side == "RecipeLeftText")
        {
            int i = GameObject.Find("RecipeLeftText").GetComponent<RecipentParse>().actions.IndexOf(action);
            if (i != -1)
            {
                GameObject.Find("RecipeLeftText").GetComponent<RecipentParse>().isCompletedActions[i] = tr;
            }
        }
    }
    public void AddedProduct(string product)
    {
        int k= new int();
        if (side == "RecipeRightText")
        {
            for (int i = 0; i < GameObject.Find("RecipeRightText").GetComponent<RecipentParse>().products.Count; i++)
            {
                if (GameObject.Find("RecipeRightText").GetComponent<RecipentParse>().products[i].getName() == product)
                    k = i;               
            }
            if (k != null)
            {
                GameObject.Find(side).GetComponent<RecipentParse>().isAddedProducts[k] = true;
            }
        }
        else if (side == "RecipeLeftText")
        {
            for (int i = 0; i < GameObject.Find("RecipeLeftText").GetComponent<RecipentParse>().products.Count; i++)
            {
                if (GameObject.Find("RecipeLeftText").GetComponent<RecipentParse>().products[i].getName() == product)
                    k = i;
            }
            if (k != null)
            {
                GameObject.Find(side).GetComponent<RecipentParse>().isAddedProducts[k] = true;
            }
        }
    }
}
