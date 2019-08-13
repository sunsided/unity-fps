using UnityEngine;

namespace Project.Scripts
{
    public class Pickup : MonoBehaviour
    {
        public PickupType type;
        public int value = 1;

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
                }
            }
        }
    }
}
