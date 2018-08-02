using System.Collections.Generic;
using MLAgents;
using UnityEngine;

namespace Assets.Scripts.Mover
{
    public class MoverAgent : Agent
    {
        private Vector3 _agentStartPos;
        private Quaternion _agentStartRot;
        [SerializeField] private GameObject _ball;
        private Rigidbody _ballRigidbody;

        private Vector3 _ballStartPos;

        private PickupObject _heldItem;
        [SerializeField] private Transform _holdingPoint;
        [SerializeField] private PickupZone _pickupZone;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _ballStartPos = _ball.transform.position;
            _ballRigidbody = _ball.transform.GetComponent<Rigidbody>();

            _agentStartPos = gameObject.transform.position;
            _agentStartRot = gameObject.transform.rotation;
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void AgentAction(float[] act, string textAction)
        {
            int action = (int) act[0];
            switch (action)
            {
                case 0:
                    gameObject.transform.Rotate(Vector3.up, -1);
                    break;
                case 1:
                    gameObject.transform.Rotate(Vector3.up, 1);
                    break;
                case 2:
                    gameObject.transform.position += gameObject.transform.forward * 0.05f;
                    break;
                case 3:
                    gameObject.transform.position += gameObject.transform.forward * -0.05f;
                    break;
                case 4:
                    PickUp();
                    break;
                case 5:
                    Drop();
                    break;
            }

            if (!IsDone())
            {
                AddReward(-Vector3.Distance(gameObject.transform.position, _ball.transform.position) * 0.000001f);
            }

            if (gameObject.transform.position.y < 0)
            {
                Done();
                AddReward(-1f);
            }

            if (_ball.transform.position.y < 0)
            {
                Done();
                AddReward(100f);
            }
        }

        public override void CollectObservations()
        {
            List<float> state = new List<float>
            {
                gameObject.transform.position.x,
                gameObject.transform.position.y,
                gameObject.transform.position.z,
                gameObject.transform.rotation.y,
                _ball.transform.position.x,
                _ball.transform.position.y,
                _ball.transform.position.z,
                _ballRigidbody.velocity.x,
                _ballRigidbody.velocity.y,
                _ballRigidbody.velocity.z,
                Vector3.Distance(gameObject.transform.position, _ball.transform.position)
            };

            AddVectorObs(state);
        }

        public override void AgentReset()
        {
            gameObject.transform.position = _agentStartPos;
            gameObject.transform.rotation = _agentStartRot;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _ball.transform.position = _ballStartPos;
            _ballRigidbody.velocity = Vector3.zero;
            _ballRigidbody.angularVelocity = Vector3.zero;

            Drop();
        }

        private void PickUp()
        {
            if (!IsItemHeld())
            {
                PickupObject pickupObject = _pickupZone.GetPickupObjectInZone();

                if (pickupObject != null)
                {
                    pickupObject.PickUp(_holdingPoint);
                    _heldItem = pickupObject;
                }
            }
        }

        private void Drop()
        {
            if (IsItemHeld())
            {
                _heldItem.Drop();
                _heldItem = null;
            }
        }

        private bool IsItemHeld()
        {
            return _heldItem != null;
        }
    }
}