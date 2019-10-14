using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AvatarControl avatar = other.GetComponent<AvatarControl>();
            if(avatar != null && !avatar.hasAuthority)
            {
                other.GetComponent<Hiding>().EnterCover();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            AvatarControl avatar = other.GetComponent<AvatarControl>();
            if (avatar != null && !avatar.hasAuthority)
            {
                other.GetComponent<Hiding>().ExitCover();
            }
        }
    }
}
