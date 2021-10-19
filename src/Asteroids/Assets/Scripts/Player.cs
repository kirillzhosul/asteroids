using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Speed for thrusting.
    [SerializeField]
    private float _thrustSpeed = 1.0f;

    // Speed for rotation.
    [SerializeField]
    private float _rotateSpeed = 1.0f;

    [SerializeField]
    private float _maxThrust = 15.0f;

    // Is we thrusting or not.
    private bool _isThrusting = false;

    // Direction in which we rotation.
    private float _rotateDirection = 0.0f;

    // Our player rigidbody.
    private Rigidbody2D _rigidbody = null;

    /// <summary>
    /// Initialising physics at awakening.
    /// </summary>
    private void Awake()
    {
        // Grab our rigidbody.
        this._rigidbody = GetComponent<Rigidbody2D>();
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
}
