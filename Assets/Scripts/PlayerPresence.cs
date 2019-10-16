using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPresence : NetworkBehaviour
{
    //[SerializeField]
    //GameObject avatarPrefab = null;

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
        int characterId = (isServer ? 2 : 2); // debug
        if (hasAuthority)
            CmdSetupPlayer(PlayerPrefs.GetString("PlayerName"), PlayerPrefs.GetInt("CharacterId"));
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    void CmdSetupPlayer(string playerName, int characterId)
    {
        this.playerName = playerName;
        gameObject.name = playerName;

        Transform start = NetworkManager.singleton.GetStartPosition();

        GameObject avatar = Instantiate(GameManager.instance.CharacterPrefabs[characterId],
                                        start.position,
                                        start.rotation); ;
        avatar.name = playerName + "Avatar";
        avatar.GetComponent<AvatarControl>().ControllingPlayer = this.gameObject;

        NetworkServer.SpawnWithClientAuthority(avatar, this.gameObject);
    }
}
