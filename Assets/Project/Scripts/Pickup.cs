using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Project.Scripts
{
    public class Pickup : MonoBehaviour
    {
        public PickupType type;
        public int value = 1;

        [Header("Bobbing")]
        public float rotateSpeed;
        public float bobSpeed;
        public float bobHeight;

        private Vector3 _startPosition;
        private bool _bobbingUp;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            var tf = transform;
            var pos = tf.position;

            // Rotation.
            tf.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

            // Up/down bobbing.
            var offset = _bobbingUp
                ? new Vector3(0, bobHeight * 0.5f, 0)
                : new Vector3(0, -bobHeight * 0.5f, 0);
            var target = _startPosition + offset;
            tf.position = pos = Vector3.MoveTowards(pos, target, bobSpeed * Time.deltaTime);
            if (pos == target)
            {
                _bobbingUp = !_bobbingUp;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<Player>();
                switch (type)
                {
                    case PickupType.Health:
                    {
                        player.GiveHealth(value);
                        break;
                    }

                    case PickupType.Ammo:
                    {
                        player.GiveAmmo(value);
                        break;
                    }

                    default:
                    {
                        Debug.Log($"Unhandled pickup type: {type}");
                        break;
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
