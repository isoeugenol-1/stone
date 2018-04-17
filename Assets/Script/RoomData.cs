using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomData : MonoBehaviour {

	[HideInInspector]
	public string roomName = "";
	[HideInInspector]
	public int connectPlayer = 0;
	[HideInInspector]
	public int maxPlayers=0;

	public Text textRoomName;
	public Text textInfo;

	public void DispRoomData(){
		textRoomName.text = roomName;
		textInfo.text = "(" + connectPlayer.ToString () + "/" + maxPlayers.ToString () + ")";

	}
}
