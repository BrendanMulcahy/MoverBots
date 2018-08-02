using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Ball")
            {
                Destroy(other.gameObject);
            }
        }
    }
}