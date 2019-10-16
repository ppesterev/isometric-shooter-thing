using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject healthDisplayPrefab = null;
    [SerializeField]
    Text score = null;
    PlayerPresence player = null;

    public static UIManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (healthDisplayPrefab == null || healthDisplayPrefab.GetComponent<HealthDisplay>() == null)
            Debug.LogError("UI manager: Invalid health display prefab");

        GameManager.instance.OnKill += ProcessKill;
    }

    public void CreateHealthDisplay(AvatarControl character)
    {
        // create new display, parent to canvas
        GameObject newDisplay = Instantiate(healthDisplayPrefab, transform);
        newDisplay.GetComponent<HealthDisplay>().Setup(character);
    }

    private void ProcessKill(GameObject attacker, GameObject victim)
    {
        PlayerPresence attackingPlayer = attacker.GetComponent<PlayerPresence>();
        PlayerPresence victimPlayer = victim.GetComponent<PlayerPresence>();
        if (attackingPlayer && victimPlayer) // could be adapted to handle environment deaths etc/
        {
            Debug.Log(attackingPlayer.PlayerName + " killed " + victimPlayer.PlayerName);
        }
    }

    public void UpdateScore(int score)
    {
        this.score.text = "<b>Score: " + score + "</b>";
    }
}
