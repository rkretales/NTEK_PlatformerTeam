using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onLongPress;
    [SerializeField] private float longPressDuration = 3f;
    public bool isPressed;
    private float pressTime;
    private InGameUI ui;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pressTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    private void Update()
    {
        Debug.Log(isPressed);
        if (isPressed && Time.time - pressTime >= longPressDuration)
        {
            if (onLongPress != null)
            {
                onLongPress.Invoke();
                ui.isPaused = false;
            }
        }
    }

    private void Start()
    {
        ui = FindObjectOfType<InGameUI>();
        ui.isPaused = true;
    }
}