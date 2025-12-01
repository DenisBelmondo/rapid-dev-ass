using Level;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy
{
    public class AlligatorAI : MonoBehaviour
    {
        private enum AlligatorState { Dormant, Active }
        private AlligatorState _currState =  AlligatorState.Dormant;
    
        [Header("Settings")]
        [SerializeField] private float activationRange = 15f;

        private EnemyInstance _enemyInstance;
        private AlligatorMovement _movement;
        private Tilemap _waterTileMap;
        private World _world;

        void Awake()
        {
            _enemyInstance = GetComponent<EnemyInstance>();
            _movement = GetComponent<AlligatorMovement>();
            _world = World.Instance;
        }

        void Start()
        {
            _waterTileMap = _world.waterTilemap;
            SetState(AlligatorState.Dormant);
        }

        void Update()
        {
            switch (_currState)
            {
                case AlligatorState.Dormant:
                    UpdateDormant();
                    break;
                case AlligatorState.Active:
                    UpdateActive();
                    break;
            }
        }

        void SetState(AlligatorState newState)
        {
            if (_currState == newState) return;

            _currState = newState;
            switch (_currState)
            {
                case AlligatorState.Dormant:
                    if (_enemyInstance != null) _enemyInstance.aggroCollider.enabled = false;
                    break;
                case AlligatorState.Active:
                    if (_enemyInstance != null) _enemyInstance.aggroCollider.enabled = true;
                    break;
            }
        }

        void UpdateDormant()
        {
            CharacterInstance target = FindValidTarget();
            if (target != null)
            {
                SetState(AlligatorState.Active);
                _movement.StartChasing(target);
            }
        }

        void UpdateActive()
        {
            if (_movement.currentState == EnemyMovement.State.Idle)
            {
                CharacterInstance newTarget = FindValidTarget();
                if (newTarget != null)
                {
                
                    _movement.StartChasing(newTarget);
                }
                else
                {
                
                    SetState(AlligatorState.Dormant);
                }
            }
        }
    
        private CharacterInstance FindValidTarget()
        {
            //TODO- Use Physics2D.OverlapCircleAll?
            var crew = _world.crewManager.crewMembers;
            CharacterInstance closestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (var member in crew)
            {
                if (member == null) continue;

                if (IsPositionOnWater(member.transform.position))
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, member.transform.position);
                    if (distanceToPlayer <= activationRange)
                    {
                        if (distanceToPlayer < closestDistance)
                        {
                            closestDistance = distanceToPlayer;
                            closestTarget = member;
                        }
                    }
                }
            }
            return closestTarget; 
        }

        private bool IsPositionOnWater(Vector3 worldPosition)
        {
            if (_waterTileMap == null) return false;
            Vector3Int cellPosition = _waterTileMap.WorldToCell(worldPosition);
            return _waterTileMap.HasTile(cellPosition);
        }
    }
}