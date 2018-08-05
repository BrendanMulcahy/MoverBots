using Assets.Scripts;
using MLAgents;
using UnityEngine;

namespace Assets.Soccer.Scripts
{
    public class SoccerAgent : Agent
    {
        private RayPerception _rayPerception;
        private Vector3 _startPosition;
        private Quaternion _startRotation;
        [SerializeField] private KickZone _kickZone;
        private Rigidbody _rigidbody;
        [SerializeField] private GameObject _goal;

        public override void InitializeAgent()
        {
            base.InitializeAgent();

            _rayPerception = GetComponent<RayPerception>();
            _rigidbody = GetComponent<Rigidbody>();

            _startPosition = gameObject.transform.position;
            _startRotation = gameObject.transform.rotation;
        }

        public override void AgentAction(float[] act, string textAction)
        {
            AddReward(-0.0001f);

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
                    //Debug.Log("Strafing left");
                    dirToGo = gameObject.transform.right * -1f;
                    break;
                case 5:
                    //Debug.Log("Strafing right");
                    dirToGo = gameObject.transform.right * 1f;
                    break;
                case 6:
                    //Debug.Log("Kicking straight");
                    KickStraight();
                    break;
                case 7:
                    //Debug.Log("Kicking Up");
                    KickUp();
                    break;
            }

            transform.Rotate(rotateDir, Time.deltaTime * 200f);
            _rigidbody.AddForce(dirToGo * 0.35f, ForceMode.VelocityChange);
        }

        public override void CollectObservations()
        {
            const float rayDistance = 50f;
            string[] detectableObjects = { "ground", "wall", "ball", "score", "blueAgent", "redAgent" };

            float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
            AddVectorObs(_rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

            float[] rayAngles1 = { 25f, 95f, 165f, 50f, 140f, 75f, 115f };
            AddVectorObs(_rayPerception.Perceive(rayDistance, rayAngles1, detectableObjects, 0f, 5f));

            float[] rayAngles2 = { 15f, 85f, 155f, 40f, 130f, 65f, 105f };
            AddVectorObs(_rayPerception.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, 10f));

            AddVectorObs(transform.InverseTransformDirection(_rigidbody.velocity));
            AddVectorObs(_kickZone.ContainsBall());
            AddVectorObs(_goal.transform.position);
        }

        public override void AgentReset()
        {
            gameObject.transform.position = _startPosition;
            gameObject.transform.rotation = _startRotation;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void RewardGoal()
        {
            SetReward(2f);
        }

        public void PenaltyGoal()
        {
            SetReward(-2f);
        }

        private void KickStraight()
        {
            if (_kickZone.ContainsBall())
            {
                var ball = _kickZone.GetBall();
                ball.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 2f, ForceMode.VelocityChange);
                AddReward(0.01f);
            }
        }

        private void KickUp()
        {
            if (_kickZone.ContainsBall())
            {
                var ball = _kickZone.GetBall();
                ball.GetComponent<Rigidbody>().AddForce((gameObject.transform.forward + gameObject.transform.up) * 2f, ForceMode.VelocityChange);
                AddReward(0.01f);
            }
        }
    }
}