using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : Photon.MonoBehaviour {
	private Transform tr;
	private Transform stone;
	private PhotonView pv;
	// Use this for initialization
	void Start () {
		pv = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Change_double(int id){
		GameObject ga = GameObject.Find (id.ToString());
		ga.transform.localScale = new Vector3 (2, 2, 2);

	}
	[PunRPC]
	public void change(int target){
		pv.RPC ("Change_double", PhotonTargets.All, target);
	}
	public void check(int t){
		change (t);
	}
}
