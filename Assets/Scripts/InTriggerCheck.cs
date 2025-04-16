using UnityEngine;

public class InTriggerCheck : MonoBehaviour
{
    private Gate parent;
    private void Awake()
    {
        parent = GetComponentInParent<Gate>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent.IsInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent.IsInTrigger = false;
        }
    }
}
