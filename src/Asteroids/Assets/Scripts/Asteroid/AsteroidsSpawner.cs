using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AsteroidsSpawner : MonoBehaviour
{
    // Asteroid prefab to spawn.
    [SerializeField] private Asteroid _asteroidPrefab = null;

    // Asteroid destroy effect.
    [SerializeField] private ParticleSystem _explosionEffect;

    // Spawn amount at start.
    [SerializeField] private int _spawnAmount = 1;

    // Spawn distance.
    [SerializeField] private float _spawnDistance = 15.0f;

    // If true, enables spawning by time.
    [SerializeField] private bool _spawnByTimeEnabled = false;

    // Time for respawn new asteroids.
    [SerializeField] public float respawnAfter = 2.0f;

    // If true, enables increasing spawning amount each spawn time.
    [SerializeField]  private bool _spawnAmountIncreasingEnabled = true;

    // Spawn delay (if _spawnByTimeEnabled).
    [SerializeField] private float _spawnDelay = 3.0f;

    // Explosion audio source.
    private AudioSource _explosionAudioSource = null;

    // Asteroid tag.
    private const string TAG_ASTEROID = "Asteroid";

    /// <summary>
    /// Component initialisation at awakening.
    /// </summary>
    private void Awake()
    {
        // Get audio source.
        if (_explosionAudioSource == null) _explosionAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Initialises spawner.
    /// </summary>
    private void Start()
    {
        // Spawn one at the start.
        Spawn();

        // Make repeating spawning.
        if (_spawnByTimeEnabled)
        {
            InvokeRepeating(nameof(Spawn), _spawnDelay, _spawnDelay);
        } 
    }
    
    /// <summary>
    /// Plays explode event.
    /// </summary>
    /// <param name="at"> Position to play at. </param>
    /// <param name="size"></param>
    public void PlayExplodeEffect(Vector3 at, float size)
    {
        // Play particles.
        _explosionEffect.transform.localScale = Vector3.one * size;
        _explosionEffect.transform.position = at;
        _explosionEffect.Play();

        // Play audio source.
        _explosionAudioSource.Play(0);
    }

    /// <summary>
    /// Spawns new asteroid and works with next spawn amount.
    /// </summary>
    public void Spawn()
    {
        for (int i = 0; i <_spawnAmount; i++)
        {
            // Iterate over spawn amount.

            // Get random position and direction.
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * _spawnDistance;
            Vector3 spawnPosition = transform.position + spawnDirection;

            // Get random rotation and variance.
            float variance = Random.Range(-15.0f, 15.0f);
            Quaternion rotation = Quaternion.AngleAxis(variance, transform.forward);

            // Create new asteroid.
            Instantiate(_asteroidPrefab, spawnPosition, rotation);
        }

        // Increase spawn amount if enabled.
        if (_spawnAmountIncreasingEnabled) _spawnAmount ++;
    }

    /// <summary>
    /// Resets spawn amount.
    /// </summary>
    public void ResetSpawnAmount()
    {
        // Reset.
        _spawnAmount = 0;
    }

    /// <summary>
    /// Player respawn callback (Actually, from PlayerRespawer).
    /// </summary>
    public void PlayerRespawnedCallback()
    {
        // Just reset spawn amount.
        ResetSpawnAmount();
    }

    /// <summary>
    /// Asteroid destroy callback (Actually, from asteroid itself).
    /// </summary>
    public void AsteroidDestroyCallback()
    {
        // Is that asteroid which called callback - last?
        bool lastAsteroid = GameObject.FindGameObjectsWithTag(TAG_ASTEROID).Length == 1;

        // Spawn new after delay.
        if (lastAsteroid) Invoke(nameof(Spawn), respawnAfter);
    }
}