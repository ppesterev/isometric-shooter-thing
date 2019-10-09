using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AvatarControl : NetworkBehaviour
{
    [SerializeField]
    float speed = 5f;
    CharacterController controller = null;
    Vector3 motion;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasAuthority)
        {
            motion.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            motion.Normalize();
            if (motion.magnitude > 0.05f)
                transform.forward = Vector3.RotateTowards(transform.forward, motion, 0.3f, 1);
            controller.Move(motion * speed * Time.fixedDeltaTime);
        }
    }
}
