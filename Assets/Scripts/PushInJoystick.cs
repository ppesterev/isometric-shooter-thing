using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PushInJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool held = false;
    float timeHeld = 0;
    [SerializeField]
    float timeThreshold = 0.15f;

    public System.Action ThresholdCrossed;
    public System.Action<bool, Vector2> Pushed; // true if released after threshold

    Joystick joystick = null;
    Vector2 direction = default;

    // Start is called before the first frame update
    void Start()
    {
        joystick = GetComponent<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (held)
            timeHeld += Time.deltaTime;
        if (timeHeld > timeThreshold)
            ThresholdCrossed?.Invoke();
        direction = joystick.Direction;
        // cache direction to send it on pointer up when the joystick
        // may have already snapped back to 0
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        held = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        held = false;
        Pushed?.Invoke(timeHeld > timeThreshold, direction);
        timeHeld = 0;
    }
}
