using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    // Delay for respawning.
    [SerializeField]
    private float _respawnDelay = 3.0f;

    // Player object.
    private Player _player = null;

    /// <summary>
    /// Initialising.
    /// </summary>
    public void Awake()
    {
        // Grab our player.
        this._player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// Should be called when player is died from player, respawn player after some respawn delay time.
    /// </summary>
    public void PlayerDied()
    {
        if (this._player.GetLives() == 0)
        {
            // If no more lives.

            // Reset player.
            this._player.ResetScoreAndLives();
        }

        // Call respawn after delay.
        Invoke(nameof(RespawnPlayer), _respawnDelay);
    }

    /// <summary>
    /// Respawns player.
    /// </summary>
    public void RespawnPlayer()
    {
        // Enable player.
        this._player.gameObject.SetActive(true);
        this._player.transform.position = Vector3.zero;
        this._player.EnableImmortal();
    }
}
