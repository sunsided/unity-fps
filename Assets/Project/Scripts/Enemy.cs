using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts
{
    public class Enemy : MonoBehaviour, ITakeDamage
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

        public void TakeDamage(int damage)
        {
            currentHp -= damage;
            if (currentHp <= 0)
            {
                Die();
            }
        }

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
            if (!TryStopAndShoot())
            {
                TryChaseTarget();
            }

            LookAtTarget();
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

        private bool TryStopAndShoot()
        {
            var dist = Vector3.Distance(transform.position, _target.transform.position);
            if (dist > attackRange) return false;
            if (_weapon.CanShoot())
            {
                _weapon.Shoot();
            }

            return true;
        }

        private void LookAtTarget()
        {
            var dir = (_target.transform.position - transform.position).normalized;
            var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            transform.eulerAngles = Vector3.up * angle;
        }

        private void Die()
        {
            Debug.Log("Enemy has died.");
            Destroy(gameObject);
        }
    }
}
