using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button buttonHost = null;
    [SerializeField]
    Button buttonJoin = null;
    [SerializeField]
    Button buttonSettings = null;
    [SerializeField]
    Button buttonQuit = null;
    [SerializeField]
    InputField InputIPAddress = null;

    [SerializeField]
    GameObject settingsPanel = null;

    // Start is called before the first frame update
    void Start()
    {
        buttonHost.onClick.AddListener(Host);
        buttonJoin.onClick.AddListener(Join);
        buttonSettings.onClick.AddListener(OpenSettings);
        buttonQuit.onClick.AddListener(Quit);
    }

    void Host()
    {
        NetworkManager.singleton.StartHost();
    }

    void Join()
    {
        try
        {
            if(InputIPAddress.text != "localhost")
                System.Net.IPAddress.Parse(InputIPAddress.text);

            buttonJoin.GetComponentInChildren<Text>().text = "Connecting...";
            NetworkManager.singleton.networkAddress = InputIPAddress.text;
            NetworkManager.singleton.StartClient();
        }
        catch
        {
            //TODO indicate bad IP
        }
    }

    void OpenSettings()
    {
        settingsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    void Quit()
    {
        Application.Quit();
    }
}
