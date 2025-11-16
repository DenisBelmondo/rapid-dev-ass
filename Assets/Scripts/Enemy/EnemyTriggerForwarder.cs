using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyTriggerForwarder : MonoBehaviour
    {
        //stupid fucking worthless unity
        [SerializeField] private EnemyInstance enemyInstance;

        private enum Type
        {
            Aggro,
            Vision,
            Kill
        }
        
        [SerializeField] private Type triggerType;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CharacterInstance>(out var character))
            {
                switch (triggerType)
                {
                    case Type.Aggro:
                        enemyInstance.OnPlayerEnterAggroRange(character);
                        break;
                    case Type.Vision:
                        enemyInstance.OnPlayerEnterVisibleRange(character);
                        break;
                    case Type.Kill:
                        enemyInstance.OnPlayerEnterKillRange(character);
                        break;
                }
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CharacterInstance>(out var character))
            {
                if (triggerType == Type.Vision)
                {
                    enemyInstance.OnPlayerExitVisibleRange(character);
                }
                
            }
        }
    }
}
