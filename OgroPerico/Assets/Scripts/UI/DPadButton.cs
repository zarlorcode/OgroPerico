using UnityEngine;
using UnityEngine.EventSystems;

public class DPadButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;
    private DPadController manager;

    void Start()
    {
        manager = GetComponentInParent<DPadController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Button pressed: " + direction);
        manager.SetPressed(direction, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Button released: " + direction);
        manager.SetPressed(direction, false);
    }
}