using UnityEngine;

public class VirtualJoystick : MonoBehaviour
{

    public RectTransform joystickBackground; // Background of the joystick
    public RectTransform joystickHandle; // Handle of the joystick
    public float joystickMoveThreshold = 1.0f; // Movement threshold
    public Transform player; // Reference to the player
    public float moveSpeed = 5f; // Speed of player movement
    private Vector2 inputVector;

    private void Update()
    {
        // If touch or mouse input is detected
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 joystickPosition = joystickBackground.position;

            inputVector = Vector2.ClampMagnitude(mousePosition - joystickPosition, joystickBackground.sizeDelta.x * 0.5f);
            joystickHandle.localPosition = inputVector;

            if (inputVector.magnitude > joystickMoveThreshold)
            {
                MovePlayer(inputVector.normalized);
            }
        }
        else
        {
            joystickHandle.localPosition = Vector3.zero; // Reset joystick when not touched
            inputVector = Vector2.zero;
        }
    }

    private void MovePlayer(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        player.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);        
    }
}
