using UnityEngine;

public class EnterRoomCheck : MonoBehaviour
{
    [SerializeField] private Gate _parent;

    // when player exit the gate => move to the room
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Helpers.Tag.Player))
        {
            EventHandlers.CallOnEnterRoom(_parent.Room);
        }
    }
}
