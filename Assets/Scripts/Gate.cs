using UnityEngine;
using static Helpers;

public class Gate : MonoBehaviour
{
    public Room ConnectedRoom { get; set; }
    public Room Room { get; set; }
    public Gate ConnectedGate { get; set; }
    public GatePosition GatePosition => _gatePosition;
    public Transform SpawnPoint => _spawnPoint;

    [Header("Position")]
    [SerializeField] private GatePosition _gatePosition;
    [Space(10)]
    [Header("Components")]
    [SerializeField] private Collider _boxCollider;
    [SerializeField] private Collider[] _gateColliders; // [0]: In, [1]: Out
    [Space(10)]
    [Header("Preferences")]
    [SerializeField] private Transform _spawnPoint;

    /// <summary>
    /// Set up the connection between gates and rooms.
    /// </summary>
    /// <param name="room1"></param>
    /// <param name="room2"></param>
    /// <param name="gate2"></param>
    public void ConnectTo(Room room1, Room room2, Gate gate2)
    {
        ConnectedRoom = room2;
        ConnectedGate = gate2;
        gate2.ConnectedRoom = room1;
        gate2.ConnectedGate = this;
    }

    /// <summary>
    /// Set the gate to be locked or unlocked.
    /// </summary>
    /// <param name="isLocked"></param>
    public void Lock(bool isLocked)
    {
        _gateColliders[0].enabled = isLocked;
    }

}
