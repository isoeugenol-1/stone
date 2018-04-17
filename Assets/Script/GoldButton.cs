using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldButton : Photon.MonoBehaviour {
	private PhotonView pv;
    public Text Gold;
	public Text Money;
	Text t1, t2, t3,t4;

	// Use this for initialization
	void Start () {
		pv = GetComponent<PhotonView> ();
		t1 = GameObject.Find ("t1").GetComponent<Text>();
		t2 = GameObject.Find ("t2").GetComponent<Text>();
		t3 = GameObject.Find ("t3").GetComponent<Text>();
		t4 = GameObject.Find ("t4").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		

	}

  public void OnClick_1()
    {
		
		int i;
		i = int.Parse(Gold.GetComponent<Text>().text);
		int save = int.Parse(Money.GetComponent<Text>().text);
		if (save >= i+1) {
			Gold.GetComponent<Text>().text = (i + 1).ToString();
			SendMoney (PhotonNetwork.player.ID, i + 1);
		}
		//Debug.Log (PhotonNetwork.player.ID.ToString());

    }
    public void OnClick_10()
    {
        int i;
        i = int.Parse(Gold.GetComponent<Text>().text);
        int save = int.Parse(Money.GetComponent<Text>().text);
        if (save >= i + 10)
        {
            Gold.GetComponent<Text>().text = (i + 10).ToString();
			SendMoney (PhotonNetwork.player.ID, i + 10);
        }

    }

    public void OnClick_100()
    {
        int i;
        i = int.Parse(Gold.GetComponent<Text>().text);
        int save = int.Parse(Money.GetComponent<Text>().text);
        if (save >= i + 100)
        {
            Gold.GetComponent<Text>().text = (i + 100).ToString();
			SendMoney (PhotonNetwork.player.ID, i + 100);
		}
    }
    public void OnClick_Cancle()
    {
        Gold.GetComponent<Text>().text = (0).ToString();
		SendMoney (PhotonNetwork.player.ID, 0);
    }

	//배팅금액을클릭하면 t1,t2,t3,t4에 맞게금액이저장된다.
	[PunRPC]
	public void SendMoneyRPC(int id,int money, PhotonMessageInfo info){
		upMoney (id,money);
	}
	public void SendMoney(int id,int money){
		SendM (PhotonTargets.All, id,money);
	}
	void SendM(PhotonTargets target, int id,int money){
		pv.RPC ("SendMoneyRPC", target, id,money);
	}
	public void upMoney(int id,int money){
		switch (id) {
		case 1:
			t1.text = money.ToString();
			break;
		case 2:
			t2.text = money.ToString();
			break;
		case 3:
			t3.text = money.ToString();
			break;
		case 4:
			t4.text = money.ToString();
			break;
		}
	}
}
