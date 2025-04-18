using UnityEngine;

public class ExitRoomCheck : MonoBehaviour
{
    [SerializeField] private Gate _gate;

    // When the player enter the collider that directly to exit the room
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EventHandlers.CallOnGetOutRoom(_gate);
        }
    }
}
