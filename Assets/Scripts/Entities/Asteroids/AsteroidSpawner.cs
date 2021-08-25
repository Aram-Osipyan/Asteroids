using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __Scripts.Asteroids
{
    public enum AsteroidType
    {
        Big = 0,
        Medium = 1,
        Small = 2
    }
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private AsteroidSpawnerScriptableObject asteroidSpawnerScriptableObject; 
        [SerializeField] private APlayerShip playerShip;
        [SerializeField] private float maximumDistanceFromPlayerShip = 2f;

        public event Action<Asteroid> OnAsteroidDestroyed;

        public event Action OnAllAsteroidDestroyed;
        
        private List<Asteroid> _asteroids;
        private Action<Asteroid> _onAsteroidDestroyed;
        private Bounds _bounds;
        private Transform _asteroidsAnchor;

        private void Awake()
        {
            _bounds = MapLimit.GetCameraBounds(Camera.main);
            _onAsteroidDestroyed = DestroyAsteroid;
            _asteroids = new List<Asteroid>(10);
        }

        private void Start()
        {
            _asteroidsAnchor = new GameObject("Asteroids anchor").transform;
            InitAsteroids();
        }

        public void InitAsteroids()
        {
            for (var i = 0; i < asteroidSpawnerScriptableObject.asteroidPrefabs.Length; i++)
            {
                var asteroidPrefab = asteroidSpawnerScriptableObject.asteroidPrefabs[i];
                InstantiateRandomAsteroidPrefab(asteroidPrefab);
            }
        }

        public void InitAsteroidByType(AsteroidType asteroidType,int quantity)
        {
            var asteroidPrefab = asteroidSpawnerScriptableObject.asteroidPrefabs[(int)asteroidType];
            for (int i = 0; i < quantity; i++)
            {                
                var asteroid = Instantiate(asteroidPrefab.prefab, GetRandomPosition(), Quaternion.identity, _asteroidsAnchor);
                var asteroidTransform = asteroid.transform;
                asteroid.SetInitialDirection(Quaternion.Euler(0, 0, Random.Range(0, 360)) * asteroidTransform.up);
                asteroid.OnDestroy += () => _onAsteroidDestroyed(asteroid);
                _asteroids.Add(asteroid);
                asteroid.Children = InstantiateChildrenPrefabs(asteroidPrefab, asteroid);
                asteroid.ActivateRigidbody();
            }
            
        }

        private void DestroyAsteroid(Asteroid asteroid)
        {
            // asteroid.transform.DetachChildren();
            if (!_asteroids.Contains(asteroid)) return;
            _asteroids.Remove(asteroid);
            foreach (var asteroidChild in asteroid.Children)
            {
                asteroidChild.ActivateRigidbody();
                asteroidChild.transform.parent = null;
            }

            if (OnAsteroidDestroyed != null) OnAsteroidDestroyed(asteroid);
            if (_asteroids.Count == 0 && OnAllAsteroidDestroyed != null) OnAllAsteroidDestroyed();
        }

        private void InstantiateRandomAsteroidPrefab(AsteroidPrefab asteroidPrefab)
        {
            for (var i = 0; i < asteroidPrefab.initialQuantity; i++)
            {
                var asteroid = Instantiate(asteroidPrefab.prefab, GetRandomPosition(), Quaternion.identity, _asteroidsAnchor);
                var asteroidTransform = asteroid.transform;
                asteroid.SetInitialDirection(Quaternion.Euler(0, 0, Random.Range(0, 360)) * asteroidTransform.up);
                asteroid.OnDestroy += () => _onAsteroidDestroyed(asteroid);
                _asteroids.Add(asteroid);
                asteroid.Children = InstantiateChildrenPrefabs(asteroidPrefab, asteroid);
                asteroid.ActivateRigidbody();
            }
        }

        private List<Asteroid> InstantiateChildrenPrefabs(AsteroidPrefab asteroidPrefab, Asteroid parent)
        {
            var asteroids = new List<Asteroid>();
            if (asteroidPrefab.childrenQuantity == 0) return asteroids;
            for (var i = 0; i < asteroidSpawnerScriptableObject.asteroidPrefabs.Length; i++)
            {
                var prefab = asteroidSpawnerScriptableObject.asteroidPrefabs[i];

                if (asteroidPrefab != prefab ||
                    i == asteroidSpawnerScriptableObject.asteroidPrefabs.Length - 1) continue;

                var prefabToSpawn = asteroidSpawnerScriptableObject.asteroidPrefabs[i + 1];
                var result = InstantiateChildrenAsteroidPrefabs(prefabToSpawn, parent, asteroidPrefab.childrenQuantity);
                foreach (var asteroidChildren in result)
                {
                    asteroidChildren.Children = InstantiateChildrenPrefabs(prefabToSpawn, asteroidChildren);
                }

                return result;
            }

            return asteroids;
        }

        private List<Asteroid> InstantiateChildrenAsteroidPrefabs(AsteroidPrefab asteroidPrefab, Asteroid parent, int quantity)
        {
            var children = new List<Asteroid>();
            var parentTransform = parent.transform;
            var separationBetweenAsteroids = 360 / quantity;
            var initialAngle = Random.Range(0, 360);
            var distance = asteroidSpawnerScriptableObject.spaceBetweenChildren;
            for (var i = 0; i < quantity; i++)
            {
                var rotation = Quaternion.Euler(0 ,0 ,(initialAngle + i *separationBetweenAsteroids) % 360);
                var offset = rotation * parentTransform.up * distance;
                var asteroid = Instantiate(asteroidPrefab.prefab, parentTransform.position + offset, Quaternion.identity, 
                    parentTransform);
                asteroid.Parent = parent;
                asteroid.SetInitialDirection(offset.normalized);
                asteroid.OnDestroy += () => _onAsteroidDestroyed(asteroid);
                _asteroids.Add(asteroid);
                children.Add(asteroid);
            }

            return children;
        }

        private Vector2 GetRandomPosition()
        {
            var playerPosition = playerShip.transform.position;
            var position = playerPosition;
            while (Vector3.Distance(playerPosition, position) < maximumDistanceFromPlayerShip)
            {
                var x = Random.Range(_bounds.min.x, _bounds.max.x);
                var y = Random.Range(_bounds.min.y, _bounds.max.y);
                position = new Vector2(x, y);
            }
            
            return position;
        }
    }
}