using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class AuctionTime : Photon.MonoBehaviour {
	public GameMgr gm;
    public GameObject AuctionItemImage;
    public Canvas AuctionCanvas;
	public Canvas PlayTime;
    public Text Money;
	public Text TimeText;
	private PhotonView pv;
	public Text Gold;
    public float timeCount;
	public Text txtMsg;
	public Text gameTimeText;
	public List<string> chatList = new List<string> ();
	private bool print = true;
	public Canvas Time_Auction;
	Quaternion Second = Quaternion.identity;
	Quaternion Third = Quaternion.identity;
	Quaternion Forth = Quaternion.identity;
	int rand;
	public static string gameState;
    bool flag = true;
    public static int propelTime = 100;
	public static int have_stone = 1;
	public Image item_image1;
	public Image item_image2;
	public Image item_image3;
	public Image item_image4;
	public Image item_image5;
	public static StoneInfo[] stoneinfo;
	public string gameStateTimer;
	public static StoneInfo[] playerStone = new StoneInfo[5];
	bool saveFlag = true;
	public GameObject startButton;

	[Serializable]
	public struct StoneInfo{
		public bool check;
		public int mass;
		public int Hp;
		public int scale;
		public string imageString;
		public string stoneTexture;
		public StoneInfo(bool check,int mass, int Hp, int scale,string imageString,string text){
			this.check = check;
			this.mass = mass;
			this.Hp = Hp;
			this.scale = scale;
			this.imageString = imageString;
			this.stoneTexture = text;
		}
	};

    // Use this for initialization
    void Start()
    {
		gameState = "";
		print = true;
		pv = GetComponent<PhotonView> ();
        timeCount = 10.0f;
        AuctionCanvas = GameObject.Find("AuctionCanvas").GetComponent<Canvas>();
		PhotonNetwork.isMessageQueueRunning = true;
		stoneinfo = new StoneInfo[] {
			new StoneInfo(true,3,10,34,"StoneImage/BaseStone","StoneMaterial/Base"),
			new StoneInfo(true,4,10,34,"StoneImage/BaseStone2","StoneMaterial/Base"),
			new StoneInfo(true,5,10,34,"StoneImage/RedStone","StoneMaterial/Red"),
			new StoneInfo(true,6,10,34,"StoneImage/CheckStone","StoneMaterial/Check"),
			new StoneInfo(true,7,10,34,"StoneImage/PurpleStone","StoneMaterial/Purple"),
			new StoneInfo(true,11,10,34,"StoneImage/SkyStone","StoneMaterial/Sky")
		};
		playerStone [0] = new StoneInfo (true,3, 10, 34,"StoneImage/BaseStone","StoneMaterial/Base");
    }


	public void Item_print(){
		for (int i = 0; i < have_stone; i++) {
			switch (i) {
			case 0:
				item_image1.sprite = Resources.Load<Sprite>(playerStone[0].imageString) as Sprite;
				break;
			case 1:
				item_image2.sprite = Resources.Load<Sprite>(playerStone[1].imageString) as Sprite;
				break;
			case 2:
				item_image3.sprite = Resources.Load<Sprite>(playerStone[2].imageString) as Sprite;
				break;
			case 3:
				item_image4.sprite = Resources.Load<Sprite>(playerStone[3].imageString) as Sprite;
				break;
			case 4:
				item_image5.sprite = Resources.Load<Sprite>(playerStone[4].imageString) as Sprite;
				break;
			}
		}
	}
    // Update is called once per frame
    void Update()
	{
		if ((string)PhotonNetwork.player.CustomProperties ["state"] == "start") {
			gameStateTimer = "auction";
			startButton.SetActive (false);
			int random = UnityEngine.Random.Range (1, 7);
			if (timeCount != 0) {
				timeCount -= Time.deltaTime;

				if (timeCount <= 0) {
					timeCount = 0;
					if (print == true) {
						//endTime ();
						print = false;
						gm.Print ();
						SendItem (rand, GameMgr.winner);
						timeCount = 5;
						gameStateTimer = "ready";
						ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable (){ { "state","ready" } };
						PhotonNetwork.player.SetCustomProperties (hash);
					}
					//********나중에 쓸거임*********
					//AuctionCanvas.enabled = false;
					//******************************
					int t = Mathf.FloorToInt (timeCount);
					printTime (t, random, gameStateTimer);
				} else {
					int t = Mathf.FloorToInt (timeCount);
					printTime (t, random, gameStateTimer);
				}

			}
			//0초 되면 함수 실행

		}else if ((string)PhotonNetwork.player.CustomProperties ["state"] == "ready") {
			PlayTime.enabled = true;
			print = true;
			if (timeCount != 0) {
				timeCount -= Time.deltaTime;

				if (timeCount <= 0) {
					timeCount = 0;
					if (print == true) {
						//endTime ();
						gameStateTimer = "game";
						print = false;
						//game state change
						timeCount = 10;
						ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable (){ { "state","game" } };
						PhotonNetwork.player.SetCustomProperties (hash);
					}
					//********나중에 쓸거임*********
					//AuctionCanvas.enabled = false;
					//******************************
					int t = Mathf.FloorToInt (timeCount);
					printgameTime (t, gameStateTimer);
				} else {
					int t = Mathf.FloorToInt (timeCount);
					printgameTime (t, gameStateTimer);
				}
			}
		}  else if ((string)PhotonNetwork.player.CustomProperties ["state"] == "game") {
			PlayTime.enabled = true;
			print = true;
			if (timeCount != 0) {
				timeCount -= Time.deltaTime;

				if (timeCount <= 0) {
					timeCount = 0;
					if (print == true) {
						//endTime ();
						gameStateTimer = "end";
						print = false;
						//game state change
						ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable (){ { "state","end" } };
						PhotonNetwork.player.SetCustomProperties (hash);
					}
					//********나중에 쓸거임*********
					//AuctionCanvas.enabled = false;
					//******************************
					int t = Mathf.FloorToInt (timeCount);
					printgameTime (t, gameStateTimer);
				} else {
					int t = Mathf.FloorToInt (timeCount);
					printgameTime (t, gameStateTimer);
				}
			}
		} else if ((string)PhotonNetwork.player.CustomProperties ["state"] == "end")  {
			GameObject[] stone = GameObject.FindGameObjectsWithTag ("Stone");
			for (int i = 0; i < stone.Length; i++) {
				Destroy (stone [i]);
			}
		}
	}
    


	//승자 인벤토리에 아이템저장

	[PunRPC]
	public void saveIn(int ran,int[] winner, PhotonMessageInfo info){
		AuctionCanvas.enabled = false;
		saveItem (ran,winner);
	}
	public void SendItem(int ran,int[] winner){
		SendIt (PhotonTargets.All,ran,winner);
	}
	void SendIt(PhotonTargets target, int ran,int[] winner){
		pv.RPC ("saveIn", target, ran, winner);
	}

	public void saveItem(int ran,int[] winner){
		//int[] winner = GameMgr.winner;
		for (int i = 0; i < 4; i++) {
			//Debug.Log (winner [i].ToString ());
			if (PhotonNetwork.player.ID == winner[i]) {
				playerStone [have_stone] = stoneinfo [ran - 1];
				have_stone++;
			}
		}
		Item_print ();
		//경매끝나면 건돈초기화
		Gold.text = "0";

	}
	//game timer
	[PunRPC]
	public void sendgameTime(int msg,string state, PhotonMessageInfo info){
		changegameTime (msg,state);
	}
	public void printgameTime(int ta,string state){
		Sendgame (PhotonTargets.All, ta,state);
	}
	void Sendgame(PhotonTargets target, int msg,string state){
		pv.RPC ("sendgameTime", target, msg,state);
	}
	public void changegameTime(int msg,string state){
		if (state == "game") {
			PlayTime.enabled = true;
			GameObject[] fence = GameObject.FindGameObjectsWithTag ("Fence");
			for (int i = 0; i < fence.Length; i++) {
				Destroy (fence [i]);
			}
			propelTime = 0;
			AuctionTime.gameState = "game";
		}
		if (state == "end") {
			propelTime = 1;
			PlayTime.enabled = false;
			AuctionCanvas.enabled = true;
			print = true;
			Debug.Log (PhotonNetwork.player.ID.ToString());
			Second.eulerAngles = new Vector3(0, 180 ,0);
			Third.eulerAngles = new Vector3(0, 90, 0);
			Forth.eulerAngles = new Vector3(0, 270, 0);
			GameObject spawn;
			//1 : 28.2, 25, -92
			//2 : 27.52, 24.5, 1.5
			//3 : -18.5, 24.5, -44.35
			//4 : 73.64, 24.5, -44.35
			if (PhotonNetwork.player.ID == 1) {
				spawn = GameObject.Find ("spawn1");
				//spawn.transform.position; new Vector3 (28.18f, 24f, -90.25f)
				PhotonNetwork.Instantiate ("FracturedStone",spawn.transform.position, Quaternion.identity, 0).transform.name = "1";
			} else if (PhotonNetwork.player.ID ==2) {
				spawn = GameObject.Find ("spawn2");
				PhotonNetwork.Instantiate ("FracturedStone", spawn.transform.position,Second, 0).transform.name = "2";
			}
			else if (PhotonNetwork.player.ID ==3) {
				spawn = GameObject.Find ("spawn3");
				PhotonNetwork.Instantiate ("FracturedStone",spawn.transform.position,Second, 0).transform.name = "3";

			}
			else if (PhotonNetwork.player.ID ==4) {
				spawn = GameObject.Find ("spawn4");
				PhotonNetwork.Instantiate ("FracturedStone", spawn.transform.position,Second, 0).transform.name = "4";
			}
			flag = true;
			timeCount = 10;
			ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable (){ { "state","start" } };
			PhotonNetwork.player.SetCustomProperties (hash);
		}
		gameTimeText.text = msg.ToString ();
	}

	//auction timer
	[PunRPC]
	public void sendTime(int msg,int ran,string state, PhotonMessageInfo info){
		changeTime (msg,state);
        RandomAuction(ran); 
	}
	public void printTime(int ta,int ran,string state){
		Send (PhotonTargets.All, ta,ran,state);
	}
	void Send(PhotonTargets target, int msg,int ran,string state){
		pv.RPC ("sendTime", target, msg,ran,state);
	}
	public void changeTime(int msg,string state){

		TimeText.text = msg.ToString ();
        //propelTime = msg;

	}

    public void RandomAuction(int ran)
    {
        
        if(flag == true)
        { 
			gameState = "Auction";
			rand = ran;
			switch (rand)
            {
                case 1:
                    AuctionItemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("StoneImage/BaseStone") as Sprite;
                    break;
                case 2:
                    AuctionItemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("StoneImage/BaseStone2") as Sprite;
                    break;

                case 3:
                    AuctionItemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("StoneImage/RedStone") as Sprite;
                    break;

                case 4:
                    AuctionItemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("StoneImage/CheckStone") as Sprite;
                    break;

                case 5:
                    AuctionItemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("StoneImage/PurpleStone") as Sprite;
                    break;

                case 6:
                    AuctionItemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("StoneImage/SkyStone") as Sprite;
                    break;

            }
            flag = false;

    }

    }

}
