using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

    public GameObject TimerObj;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void OnClick_Start()
    {
        TimerObj.SetActive(true);
    }

}
