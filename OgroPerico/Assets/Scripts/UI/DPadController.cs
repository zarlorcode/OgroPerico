using UnityEngine;

public class DPadController : MonoBehaviour
{
    public PlayerMovement player;

    private bool upPressed, downPressed, leftPressed, rightPressed;

    void Start()
    {
    }

    public void SetPressed(DPadButton.Direction dir, bool isPressed)
    {
        switch (dir)
        {
            case DPadButton.Direction.Up: upPressed = isPressed; break;
            case DPadButton.Direction.Down: downPressed = isPressed; break;
            case DPadButton.Direction.Left: leftPressed = isPressed; break;
            case DPadButton.Direction.Right: rightPressed = isPressed; break;
        }

        UpdateDirection();
    }

    void UpdateDirection()
    {
        Vector2 direction = Vector2.zero;

        if (upPressed) direction.y += 1;
        if (downPressed) direction.y -= 1;
        if (leftPressed) direction.x -= 1;
        if (rightPressed) direction.x += 1;

        direction = direction.normalized;

        player.SetInputDirection(direction);
    }
}