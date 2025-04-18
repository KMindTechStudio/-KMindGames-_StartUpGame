using UnityEngine;

public static class Helpers
{
    public enum GateType
    {
        In,
        Out,
    }
    public enum GatePosition
    {
        None = -1,
        North = 0,
        South = 1,
        East = 2,
        West = 3
    }

    public static class Tag
    {
        public const string Player = "Player";
    }
}
