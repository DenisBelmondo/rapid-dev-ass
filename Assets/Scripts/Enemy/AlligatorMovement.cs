using Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy
{
    public class AlligatorMovement : EnemyMovement
    {
        private Tilemap _waterTilemap;

        public override void Awake()
        {
            base.Awake();
            _waterTilemap = CrewManager.Instance.waterTilemap;
        }

        public override void HandleChasing()
        {
            _aggroTimer -= Time.deltaTime;
            if (_target == null || _aggroTimer <= 0)
            {
                StartCooldown();
                return;
            }
            
            if (!IsPositionOnWater(_target.transform.position))
            {
                Debug.Log($"Target {_target.name} left the water. Returning to Idle to find a new target.", this);
                CurrentState = State.Idle;
                _rb.linearVelocity = Vector3.zero;
                _animator.SetTrigger("Cooldown");
                return;
            }
            
            if (transform.position.x > _target.transform.position.x)
                _spriteRenderer.flipX = true;
            else
                _spriteRenderer.flipX = false;
            
            var desiredVelocity = (_target.transform.position - transform.position).normalized * _enemyInstance.enemyData.runSpeed;
            Vector3 nextPos = transform.position + desiredVelocity * Time.deltaTime;

            if (IsPositionOnWater(transform.position) && IsPositionOnWater(nextPos))
            {
                _rb.linearVelocity = desiredVelocity;
            }
            else
            {
                _rb.linearVelocity = Vector3.zero;
            }
        }

        private bool IsPositionOnWater(Vector3 worldPosition)
        {
            if (_waterTilemap == null) return false;
            
            Vector3Int cellPosition = _waterTilemap.WorldToCell(worldPosition);
            bool hasTile = _waterTilemap.HasTile(cellPosition);
            
            return hasTile;
        }
    }
}