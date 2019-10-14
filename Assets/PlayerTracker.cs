using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    GameObject player = null;

    Vector3 offset = default;
    bool following = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && following)
        {
            transform.position = player.transform.position + offset;
        }
    }

    public void StartFollowing(GameObject player)
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        Vector3 xyProjection = toPlayer - (transform.forward * Vector3.Dot(transform.forward, toPlayer));
        transform.position = transform.position + xyProjection;

        offset = transform.position - player.transform.position;
        this.player = player;
        following = true;
    }
}
