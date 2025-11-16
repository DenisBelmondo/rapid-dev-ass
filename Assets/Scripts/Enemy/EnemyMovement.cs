using System;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        //has stamina, has sprint speed, has walk speed
        private Rigidbody2D _rb;
        private EnemyInstance _enemyInstance;
        
        private Vector3 _startPosition;
        private float _aggroDurationInSeconds;
        private float _aggroTimer;
        private CharacterInstance _target;
        private float _cooldownTimer;
        
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private enum State
        {
            Idle,
            Chasing,
            Cooldown
        }
        
        private State CurrentState { get; set; }
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _enemyInstance = GetComponent<EnemyInstance>();
            
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            CurrentState = State.Idle;
            _startPosition = transform.position;
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case State.Idle:
                    HandleIdle();
                    break;
                case State.Chasing:
                    HandleChasing();
                    break;
                case State.Cooldown:
                    HandleCooldown();
                    break;
            }
        }

        public void StartChasing(CharacterInstance target)
        {
            _target = target;
            CurrentState = State.Chasing;
            _aggroTimer = _enemyInstance.enemyData.aggroDurationInSeconds;
            _animator.SetTrigger("Aggro");
        }
        
        void HandleIdle()
        {
            //enemy is sleeping, will stay here until EnemyInstance sets off aggro.
            _rb.linearVelocity = Vector3.zero;
        }

        void HandleChasing()
        {
            if (_target != null)
            {
                if (transform.position.x > _target.transform.position.x)
                {
                    _spriteRenderer.flipX = true;
                }
                else
                {
                    _spriteRenderer.flipX = false;
                }
            }
            
            _aggroTimer -= Time.deltaTime;

            if (_target == null || _aggroTimer <= 0)
            {
                CurrentState = State.Cooldown;
                _cooldownTimer = _enemyInstance.enemyData.cooldown;
                _target = null;
                _rb.linearVelocity = Vector3.zero;
                _animator.SetTrigger("Cooldown");
                return;
            }
            
            _rb.linearVelocity = (_target.transform.position - transform.position).normalized * _enemyInstance.enemyData.runSpeed;
        }

        void HandleCooldown()
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0)
            {
                CurrentState = State.Idle;
            }
        }
        
    }
}

