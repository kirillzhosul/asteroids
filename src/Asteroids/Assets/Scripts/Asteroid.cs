using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Sprites array.
    [SerializeField]
    private Sprite[] _sprites;

    // Scores 
    [SerializeField]
    private int[] _scores = { 100, 50, 20 };

    // Our rigidbody.
    private Rigidbody2D _rigidbody = null;

    // Our sprite renderer.
    private SpriteRenderer _spriteRenderer = null;

    // Our asteroids spawner.
    private AsteroidsSpawner _spawner = null;

    // Size of the asteroid.
    private int _size = 3;

    // Bullet tag.
    private const string TAG_BULLET = "Bullet";

    /// <summary>
    /// Initialising at awakening.
    /// </summary>
    private void Awake()
    {
        // Grab our rigidbody.
        if (this._rigidbody == null) this._rigidbody = GetComponent<Rigidbody2D>();

        // Grab our sprite renderer.
        if (this._spriteRenderer == null) this._spriteRenderer = GetComponent<SpriteRenderer>();

        // Grab our spawner.
        // GameObject.Find("AsteroidsSpawner").
        if (this._spawner == null) this._spawner = FindObjectOfType<AsteroidsSpawner>();
    }

    /// <summary>
    /// Initialise.
    /// </summary>
    private void Start()
    {
        // Choose random sprite.
        this._spriteRenderer.sprite = _sprites[Random.Range(0, this._sprites.Length)];

        // Rotate random angle.
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);

        // Set size.
        this.setSize(this._size);
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

            if (this._size == 1)
            {
                // If we last size.

                // Spawn new.
                if (GameObject.FindGameObjectsWithTag("Asteroid").Length == 1)
                {
                    // Spawn after time.
                    Invoke(nameof(this._spawner.Spawn), this._spawner.respawnAfter);
                }
            }
            else
            {
                // Split.
                this.Split();
                this.Split();
            }

            // Add score to player.
            Player player = FindObjectOfType<Player>();
            if (player != null) player.AddScore(this.GetScoreReward());

            // Play destroy effect.
            this._spawner.PlayExplodeEffect(this.transform.position, this._size);

            // Destroy self.
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Sets size for asteroid.
    /// </summary>
    /// <param name="size">Size to set.</param>
    public void setSize(int size)
    {
        // Update size.
        this._size = size;
        this.transform.localScale = Vector3.one * this._size;
    }

    /// <summary>
    /// Returns asteroid score rewars.
    /// </summary>
    /// <returns>Score reward</returns>
    private int GetScoreReward()
    {
        // Check if valid.
        if (this._size <= 1) return 0;
        if (this._size >= this._scores.Length) return 0;

        // Returning score.
        return this._scores[this._size - 1];
    }

    /// <summary>
    /// Splits asteroids in half.
    /// </summary>
    private void Split()
    {
        // Return if last size.
        if (this._size == 1) return;

        // Get position.
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        // Create asteroid.
        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.setSize(_size - 1);
    }
}
