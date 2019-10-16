using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject healthDisplayPrefab = null;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateHealthDisplay(AvatarControl character)
    {
        // create new display, parent to canvas
        GameObject newDisplay = Instantiate(healthDisplayPrefab, transform);
        newDisplay.GetComponent<HealthDisplay>().Setup(character);
    }
}
