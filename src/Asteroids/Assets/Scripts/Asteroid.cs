using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Sprites array.
    [SerializeField]
    private Sprite[] _sprites;

    // Our rigidbody.
    private Rigidbody2D _rigidbody = null;

    // Our sprite renderer.
    private SpriteRenderer _spriteRenderer = null;

    /// <summary>
    /// Initialising at awakening.
    /// </summary>
    private void Awake()
    {
        // Grab our rigidbody.
        this._rigidbody = GetComponent<Rigidbody2D>();

        // Grab our sprite renderer.
        this._spriteRenderer = GetComponent<SpriteRenderer>();
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
    }
}
