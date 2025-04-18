using System.Collections.Generic;
using UnityEngine;
using static Helpers;

public class Room : MonoBehaviour
{
    public int RoomID { get; set; } = 0;
    public Gate GateIn => _gateIn;
    public Gate GateOut => _gateOut;

    [Header("Static Variables")]
    [SerializeField] private GatePosition _gateInPosition;
    [SerializeField] private GatePosition _gateOutPosition;
    [SerializeField] private GameObject[] _gates; // [0]: North, [1]: South, [2]: East, [3]: West
    [SerializeField] private Collider[] _wallColliders;
    [SerializeField] private bool _isLocked = false;
    [SerializeField] private bool _isCompleted = false;
    private RoomManager _roomManager;
    private Gate _gateIn;
    private Gate _gateOut;

    private void Start()
    {
        EventHandlers.OnEnterRoom += OnEnterRoom;
    }
    private void OnDisable()
    {
        EventHandlers.OnEnterRoom -= OnEnterRoom;
    }
    private void OnEnterRoom(Room room)
    {
        if (room != this) return;

        _isLocked = !_isCompleted;

        if (_gateIn != null) _gateIn.Lock(_isLocked);
        if (_gateOut != null) _gateOut.Lock(_isLocked);
    }

    public void SetUpRoom(RoomManager roomManager)
    {
        _roomManager = roomManager;
        _isCompleted = RoomID == 0; // start room is completed default
        _isLocked = false;

        SetUpGate();
        SetUpColliders();
    }

    private void SetUpGate()
    {
        foreach (var gate in _gates)
        {
            gate.gameObject.SetActive(false);
        }
        if (RoomID == 0)
        {
            _gateIn = null;
            _gateOutPosition = GatePosition.East;
            _gateOut = ActiveGate(_gateOutPosition);
            _gateOut.Room = this;
        }
        else
        {
            // gateIn is the gate that player will enter the room
            // from the gate out of previous room
            _gateInPosition = GetTheOppositeGatePosition(_roomManager.RoomList[RoomID - 1]._gateOutPosition);
            _gateIn = ActiveGate(_gateInPosition);

            // randome gate out position except the gate in position
            int randomGateOut = Random.Range(0, _gates.Length);
            while (randomGateOut == (int)_gateInPosition)
            {
                randomGateOut = Random.Range(0, _gates.Length);
            }
            _gateOutPosition = (GatePosition)randomGateOut;
            _gateOut = ActiveGate(_gateOutPosition);

            _gateIn.Room = this;
            _gateOut.Room = this;
        }
    }
    private void SetUpColliders()
    {
        /*
            [0],[1]: North Segment
            [2],[3]: South Segment
            [4],[5]: East Segment
            [6],[7]: West Segment
            [8]    : North Full
            [9]    : South Full
            [10]   : East Full
            [11]   : West Full
        */
        // Disable all colliders
        foreach (var col in _wallColliders)
        {
            if (col != null)
                col.enabled = false;
        }

        // Enable all full wall colliders (8 to 11)
        for (int i = 8; i < 12; i++)
        {
            if (_wallColliders[i] != null)
                _wallColliders[i].enabled = true;
        }

        // Dữ liệu cấu hình từng hướng: (segment1, segment2, fullWallIndex)
        var gateData = new Dictionary<GatePosition, (int seg1, int seg2, int full)>
        {
            { GatePosition.North, (0, 1, 8) },
            { GatePosition.South, (2, 3, 9) },
            { GatePosition.East,  (4, 5,10) },
            { GatePosition.West,  (6, 7,11) },
        };
        void EnableGateColliders(GatePosition pos)
        {
            if (gateData.TryGetValue(pos, out var data))
            {
                if (_wallColliders[data.seg1] != null) _wallColliders[data.seg1].enabled = true;
                if (_wallColliders[data.seg2] != null) _wallColliders[data.seg2].enabled = true;
                if (_wallColliders[data.full] != null) _wallColliders[data.full].enabled = false;
            }
        }
        EnableGateColliders(_gateInPosition);
        if (_gateOutPosition != _gateInPosition)
            EnableGateColliders(_gateOutPosition);
    }
    private Gate ActiveGate(GatePosition gatePosition, bool active = true)
    {

        foreach (var gate in _gates)
        {
            Gate gateObj = gate.GetComponent<Gate>();
            if (gateObj.GatePosition == gatePosition)
            {
                gateObj.gameObject.SetActive(active);
                return gateObj;
            }
        }
        return null; // Return null if no matching gate is found
    }
    private GatePosition GetTheOppositeGatePosition(GatePosition gatePosition)
    {
        switch (gatePosition)
        {
            case GatePosition.North:
                return GatePosition.South;
            case GatePosition.South:
                return GatePosition.North;
            case GatePosition.East:
                return GatePosition.West;
            case GatePosition.West:
                return GatePosition.East;
            default:
                return GatePosition.None;
        }
    }
}
