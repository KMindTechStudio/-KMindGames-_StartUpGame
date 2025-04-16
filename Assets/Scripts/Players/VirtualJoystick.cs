using UnityEngine;

public class VirtualJoystick : MonoBehaviour
{

    public RectTransform joystickBackground; // Background of the joystick
    public RectTransform joystickHandle; // Handle of the joystick
    public Transform player; // Reference to the player
    public float moveSpeed = 3f; // Speed of player movement
    private Vector2 inputVector;
    private float distance; // Distance from the joystick center to the touch point
    public int maxDistance = 900; // Maximum distance for joystick movement
    public float joystickMoveThreshold = 1.0f; // Movement threshold

    private void Update()
    {
        // If touch or mouse input is detected
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 joystickPosition = joystickBackground.position;

            //Calculate the distance from the joystick center to the touch point
            distance = Vector2.Distance(mousePosition, joystickPosition);

            // If the distance is less than the maximum distance, move the joystick handle
            if (distance < maxDistance)
            {
                inputVector = Vector2.ClampMagnitude(mousePosition - joystickPosition, joystickBackground.sizeDelta.x * 0.5f);
                joystickHandle.localPosition = inputVector;
                if (inputVector.magnitude > joystickMoveThreshold)
                {
                    MovePlayer(inputVector.normalized);
                }
                else
                {
                    MovePlayer(inputVector.normalized);
                }

            }
            // If the distance is greater than the maximum distance, move the joystick handle to the edge
            else
            {
                joystickHandle.localPosition = Vector3.zero; // Reset joystick when not touched
                inputVector = Vector2.zero;
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
        // Move the player based on the joystick input (transformed y axis to z axis)
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        player.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Rotate the player to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
