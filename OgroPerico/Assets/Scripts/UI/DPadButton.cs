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
        manager.SetPressed(direction, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        manager.SetPressed(direction, false);
    }
}