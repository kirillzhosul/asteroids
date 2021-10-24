using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Speed for thrusting.
    [SerializeField]
    private float _thrustSpeed = 1.0f;

    // Bullet prefab for shooting.
    [SerializeField]
    private Bullet _bulletPrefab = null;

    // Speed for rotation.
    [SerializeField]
    private float _rotateSpeed = 1.0f;

    // Max thrust limit.
    [SerializeField]
    private float _maxThrust = 15.0f;

    // You may shoot every that time.
    [SerializeField]
    private float _shootSpeedLimitTime = 1.0f;

    // Start immortal time.
    [SerializeField]
    private float _startImmortalTime = 3.0f;

    // Start immortal blink speed.
    [SerializeField]
    private float _immortalBlinkDelay = 0.5f;

    // Score text object.
    [SerializeField]
    private Text _scoreText;

    // Lives text object.
    [SerializeField]
    private Text _livesText;

    // Default score.
    [SerializeField]
    private int _defaultScore = 0;

    // Default lives.
    [SerializeField]
    private int _defaultLives = 3;

    // Default score counter.
    private int _score;

    // Default lives counter.
    private int _lives;

    // Is we immortal or not.
    private bool _isImmortal = true;

    // If true, we can shoot.
    private bool _canShoot = true;

    // Is we thrusting or not.
    private bool _isThrusting = false;

    // Direction in which we rotation.
    private float _rotateDirection = 0.0f;

    // Our rigidbody.
    private Rigidbody2D _rigidbody = null;

    // Our sprite renderer.
    private SpriteRenderer _spriteRenderer = null;

    // Our player respawner.
    private PlayerRespawner _respawner = null;

    /// <summary>
    /// Initialising at awakening.
    /// </summary>
    private void Awake()
    {
        // Grab our rigidbody.
        if (this._rigidbody == null) this._rigidbody = GetComponent<Rigidbody2D>();

        // Grab our sprite renderer.
        if (this._spriteRenderer) this._spriteRenderer = GetComponent<SpriteRenderer>();

        // Grab our player respawner.
        if (this._respawner) this._respawner = FindObjectOfType<PlayerRespawner>();
    }

    /// <summary>
    /// Start initialisation.
    /// </summary>
    private void Start()
    {
        // Reset score and lives to default.
        this.ResetScoreAndLives();
        this._scoreText.text = this._score.ToString();
        this._livesText.text = this._lives.ToString();

        // Enable immortal.
        this.EnableImmortal();
    }

    /// <summary>
    /// Reset can shoot value (reset shoot speed limit).
    /// </summary>
    private void ResetShootSpeedLimit()
    {
        // Reset.
        this._canShoot = true;
    }

    /// <summary>
    /// Updating our controls.
    /// </summary>
    private void Update()
    {
        // Update is we thrusting or not.
        this._isThrusting = (Input.GetAxis("Vertical") > 0);

        // Update our rotate direction.
        this._rotateDirection = Input.GetAxis("Horizontal");

        // Shoot if we press shoot button.
        if (Input.GetButtonDown("Shoot"))
        {
            // Shoot.
            Shoot();
        }
    }

    /// <summary>
    /// Updating our physics.
    /// </summary>
    private void FixedUpdate()
    {
        if (this._isThrusting)
        {
            // If we are thrusting.

            // Add force.
            _rigidbody.AddForce(this.transform.up * this._thrustSpeed);
        }

        if (this._rotateDirection != 0.0f)
        {
            // If we are turning in any direction.

            // Add torque.
            _rigidbody.AddTorque(this._rotateDirection * -1 * this._rotateSpeed);
        }

        // Clamp max thrust.
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxThrust);
    }

    /// <summary>
    /// Creates and projects bullet.
    /// </summary>
    private void Shoot()
    {
        // Return if not can shoot.
        if (!this._canShoot) return;

        // Instantiate bullet.
        Bullet bullet = Instantiate(this._bulletPrefab, this.transform.position, this.transform.rotation);

        // Project bullet.
        bullet.Project(this.transform.up);

        // Reset can shoot.
        this._canShoot = false;
        Invoke(nameof(ResetShootSpeedLimit), _shootSpeedLimitTime);
    }

    /// <summary>
    /// Start immortal state.
    /// </summary>
    public IEnumerator StartImmortal()
    {
        // Enable immortal.
        this._isImmortal = true;

        // Start indication.
        StartCoroutine(IndicateImmortal());

        // Wait time.
        yield return new WaitForSeconds(this._startImmortalTime);

        // Disable immortal.
        this._isImmortal = false;
    }

    /// <summary>
    /// Indicates immortal.
    /// </summary>
    private IEnumerator IndicateImmortal()
    {
        while (this._isImmortal)
        {
            // While we are immortal.

            // Switch.
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(this._immortalBlinkDelay);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(this._immortalBlinkDelay);
        }
    }

    /// <summary>
    /// Check for collision.
    /// </summary>
    /// <param name="collision"> Other collide object. </param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            // If this is asteroid.

            if (!this._isImmortal)
            {
                // If we are not immortal.

                // Set velocity to zero.
                this._rigidbody.velocity = Vector3.zero;
                this._rigidbody.angularVelocity = 0.0f;

                // Disable our game object.
                this.gameObject.SetActive(false);

                // Decrease lives.
                this._lives--;
                this._livesText.text = this._lives.ToString();

                // Call player died of player respawner object method.
                this._respawner.PlayerDied();
            }
        }
    }

    /// <summary>
    /// Returns lives.
    /// </summary>
    /// <returns>Lives</returns>
    public int GetLives()
    {
        // Return lives.
        return this._lives;
    }

    /// <summary>
    /// Increase score by given amount.
    /// </summary>
    /// <param name="amount">Amount to add</param>
    public void AddScore(int amount)
    {
        // Add score.
        this._score += amount;
        this._scoreText.text = this._score.ToString();
    }

    /// <summary>
    /// Reset values to zero.
    /// </summary>
    public void ResetScoreAndLives()
    {
        // Reset score.
        this._score = this._defaultScore;
        this._scoreText.text = this._score.ToString();

        // Reset lives.
        this._lives = this._defaultLives;
        this._livesText.text = this._lives.ToString();
    }

    /// <summary>
    /// Enables immortal state.
    /// </summary>
    public void EnableImmortal()
    {
        // Enable immortal.
        StartCoroutine(StartImmortal());
    }
}
