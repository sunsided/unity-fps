using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int currentHp;
    public int maximumHp;
    public int scoreToGive;

    [Header("Movement")]
    public float moveSpeed;
    public float attackRange;
    public float yPathOffset;

    private readonly List<Vector3> _path = new List<Vector3>();
    private Weapon _weapon;
    private GameObject _target;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    private void Start()
    {
        // Assuming that there is only one player object in the game, we pick
        // the first (and only 🙃) one we find as our target.
        _target = FindObjectOfType<Player>().gameObject;

        // Update the path to the target every 0.5 seconds.
        InvokeRepeating(nameof(UpdatePath), 0.0f, 0.5f);
    }

    private void Update()
    {
        TryChaseTarget();
    }

    private void UpdatePath()
    {
        // Calculate a path to the target.
        var navMeshPath = new NavMeshPath();
        NavMesh.CalculatePath(
            sourcePosition: transform.position,
            targetPosition: _target.transform.position,
            areaMask: NavMesh.AllAreas,
            path: navMeshPath);

        // Convert the path into a list.
        _path.Clear();
        _path.AddRange(navMeshPath.corners);
    }

    private void TryChaseTarget()
    {
        if (_path.Count == 0) return;

        // Move towards the closest waypoint.
        // Offset the target such that we're not trying to walk into the ground.
        var yOffset = new Vector3(0, yPathOffset, 0);
        var targetPosition = _path[0] + yOffset;
        transform.position = Vector3.MoveTowards(
            current: transform.position,
            target: targetPosition,
            maxDistanceDelta: moveSpeed * Time.deltaTime);

        // If we hit the target position, remove it from the waypoint list (thus chasing the next waypoint).
        if (transform.position == targetPosition)
        {
            _path.RemoveAt(0);
        }
    }
}
