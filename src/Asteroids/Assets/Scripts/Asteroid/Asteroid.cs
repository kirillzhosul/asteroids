using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    // Random selected sprites array.
    [SerializeField] private Sprite[] _sprites;

    // Scores result for destroying asteroid.
    [SerializeField] private int[] _scores = { 100, 50, 20 };

    // Components.
    private Rigidbody2D _rigidbody = null;
    private SpriteRenderer _spriteRenderer = null;
    private AsteroidsSpawner _spawner = null;

    // Size of the asteroid.
    private int _size = 3;

    // Bullet tag.
    private const string TAG_BULLET = "Bullet";

    /// <summary>
    /// Initialising components at awakening.
    /// </summary>
    private void Awake()
    {
        // Grab rigidbody.
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();

        // Grab sprite renderer.
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

        // Grab spawner.
        if (_spawner == null) _spawner = FindObjectOfType<AsteroidsSpawner>();
        if (_spawner == null) Debug.Log("Unable to get asteroids spawner for asteroid.");
    }

    /// <summary>
    /// Initialise.
    /// </summary>
    private void Start()
    {
        // Choose random sprite.
        _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];

        // Rotate random angle.
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);

        // Set size at start.
        SetSize(_size);
    }

    /// <summary>
    /// When we enter collision 2D.
    /// </summary>
    /// <param name="collision"> Other collision. </param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_BULLET))
        {
            // If this is bullet.

            if (_size <= 1)
            {
                // If last size.

                // Call spawner asteroid destroy callback.
                _spawner.AsteroidDestroyCallback();
            }
            else
            {
                // If other size.

                // Split.
                Split();
                Split();
            }

            // Add score to player.
            Player player = FindObjectOfType<Player>();
            if (player != null) player.AddScore(GetScoreReward());
            if (player == null) Debug.Log("Unable to add score for asteroid destroying to player.");

            // Play destroy effect.
            _spawner.PlayExplodeEffect(transform.position, _size);

            // Destroy self.
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets size for asteroid.
    /// </summary>
    /// <param name="size">Size to set.</param>
    public void SetSize(int size)
    {
        // Update size.
        _size = size;
        transform.localScale = Vector3.one * this._size;
    }

    /// <summary>
    /// Returns asteroid score rewars.
    /// </summary>
    /// <returns>Score reward</returns>
    private int GetScoreReward()
    {
        // Check if valid.
        if (_size <= 1) return 0;
        if (_size >= _scores.Length) return _scores[_scores.Length - 1];

        // Returning score.
        return _scores[_size - 1];
    }

    /// <summary>
    /// Splits asteroids in half.
    /// </summary>
    private void Split()
    {
        // Return if last size (Dont split if last size).
        if (_size == 1) return;

        // Get position to split at.
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        // Create asteroid new asteroid.
        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.SetSize(_size - 1);
    }
}
