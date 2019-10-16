using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detector enter");
        if (other.tag == "Player")
        {
            AvatarControl avatar = other.GetComponent<AvatarControl>();
            if (avatar != null && !avatar.hasAuthority)
            {
                Debug.Log("EnterDetectionRange called");
                other.GetComponent<Hiding>().EnterDetectionRange();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Detector exit");
        if (other.tag == "Player")
        {
            AvatarControl avatar = other.GetComponent<AvatarControl>();
            if (avatar != null && !avatar.hasAuthority)
            {
                Debug.Log("ExitDetectionRange called");
                other.GetComponent<Hiding>().ExitDetectionRange();
            }
        }
    }
}
