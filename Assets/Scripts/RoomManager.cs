using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public Room LastestTowerRoom { get; set; }
    public List<Room> RoomList => _roomList;
    [SerializeField] private GameObject _roomPrefab;

    [Space(10)]
    [Header("Fade Animation")]
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private CanvasGroup _fadeCanvasGroup;
    [Header("Tower Check Point")]
    [SerializeField] private Vector2 _nextHasTowerRound;
    private int _nextTowerCounter = 0;
    private bool _isFading = false;
    private List<Room> _roomList;
    private Room _currentRoom;
    private GameObject _player;
    private void Awake()
    {
        _roomList = new List<Room>();
        _player = GameObject.FindGameObjectWithTag(Helpers.Tag.Player);
        EventHandlers.OnGetOutRoom += OnGetOutRoom;
        EventHandlers.OnPlayerDie += OnPlayerDie;
    }

    private void Start()
    {
        _nextTowerCounter = 1;
        _currentRoom = CreateRoom(ShouldHasTowerCheckPoint(), true);
        FadeAndLoadRoom(_currentRoom.RoomID, new Vector3(0, 0, 0), () =>
        {
            EventHandlers.CallOnActivateTower(_currentRoom);
        });
    }
    private void OnDestroy()
    {
        EventHandlers.OnGetOutRoom -= OnGetOutRoom;
    }
    private void OnGetOutRoom(Gate gate)
    {
        Room room = gate.ConnectedRoom;

        // Check if the gate is connected to another room
        if (room == null)
        {
            room = CreateRoom(ShouldHasTowerCheckPoint());
            gate.ConnectTo(_currentRoom, room, room.GateIn);
        }

        // Call fade and load the given room
        FadeAndLoadRoom(room.RoomID, gate.ConnectedGate.SpawnPoint.position);
    }
    private void OnPlayerDie()
    {
        // Reset current room state
        _currentRoom.ResetRoom();
        // Fade and load to thelastest room that has tower checkpoint
        FadeAndLoadRoom(LastestTowerRoom.RoomID, LastestTowerRoom.ReviveTransform.position);
        // Set the lastest tower room to the start room
        LastestTowerRoom = _roomList[0];
    }
    /// <summary>
    /// Create a new room and add it to the list.
    /// The new room will be inactive by default.
    /// </summary>
    /// <returns></returns>
    public Room CreateRoom(bool hasTower = false, bool isActivateTower = false)
    {
        GameObject newRoom = Instantiate(_roomPrefab, transform.position, Quaternion.identity);
        Room room = newRoom.GetComponent<Room>();
        room.RoomID = _roomList.Count;
        newRoom.name = "Room " + room.RoomID;
        room.SetUpRoom(this, hasTower, isActivateTower);
        room.gameObject.SetActive(false);
        _roomList.Add(room);
        return room;
    }
    private bool ShouldHasTowerCheckPoint()
    {
        bool hasTower = _nextTowerCounter == 1;

        if (hasTower)
        {
            _nextTowerCounter = UnityEngine.Random.Range((int)_nextHasTowerRound.x, (int)_nextHasTowerRound.y + 1);
        }
        else
        {
            _nextTowerCounter--;
        }

        return hasTower;
    }
    #region Fade and Load Room

    // Call this method to fade and load the room only when not fading
    private void FadeAndLoadRoom(int roomIndex, Vector3 spawnPosition, Action callback = null)
    {
        if (_isFading) return;
        StartCoroutine(FadeAndSwitchRoom(roomIndex, spawnPosition, callback));
    }

    // Switch room process 
    private IEnumerator FadeAndSwitchRoom(int roomIndex, Vector3 spawnPosition, Action callback = null)
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

        // Callback Action
        callback?.Invoke();

        yield return StartCoroutine(Fade(0f));

        // Call after room fade in event
        EventHandlers.CallOnAfterRoomFadeIn();
    }

    // Load the room and set its active
    private void LoadAndActiveRoom(int roomIndex)
    {
        _currentRoom = _roomList[roomIndex];
        _currentRoom.gameObject.SetActive(true);
    }

    // Fade animation
    private IEnumerator Fade(float alpha)
    {
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
    }
    #endregion
}
