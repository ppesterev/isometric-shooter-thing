using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> characterPrefabs = null;
    public List<GameObject> CharacterPrefabs => characterPrefabs;
    [SerializeField]

    Dictionary<string, GameObject> characterPrefabDict = new Dictionary<string, GameObject>();
    public static GameManager instance;

    public System.Action<GameObject, GameObject> OnKill;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Debug.LogError("More than one GameManager in a scene");
        else
            instance = this;

        OnKill += KillMessage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void KillMessage(GameObject one, GameObject other)
    {
        Debug.Log("Kill");
    }
}
