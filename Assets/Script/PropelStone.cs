using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropelStone : MonoBehaviour
{
    public Rigidbody propel;

    float speed = 500.0f;
    bool isPropeled = false;
    int count = 0;
    // Use this for initialization
    void Start()
    {
        propel = GetComponent<Rigidbody>();
        //StartCoroutine("WaitAuction");
    }

    // Update is called once per frame
    void Update()
    {
        if (AuctionTime.propelTime <= 0 && isPropeled == false)
        {
            StartCoroutine("WaitAuction");
            isPropeled = true;
        }
    }

    IEnumerator WaitAuction()
    {
            StartCoroutine(MoveStone());
            yield return new WaitForSeconds(10.0f);
    }

    IEnumerator MoveStone()
    {
            propel.AddRelativeForce(Vector3.forward * propel.mass * speed);
            yield return new WaitForSeconds(0.1f);
    }

}