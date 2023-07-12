using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onLongPress;
    [SerializeField] private float longPressDuration = 3f;
    public bool isPressed;
    private Automate automate;
    private float pressTime;
    private InGameUI ui;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!automate.shuffling)
        {
            isPressed = true;
            pressTime = Time.time;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    private void Update()
    {
        Debug.Log(automate.shuffling);
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
        automate = FindObjectOfType<Automate>();
        ui = FindObjectOfType<InGameUI>();
        ui.isPaused = true;
    }
}