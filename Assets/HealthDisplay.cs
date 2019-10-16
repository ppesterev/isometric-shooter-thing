using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Slider healthBar = null;
    Text healthNumber = null;

    AvatarControl character = null;
    Hiding hidingState = null;

    Camera mainCamera = null;

    bool visible = false;
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
        hidingState = character.gameObject.GetComponent<Hiding>();
        PositionOverhead();
    }

    void PositionOverhead()
    {
        if (!character || !mainCamera)
            return;
        transform.position = mainCamera.WorldToScreenPoint(character.transform.position + Vector3.up * 2.5f);
    }

    void SetVisibility(bool visible)
    {
        this.visible = visible;
        healthBar.gameObject.SetActive(visible);
        healthNumber.gameObject.SetActive(visible);
    }

    // Update is called once per frame
    void Update()
    {
        if (!character)
            Destroy(gameObject);

        if ((!hidingState.hidden || hidingState.detected) && !visible)
            SetVisibility(true);

        if (((hidingState.hidden && !hidingState.detected) && visible))
            SetVisibility(false);

        PositionOverhead();
        healthBar.value = character.Health / (1.0f * character.MaxHealth);
        healthNumber.text = character.Health.ToString();
    }
}
