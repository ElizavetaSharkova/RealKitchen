using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Click_on_books : MonoBehaviour
{
    public Button Recipe_Book;
    public Canvas Book;
    public Canvas Inventory;
    // Use this for initialization
    void Start()
    {
        Recipe_Book.onClick.AddListener(GoToScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickPosition = Input.mousePosition;
            RaycastHit hit = new RaycastHit();
            Camera c = Camera.main;
            Ray ray = c.ScreenPointToRay(clickPosition);

            Vector2 oldPosition = new Vector2(transform.localPosition.x, transform.localPosition.z);
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.gameObject.name == "book1" || hit.collider.gameObject.name == "book2" || hit.collider.gameObject.name == "book3" || hit.collider.gameObject.name == "book4" || hit.collider.gameObject.name == "book5" || hit.collider.gameObject.name == "Recipe_Book")
                {
                    GoToScene();
                }
            }
        }
    }
    public void GoToScene()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("In_book_scene");
        Book.enabled = true;
        Inventory.enabled = false;
        if (GameObject.Find("myBook").gameObject.GetComponent<Book>().leftOrRight == "")
        {
            GameObject.Find("myBook").GetComponent<Book>().UpdateSprites();
        }
        
    }
}
