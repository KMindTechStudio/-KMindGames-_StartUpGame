using UnityEngine;
using static Helpers;

public class Gate : MonoBehaviour
{
    public bool IsInTrigger { get; set; } = false;
    public Room ConnectedRoom { get; set; }
    public GatePosition GatePosition => _gatePosition;

    [Header("Position")]
    [SerializeField] private GatePosition _gatePosition;
    [Space(10)]
    [SerializeField] private Collider _boxCollider;
    [SerializeField] private Collider[] _gateColliders; // [0]: In, [1]: Out

    [Header("Preferences")]
    [SerializeField] private Transform _spawnPoint;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_gateColliders[1].bounds.Intersects(other.collider.bounds))
            {
                // Move the player to other room
                EventHandlers.CallOnGetOutRoom(this);
            }
        }
    }
}
