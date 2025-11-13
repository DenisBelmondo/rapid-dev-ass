using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerPathManager : Singleton<PlayerPathManager>
    {
        protected override bool PersistBetweenScenes => false;

        //public List<Vector3> pathHistory = new List<Vector3>();
        public Queue<Vector3> pathHistory = new Queue<Vector3>();
        [SerializeField] private float pointSpacing = 0.5f;
        [SerializeField] private int maxPoints = 80; // max length of the path to store.
        private Vector3 _lastPoint;
        
        private Transform _leaderTransform;
        
        public void AddPoint(Vector3 point)
        {
            //if empty or point is further than pointSpacing from the last point
            if (pathHistory.Count == 0 || Vector3.Distance(_lastPoint, point) > pointSpacing)
            {
                pathHistory.Enqueue(point);
                _lastPoint = point;

                if (pathHistory.Count > maxPoints)
                {
                    pathHistory.Dequeue();
                }
            }
        }

        public void SetLeader(Transform leader)
        {
            _leaderTransform = leader;
        }

        private void LateUpdate()
        {
            if (_leaderTransform != null)
            {
                AddPoint(_leaderTransform.position);
            }
        }
    }
}
