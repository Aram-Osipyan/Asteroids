using System;
using UnityEngine;
using Object = System.Object;

namespace __Scripts.Asteroids
{
    [Serializable]
    public struct AsteroidPrefab
    {
        public Asteroid prefab;
        public int size;
        public int childrenQuantity;
        public int initialQuantity;

        public override bool Equals(object obj)
        {
            return obj != null && this == ((AsteroidPrefab) obj);
        }

        public bool Equals(AsteroidPrefab other)
        {
            return Equals(prefab, other.prefab) && size == other.size && childrenQuantity == other.childrenQuantity 
                   && initialQuantity == other.initialQuantity;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (prefab != null ? prefab.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ size;
                hashCode = (hashCode * 397) ^ childrenQuantity;
                hashCode = (hashCode * 397) ^ initialQuantity;
                return hashCode;
            }
        }

        public static bool operator ==(AsteroidPrefab lhs, AsteroidPrefab rhs)
        {
            return lhs.prefab == rhs.prefab;
        }

        public static bool operator !=(AsteroidPrefab lhs, AsteroidPrefab rhs)
        {
            return !(lhs == rhs);
        }
    }
    
    [CreateAssetMenu(fileName = "Asteroid Spawner", menuName = "Asteroid/New Asteroid Spawner", order = 0)]
    public class AsteroidSpawnerScriptableObject : ScriptableObject
    {
        public float spaceBetweenChildren = 1f;
        public AsteroidPrefab[] asteroidPrefabs;
        
    }
}