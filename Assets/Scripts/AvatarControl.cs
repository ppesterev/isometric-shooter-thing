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

    [SerializeField]
    private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    //TODO implement hiding
    //[SyncVar]
    //bool hidden = false;

    [SyncVar]
    GameObject controllingPlayer;
    public GameObject ControllingPlayer
    {
        get
        {
            return controllingPlayer;
        }
        set
        {
            controllingPlayer = value;
        }
    }


    CharacterController controller = null;
    Animator animator = null;
    QuickAttack quickAttack = null;
    Vector3 motion;

    public System.Action<GameObject, GameObject> KillEvent;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        quickAttack = GetComponent<QuickAttack>();
    }

    private void Update()
    {
        if (!hasAuthority || health <= 0)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("QuickAttack");
            CmdQuickAttack();
        }
    }

    void FixedUpdate()
    {
        if (!hasAuthority || health <= 0)
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
        RpcSyncAnimation("QuickAttack");
        quickAttack.Attack(transform.forward, controllingPlayer);
    }

    [ClientRpc]
    void RpcSyncAnimation(string triggerName)
    {
        if(!hasAuthority)
        animator.SetTrigger(triggerName);
    }

    [ClientRpc]
    void RpcDie(GameObject attacker)
    {
        // death animation, etc.
        GameManager.instance.OnKill(attacker, controllingPlayer);
            
    }

    public void TakeDamage(int damage, GameObject source)
    {
        if (!isServer)
            return;

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            PlayerPresence killer = source.GetComponent<PlayerPresence>();
            if (killer != null)
                killer.Score++;
            RpcDie(source);
            StartCoroutine(WaitAndRespawn());
        }            
    }

    IEnumerator WaitAndRespawn()
    {
        yield return new WaitForSeconds(3.0f);
        health = maxHealth;
        RpcResetCharacter(NetworkManager.singleton.GetStartPosition().position);
        //RcSyncAnimation("Respawn");
    }

    [ClientRpc]
    void RpcResetCharacter(Vector3 position)
    {
        transform.position = NetworkManager.singleton.GetStartPosition().position;
        Physics.SyncTransforms();
    }
}
