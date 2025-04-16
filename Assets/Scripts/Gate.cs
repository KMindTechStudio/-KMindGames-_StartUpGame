using UnityEngine;
using static Helpers;

public class Gate : MonoBehaviour
{
    public bool IsInTrigger { get; set; } = false;
    public bool IsLooked
    {
        get => _isLocked;
        set
        {
            _isLocked = value;
            if (_isLocked)
            {
                _inCollder.isTrigger = false;
            }
            else
            {
                _inCollder.isTrigger = true;
            }
        }
    }
    [Header("Position")]
    [SerializeField] private GatePosition _gatePosition;
    [Space(10)]
    [SerializeField] private Collider _boxCollider;
    [SerializeField] private Collider[] _walls; // [0]: North, [1]: South, [2]: East, [3]: West
    private Collider _inCollder;
    private Collider _outCollider;
    private bool _isLocked = false;
    private void Start()
    {
        SetUpInOutCollider();
    }

    private void SetUpInOutCollider()
    {
        switch (_gatePosition)
        {
            // [0]: North, [1]: South, [2]: East, [3]: West
            case GatePosition.North:
                _outCollider = _walls[0];
                _inCollder = _walls[1];
                break;
            case GatePosition.South:
                _outCollider = _walls[1];
                _inCollder = _walls[0];
                break;
            case GatePosition.East:
                _outCollider = _walls[2];
                _inCollder = _walls[3];
                break;
            case GatePosition.West:
                _outCollider = _walls[3];
                _inCollder = _walls[2];
                break;
        }
    }
    private void OnValidate()
    {
        SetUpInOutCollider();
    }

}
