using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimingIndicator : MonoBehaviour
{
    [SerializeField]
    Image imageLeftHalf = null;
    [SerializeField]
    Image imageRightHalf = null;
    [SerializeField]
    Image PowerAttack = null;

    Vector2 direction = default;

    // Start is called before the first frame update
    void Start()
    {
        QuickAttack attack = GetComponent<QuickAttack>();
        float size = 2 * attack.Range;
        imageLeftHalf.transform.localScale = imageRightHalf.transform.localScale = new Vector3(size, size, size);
        float fill = attack.SpreadAngle / (2 * 360);
        imageLeftHalf.fillAmount = imageRightHalf.fillAmount = fill;

        imageLeftHalf.enabled = imageRightHalf.enabled = false;
    }

    public void SetVisibility(bool visible)
    {
        imageLeftHalf.enabled = imageRightHalf.enabled = visible;
    }

    public void UpdateDirection(Vector3 newDirection)
    {
        
        direction.Set(newDirection.x, newDirection.z);
        Quaternion rotation = Quaternion.Euler(90, 0, Vector2.SignedAngle(Vector2.up, direction));
        imageLeftHalf.transform.rotation = imageRightHalf.transform.rotation = rotation;
    }
}
