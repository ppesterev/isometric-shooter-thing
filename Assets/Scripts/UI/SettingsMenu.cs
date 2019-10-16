 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    Button buttonOK = null;
    [SerializeField]
    InputField inputName = null;

    [SerializeField]
    GameObject mainMenu = null;

    [SerializeField]
    Text[] descriptions = null;

    [SerializeField]
    string defaultName = "Player";

    // Start is called before the first frame update
    void Start()
    {
        buttonOK.onClick.AddListener(CloseSettings);
        SetName(inputName.text);
        inputName.onEndEdit.AddListener(SetName);

        SetCharacter(0);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    void SetName(string name)
    {
        if(name == null || name == "")
        {
            inputName.text = defaultName;
        }
        PlayerPrefs.SetString("PlayerName", inputName.text);
    }

    // 
    public void SetCharacter(int id)
    {
        PlayerPrefs.SetInt("CharacterId", id);
        foreach (Text desc in descriptions)
            desc.enabled = false;
        descriptions[id].enabled = true;
    }

    public void CloseSettings()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
