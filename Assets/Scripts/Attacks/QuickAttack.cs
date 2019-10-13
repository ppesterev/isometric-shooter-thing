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
    GameObject effectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Vector3 attackDirection, GameObject attacker)
    {
        if (!isServer)
            return;
        //TODO solution for spawning at appropriate position
        RpcDisplayEffect(transform.position + transform.up + transform.forward * 0.5f, transform.rotation);

        Collider[] avatars = Physics.OverlapSphere(transform.position, range, 1 << 8); // Layer mask for avatars
        foreach(Collider other in avatars)
        {
            // skip our own collider
            if (other.gameObject == gameObject)
                continue;

            Vector3 direction = other.transform.position - transform.position;
            if (Vector3.Angle(direction, attackDirection) < spreadAngle)
                other.GetComponent<AvatarControl>().TakeDamage(damage, attacker);
        }
    }

    [ClientRpc]
    void RpcDisplayEffect(Vector3 position, Quaternion rotation)
    {
        Instantiate(effectPrefab, position, rotation);
    }
}
