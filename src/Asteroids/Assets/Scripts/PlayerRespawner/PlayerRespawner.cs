using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerRespawner : MonoBehaviour
{
    // Delay for respawning.
    [SerializeField] private float _respawnDelay = 3.0f;

    // Player object.
    private Player _player = null;

    // Death audio source.
    private AudioSource _deathAudioSource = null;

    /// <summary>
    /// Initialising components at awakening.
    /// </summary>
    public void Awake()
    {
        // Grab player.
        if (_player == null) _player = FindObjectOfType<Player>();
        if (_player == null) Debug.Log("Unable to get player for player respawner.");

        // Get audio source.
        if (_deathAudioSource == null) _deathAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Should be called when player is died from player, respawn player after some respawn delay time.
    /// </summary>
    public void PlayerDiedCallback()
    {
        // Play death sound.
        _deathAudioSource.Play(0);

        // Call respawn after delay.
        Invoke(nameof(RespawnPlayer), _respawnDelay);
    }

    /// <summary>
    /// Respawns player.
    /// </summary>
    public void RespawnPlayer()
    {
        // Enable player and call respawn callback.
        _player.gameObject.SetActive(true);
        _player.RespawnCallback();

        // Call asteroids spawner player respawn callback.
        AsteroidsSpawner spawner = FindObjectOfType<AsteroidsSpawner>();
        if (spawner != null) spawner.PlayerRespawnedCallback();
        if (spawner == null) Debug.Log("Unable to call spawner player respawned callback.");
    }
}
