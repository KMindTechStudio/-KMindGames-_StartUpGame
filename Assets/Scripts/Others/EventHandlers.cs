using System;
using UnityEngine;

public class EventHandlers : MonoBehaviour
{
    public static event Action<Gate> OnGetOutRoom;

    /// <summary>
    /// Call when the player gets out of the gate.
    /// </summary>
    public static void CallOnGetOutRoom(Gate gate)
    {
        Debug.Log("CallOnGetOutRoom: " + gate.name);
        OnGetOutRoom?.Invoke(gate);
    }

    public static event Action<Gate> OnEnterRoom;

    /// <summary>
    /// Call when the player enters the room.
    /// </summary>
    /// <param name="gate"></param>
    public static void CallOnEnterRoom(Gate gate)
    {
        OnEnterRoom?.Invoke(gate);
    }

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


}
