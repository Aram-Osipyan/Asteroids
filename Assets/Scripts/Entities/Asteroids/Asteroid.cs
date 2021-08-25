using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __Scripts.Asteroids
{
    [RequireComponent(typeof(Stats), typeof(SpaceObject) )]
    public class Asteroid: MonoBehaviour
    {
        [SerializeField] private float points;

        public event Action OnDestroy;
        public List<Asteroid> Children { get; set; }
        public float Points
        {
            get { return points; }
        }
        public bool DestroyedByBullet { get; set; }

        public Asteroid Parent { get; set; }

        private Rigidbody _rigidbody;
        private Stats _stats;
        private Transform _transform;
        private bool _activated;
        private SpaceObject _spaceObject;
        private Vector3 _velocity;

        private void Awake()
        {
            _stats = GetComponent<Stats>();
            _stats.OnDie += Destroy;
            _transform = transform;
            _spaceObject = GetComponent<SpaceObject>();
            _spaceObject.enabled = false;
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!_activated) return;
            
            _transform.Rotate(Vector3.back, _stats.RotationSpeed);
        }

        public void ActivateRigidbody()
        {
            _rigidbody.isKinematic = false;
            _spaceObject.enabled = true;
            _activated = true;
            _rigidbody.velocity = _velocity;
        }

        public void SetInitialDirection(Vector3 initialDirection)
        {
            var velocity = Random.Range(_stats.MovementSpeed, _stats.MaxSpeed);
            _velocity = initialDirection * velocity;
        }

        private void Destroy()
        {
            if (OnDestroy != null) 
                OnDestroy();
            Destroy(gameObject);
        }

        public void ReceiveDamage(float damage, bool isBullet)
        {
            if (Parent != null)
            {
                Parent.ReceiveDamage(damage, isBullet);
                return;
            }

            if (isBullet && _stats.Health - damage <= 0.0f)
            {
                DestroyedByBullet = true;
            }
            if (!isBullet)
            {
                Destroy();
                Debug.Log(gameObject.name);
            }
            _stats.Health -= damage;
            
 
        }
        
        
    }
}