using UnityEngine;

public class ExitRoomCheck : MonoBehaviour
{
    [SerializeField] private Gate _gate;

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            EventHandlers.CallOnGetOutRoom(_gate);
        }
    }
}
