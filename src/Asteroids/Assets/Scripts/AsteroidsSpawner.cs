using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
    // Asteroid prefab to instantiate (Spawn).
    [SerializeField]
    private Asteroid _asteroidPrefab = null;

    // Asteroid destroy effect.
    [SerializeField]
    private ParticleSystem _explosionEffect;

    // Spawn amount to spawn.
    [SerializeField]
    private int _spawnAmount = 1;

    // Spawn distance to spawn.
    [SerializeField]
    private float _spawnDistance = 15.0f;

    // If true, enables time spawning.
    [SerializeField]
    private bool _spawnEnabled = false;

    // Time for respawn new asteroids.
    [SerializeField]
    public float respawnAfter = 2.0f;

    // If true, enables more spawning from counter.
    [SerializeField]
    private bool _spawnAmountIncreasingEnabled = true;

    // Spawn delay for spawner.
    [SerializeField]
    private float _spawnDelay = 3.0f;

    // Explosion audio source.
    private AudioSource _explosionAudioSource = null;

    /// <summary>
    /// Initialises spawner.
    /// </summary>
    private void Start()
    {
        // Get audio source.
        _explosionAudioSource = GetComponent<AudioSource>();

        // Spawn one at the start.
        Spawn();

        // Make repeating spawning.
        if (_spawnEnabled)
        {
            InvokeRepeating(nameof(Spawn), this._spawnDelay, this._spawnDelay);
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
        this._explosionEffect.transform.localScale = Vector3.one * size;
        this._explosionEffect.transform.position = at;
        this._explosionEffect.Play();

        // Audio source.
        this._explosionAudioSource.Play(0);
    }

    /// <summary>
    /// Spawns new asteroid.
    /// </summary>
    public void Spawn()
    {
        for (int i = 0; i < this._spawnAmount; i++)
        {
            // Iterate over spawn amount.

            // Get random position.
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this._spawnDistance;
            Vector3 spawnPosition = this.transform.position + spawnDirection;

            // Rotation.
            float variance = Random.Range(-15.0f, 15.0f);
            Quaternion rotation = Quaternion.AngleAxis(variance, this.transform.forward);

            // Create new asteroid.
            Asteroid asteroid = Instantiate(this._asteroidPrefab, spawnPosition, rotation);
        }

        // Increase spawn amount.
        if (this._spawnAmountIncreasingEnabled)
        {
            // Increase spawn amount.
            this._spawnAmount ++;
        }
    }
}
