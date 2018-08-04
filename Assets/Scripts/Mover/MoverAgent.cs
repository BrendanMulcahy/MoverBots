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
            if (gameObject.transform.position.y < 0)
            {
                AddReward(-1f);
                Done();
            }

            if (_ball.transform.position.y < 0)
            {
                SetReward(2f);
                Done();
            }

            AddReward(-1f / agentParameters.maxStep);

            var dirToGo = Vector3.zero;
            var rotateDir = Vector3.zero;

            int action = (int) act[0];
            switch (action)
            {
                case 0:
                    //Debug.Log("Rotating left");
                    rotateDir = Vector3.up * -1f;
                    break;
                case 1:
                    //Debug.Log("Rotating right");
                    rotateDir = Vector3.up * 1f;
                    break;
                case 2:
                    //Debug.Log("Moving forward");
                    dirToGo = gameObject.transform.forward * 1f;
                    break;
                case 3:
                    //Debug.Log("Moving backward");
                    dirToGo = gameObject.transform.forward * -1f;
                    break;
                case 4:
                    PickUp();
                    //Debug.Log("Picking up");
                    break;
                case 5:
                    Drop();
                    //Debug.Log("Dropping");
                    break;
            }

            transform.Rotate(rotateDir, Time.deltaTime * 200f);
            _rigidbody.AddForce(dirToGo * 1.5f, ForceMode.VelocityChange);
        }

        public override void CollectObservations()
        {
            AddVectorObs(transform.position);
            AddVectorObs(_ball.transform.position);
            AddVectorObs(_ballRigidbody.velocity);
            AddVectorObs(_ballRigidbody.angularVelocity);
            AddVectorObs(transform.rotation.y);
        }

        public override void AgentReset()
        {
            gameObject.transform.position = _agentStartPos;
            gameObject.transform.rotation = _agentStartRot;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _ball.transform.position = new Vector3(_ballStartPos.x, _ballStartPos.y, _ballStartPos.z + Random.value * 3.5f - 7);
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
                else
                {
                    AddReward(-0.01f);
                }
            }
            else
            {
                AddReward(-0.01f);
            }
        }

        private void Drop()
        {
            if (IsItemHeld())
            {
                _heldItem.Drop();
                _heldItem = null;
            }
            else
            {
                AddReward(-0.01f);
            }
        }

        private bool IsItemHeld()
        {
            return _heldItem != null;
        }
    }
}