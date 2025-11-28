using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        protected Rigidbody2D Rb;
        protected EnemyInstance EnemyInstance;
        
        private Vector3 _startPosition;
        private float _aggroDurationInSeconds;
        protected float AggroTimer;
        protected CharacterInstance Target;
        public float cooldownTimer;
        
        protected Animator Animator;
        protected SpriteRenderer SpriteRenderer;
        
        private AudioSource _audioSource;

        public enum State
        {
            Idle,
            Chasing,
            Cooldown
        }

        public State currentState;
        public virtual void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            EnemyInstance = GetComponent<EnemyInstance>();
            
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            currentState = State.Idle;
            _startPosition = transform.position;
        }

        private void Update()
        {
            switch (currentState)
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
            Target = target;

            if (currentState != State.Chasing)
            {
                if(!_audioSource.isPlaying)
                {
                    _audioSource.resource = EnemyInstance.enemyData.alertSound;
                    _audioSource.Play();
                }
            }
            currentState = State.Chasing;
            AggroTimer = EnemyInstance.enemyData.aggroDurationInSeconds;
            Animator.SetTrigger("Aggro");
        }
        
        void HandleIdle()
        {
            //enemy is sleeping, will stay here until EnemyInstance sets off aggro.
            Rb.linearVelocity = Vector3.zero;
        }

        public virtual void HandleChasing()
        {
            if (Target != null)
            {
                if (transform.position.x > Target.transform.position.x)
                {
                    SpriteRenderer.flipX = true;
                }
                else
                {
                    SpriteRenderer.flipX = false;
                }
            }
            
            AggroTimer -= Time.deltaTime;

            if (Target == null || AggroTimer <= 0)
            {
                StartCooldown();
                return;
            }
            
            Rb.linearVelocity = (Target.transform.position - transform.position).normalized * EnemyInstance.enemyData.runSpeed;
        }

        public void StartCooldown()
        {
            //Debug.Log("StartCooldown called.", this);
            currentState = State.Cooldown;
            cooldownTimer = EnemyInstance.enemyData.cooldown;
            Target = null;
            Rb.linearVelocity = Vector3.zero;
            Animator.SetTrigger("Cooldown");
        }
        void HandleCooldown()
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
            {
                currentState = State.Idle;
            }
        }
        
    }
}

