using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Name")]
        public string Name = "Generic Enemy";

        /*
         TODO- this part...
        [Header("Sprite")] 
        public Sprite Sprite;
        */

        [Header("Stats")] 
        public float speed = 4;
        public float aggroDurationInSeconds = 4;

        [Header("Radii")] 
        public float visibleRangeRadius = 5f;
        public float aggroRangeRadius = 3f;
    }
}
