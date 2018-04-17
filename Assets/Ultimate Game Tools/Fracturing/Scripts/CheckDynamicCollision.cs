using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// This component will enable fracturable objects with dynamic properties
/// </summary>
public class CheckDynamicCollision : Photon.MonoBehaviour
{
	public PhotonView pv;
	public GameObject juk;
	Text k1,k2,k3,k4;
    void Start()
    {
        // Enable fracturable object collider

        FracturedObject fracturedObject = GetComponent<FracturedObject>();
		pv = fracturedObject.GetComponent<PhotonView> ();
        // Disable chunk colliders
		k1= GameObject.Find ("k1").GetComponent<Text>();
		k2 = GameObject.Find ("k2").GetComponent<Text>();
		k3 = GameObject.Find ("k3").GetComponent<Text>();
		k4 = GameObject.Find ("k4").GetComponent<Text>();
        if(fracturedObject != null)
        {
            if(fracturedObject.GetComponent<Collider>() != null)
            {
                fracturedObject.GetComponent<Collider>().enabled = true;
            }
            else
            {
                Debug.LogWarning("Fracturable Object " + gameObject.name + " has a dynamic rigidbody but no collider. Object will not be able to collide.");
            }

            for(int i = 0; i < fracturedObject.ListFracturedChunks.Count; i++)
            {
                EnableObjectColliders(fracturedObject.ListFracturedChunks[i].gameObject, false);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.contacts == null)
        {
            return;
        }

        if(collision.contacts.Length == 0)
        {
            return;
        }

        // Was it a big enough hit?

        FracturedObject fracturedObject = gameObject.GetComponent<FracturedObject>();

        if(fracturedObject != null)
        {
            float fMass = collision.rigidbody != null ? collision.rigidbody.mass : Mathf.Infinity;
			//Debug.Log (collision.gameObject.tag.ToString ());충돌체의 tag탐색
			//Debug.Log(collision.gameObject.GetComponent<FracturedObject>().EventDetachMinMass.ToString());
            if(collision.relativeVelocity.magnitude > fracturedObject.EventDetachMinVelocity && fMass > fracturedObject.EventDetachMinVelocity)
            {
                // Disable fracturable object collider
				if (collision.rigidbody != null) {
					//SendKill (PhotonNetwork.player.ID);
					fracturedObject.EventDetachMinMass -= collision.rigidbody.mass;

				}
				if (fracturedObject.EventDetachMinMass < 0) {
					//Debug.Log (collision.gameObject.GetComponent<PhotonView> ().ownerId.ToString ());
					//부순사람 카운트 업
					SendKill (collision.gameObject.GetComponent<PhotonView> ().ownerId);
					//부셔진돌 캔버스 제거
					//DeleteName1 (fracturedObject,collision);
					//fracturedObject.transform.Find("Canvas").GetComponent<Canvas>().enabled = false;
					//Debug.Log (fracturedObject.name);
					fracturedObject.GetComponent<Collider> ().enabled = false;

					Rigidbody fracturableRigidbody = fracturedObject.GetComponent<Rigidbody> ();

					if (fracturableRigidbody != null) {
						fracturableRigidbody.isKinematic = true;
					}
					for(int i = 0; i < fracturedObject.ListFracturedChunks.Count; i++)
					{
						EnableObjectColliders(fracturedObject.ListFracturedChunks[i].gameObject, true);
					}

					// Explode

					fracturedObject.Explode(collision.contacts[0].point, collision.relativeVelocity.magnitude);
					//죽은돌 삭제
					GameObject delStone = GameObject.Find (fracturedObject.name);
					Destroy (delStone);
				}
                // Enable chunk colliders

                
            }
        }
    }

    private void EnableObjectColliders(GameObject chunk, bool bEnable)
    {
        List<Collider> chunkColliders = new List<Collider>();

        SearchForAllComponentsInHierarchy<Collider>(chunk, ref chunkColliders);

        for(int i = 0; i < chunkColliders.Count; ++i)
        {
            chunkColliders[i].enabled = bEnable;

            if(bEnable)
            {
                chunkColliders[i].isTrigger = false;
            }
        }
    }

    private static void SearchForAllComponentsInHierarchy<T>(GameObject current, ref List<T> listOut) where T : Component
    {
      T myComponent = current.GetComponent<T>();

      if (myComponent != null)
      {
        listOut.Add(myComponent);
      }

      for (int i = 0; i < current.transform.childCount; ++i)
      {
        SearchForAllComponentsInHierarchy(current.transform.GetChild(i).gameObject, ref listOut);
      }
    }
	/*
	[PunRPC]
	public void DeleteName3(FracturedObject f,Collision c, PhotonMessageInfo info){
		DeleteName4 (f,c);

	}
	public void DeleteName1(FracturedObject f,Collision c){
		DeleteName2 (PhotonTargets.All,f,c);
	}
	void DeleteName2(PhotonTargets target,FracturedObject f,Collision c){
		pv.RPC ("DeleteName3", target, f,c);
	}
	public void DeleteName4(FracturedObject f,Collision c){
		f.transform.Find("Canvas").GetComponent<Canvas>().enabled = false;
	}
	*/

	[PunRPC]
	public void SendKillRpc(int id, PhotonMessageInfo info){
		upKill (id);
		deleteIdCanvas (id);
	}
	public void SendKill(int id){
		SendK (PhotonTargets.All, id);
	}
	void SendK(PhotonTargets target, int id){
		pv.RPC ("SendKillRpc", target, id);
	}
	public void deleteIdCanvas(int id){
		GameObject[] fg = GameObject.FindGameObjectsWithTag ("Stone");
		for (int i = 0; i < fg.Length; i++) {
			if (fg [i].GetComponent<PhotonView> ().ownerId == id) {
				Destroy (fg [i].GetComponentInChildren<Canvas> ());
			}
		}
	}
	public void upKill(int id){
		int save = 0;
		switch (id) {
		case 1:
			save = int.Parse (k1.text)+1;
			k1.text = save.ToString();
			break;
		case 2:
			save = int.Parse (k2.text)+1;
			k2.text = save.ToString();
			break;
		case 3:
			save = int.Parse (k3.text)+1;
			k3.text = save.ToString();
			break;
		case 4:
			save = int.Parse (k4.text)+1;
			k4.text = save.ToString();
			break;
		}
	}

}
