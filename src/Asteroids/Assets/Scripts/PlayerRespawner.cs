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

    // Death audio source.
    private AudioSource _deathAudioSource = null;

    /// <summary>
    /// Initialising.
    /// </summary>
    public void Awake()
    {
        // Grab our player.
        if (this._player == null) this._player = FindObjectOfType<Player>();

        // Get audio source.
        if (this._deathAudioSource == null) this._deathAudioSource = this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Should be called when player is died from player, respawn player after some respawn delay time.
    /// </summary>
    public void PlayerDied()
    {
        // Play death sound.
        this._deathAudioSource.Play(0);

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

        // Reset spawner count.
        AsteroidsSpawner spawner = FindObjectOfType<AsteroidsSpawner>();
        if (spawner == null) spawner.ResetSpawnAmount();

        if (this._player.GetLives() == 0)
        {
            // If no more lives.

            // Reset player.
            this._player.ResetScoreAndLives();
        }
    }
}
