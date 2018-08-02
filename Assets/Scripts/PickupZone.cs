using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PickupZone : MonoBehaviour
    {
        private readonly List<GameObject> _pickupableObjects = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Ball")
            {
                _pickupableObjects.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "Ball")
            {
                _pickupableObjects.Remove(other.gameObject);
            }
        }

        public PickupObject GetPickupObjectInZone()
        {
            return _pickupableObjects.Count > 0 ? _pickupableObjects[0].GetComponent<PickupObject>() : null;
        }
    }
}