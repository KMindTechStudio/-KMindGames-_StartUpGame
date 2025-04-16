using UnityEngine;
using static Helpers;

public class Gate : MonoBehaviour
{
    public GatePosition GatePosition => _gatePosition;
    public bool IsInTrigger { get; set; } = false;

    [Header("Position")]
    [SerializeField] private GatePosition _gatePosition;
    [Space(10)]
    [SerializeField] private Collider _boxCollider;
}
