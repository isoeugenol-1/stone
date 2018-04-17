using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayUserId : Photon.MonoBehaviour {
	public Text userId;
	private PhotonView pv = null;
	// Use this for initialization
	void Start () {
		pv = GetComponent<PhotonView> ();
		userId.text = pv.owner.NickName;
	}
}
