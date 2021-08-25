using UnityEngine;

namespace __Scripts
{
    [CreateAssetMenu(fileName = "Stats", menuName = "Stats/New Stats", order = 0)]
    public class StatsScriptableObject : ScriptableObject
    {
        public float movementSpeed;
        public float rotationSpeed;
        public float health;
        public float maxSpeed;
    }
}