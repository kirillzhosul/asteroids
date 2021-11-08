using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Player))]
public class PlayerImmortalIndicator : MonoBehaviour
{
    // Start immortal blink speed.
    [SerializeField] private float _immortalBlinkDelay = 0.5f;

    // Components.
    private SpriteRenderer _spriteRenderer = null;
    private Player _player = null;

    /// <summary>
    /// Initialising components at awakening.
    /// </summary>
    private void Awake()
    {
        // Grab sprite renderer.
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

        // Grab player.
        if (_player == null) _player = GetComponent<Player>();
    }

    /// <summary>
    /// Indicates immortal.
    /// </summary>
    public async void IndicateImmortal()
    {
        while (_player.isImmortal)
        {
            // While we are immortal.

            // Switch.
            _spriteRenderer.enabled = false;
            await Task.Delay((int)(_immortalBlinkDelay * 1000));
            _spriteRenderer.enabled = true;
            await Task.Delay((int)(_immortalBlinkDelay * 1000));
        }
    }
}
