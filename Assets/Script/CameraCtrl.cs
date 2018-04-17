using System.Collections;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

    private float h = 0.0f;
    private float v = 0.0f;

    private Transform tr;
    public float moveSpeed = 30.0f;

    public float xSpeed = 100.0f;
    public float ySpeed = 100.0f;


	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {            
        Debug.Log("h = " + h.ToString());
        Debug.Log("v = " + v.ToString());

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Quaternion rotation = Quaternion.Euler(v, h, 0);

        Vector3 movedir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(movedir * Time.deltaTime * moveSpeed, Space.Self);
        tr.Rotate(Vector3.up * Time.deltaTime * xSpeed * Input.GetAxis("Mouse X"));
        tr.Rotate(Vector3.left * Time.deltaTime * ySpeed * Input.GetAxis("Mouse Y"));
    }
}
