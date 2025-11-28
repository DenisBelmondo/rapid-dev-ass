using Level;
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
            _waterTilemap = World.Instance.waterTilemap;
        }

        public override void HandleChasing()
        {
            AggroTimer -= Time.deltaTime;
            if (Target == null || AggroTimer <= 0)
            {
                StartCooldown();
                return;
            }
            
            if (!IsPositionOnWater(Target.transform.position))
            {
                Debug.Log($"Target {Target.name} left the water. Returning to Idle to find a new target.", this);
                currentState = State.Idle;
                Rb.linearVelocity = Vector3.zero;
                Animator.SetTrigger("Cooldown");
                return;
            }
            
            if (transform.position.x > Target.transform.position.x)
                SpriteRenderer.flipX = true;
            else
                SpriteRenderer.flipX = false;
            
            var desiredVelocity = (Target.transform.position - transform.position).normalized * EnemyInstance.enemyData.runSpeed;
            Vector3 nextPos = transform.position + desiredVelocity * Time.deltaTime;

            if (IsPositionOnWater(transform.position) && IsPositionOnWater(nextPos))
            {
                Rb.linearVelocity = desiredVelocity;
            }
            else
            {
                Rb.linearVelocity = Vector3.zero;
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