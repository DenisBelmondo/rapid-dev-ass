using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterInstance), typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private CharacterInstance characterInstance;
        private Vector2 _moveDirection;

		//leashing
		private Vector3 _leashTargetWorldPosition;
        private int LeashPathIndex {get; set;}
        private float LeashDistanceAlongSegment { get; set; }
        private CharacterMovement _characterToFollow;
        private bool _isLeader = false;
        private float _movementStoppingDistance = 0.05f;

        private struct LeashPosition
        {
            public readonly int Index;
            public readonly float SegmentAlpha;
            public readonly Vector3 WorldPosition;

            public LeashPosition(int index, float segmentAlpha, Vector3 worldPosition)
            {
                Index = index;
                SegmentAlpha = segmentAlpha;
                WorldPosition = worldPosition;
            }
        }
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            characterInstance = GetComponent<CharacterInstance>();
            _rb.gravityScale = 0;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public void SetFollowTarget(CharacterInstance target)
        {
            _isLeader = (target == null);

            _characterToFollow = !_isLeader ? target.GetComponent<CharacterMovement>() : null;
        }

        public void Move(Vector2 direction)
        {
            _moveDirection = direction;
        }

        private void FixedUpdate()
        {
            if (_isLeader)
            {
                _rb.linearVelocity = _moveDirection * CrewManager.Instance.GroupSpeed;

                var path = PlayerPathManager.Instance.pathHistory;
                if (path.Count >= 2)
                {
                    LeashPathIndex = path.Count - 2;
                    LeashDistanceAlongSegment = 1.0f;
                } else if (path.Count > 0)
                {
                    LeashPathIndex = 0;
                    LeashDistanceAlongSegment = 1.0f;
                }
                
                _leashTargetWorldPosition = transform.position;
            }
            else
            {
                UpdateLeashPosition();

                Vector3 currentPos = transform.position;
                Vector3 targetPos = _leashTargetWorldPosition;
                float distanceToTarget = Vector3.Distance(currentPos, targetPos);

                if (distanceToTarget > _movementStoppingDistance)
                {
                    float speed = CrewManager.Instance.GroupSpeed;
                    Vector3 direction = (targetPos - currentPos).normalized;
                    
                    //check for overshoot
                    if((speed * Time.fixedDeltaTime) > distanceToTarget)
                    {
                        //haha... what could possibly go wrong?
                        _rb.MovePosition(targetPos);
                    }
                    else
                    {
                        _rb.linearVelocity = direction * speed;
                    }
                }
                else
                {
                    _rb.linearVelocity = Vector3.zero;
                }
            }
        }

        void UpdateLeashPosition()
        {
            var path = PlayerPathManager.Instance.pathHistory.ToArray();
            if (_characterToFollow == null || path.Length < 2) return;

            LeashPosition newLeashPosition = CalculateTargetLeashPosition(path);
            LeashPathIndex = newLeashPosition.Index;
            LeashDistanceAlongSegment = newLeashPosition.SegmentAlpha;
            _leashTargetWorldPosition = newLeashPosition.WorldPosition;
        }

        LeashPosition CalculateTargetLeashPosition(Vector3[] path)
        {
            float desiredFollowDistance = characterInstance.GetFollowDistance();
            
            //get the state of the character we're following
            int followedCharIndex = _characterToFollow.LeashPathIndex;
            float followedCharAlpha = _characterToFollow.LeashDistanceAlongSegment;
            
            //edge case for when characters are at the very edge of the path
            if (followedCharIndex >= path.Length - 1)
            {
                followedCharIndex = path.Length - 2;
                followedCharAlpha = 1.0f;
            }
            
            float segmentLength = Vector3.Distance(path[followedCharIndex], path[followedCharIndex + 1]);
            float distanceIntoSegment = segmentLength * followedCharAlpha;
            
            //now try and find the pos on the current segment
            if (distanceIntoSegment >= desiredFollowDistance)
            {
                return CalculatePositionOnSameSegment(path, followedCharIndex, distanceIntoSegment, desiredFollowDistance, segmentLength);
            }
            
            //if not on the same segment, walk backwards along the path
            float remainingDistance = desiredFollowDistance - distanceIntoSegment;
            return FindPositionByWalkingBackwards(path, followedCharIndex - 1, remainingDistance);
        }

        private LeashPosition CalculatePositionOnSameSegment(Vector3[] path, int index, float distanceIntoSegment, float desiredFollowDistance, float segmentLength)
        {
            float newDistanceIntoSegment = distanceIntoSegment - desiredFollowDistance;
            float newAlpha = (segmentLength > 0) ? newDistanceIntoSegment / segmentLength : 0;
            Vector3 worldPos = Vector3.Lerp(path[index], path[index + 1], newAlpha);
            return new LeashPosition(index, newAlpha, worldPos);
        }

        private LeashPosition FindPositionByWalkingBackwards(Vector3[] path, int startIndex, float remainingDistance)
        {
            int currentIndex = startIndex;
            while (currentIndex >= 0)
            {
                float prevSegmentLength = Vector3.Distance(path[currentIndex], path[currentIndex + 1]);

                if (prevSegmentLength >= remainingDistance)
                {
                    //we found the correct segment
                    float newAlpha = (prevSegmentLength > 0) ? prevSegmentLength - remainingDistance / prevSegmentLength : 0;
                    Vector3 worldPos = Vector3.Lerp(path[currentIndex], path[currentIndex + 1], newAlpha);
                    return new LeashPosition(currentIndex, newAlpha, worldPos);
                }
                remainingDistance -= prevSegmentLength;
                currentIndex--;
            }
            
            //if we ran out of path, clamp to the beginning
            return new LeashPosition(0, 0, path[0]);
        }
    }
}
