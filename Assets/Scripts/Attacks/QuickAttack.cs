using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QuickAttack : NetworkBehaviour
{
    [SerializeField]
    float spreadAngle = 45;
    [SerializeField]
    float range = 5;
    [SerializeField]
    int damage = 20;
    [SerializeField]
    float cooldown = 0.5f;
    float cooldownLeft = 0f;

    public float SpreadAngle => spreadAngle;
    public float Range => range;
    public int Damage => damage;

    [SerializeField]
    GameObject effectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownLeft > 0)
            cooldownLeft -= Time.deltaTime;
    }

    public void Attack(Vector3 attackDirection, GameObject attacker)
    {
        if (!isServer || cooldownLeft > 0)
            return;
        cooldownLeft = cooldown;
        Quaternion effectRotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, attackDirection, Vector3.up), 0);
        RpcDisplayEffect(transform.position + transform.up + transform.forward * 0.5f, effectRotation);

        Collider[] avatars = Physics.OverlapSphere(transform.position, range, 1 << 8); // Layer mask for avatars
        foreach(Collider other in avatars)
        {
            // skip our own collider
            if (other.gameObject == gameObject)
                continue;

            Vector3 direction = other.transform.position - transform.position;
            bool blocked = Physics.Raycast(origin: transform.position,
                                           direction: direction,
                                           maxDistance: direction.magnitude,
                                           layerMask: 1 << 9);
            if (!blocked && Vector3.Angle(direction, attackDirection) < spreadAngle / 2)
                other.GetComponent<AvatarControl>().TakeDamage(damage, attacker);
        }
    }

    [ClientRpc]
    void RpcDisplayEffect(Vector3 position, Quaternion rotation)
    {
        Instantiate(effectPrefab, position, rotation);
    }
}
