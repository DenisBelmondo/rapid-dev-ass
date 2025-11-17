using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;

/// <summary>
/// An object that contains "global" game state and keeps track of it. It is,
/// itself, instantiated by `class StaticGame`.
/// </summary>
public class Game : MonoBehaviour
{
    private LineRenderer _lines;
    private List<GameObject> _pylonStack = new();
    //private Transform _playerTransform;
    private CrewManager _crewManager;

    public virtual void Start()
    {
        _lines = transform.Find("Lines").GetComponent<LineRenderer>();
        //_playerTransform = transform.Find("Crew");
        _crewManager = CrewManager.Instance;
        PylonManager.Instance.PylonRegistered.AddListener(OnPylonRegistered);
        PylonManager.Instance.onTriangleFormed.AddListener(OnTriangleFormed);
    }

    public virtual void Update()
    {
        if (_pylonStack.Count > 0)
        {
            _lines.SetPosition(_lines.positionCount - 1, _crewManager.Leader.transform.position);
        }
    }

    public virtual void OnPylonRegistered(GameObject pylon)
    {
        _pylonStack.Add(pylon);
        _lines.positionCount = _pylonStack.Count + 1;

        for (int i = 0; i < _pylonStack.Count; i++)
        {
            _lines.SetPosition(i, _pylonStack[i].transform.position);
        }
    }

    public virtual void OnTriangleFormed(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        for (int i = 0; i < _lines.positionCount; i++)
        {
            _lines.SetPosition(i, Vector3.zero);
        }

        _pylonStack.Clear();
    }
}
