using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Back_in_kitchen : MonoBehaviour {

    public Button btn_back;
    public Canvas Book;
    public Canvas Inventory;
    // Use this for initialization
    void Start() {
        btn_back.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update() {

        
    }
    public void OnClick()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Scene");
        Book.enabled = false;
        Inventory.enabled = true;
    }
}
