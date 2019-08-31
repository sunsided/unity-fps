using UnityEngine;

namespace Project.Scripts
{
    public class Player : MonoBehaviour, ITakeDamage
    {
        /// <summary>
        /// The current health points.
        /// </summary>
        [Header("Stats")]
        public int currentHp;

        /// <summary>
        /// The maximum health points.
        /// </summary>
        public int maximumHp;

        /// <summary>
        /// Movement speed in units per second.
        /// </summary>
        [Header("Movement")]
        public float moveSpeed;

        /// <summary>
        /// Force applied upwards when jumping.
        /// </summary>
        public float jumpForce;

        /// <summary>
        /// The distance used for a raycast to determine whether we're on the ground.
        /// </summary>
        public float jumpGroundDetectionDistance;

        /// <summary>
        /// Mouse look sensitivity.
        /// </summary>
        [Header("Camera")]
        public float lookSensitivity;

        /// <summary>
        /// Highest up angle we can have.
        /// </summary>
        public float maxLookX;

        /// <summary>
        /// Lowest up angle we can have.
        /// </summary>
        public float minLookX;

        /// <summary>
        /// Current X rotation of the camera.
        /// </summary>
        private float _rotX;

        /// <summary>
        /// Main camera.
        /// </summary>
        private Camera _cam;

        /// <summary>
        /// The <see cref="Rigidbody"/> component.
        /// </summary>
        private Rigidbody _rig;

        /// <summary>
        /// The player's weapon.
        /// </summary>
        private Weapon _weapon;

        public void TakeDamage(int damage)
        {
            currentHp = Mathf.Clamp(currentHp - damage, 0, maximumHp);;
            GameUI.Instance.UpdateHealthBar(currentHp, maximumHp);
            if (currentHp <= 0)
            {
                Die();
            }
        }

        public void GiveHealth(int amountToGive)
        {
            currentHp = Mathf.Clamp(currentHp + amountToGive, 0, maximumHp);
            GameUI.Instance.UpdateHealthBar(currentHp, maximumHp);
        }

        public void GiveAmmo(int amountToGive)
        {
            _weapon.currentAmmo = Mathf.Clamp(_weapon.currentAmmo + amountToGive, 0, _weapon.maxAmmo);
            GameUI.Instance.UpdateAmmoText(_weapon.currentAmmo, _weapon.maxAmmo);
        }

        private void Awake()
        {
            _cam = Camera.main;
            _rig = GetComponent<Rigidbody>();
            _weapon = GetComponent<Weapon>();

            // Disable the cursor.
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            Move();
            TryJump();
            TryShoot();
            CamLook();
        }

        private void TryShoot()
        {
            // Note: GetButton triggers every frame the button is depressed.
            if (!Input.GetButton("Fire1")) return;
            if (!_weapon.CanShoot()) return;
            _weapon.Shoot();
        }

        private void TryJump()
        {
            // Note: GetButtonDown only triggers in the first frame the button was depressed.
            if (!Input.GetButtonDown("Jump")) return;

            var ray = new Ray(transform.position, Vector3.down);
            if (!Physics.Raycast(ray, jumpGroundDetectionDistance)) return;

            _rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        void Move()
        {
            var x = Input.GetAxis("Horizontal") * moveSpeed;
            var z = Input.GetAxis("Vertical") * moveSpeed;

            var tf = transform;
            var movementDirection = tf.right * x + tf.forward * z;
            movementDirection.y = _rig.velocity.y;

            _rig.velocity = movementDirection;
        }

        void CamLook()
        {
            var y = Input.GetAxis("Mouse X") * lookSensitivity;
            var x = Input.GetAxis("Mouse Y") * lookSensitivity;

            // Y rotation (left/right) is applied to the player (not the camera!), so that the
            // player is always facing towards the player model's forward direction.
            transform.eulerAngles += Vector3.up * y;

            // X rotation (up/down) is applied to the camera.
            // By changing the axis sign to positive, vertical mouse motion is flipped.
            const float axisSign = -1;
            _rotX = Mathf.Clamp(_rotX + x, minLookX, maxLookX);
            _cam.transform.localRotation = Quaternion.Euler(axisSign * _rotX, 0, 0);
        }

        private void Die()
        {
            Debug.Log("Player has died.");
        }
    }
}
