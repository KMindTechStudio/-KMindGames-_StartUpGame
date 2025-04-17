using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> RoomList => _roomList;
    [SerializeField] private GameObject _roomPrefab;
    private List<Room> _roomList;
    private Room _currentRoom;
    private bool _isFading = false;
    private GameObject _player;
    private void Awake()
    {
        _roomList = new List<Room>();

        EventHandlers.OnGetOutRoom += OnGetOutRoom;
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        _currentRoom = CreateRoom();
    }

    private void OnGetOutRoom(Gate gate)
    {
        int? roomIndex = gate.ConnectedRoom?.RoomID;

        if (!roomIndex.HasValue)
        {
            gate.ConnectedRoom = CreateRoom();
        }


    }


    public Room CreateRoom()
    {
        GameObject newRoom = Instantiate(_roomPrefab, transform.position, Quaternion.identity);
        Room room = newRoom.GetComponent<Room>();
        room.RoomID = _roomList.Count;
        room.SetUpRoom(this);
        _roomList.Add(room);
        return room;
    }

    public void FadeAndLoadRoom(int roomIndex, Vector3 spawnPosition)
    {
        if (_isFading) return;
        StartCoroutine(FadeAndSwitchRoom(roomIndex, spawnPosition));
    }
    private IEnumerator FadeAndSwitchRoom(int roomIndex, Vector3 spawnPosition)
    {
        EventHandlers.CallOnBeforeRoomUnloadFadeOut();

        yield return StartCoroutine(Fade(0.5f));

        // Call save load room in here
        //////////////////////////////////////////////////

        // Set player position to spawn position
        _player.transform.position = spawnPosition;

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

        yield return StartCoroutine(Fade(0.5f));

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
        // Simulate fade out effect 
        _isFading = true;
        yield return new WaitForSeconds(alpha); // Simulate fade out time
        _isFading = false;

        #region Commented out code with fade with the fadeCanvasGroup
        //  isFading = true;

        //     // Make sure the CanvasGroup blocks raycasts so player can't interact with the UI
        //     fadeCanvasGroup.blocksRaycasts = true;

        //     // Calculate how fast the CanvasGroup should fade based on its current alpha, its final alpha, and how long it has to change between the two
        //     float fadeSpeed = Mathf.Abs(fadeCanvasGroup.alpha - alpha) / fadeDuration;

        //     // While the CanvasGroup hasn't reached the final alpha yet...
        //     while (!Mathf.Approximately(fadeCanvasGroup.alpha, alpha))
        //     {
        //         // ... move the alpha towards its target alpha
        //         fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, alpha, fadeSpeed * Time.deltaTime);

        //         // Wait for a frame then continue
        //         yield return null;
        //     }

        //     isFading = false;

        //     // Stop blocking raycasts so player can interact with the UI
        //     fadeCanvasGroup.blocksRaycasts = false;
        #endregion
    }
}
