using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AvatarControl : NetworkBehaviour
{
    [SerializeField]
    float speed = 5f;
    CharacterController controller = null;
    Animator animator = null;
    Vector3 motion;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if(hasAuthority)
        {
            motion.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (motion.magnitude > 0.03f)
                transform.forward = Vector3.RotateTowards(transform.forward, motion, 0.3f, 1);
            controller.Move(motion * speed * Time.fixedDeltaTime);

            if(Input.GetButtonDown("Fire1"))
            {
                CmdQuickAttack();
            }
        }

        animator.SetFloat("Speed", motion.magnitude);
    }

    [Command]
    void CmdQuickAttack()
    {
        Debug.Log("Command called");
        animator.SetTrigger("QuickAttack");
    }
}
