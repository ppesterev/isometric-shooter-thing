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
    AimingIndicator indicator = null;

    Vector3 motion;
    bool aiming = false;


    Joystick joystickMovement = null;
    Joystick joystickAttack = null;
    PushInJoystick attackButton = null;

    Camera mainCamera = null;

    public System.Action<GameObject, GameObject> KillEvent;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        quickAttack = GetComponent<QuickAttack>();

        UIManager.instance.CreateHealthDisplay(this);
    }

    private void Update()
    {
        if (!hasAuthority || health <= 0)
            return;

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    animator.SetTrigger("QuickAttack");
        //    CmdQuickAttack();
        //}

        if (aiming)
            indicator.UpdateDirection(UIToWorldDirection(joystickAttack.Direction));
    }

    void OnAtkBtnPushed(bool pastThreshold, Vector2 direction)
    {
        indicator.SetVisibility(false);
        aiming = false;

        if(pastThreshold) // aiming and not attacking along movement
            transform.forward = UIToWorldDirection(direction);

        animator.SetTrigger("QuickAttack");
        CmdQuickAttack(transform.forward);
    }

    void OnAtkBtnCrossed()
    {
        indicator.SetVisibility(true);
        aiming = true;
    }

    Vector3 UIToWorldDirection(Vector2 direction)
    {
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = mainCamera.transform.right;

        return forward * direction.normalized.y + right * direction.normalized.x;
    }

    void FixedUpdate()
    {
        if (!hasAuthority || health <= 0)
            return;

        //motion.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        motion = UIToWorldDirection(joystickMovement.Direction);

        if (motion.magnitude > 0.1f)
            transform.forward = Vector3.RotateTowards(transform.forward, motion, 0.3f, 1);
        controller.Move(motion * speed * Time.fixedDeltaTime);

        animator.SetFloat("Speed", motion.magnitude);
    }

    [Command]
    void CmdQuickAttack(Vector3 direction)
    {
        RpcSyncAnimation("QuickAttack");
        quickAttack.Attack(direction, controllingPlayer); // TODO effect spawning with outdated direction
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
        animator.SetTrigger("Die");
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
    }

    [ClientRpc]
    void RpcResetCharacter(Vector3 position)
    {
        transform.position = NetworkManager.singleton.GetStartPosition().position;
        Physics.SyncTransforms();
        animator.SetTrigger("Reset");
    }

    public override void OnStartAuthority()
    {
        if (isClient)
        {
            mainCamera = Camera.main;
            PlayerTracker trackingCamera = mainCamera.GetComponent<PlayerTracker>();
            if (trackingCamera != null)
            {
                trackingCamera.StartFollowing(this.gameObject);
            }

            // on the authoritative client, detection of hidden players is enabled
            Detector det = GetComponentInChildren<Detector>();
            if(det != null)
                det.enabled = true;

            indicator = GetComponent<AimingIndicator>();
            indicator.enabled = true;

            joystickMovement = GameObject.Find("JoystickMovement").GetComponent<Joystick>();
            joystickAttack = GameObject.Find("JoystickAttack").GetComponent<Joystick>();
            attackButton = GameObject.Find("JoystickAttack").GetComponent<PushInJoystick>();

            attackButton.Pushed += OnAtkBtnPushed;
            attackButton.ThresholdCrossed += OnAtkBtnCrossed;
        }
    }
}
