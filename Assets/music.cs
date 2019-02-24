using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            //gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("bobby-mcferrin-don039t-worry-be-happy") as AudioClip;
            gameObject.GetComponent<AudioSource>().Play();

        }
        
        //for (float i = 0; i < gameObject.GetComponent<AudioSource>().clip.length; i++)
        //{
        //    gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("bobby-mcferrin-don039t-worry-be-happy") as AudioClip;
        //    gameObject.GetComponent<AudioSource>().Play();
        //}

    }
}
