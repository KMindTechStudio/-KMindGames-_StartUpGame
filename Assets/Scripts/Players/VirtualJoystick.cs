using UnityEngine;

public class VirtualJoystick : MonoBehaviour
{
    [Header("Assign Virtual Joystick")]
    public RectTransform joystickBackground; 
    public RectTransform joystickHandle; 
    public float joystickMoveThreshold = 1.0f; // Movement threshold

    [Header("Player Settings")]
    private GameObject playerObject; 
    private Transform player; 
    public float moveSpeed = 3f; 
    public float playerRotationX = 70f; 

    public Animator animator;

    private Vector2 inputVector;
    private float distance; // Distance from the joystick center to the touch point
    private int maxDistance = 1000; 

    public Vector3 moveDirection; // Direction of player movement
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("player");
        player = playerObject.transform; 
    }
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
                    animator.SetBool("IsMoving", true);

                }
                else
                {
                    MovePlayer(inputVector.normalized);
                    animator.SetBool("IsMoving", true); 

                }

            }
            // If the distance is greater than the maximum distance, move the joystick handle to the edge
            else
            {
                joystickHandle.localPosition = Vector3.zero; // Reset joystick when not touched
                inputVector = Vector2.zero;
                animator.SetBool("IsMoving", false);
            }
        }
        else
        {
            joystickHandle.localPosition = Vector3.zero; 
            inputVector = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    public void MovePlayer(Vector2 direction)
    {
        // Move the player based on the joystick input (transformed y axis to z axis)
        moveDirection = new Vector3(direction.x, 0, direction.y);
        player.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Rotate the player to face the movement direction
        if (direction.x <0)
        {
            player.rotation = Quaternion.Euler(playerRotationX, 0, 0);
        }
        if (direction.x > 0)
        {
            player.rotation = Quaternion.Euler(-playerRotationX, 180, 0);
        }
    }
}
