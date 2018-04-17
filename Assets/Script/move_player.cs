using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
public class move_player : MonoBehaviour {
	private PhotonView pv = null;

	public Transform camPivot;
	// Use this for initialization
	void Start () {
		pv = GetComponent<PhotonView> ();
		if (pv.isMine) {
			Camera.main.GetComponent<SmoothFollow> ().target = camPivot;
		}
	}
	
	// Update is called once per frame
	void Update () { 

	}

}

