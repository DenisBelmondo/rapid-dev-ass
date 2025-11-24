using System;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        //has stamina, has sprint speed, has walk speed
        protected Rigidbody2D _rb;
        protected EnemyInstance _enemyInstance;
        
        private Vector3 _startPosition;
        private float _aggroDurationInSeconds;
        protected float _aggroTimer;
        protected CharacterInstance _target;
        public float cooldownTimer;
        
        protected Animator _animator;
        protected SpriteRenderer _spriteRenderer;
        
        private AudioSource _audioSource;

        public enum State
        {
            Idle,
            Chasing,
            Cooldown
        }

        public State CurrentState;
        public virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _enemyInstance = GetComponent<EnemyInstance>();
            
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
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
            Debug.Log($"StartChasing called. Target: {target.name}", this);
            _target = target;

            if (CurrentState != State.Chasing)
            {
                if(!_audioSource.isPlaying)
                {
                    _audioSource.resource = _enemyInstance.enemyData.alertSound;
                    _audioSource.Play();
                }
            }
            CurrentState = State.Chasing;
            _aggroTimer = _enemyInstance.enemyData.aggroDurationInSeconds;
            _animator.SetTrigger("Aggro");
        }
        
        void HandleIdle()
        {
            //enemy is sleeping, will stay here until EnemyInstance sets off aggro.
            _rb.linearVelocity = Vector3.zero;
        }

        public virtual void HandleChasing()
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
                StartCooldown();
                return;
            }
            
            _rb.linearVelocity = (_target.transform.position - transform.position).normalized * _enemyInstance.enemyData.runSpeed;
        }

        public void StartCooldown()
        {
            //Debug.Log("StartCooldown called.", this);
            CurrentState = State.Cooldown;
            cooldownTimer = _enemyInstance.enemyData.cooldown;
            _target = null;
            _rb.linearVelocity = Vector3.zero;
            _animator.SetTrigger("Cooldown");
        }
        void HandleCooldown()
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
            {
                CurrentState = State.Idle;
            }
        }
        
    }
}

