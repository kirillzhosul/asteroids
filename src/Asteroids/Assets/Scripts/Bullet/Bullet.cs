using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    // Move speed.
    [SerializeField] private float _moveSpeed = 500.0f;

    // Life time of a bullet.
    [SerializeField] private float _lifetime = 10.0f;

    // Our rigidbody.
    private Rigidbody2D _rigidbody = null;

    /// <summary>
    /// Initialising components at awakening.
    /// </summary>
    private void Awake()
    {
        // Grab our rigidbody.
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Projects bullet in direction.
    /// </summary>
    /// <param name="direction"> Direction to project on to. </param>
    public void Project(Vector2 direction)
    {
        // Add force.
        _rigidbody.AddForce(direction * this._moveSpeed);

        // Destroy after lifetime ends.
        Destroy(gameObject, _lifetime);
    }

    /// <summary>
    /// When we enter collision 2D.
    /// </summary>
    private void OnCollisionEnter2D()
    {
        // Destroy self.
        Destroy(gameObject);
    }
}
