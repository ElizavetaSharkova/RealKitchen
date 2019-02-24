using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScoreParse : MonoBehaviour {

    public int point;
    // Use this for initialization
    void Start() {
        ReadScore();
    }

    // Update is called once per frame
    void Update() {
        gameObject.GetComponent<Text>().text = "Очки опыта: " + point;
    }
   public void WriteScore(int Cookpoint)
    {
        point += Cookpoint;
        using (StreamWriter file = new StreamWriter(@".\Assets\GameData\Point.txt", false))
        {
            file.WriteLine((point).ToString());
        }
    }
    void ReadScore()
    {
        using (StreamReader file = new StreamReader(@".\Assets\GameData\Point.txt"))
        {
            point = int.Parse(file.ReadLine());
        }           
    }
}
