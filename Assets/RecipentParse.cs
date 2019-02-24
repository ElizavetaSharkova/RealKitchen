using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;


public class Product
{
    private string name;
    public string getName()
    {
        return name;
    }
    private float count;
    public float getCount()
    {
        return count;
    }
    private string unit;
    public string getUnit()
    {
        return unit;
    }

    public Product(string name, float count, string unit)
    {
        this.name = name;
        this.count = count;
        this.unit = unit;
    }

}

public class RecipentParse : MonoBehaviour {

    public Text rightPage;
    public Text title;
    public Text crockeryText;
    public Text productText;   
    public List<Product> products;
    public List<string> actions;
    public List<bool> isAddedProducts;  // лист с теми же ключами что в продуктах и пометкой о добавленности этого продукта
    public List<bool> isCompletedActions; //лист с теми же ключами что в действиях и пометкой о выполненности этого действия
    public List<bool> isNecessaryProductOrAction; //лист с ключами сначала из продуктов потом из действий и пометкой об обязательности этого продукта или действия
    public int minPoint;
    public int point;
    public GameObject crockery;
    public Image cong;
    // Use this for initialization
    void Start () {

       

        // StartStatus(); //пока здесь, потом по нажатию кнопки

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ParseXML(int currentPage)
    {
        products = new List<Product>();
        actions = new List<string>();
        isAddedProducts = new List<bool>();
        isCompletedActions = new List<bool>();
        isNecessaryProductOrAction = new List<bool>();
        XmlDocument doc = new XmlDocument();
        doc.Load(@"Assets\GameData\RecipensBook.xml");       
        foreach (XmlNode node in doc.DocumentElement)
        {
            if (int.Parse(node.Attributes[0].Value) == currentPage)
            {                               
                title.text = node["TITLE"].InnerText;
                // считали название
                XmlNodeList nodeList = node["PRODUCTSLIST"].ChildNodes;
                foreach (XmlNode n in nodeList)
                {
                    products.Add(new Product( n.InnerText, float.Parse(n.Attributes["COUNT"].Value), n.Attributes["UNITS"].Value));
                    isNecessaryProductOrAction.Add(bool.Parse(n.Attributes["NECESSARY"].Value));                  
                }              
                // считали продукты
                crockery = GameObject.Find(node["CROCKERY"].InnerText).gameObject;               
                // считали посуду
                nodeList = node["ACTIONS"].ChildNodes;
                foreach (XmlNode n in nodeList)
                {
                    if (n.Attributes["TIME"] != null)
                    {
                        GameObject.Find("hotPlates").GetComponent<Cooking>().CookingTime.Add(float.Parse(n.Attributes["TIME"].Value));
                    }
                    if (n.Attributes["NAME"] != null)
                    {
                        GameObject.Find("hotPlates").GetComponent<Cooking>().Type.Add(n.Attributes["NAME"].Value);
                        actions.Add(n.Attributes["NAME"].Value + ": " + n.InnerText);
                    }
                    else actions.Add(n.InnerText);
                    isNecessaryProductOrAction.Add(bool.Parse(n.Attributes["NECESSARY"].Value));
                }  
                // считали действия                                            
               minPoint = int.Parse(node["MIN_POINT"].InnerText); 
               // считали очки                
            }
        }
        PrintRecipe();
    }
    public void PrintRecipe() // TODO: потом причесать
    {
        
        productText.text = "";
        gameObject.GetComponent<Text>().text = "Шаги:\n";
        crockeryText.text = "Посуда: "; 
        productText.transform.parent.GetComponent<Text>().text = "Ингредиенты: ";
        if(actions.Count == 0)
        {
            title.text = "Новый рецепт:";
            crockery = null;
        }
        else 
        {
            crockeryText.text += crockery.name.ToString();
        
            for (int i = 0; i < products.Count; i++)
            {
                
                productText.text += (i + 1).ToString() + ") " + products[i].getName() +" - " + products[i].getCount() + products[i].getUnit() + "\n";
            }
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].Contains(":"))
                {
                    gameObject.GetComponent<Text>().text += actions[i] + "\n";
                }
                else  gameObject.GetComponent<Text>().text += (i + 1).ToString() + ") " + actions[i] + "\n";

            }
        }
    }
    public void StartStatus()
    {
        for (int i = 0; i < products.Count; i++)
        {
            isAddedProducts.Add(false);
        }
        for (int i = 0; i < actions.Count; i++)
        {
            isCompletedActions.Add(false);
        }
        point = 0;
    }
    public void Scoring()
    {
        point = minPoint;
        for (int i = 0; i < isNecessaryProductOrAction.Count; i++)
        {
            if (i < isAddedProducts.Count)
            {
                if (isNecessaryProductOrAction[i] && !isAddedProducts[i])
                {
                    point = 0;
                    break;
                }
                else if (!isNecessaryProductOrAction[i] && isAddedProducts[i])
                {
                    point += 10;
                }
                isAddedProducts[i] = false;
            }
            else
            {
                if (isNecessaryProductOrAction[i] && !isCompletedActions[i- isAddedProducts.Count])
                {
                    point = 0;
                    break;
                }
                else if (!isNecessaryProductOrAction[i] && isCompletedActions[i- isAddedProducts.Count])
                {
                    point += 10;
                }
                isCompletedActions[i - isAddedProducts.Count] = false;
            }
            
        }
        StartCoroutine(Сongratulation());
        GameObject.Find("Score").GetComponent<ScoreParse>().WriteScore(point);
        GameObject.Find("CookLeft").active = true;
        GameObject.Find("CookRight").active = true;
        
    }

    public IEnumerator Сongratulation()
    {
        cong.gameObject.active = true;
        cong.gameObject.transform.GetChild(1).GetComponent<Text>().text = "Вы приготовили: " + title.text;
        cong.gameObject.transform.GetChild(2).GetComponent<Text>().text = "Вы получаете: " + point + " очков опыта";
        yield return new WaitForSeconds(6f);
        cong.gameObject.active = false;
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene");
    }

}

