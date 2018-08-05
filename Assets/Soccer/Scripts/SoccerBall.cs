using UnityEngine;

namespace Assets.Soccer.Scripts
{
    public class SoccerBall : MonoBehaviour
    {
        private Vector3 _ballStartPos;
        private Rigidbody _ballRigidbody;
        [SerializeField] private SoccerAcademy _soccerAcademy;

        private void Start()
        {
            _ballStartPos = transform.position;
            _ballRigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("score"))
            {
                _soccerAcademy.Score(collision.gameObject.name == "Blue Score");
            }
        }

        private void Update()
        {
            if (transform.position.y < 0)
            {
                Reset();
            }
        }

        public void Reset()
        {
            transform.position = _ballStartPos;
            _ballRigidbody.velocity = Vector3.zero;
            _ballRigidbody.angularVelocity = Vector3.zero;
        }
    }
}