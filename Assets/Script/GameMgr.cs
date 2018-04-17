using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameMgr : Photon.MonoBehaviour {
	public Camera main;
	public Canvas Exit_Login;
	public Canvas Exit_Exit;
	public Canvas Exit_Chat;
	public Canvas Exit_Item;
	public Canvas Item_openCanvas;
    public Canvas AuctionCanvas;
	public Canvas infoCanvas;
	public Text txtConnect;
	public Text txtMsg;
	public List<string> chatList = new List<string> ();
	private PhotonView pv;
	public InputField inputTxt;
	public Text Money;
	public Canvas Game_AuctionCanvas;
	private bool exit;
	public int save = 0;
	public static int[] winner = new int[4];
	public Text Gold;
	public GameObject startButton;
	public Text fStoneT;
	// Use this for initialization

	//돌 정보 저장 구조체 
	public AuctionTime auctionTime;


	void Start () {
		pv = GetComponent<PhotonView> ();
        AuctionCanvas = GameObject.Find("AuctionCanvas").GetComponent<Canvas>();
		PhotonNetwork.isMessageQueueRunning = true;
		inputTxt.text = "";
		//GetConnectPlayerCount ();
		startButton.SetActive(false);
		Exit_Item.enabled = false;
		Item_openCanvas.enabled = false;
        for (int i = 0; i < 4; i++){
            winner[i] = 0;
        }
	}
	void Update(){
        if (Input.GetKeyDown (KeyCode.Return)) {
			SendButton ();
		}
	}
	public void Item_Close(){
		fStoneT.text = "합성";
		InvenImgClick.fStone = false;
		Exit_Item.enabled = false;
		Item_openCanvas.enabled = true;
		infoCanvas.enabled = false;
	}
	public void InfoClose(){
		fStoneT.text = "합성";
		InvenImgClick.fStone = false;
		infoCanvas.enabled = false;

	}
	public void Item_Open(){
		Exit_Item.enabled = true;
		Item_openCanvas.enabled = false;
		auctionTime.Item_print ();
	}
	public void Join(){
		if (PhotonNetwork.player.ID == 1) {
			startButton.SetActive(true);
		} else {
			startButton.SetActive(false);
		}


		Item_openCanvas.enabled = true;
		Game_AuctionCanvas.enabled = true;
		string msg = " CONNECT";
		txtMsg.text = string.Empty;
		pv.RPC ("SengMsg", PhotonTargets.All, msg);
		inputTxt.caretWidth = 50;
	}

	public void GetConnectPlayerCount(){
		PhotonNetwork.isMessageQueueRunning = true;
		Room currRoom = PhotonNetwork.room;
		txtConnect.text = currRoom.PlayerCount.ToString () + "/"+ currRoom.MaxPlayers.ToString();

	}
	void OnPhotonPlayerConnected(PhotonPlayer Player){
		GetConnectPlayerCount();
	}
	void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer){
		GetConnectPlayerCount ();
	}
	void OnClickExitRoom(){
		
		string msg = " DISCONNECT";
		pv.RPC ("SengMsg", PhotonTargets.All, msg);

		PhotonNetwork.LeaveRoom ();
	}
	void OnLeftRoom(){

		main.transform.position = new Vector3 (0.0f, 300.0f, 0.0f);
		main.transform.rotation = Quaternion.Euler (0, 0, 0);
		Exit_Login.enabled = true;
		Exit_Exit.enabled = false;
		Exit_Chat.enabled = false;
        AuctionCanvas.enabled = false;
	}

	[PunRPC]
	public void SengMsg(string msg, PhotonMessageInfo info){

		//해시테이블 사용해서 데이터 저장-각 플레이어의 골드 출력
		/*PhotonPlayer[] p = PhotonNetwork.playerList;
		foreach (PhotonPlayer _p in p) {
			ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable (){ {"Gold",Gold.text } };
			_p.SetCustomProperties (hash);
		}*/

		Addchat (string.Format ("{0} : {1}", info.sender.NickName, msg));
	}
	public void SendButton(){
		string currTxt = inputTxt.text;
		if (currTxt == "") {
			inputTxt.ActivateInputField ();
		} else {
			Send (PhotonTargets.All, currTxt);
			inputTxt.text = string.Empty;
		}
	}
	void Send(PhotonTargets target, string msg){
		pv.RPC ("SengMsg", target, msg);
	}
	public void Addchat(string msg){
		string chat = txtMsg.text;
		chat += string.Format ("\n{0}", msg);
		txtMsg.text = chat;
		chatList.Add (msg);
	}

	public void Print(){
		Text t1, t2, t3,t4;
		t1 = GameObject.Find ("t1").GetComponent<Text>();
		t2 = GameObject.Find ("t2").GetComponent<Text>();
		t3 = GameObject.Find ("t3").GetComponent<Text>();
		t4 = GameObject.Find ("t4").GetComponent<Text>();

		if (save <= int.Parse (t1.text)) {
			save = int.Parse (t1.text);
			winner[0] = 1;
		}
		if (save < int.Parse (t2.text)) {
			save = int.Parse (t2.text);
			winner[0] = 2;
		} else if (save == int.Parse (t2.text)) {
			winner [1] = 2;
		}

		if (save < int.Parse (t3.text)) {
			save = int.Parse (t3.text);
			winner[0] = 3;
			winner [1] = 0;
		}else if (save == int.Parse (t3.text)) {
			for (int i = 0; i < 4; i++) {
				if (winner [i] == 0) {
					winner [i] = 3;
					break;
				}
			}
		}

		if (save < int.Parse (t4.text)) {
			save = int.Parse (t4.text);
			winner[0] = 4;
			winner [1] = 0;
			winner [2] = 0;
		}else if (save == int.Parse (t4.text)) {
			for (int i = 0; i < 4; i++) {
				if (winner [i] == 0) {
					winner [i] = 4;
					break;
				}
			}
		}

		if ((string)PhotonNetwork.player.CustomProperties ["state"]=="start")
			asdf(winner,save);
		//string re = winner.ToString () + ";" + save.ToString ();
		//return string.Format("{0};{1}",winner,save);
	}

	//돌 커는 부분 시작
	[PunRPC]
	public void check(int[] t){
		Change_double (t);
	}
	public void changescale(int[] tt){
		change (PhotonTargets.All, tt);

	}
	void change(PhotonTargets target, int[] msg){
		pv.RPC ("check", target, msg);
	}
	public void Change_double(int[] id){
		/*if (GameObject.Find (id.ToString ()) != null) {
			GameObject ga = GameObject.Find (id.ToString ());
			ga.transform.localScale = new Vector3 (2, 2, 2);
		} else {
			GameObject ga = GameObject.FindGameObjectWithTag ("Stone");
			if (ga.GetComponent<PhotonView> ().owner.ID == id) {
				ga.transform.localScale = new Vector3 (2, 2, 2);
			}
		}*/
		//돌 체크 커 지 는 부 분
		GameObject[] ga = GameObject.FindGameObjectsWithTag ("Stone");
		//이부분은 수정안됨
		for (int i = 0; i < ga.Length; i++) {
			if (id [i] == ga [i].GetComponent<PhotonView> ().ownerId) {
				ga [i].transform.localScale = new Vector3 (2, 2, 2);
				ga [i].GetComponent<Rigidbody> ().mass = 200;
			}
		}
		//Rigidbody rb = GameObject.Find (id.ToString ()).GetComponent<Rigidbody> ();;
		//rb.mass = 200;
	}
	//돌 크 변경 하는 곳 끝

	//각자의 돈비교 동일 경우면 앞 플레이어가 이김
	void Test(PhotonTargets target, int[] win, int save){
		pv.RPC ("compareMoney", target,win,save);
	}
	public void asdf(int[] win, int save){
		Test (PhotonTargets.All,win,save);
	}
	[PunRPC]
	public void compareMoney(int[] win, int save){
		compute(win, save);
	}
	public void compute(int[] win, int save){
		int nowMoney = int.Parse (Money.text);
		int data = nowMoney - save;
		for (int i = 0; i < 4; i++) {
			if (GameObject.Find(win[i].ToString())!=null) {
				checkmoney2 (data, win[i]);
				//changescale (win);//돌크기 변경하는코드지만 필요없음
				SendWinner (win[i]);
			}
		}
		save = 0;
	}

	[PunRPC]
	public void checkmoney1(int m, int win, PhotonMessageInfo info){
		checkmoney4 (m,win);
	}
	public void checkmoney2(int m, int win){
		checkmoney3 (PhotonTargets.All ,m,win);
	}
	void checkmoney3(PhotonTargets target, int m, int win){
		pv.RPC ("checkmoney1", target, m,win);
	}
	public void checkmoney4(int m,int win){
		if (GameObject.Find (win.ToString ()) != null) {
			Money.text = m.ToString ();
		}

	}

	//승자 RPC통신
	[PunRPC]
	public void SendWin(int msg, PhotonMessageInfo info){
		Addwinner (msg);
	}
	public void SendWinner(int win){
		Sendww (PhotonTargets.All, win);
	}
	void Sendww(PhotonTargets target, int msg){
		pv.RPC ("SendWin", target, msg);
	}
	public void Addwinner(int msg){
		string chat = txtMsg.text;
		chat += string.Format ("\nwinner : {0}", msg);
		txtMsg.text = chat;
		chatList.Add (msg.ToString());
	}
	public void restart(){
		Game_AuctionCanvas.enabled = true;
	}
}
