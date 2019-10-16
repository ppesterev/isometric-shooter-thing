using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPresence : NetworkBehaviour
{
    [SyncVar]
    string playerName = null;
    public string PlayerName => playerName;

    [SyncVar (hook="OnScoreChanged")]
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
            }
        }
    }

    void OnScoreChanged(int score)
    {
        if(hasAuthority)
            UIManager.instance.UpdateScore(score);
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

    public override void OnStartAuthority()
    {
        if (isClient)
        {
            CmdSetupPlayer(PlayerPrefs.GetString("PlayerName"), PlayerPrefs.GetInt("CharacterId"));
        }
    }
}
