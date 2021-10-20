using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
    // Asteroid prefab to instantiate (Spawn).
    [SerializeField]
    private Asteroid _asteroidPrefab = null;

    // Spawn amount to spawn.
    [SerializeField]
    private int _spawnAmount = 1;

    // Spawn distance to spawn.
    [SerializeField]
    private float _spawnDistance = 15.0f;

    // If true, enables time spawning.
    [SerializeField]
    private bool _spawnEnabled = false;

    // Spawn delay for spawner.
    [SerializeField]
    private float _spawnDelay = 3.0f;

    /// <summary>
    /// Initialises spawner.
    /// </summary>
    private void Start()
    {
        // Spawn one at the start.
        Spawn();

        // Make repeating spawning.
        if (_spawnEnabled)
        {
            InvokeRepeating(nameof(Spawn), this._spawnDelay, this._spawnDelay);
        }
        
    }
    
    /// <summary>
    /// Spawns new asteroid.
    /// </summary>
    private void Spawn()
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
        
    }
}
