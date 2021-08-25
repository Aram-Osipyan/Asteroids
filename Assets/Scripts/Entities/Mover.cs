using UnityEngine;

[RequireComponent(typeof(Stats), typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float tiltWhenMoving;

    private Stats _stats;
    private Rigidbody _rigidbody;
    private Vector2 _direction;
    private Transform _transform;

    private void Awake()
    {
        _stats = GetComponent<Stats>();
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _direction * _stats.MovementSpeed;
        
        _transform.rotation =
            Quaternion.Euler(new Vector3(_direction.y * tiltWhenMoving, _direction.x * tiltWhenMoving * -1, 0));
    }

    public void Move(Vector2 direction)
    {
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        _direction = direction;
    }
}