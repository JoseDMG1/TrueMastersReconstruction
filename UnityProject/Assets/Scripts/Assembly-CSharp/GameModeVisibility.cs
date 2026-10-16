using System;
using UnityEngine;

public class GameModeVisibility : MonoBehaviour
{
    [Tooltip("List of game modes in which this object should be visible")]
    [SerializeField]
    private GameMode[] visibleInModes;

    private void Awake()
    {
        // Native VA 0x1236AD4 destroys the object when Contains(mode) is false.
        if (Array.IndexOf(visibleInModes, SystemManager.i.mode) < 0)
            Destroy(gameObject);
    }
}

