using System;
using UnityEngine;

public class EventHandlers : MonoBehaviour
{

    #region Player Events
    /// <summary>
    /// Event when player die.
    /// </summary>
    public static event Action OnPlayerDie;
    /// <summary>
    /// Caall event when player die.
    /// </summary>
    public static void CallOnPlayerDie()
    {
        OnPlayerDie?.Invoke();
    }
    #endregion
    #region Room Events
    /// <summary>
    /// Event when player move out of the gate that direction is towards to the other room.
    /// </summary>
    public static event Action<Gate> OnGetOutRoom;

    /// <summary>
    /// Call event when player move out of the gate that direction is towards to the other room.
    /// </summary>
    public static void CallOnGetOutRoom(Gate gate)
    {
        OnGetOutRoom?.Invoke(gate);
    }

    /// <summary>
    /// Event when player enter the main room area.
    /// </summary>
    public static event Action<Room> OnEnterRoom;

    /// <summary>
    /// Call event when the player enters the main room area.
    /// </summary>
    /// <param name="gate"></param>
    public static void CallOnEnterRoom(Room room)
    {
        OnEnterRoom?.Invoke(room);
    }

    public static event Action<Room> OnCompleteRoom;
    public static void CallOnCompleteRoom(Room room)
    {
        OnCompleteRoom?.Invoke(room);
    }

    public static event Action<Room> OnActivateTower;
    public static void CallOnActivateTower(Room room)
    {
        OnActivateTower?.Invoke(room);
    }

    #region Room Load Events
    // Before the room unload fade out - Fade out event
    public static event Action OnBeforeRoomUnloadFadeOut;
    public static void CallOnBeforeRoomUnloadFadeOut()
    {
        OnBeforeRoomUnloadFadeOut?.Invoke();
    }

    public static event Action OnBeforeRoomUnload;
    public static void CallOnBeforeRoomUnload()
    {
        OnBeforeRoomUnload?.Invoke();
    }

    public static event Action OnAfterRoomLoad;
    public static void CallOnAfterRoomLoad()
    {
        OnAfterRoomLoad?.Invoke();
    }

    public static event Action OnAfterRoomFadeIn;
    public static void CallOnAfterRoomFadeIn()
    {
        OnAfterRoomFadeIn?.Invoke();
    }
    #endregion
    #endregion
}
