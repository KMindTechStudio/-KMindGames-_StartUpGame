using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static Helpers;

public class GateController : MonoBehaviour
{
    [SerializeField] private GateType _gateType;
    [SerializeField] private GatePosition _gateDirection;
    [SerializeField] private Collider[] _wallColliders;
    private Collider _inCollider;
    private Collider _outCollider;
    private Map _map;

    public void Initialize(Map map, GateType gateType, GatePosition gateDirection)
    {
        _map = map;
        _gateType = gateType;
        _gateDirection = gateDirection;
    }

    private void SetGateDirection(GateType gateType, GatePosition gateDirection)
    {
        /*
            [0] North Collider
            [1] South Collider
            [2] East Collider
            [3] West Collider
        */

        if (gateType == GateType.In)
        {
            switch (_gateDirection)
            {
                case GatePosition.North:
                    _inCollider = _wallColliders[0];
                    _outCollider = _wallColliders[1];
                    break;
                case GatePosition.South:
                    _inCollider = _wallColliders[1];
                    _outCollider = _wallColliders[0];
                    break;
                case GatePosition.East:
                    _inCollider = _wallColliders[2];
                    _outCollider = _wallColliders[3];
                    break;
                case GatePosition.West:
                    _inCollider = _wallColliders[3];
                    _outCollider = _wallColliders[2];
                    break;
            }
        }


    }




}

