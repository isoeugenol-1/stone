using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnAuctionEventMrg : Photon.MonoBehaviour {
	private PhotonView pv;
	// Use this for initialization
	public Quaternion Second = Quaternion.identity;
	public Quaternion Third = Quaternion.identity;
	public Quaternion Forth = Quaternion.identity;
	void Start () {
		pv = GetComponent<PhotonView> ();
		PhotonNetwork.isMessageQueueRunning = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick_StartBtn()
    {//스타트버튼누르면 상태를 시작으로 변경
		pv = GetComponent<PhotonView> ();

		PhotonNetwork.isMessageQueueRunning = true;
        //AEM.SetActive(true);
		PhotonNetwork.room.IsOpen = false;

		CreateStone1 (0);

		ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable (){ { "state","start" } };
		PhotonNetwork.player.SetCustomProperties (hash);
    }

	[PunRPC]
	public void CreateStone3(int i,PhotonMessageInfo info){
		CreateStone4 (i);
	}
	public void CreateStone1(int i){
		CreateStone2 (PhotonTargets.All,i);
	}
	void CreateStone2(PhotonTargets target,int i){
		pv.RPC ("CreateStone3", target,i);
	}
	public void CreateStone4(int id){
		//id =GetComponent<PhotonView>().ownerId;
		id=PhotonNetwork.player.ID;
		Second.eulerAngles = new Vector3(0, 180 ,0);
		Third.eulerAngles = new Vector3(0, 90, 0);
		Forth.eulerAngles = new Vector3(0, 270, 0);
		GameObject spawn;
		//1 : 28.2, 25, -92
		//2 : 27.52, 24.5, 1.5
		//3 : -18.5, 24.5, -44.35
		//4 : 73.64, 24.5, -44.35
		if (id == 1) {
			spawn = GameObject.Find ("spawn1");
			//spawn.transform.position; new Vector3 (28.18f, 24f, -90.25f)
			PhotonNetwork.Instantiate ("FracturedStone",spawn.transform.position, Quaternion.identity, 0).transform.name = "1";
		} else if (id ==2) {
			spawn = GameObject.Find ("spawn2");
			PhotonNetwork.Instantiate ("FracturedStone", spawn.transform.position,Second, 0).transform.name = "2";
		}
		else if (id ==3) {
			spawn = GameObject.Find ("spawn3");
			PhotonNetwork.Instantiate ("FracturedStone",spawn.transform.position,Second, 0).transform.name = "3";

		}
		else if (id ==4) {
			spawn = GameObject.Find ("spawn4");
			PhotonNetwork.Instantiate ("FracturedStone", spawn.transform.position,Second, 0).transform.name = "4";
		}
	}
}
