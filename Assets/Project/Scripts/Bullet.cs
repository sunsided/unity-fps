﻿using UnityEngine;

namespace Project.Scripts
{
    public class Bullet : MonoBehaviour
    {
        public int damage = 1;
        public float lifetime = 5;

        public GameObject hitParticlePrefab;

        private float _shootTime;

        private void OnEnable()
        {
            // Bullets are being pooled, thus the Awake/Start methods don't apply.
            // However, the pool sets the bullet to be active after it's picked, triggering this event.
            _shootTime = Time.time;
        }

        private void Update()
        {
            TryExpire();
        }

        private void OnTriggerEnter(Collider other)
        {
            // Looking at this, we should probably have a "damage" or "health" component.
#if false
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(damage);
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
#else
            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                other.GetComponent<ITakeDamage>().TakeDamage(damage);
            }
#endif

            // Create the hit particle.
            const float particleLifeTime = 0.5f;
            var particles = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
            Destroy(particles, particleLifeTime);

            // Remove the bullet from the scene.
            gameObject.SetActive(false);
        }

        private void TryExpire()
        {
            if (Time.time - _shootTime >= lifetime)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
