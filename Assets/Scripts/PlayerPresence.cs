using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPresence : NetworkBehaviour
{
    //TODO This will be set by player before they connect
    [SerializeField]
    GameObject avatarPrefab = null;

    //AvatarControl avatar = null;

    [SyncVar]
    string playerName = null;
    public string PlayerName => playerName;

    [SyncVar]
    int score = 0;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            if (isServer)
            {
                score = value;
                //TODO update display
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CmdSetupPlayer(PlayerPrefs.GetString("PlayerName"));
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    void CmdSetupPlayer(string playerName)
    {
        this.playerName = playerName;
        GameObject avatar = Instantiate(avatarPrefab);
        avatar.GetComponent<AvatarControl>().ControllingPlayer = this.gameObject;
        NetworkServer.SpawnWithClientAuthority(avatar, this.gameObject);
    }

    //public void DieAndRespawn(GameObject avatar)
    //{
    //    if (!hasAuthority)
    //        return;
    //    NetworkServer.UnSpawn(avatar);
    //    StartCoroutine(WaitAndRespawn());
    //}

    //IEnumerator WaitAndRespawn()
    //{
    //    //yield return new WaitForSeconds(3.0f);
    //    //NetworkServer.SpawnWithClientAuthority()
    //    yield return null;
    //}
}
