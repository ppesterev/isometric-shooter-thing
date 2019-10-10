using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QuickAttack : NetworkBehaviour
{
    [SerializeField]
    float spreadAngle = 45;
    float range = 5;
    int damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Vector3 attackDirection)
    {
        if (!isServer)
            return;

        Collider[] avatars = Physics.OverlapSphere(transform.position, range, 1 << 8); // Layer mask for avatars
        foreach(Collider other in avatars)
        {
            // skip our own collider
            if (other.gameObject == gameObject)
                continue;

            Vector3 direction = other.transform.position - transform.position;
            if (Vector3.Angle(direction, attackDirection) < spreadAngle)
                other.GetComponent<AvatarControl>().TakeDamage(damage);
        }
    }
}
