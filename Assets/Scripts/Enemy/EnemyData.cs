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
        public float walkSpeed = 4;
        public float runSpeed = 4;
        
        public float aggroDurationInSeconds = 4;
        public float cooldown = 2f;

        [Header("Radii")] 
        public float visibleRangeRadius = 8f;
        public float aggroRangeRadius = 3f;
        public float killRangeRadius = 0.5f;
    }
}
