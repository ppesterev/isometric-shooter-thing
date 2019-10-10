using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AvatarControl : NetworkBehaviour
{
    [SerializeField]
    float speed = 5f;
    [SerializeField, SyncVar]
    int health = 100;
    public int Health => health;

    CharacterController controller = null;
    Animator animator = null;
    QuickAttack quickAttack = null;
    Vector3 motion;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        quickAttack = GetComponent<QuickAttack>();
    }

    private void Update()
    {
        if (!hasAuthority)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            CmdQuickAttack();
            animator.SetTrigger("QuickAttack");
        }
    }

    void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        motion.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (motion.magnitude > 0.1f)
            transform.forward = Vector3.RotateTowards(transform.forward, motion, 0.3f, 1);
        controller.Move(motion * speed * Time.fixedDeltaTime);

        animator.SetFloat("Speed", motion.magnitude);
    }

    [Command]
    void CmdQuickAttack()
    {
        quickAttack.Attack(transform.forward);
        RpcSyncAnimation("QuickAttack");
    }

    [ClientRpc]
    void RpcSyncAnimation(string triggerName)
    {
        if(!hasAuthority)
        animator.SetTrigger(triggerName);
    }

    public void TakeDamage(int damage)
    {
        if (!isServer)
            return;
        health -= damage;
    }
}
