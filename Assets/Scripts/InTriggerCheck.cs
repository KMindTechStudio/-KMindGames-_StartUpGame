using UnityEngine;

public class InTriggerCheck : MonoBehaviour
{
    [SerializeField] private Gate _parent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _parent.IsInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _parent.IsInTrigger = false;
        }
    }
}
