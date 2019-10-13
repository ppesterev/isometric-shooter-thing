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
    string defaultName = "Player";

    // Start is called before the first frame update
    void Start()
    {
        buttonOK.onClick.AddListener(CloseSettings);
        SetName(inputName.text);
        inputName.onEndEdit.AddListener(SetName);

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

    public void CloseSettings()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
