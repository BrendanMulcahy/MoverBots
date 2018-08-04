using System.Collections.Generic;
using MLAgents;
using UnityEngine;

namespace Assets.Scripts.Mover
{
    public class MoverAgent : Agent
    {
        private RayPerception _rayPer;
        private Vector3 _agentStartPos;
        private Quaternion _agentStartRot;
        [SerializeField] private GameObject _ball;
        private Rigidbody _ballRigidbody;

        private Vector3 _ballStartPos;

        private PickupObject _heldItem;
        [SerializeField] private Transform _holdingPoint;
        [SerializeField] private PickupZone _pickupZone;
        private Rigidbody _rigidbody;
        private PickupObject _ballPickup;


        public override void InitializeAgent()
        {
            base.InitializeAgent();
            _ballStartPos = _ball.transform.position;
            _ballRigidbody = _ball.transform.GetComponent<Rigidbody>();
            _rayPer = GetComponent<RayPerception>();

            _agentStartPos = gameObject.transform.position;
            _agentStartRot = gameObject.transform.rotation;
            _rigidbody = GetComponent<Rigidbody>();
            _ballPickup = _ball.GetComponent<PickupObject>();
        }

        public override void AgentAction(float[] act, string textAction)
        {
            if (_ballPickup.Scored)
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
            _rigidbody.AddForce(dirToGo * 0.35f, ForceMode.VelocityChange);
        }

        public override void CollectObservations()
        {
            const float rayDistance = 15f;
            string[] detectableObjects = { "ground", "wall", "ball", "score" };

            float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
            AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

            float[] rayAngles1 = { 25f, 95f, 165f, 50f, 140f, 75f, 115f };
            AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles1, detectableObjects, 0f, -5f));

            //float[] rayAngles2 = { 15f, 85f, 155f, 40f, 130f, 65f, 105f };
            //AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, -10f));

            AddVectorObs(transform.InverseTransformDirection(_rigidbody.velocity));
            AddVectorObs(IsItemHeld());
        }

        public override void AgentReset()
        {
            _ballPickup.Scored = false;
            Drop();

            gameObject.transform.position = _agentStartPos;
            gameObject.transform.rotation = _agentStartRot;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _ball.transform.position = new Vector3(_ballStartPos.x, _ballStartPos.y, _ballStartPos.z + Random.value * 10f);
            _ballRigidbody.velocity = Vector3.zero;
            _ballRigidbody.angularVelocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("score"))
            {
                SetReward(-1f);
                Done();
            }
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