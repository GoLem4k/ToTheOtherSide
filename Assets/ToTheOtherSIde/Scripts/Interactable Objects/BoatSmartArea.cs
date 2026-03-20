using System;
using UnityEngine;

public class BoatSmartArea : SmartArea
{
    private void Awake()
    {
        OnPlayerEnter += CatchPlayer;
        OnPlayerExit += UnCatchPlayer;
    }

    private void CatchPlayer(GameObject player)
    {
        player.GetComponent<PlayerMovementController>().SetPlatform(transform);
    }

    private void UnCatchPlayer(GameObject player)
    {
        player.GetComponent<PlayerMovementController>().SetPlatform(null);
    }
}
