using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;

    // amount of movement of the handle
    public float handleRange = 50f; 

    private Vector2 input = Vector2.zero;
    public Vector2 InputDirection => input; // propiedad pública para PlayerMovement

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out pos))
        {
            // Convertir a rango [-1,1]
            pos.x = (pos.x / background.sizeDelta.x) * 2;
            pos.y = (pos.y / background.sizeDelta.y) * 2;

            input = new Vector2(pos.x, pos.y);
            input = (input.magnitude > 1.0f) ? input.normalized : input;

            handle.anchoredPosition = input * handleRange;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
