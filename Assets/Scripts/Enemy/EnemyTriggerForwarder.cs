using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyTriggerForwarder : MonoBehaviour
    {
        [SerializeField] private EnemyInstance enemyInstance;
        [SerializeField] private bool isAggroRange;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CharacterInstance>(out var character))
            {
                if (isAggroRange)
                {
                    enemyInstance.OnPlayerEnterAggroRange(character);
                }
                else
                {
                    enemyInstance.OnPlayerEnterVisibleRange(character);
                }
                
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CharacterInstance>(out var character))
            {
                if (isAggroRange)
                {
                    //do nothing?
                    //enemyInstance.OnPlayerExitAggroRange();
                }
                else
                {
                    enemyInstance.OnPlayerExitVisibleRange(character);
                }
            }
        }
    }
}
