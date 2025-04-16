using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> RoomList => _roomList;
    [SerializeField] private GameObject _roomPrefab;
    private List<Room> _roomList;
    private Room _currentRoom;
    private void Awake()
    {
        _roomList = new List<Room>();
    }

    private void Start()
    {
        CreateRoom();
    }
    public void CreateRoom()
    {
        GameObject newRoom = Instantiate(_roomPrefab, transform.position, Quaternion.identity);
        Room room = newRoom.GetComponent<Room>();
        room.RoomID = _roomList.Count;
        room.SetUpRoom(this);
        _roomList.Add(room);
        _currentRoom = room;
        _currentRoom.gameObject.SetActive(true);
    }

    public void ChangeRoom(int nextRoomId)
    {
        for (int i = 0; i < _roomList.Count; i++)
        {
            if (_roomList[i].RoomID == nextRoomId)
            {
                _currentRoom = _roomList[i];
                break;
            }
        }
    }
}
