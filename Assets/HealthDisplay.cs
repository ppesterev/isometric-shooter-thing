using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Slider healthBar = null;
    Text healthNumber = null;

    AvatarControl character = null;
    Camera mainCamera = null;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        healthNumber = GetComponentInChildren<Text>();
        mainCamera = Camera.main;
    }

    public void Setup(AvatarControl character)
    {
        this.character = character;
        PositionOverhead();
    }

    void PositionOverhead()
    {
        if (!character || !mainCamera)
            return;
        transform.position = mainCamera.WorldToScreenPoint(character.transform.position + Vector3.up * 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        PositionOverhead();
        healthBar.value = character.Health / (1.0f * character.MaxHealth);
        healthNumber.text = character.Health.ToString();
    }
}
