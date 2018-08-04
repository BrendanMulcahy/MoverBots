using UnityEngine;

namespace Assets.Scripts
{
    public class PickupObject : MonoBehaviour
    {
        public bool Scored = false;

        private Transform _holdingPoint;

        private Rigidbody _rigidBody;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_holdingPoint != null)
            {
                _rigidBody.velocity = (_holdingPoint.position - transform.position) * 10;
            }
        }

        /// <summary>
        ///     Pick up this object.  Turns off gravity and cause this object to move towards the holding point until
        ///     dropped.
        /// </summary>
        /// <param name="holdingPoint">The place to move towards</param>
        public void PickUp(Transform holdingPoint)
        {
            _holdingPoint = holdingPoint;
            _rigidBody.useGravity = false;
        }

        /// <summary>
        ///     Drop this object.  Turns on gravity and stops moving the object toward the holding point.
        /// </summary>
        public void Drop()
        {
            _holdingPoint = null;
            _rigidBody.useGravity = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("score"))
            {
                Scored = true;
            }
        }
    }
}