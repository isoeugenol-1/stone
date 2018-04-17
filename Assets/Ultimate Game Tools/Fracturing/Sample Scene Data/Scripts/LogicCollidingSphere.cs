using UnityEngine;
using System.Collections;

public class LogicCollidingSphere : MonoBehaviour
{
    public Rigidbody ObjectToDrop = null;

    bool bDropped = false;
    int  nChunksDetached  = 0;
    int  nChunkCollisions = 0;

    void Start()
    {
        nChunksDetached  = 0;
        nChunkCollisions = 0;
    }

    void Update()
    {
        if(bDropped && ObjectToDrop.isKinematic == true)
        {
            ObjectToDrop.isKinematic = false;
            ObjectToDrop.WakeUp();
        }
    }


    void OnChunkDetach(FracturedChunk.CollisionInfo info)
    {
        // We can cancel the collision processing here
        info.bCancelCollisionEvent = false;

        // Simply increase a counter for each chunk detached
        nChunksDetached++;
    } 

    void OnFreeChunkCollision(FracturedChunk.CollisionInfo info)
    {
        // We can cancel the collision processing here
        info.bCancelCollisionEvent = false;

        // Simply increase a counter for each collision
        nChunkCollisions++;
    } 
}
