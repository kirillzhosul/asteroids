using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    // Speed for thrusting.
    [SerializeField] private float _thrustSpeed = 1.0f;

    // Bullet prefab for shooting.
    [SerializeField] private Bullet _bulletPrefab = null;

    // Speed for rotation.
    [SerializeField] private float _rotateSpeed = 1.0f;

    // Max thrust limit.
    [SerializeField] private float _maxThrust = 15.0f;

    // You may shoot every that time.
    [SerializeField] private float _shootSpeedLimitTime = 1.0f;

    // Start immortal time.
    [SerializeField] private float _startImmortalTime = 3.0f;

    // Start immortal blink speed.
    [SerializeField] private float _immortalBlinkDelay = 0.5f;

    // Score text object.
    [SerializeField] private Text _scoreText;

    // Lives text object.
    [SerializeField] private Text _livesText;

    // Default score.
    [SerializeField] private int _defaultScore = 0;

    // Default lives.
    [SerializeField] private int _defaultLives = 3;

    // Counters.
    private int _score;
    private int _lives;

    // Is we immortal or not.
    private bool _isImmortal = true;

    // If true, we can shoot.
    private bool _canShoot = true;

    // Is we thrusting or not.
    private bool _isThrusting = false;

    // Direction in which we rotation.
    private float _rotateDirection = 0.0f;

    // Asteroid tag.
    private const string TAG_ASTEROID = "Asteroid";

    // Components.
    private Rigidbody2D _rigidbody = null;
    private SpriteRenderer _spriteRenderer = null;
    private PlayerRespawner _respawner = null;

    /// <summary>
    /// Initialising components at awakening.
    /// </summary>
    private void Awake()
    {
        // Grab rigidbody.
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();

        // Grab sprite renderer.
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

        // Grab our player respawner.
        if (_respawner == null) _respawner = FindObjectOfType<PlayerRespawner>();
        if (_respawner == null) Debug.Log("Unable to get player respawner for player object.");
    }

    /// <summary>
    /// Start initialisation.
    /// </summary>
    private void Start()
    {
        // Reset score and lives to default.
        ResetScoreAndLives();
        _scoreText.text = _score.ToString();
        _livesText.text = _lives.ToString();

        // Enable immortal.
        EnableImmortal();
    }

    /// <summary>
    /// Reset can shoot value (reset shoot speed limit).
    /// </summary>
    private void ResetShootSpeedLimit()
    {
        // Reset.
        _canShoot = true;
    }

    /// <summary>
    /// Updating our controls.
    /// </summary>
    private void Update()
    {
        // Update is we thrusting or not.
        _isThrusting = (Input.GetAxis("Vertical") > 0);

        // Update our rotate direction.
        _rotateDirection = Input.GetAxis("Horizontal");

        // Shoot if we press shoot button.
        if (Input.GetButtonDown("Shoot")) Shoot();
    }

    /// <summary>
    /// Updating physics.
    /// </summary>
    private void FixedUpdate()
    {
        if (_isThrusting)
        {
            // If we are thrusting.

            // Add force.
            _rigidbody.AddForce(transform.up * _thrustSpeed);
        }

        if (_rotateDirection != 0.0f)
        {
            // If we are turning in any direction.

            // Add torque.
            _rigidbody.AddTorque(_rotateDirection * -1 * _rotateSpeed);
        }

        // Clamp max thrust.
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxThrust);
    }

    /// <summary>
    /// Creates and projects bullet.
    /// </summary>
    private void Shoot()
    {
        // Return if can`t shoot.
        if (!_canShoot) return;

        // Instantiate bullet.
        Bullet bullet = Instantiate(this._bulletPrefab, this.transform.position, this.transform.rotation);

        // Project bullet.
        bullet.Project(transform.up);

        // Reset can shoot.
        _canShoot = false;
        Invoke(nameof(ResetShootSpeedLimit), _shootSpeedLimitTime);
    }

    /// <summary>
    /// Check for collision.
    /// </summary>
    /// <param name="collision"> Other collide object. </param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_ASTEROID))
        {
            // If this is asteroid.

            // We are immortal, dont process any damage.
            if (_isImmortal) return;

            // Set velocity to zero.
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0.0f;

            // Disable game object.
            gameObject.SetActive(false);

            // Decrease lives.
            _lives--;
            _livesText.text = _lives.ToString();

            // Call player died of player respawner object method.
            _respawner.PlayerDiedCallback();
        }
    }

    /// <summary>
    /// Reset values to zero.
    /// </summary>
    private void ResetScoreAndLives()
    {
        // Reset score.
        _score = _defaultScore;
        _scoreText.text = _score.ToString();

        // Reset lives.
        _lives = _defaultLives;
        _livesText.text = _lives.ToString();
    }

    /// <summary>
    /// Start immortal state.
    /// </summary>
    private IEnumerator StartImmortal()
    {
        // Enable immortal state.
        _isImmortal = true;

        // Start indication.
        StartCoroutine(IndicateImmortal());

        // Wait time.
        yield return new WaitForSeconds(_startImmortalTime);

        // Disable immortal state.
        _isImmortal = false;
    }

    /// <summary>
    /// Indicates immortal.
    /// </summary>
    private IEnumerator IndicateImmortal()
    {
        while (_isImmortal)
        {
            // While we are immortal.

            // Switch.
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(_immortalBlinkDelay);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(_immortalBlinkDelay);
        }
    }

    /// <summary>
    /// Enables immortal state.
    /// </summary>
    public void EnableImmortal()
    {
        // Enable immortal.
        StartCoroutine(StartImmortal());
    }

    /// <summary>
    /// Increase score by given amount.
    /// </summary>
    /// <param name="amount">Amount to add</param>
    public void AddScore(int amount)
    {
        // Add score.
        _score += amount;
        _scoreText.text = _score.ToString();
    }

    /// <summary>
    /// Player respawned callback (Actually, from respawner).
    /// </summary>
    public void RespawnCallback()
    {
        if (_lives == 0)
        {
            // If no more lives.

            // Reset player.
            ResetScoreAndLives();
        }
    }
}
