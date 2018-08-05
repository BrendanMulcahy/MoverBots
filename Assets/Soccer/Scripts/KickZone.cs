using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Soccer.Scripts
{
    public class KickZone : MonoBehaviour
    {
        private readonly List<GameObject> _containedObjects = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "ball")
            {
                _containedObjects.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "ball")
            {
                _containedObjects.Remove(other.gameObject);
            }
        }

        public bool ContainsBall()
        {
            return _containedObjects.Count > 0;
        }

        public GameObject GetBall()
        {
            return _containedObjects.FirstOrDefault();
        }
    }
}