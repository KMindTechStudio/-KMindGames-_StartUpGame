using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public List<Room> RoomList => _roomList;
    [SerializeField] private GameObject _roomPrefab;

    [Space(10)]
    [Header("Fade Animation")]
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    private bool _isFading = false;
    private List<Room> _roomList;
    private Room _currentRoom;
    private GameObject _player;
    private void Awake()
    {
        _roomList = new List<Room>();
        _player = GameObject.FindGameObjectWithTag("Player");

        EventHandlers.OnGetOutRoom += OnGetOutRoom;
    }
    private void OnDestroy()
    {
        EventHandlers.OnGetOutRoom -= OnGetOutRoom;
    }

    private void Start()
    {
        _currentRoom = CreateRoom();
        FadeAndLoadRoom(_currentRoom.RoomID, new Vector3(0, 0, 0));
    }

    private void OnGetOutRoom(Gate gate)
    {
        Room? room = gate.ConnectedRoom;

        // Check if the gate is connected to another room
        if (room == null)
        {
            room = CreateRoom();
            gate.ConnectTo(_currentRoom, room, room.GateIn);
        }

        // Call fade and load the given room
        FadeAndLoadRoom(room.RoomID, gate.ConnectedGate.SpawnPoint.position);
    }
    public Room CreateRoom()
    {
        GameObject newRoom = Instantiate(_roomPrefab, transform.position, Quaternion.identity);
        Room room = newRoom.GetComponent<Room>();
        room.RoomID = _roomList.Count;
        newRoom.name = "Room " + room.RoomID;
        room.SetUpRoom(this);
        room.gameObject.SetActive(false);
        _roomList.Add(room);
        return room;
    }

    #region Fade and Load Room
    private void FadeAndLoadRoom(int roomIndex, Vector3 spawnPosition)
    {
        if (_isFading) return;
        StartCoroutine(FadeAndSwitchRoom(roomIndex, spawnPosition));
    }
    private IEnumerator FadeAndSwitchRoom(int roomIndex, Vector3 spawnPosition)
    {
        EventHandlers.CallOnBeforeRoomUnloadFadeOut();

        yield return StartCoroutine(Fade(1f));

        // Call save load room in here
        //////////////////////////////////////////////////

        // Call before room unload event
        EventHandlers.CallOnBeforeRoomUnload();

        // Unload the current room
        _currentRoom.gameObject.SetActive(false);

        // Loading the given room 
        LoadAndActiveRoom(roomIndex);

        // Call after room load event
        EventHandlers.CallOnAfterRoomLoad();

        // Restore new room data here
        //////////////////////////////////////////////////

        // Set player position to spawn position
        _player.transform.position = spawnPosition;

        yield return StartCoroutine(Fade(0f));

        // Call after room fade in event
        EventHandlers.CallOnAfterRoomFadeIn();
    }
    private void LoadAndActiveRoom(int roomIndex)
    {
        _currentRoom = _roomList[roomIndex];
        _currentRoom.gameObject.SetActive(true);
    }
    private IEnumerator Fade(float alpha)
    {
        // // Simulate fade out effect 
        // _isFading = true;
        // yield return new WaitForSeconds(alpha); // Simulate fade out time
        // _isFading = false;

        #region Commented out code with fade with the fadeCanvasGroup
        _isFading = true;

        // Make sure the CanvasGroup blocks raycasts so player can't interact with the UI
        _fadeCanvasGroup.blocksRaycasts = true;

        // Calculate how fast the CanvasGroup should fade based on its current alpha, its final alpha, and how long it has to change between the two
        float fadeSpeed = Mathf.Abs(_fadeCanvasGroup.alpha - alpha) / _fadeDuration;

        // While the CanvasGroup hasn't reached the final alpha yet...
        while (!Mathf.Approximately(_fadeCanvasGroup.alpha, alpha))
        {
            // ... move the alpha towards its target alpha
            _fadeCanvasGroup.alpha = Mathf.MoveTowards(_fadeCanvasGroup.alpha, alpha, fadeSpeed * Time.deltaTime);

            // Wait for a frame then continue
            yield return null;
        }

        _isFading = false;

        // Stop blocking raycasts so player can interact with the UI
        _fadeCanvasGroup.blocksRaycasts = false;
        #endregion
    }
    #endregion
}
