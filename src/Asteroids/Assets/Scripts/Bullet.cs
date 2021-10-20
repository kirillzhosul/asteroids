using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Our rigidbody.
    private Rigidbody2D _rigidbody = null;

    // Fly speed.
    [SerializeField]
    private float _moveSpeed = 500.0f;

    // Life time of a bullet.
    [SerializeField]
    private float _lifetime = 10.0f;

    /// <summary>
    /// Initialising physics at awakening.
    /// </summary>
    private void Awake()
    {
        // Grab our rigidbody.
        this._rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Projects bullet in direction.
    /// </summary>
    /// <param name="direction"> Direction to project on to. </param>
    public void Project(Vector2 direction)
    {
        // Add force.
        this._rigidbody.AddForce(direction * this._moveSpeed);

        // Destroy after lifetime ends.
        Destroy(this.gameObject, _lifetime);
    }

    /// <summary>
    /// When we enter collision 2D.
    /// </summary>
    /// <param name="collision"> Other collision. </param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy self.
        Destroy(this.gameObject);
    }
}
