using UnityEngine;
using Pathfinding;

public enum NPCState
{
    PatrolIdle,
    PatrolWalk,
    Chase,
}

public enum EnumMoveState
{
    Move,
    Climp,
}

public enum EnumClimbDir
{
    None,
    WU,
    WD,
    AU,
    AD,
}

public enum Axis
{
    X,
    Y,
    Z,
}