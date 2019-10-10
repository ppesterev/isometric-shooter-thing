using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPresence : NetworkBehaviour
{
    [SerializeField]
    GameObject avatarPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        // each player presence on the server spawns their selected avatar
        if (isServer)
        {
            SpawnAvatar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnAvatar()
    {
        GameObject avatar = Instantiate(avatarPrefab);
        NetworkServer.SpawnWithClientAuthority(avatar, this.gameObject);
    }
}
