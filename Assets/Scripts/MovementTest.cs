using UnityEngine;

public class MovementTest : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 720f; // Degrees per second
    private Vector2 _movementInput;
    private Rigidbody _rigidbody;
    public bool Dead;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        _movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _movementInput.Normalize();
        _rigidbody.linearVelocity = new Vector3(_movementInput.x, 0, _movementInput.y) * speed;

        if (Dead) CallDeadEvent();
    }

    private void CallDeadEvent()
    {
        Dead = false;
        EventHandlers.CallOnPlayerDie();
    }
}
