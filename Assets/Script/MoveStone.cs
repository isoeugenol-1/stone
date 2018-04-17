using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
public class MoveStone : MonoBehaviour {

    private float moveSpeed = 2.0f;
    private float rotSpeed = 0.05f;

    private Rigidbody rbody;
    private Transform tr;
    private float h, v;
	private PhotonView pv = null;
	public Transform camPivot;

	private Vector3 currpos = Vector3.zero;
	private Quaternion currRot = Quaternion.identity;
	// Use this for initialization
	void Start () {

        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
		pv = GetComponent<PhotonView> ();
		pv.synchronization = ViewSynchronization.UnreliableOnChange;

		pv.ObservedComponents [0] = this;

		if (pv.isMine) {
			Camera.main.GetComponent<SmoothFollow> ().target = camPivot;


		} else {
			rbody.isKinematic = true;
		}
		currpos = tr.position;
		currRot = tr.rotation;
	}
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting) {
			stream.SendNext (tr.position);
			stream.SendNext (tr.rotation);

		} else {
			currpos = (Vector3)stream.ReceiveNext ();
			currRot = (Quaternion)stream.ReceiveNext ();
		}
	}
	// Update is called once per frame
	void Update () {
		if (pv.isMine) {
			
			h = Input.GetAxis ("Horizontal");
			v = Input.GetAxis ("Vertical");

            if(Input.GetKey(KeyCode.M))
            {
                Debug.Log("KEY 'M' IS DONW");
                tr.localScale = new Vector3(2,2,2);
            }


			if (Input.GetKey (KeyCode.UpArrow) == true) {
				tr.Translate (Vector3.forward * v * moveSpeed * Time.deltaTime);
				tr.Rotate (Vector3.up * rotSpeed * h * Time.deltaTime);
			}

			if (Input.GetKey (KeyCode.DownArrow) == true) {
				tr.Translate (-Vector3.forward * v * moveSpeed * Time.deltaTime);
				tr.Rotate (-Vector3.up * rotSpeed * h * Time.deltaTime);
			}

			if (Input.GetKey (KeyCode.LeftArrow) == true) {
				tr.Translate (Vector3.left * v * moveSpeed * Time.deltaTime);
				tr.Rotate (Vector3.left * rotSpeed * h * Time.deltaTime);
			}

			if (Input.GetKey (KeyCode.RightArrow) == true) {
				tr.Translate (Vector3.right * v * moveSpeed * Time.deltaTime);
				tr.Rotate (Vector3.right * rotSpeed * h * Time.deltaTime);
			}

		} else {
			tr.position = Vector3.Lerp (tr.position, currpos, Time.deltaTime * 3.0f);
			tr.rotation = Quaternion.Slerp (tr.rotation, currRot, Time.deltaTime * 3.0f);

		}

    }
}
