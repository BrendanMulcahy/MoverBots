﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    ///     Ray perception component. Attach this to agents to enable "local perception"
    ///     via the use of ray casts directed outward from the agent.
    /// </summary>
    public class RayPerception : MonoBehaviour
    {
        private readonly HashSet<Collider> _selfCollider = new HashSet<Collider>();

        private readonly Color[] colors =
        {
            Color.yellow,
            Color.green,
            Color.white,
            Color.magenta,
            Color.blue,
            Color.red,
            Color.yellow,
            Color.cyan
        };

        private Vector3 endPosition;
        private readonly List<float> perceptionBuffer = new List<float>();

        private void Start()
        {
            _selfCollider.Add(gameObject.GetComponent<Collider>());
            foreach (Collider childCollider in gameObject.GetComponentsInChildren<Collider>())
            {
                _selfCollider.Add(childCollider);
            }
        }

        /// <summary>
        ///     Creates perception vector to be used as part of an observation of an agent.
        /// </summary>
        /// <returns>The partial vector observation corresponding to the set of rays</returns>
        /// <param name="rayDistance">Radius of rays</param>
        /// <param name="rayAngles">Anlges of rays (starting from (1,0) on unit circle).</param>
        /// <param name="detectableObjects">List of tags which correspond to object types agent can see</param>
        /// <param name="startOffset">Starting heigh offset of ray from center of agent.</param>
        /// <param name="endOffset">Ending height offset of ray from center of agent.</param>
        public List<float> Perceive(float rayDistance,
            float[] rayAngles, string[] detectableObjects,
            float startOffset, float endOffset)
        {
            perceptionBuffer.Clear();
            // For each ray sublist stores categorial information on detected object
            // along with object distance.
            foreach (float angle in rayAngles)
            {
                endPosition = transform.TransformDirection(
                    PolarToCartesian(rayDistance, angle));
                endPosition.y = endOffset;

                Color color = Color.black;

                float[] subList = new float[detectableObjects.Length + 2];
                List<RaycastHit> hits = Physics
                    .SphereCastAll(transform.position + new Vector3(0f, startOffset, 0f), 0.3f, endPosition,
                        rayDistance).Where(h => !_selfCollider.Contains(h.collider)).ToList();
                if (hits.Count > 0)
                {
                    RaycastHit hit = hits.First();
                    for (int i = 0; i < detectableObjects.Length; i++)
                    {
                        if (hit.collider.gameObject.CompareTag(detectableObjects[i]))
                        {
                            subList[i] = 1;
                            subList[detectableObjects.Length + 1] = hit.distance / rayDistance;
                            color = colors[i];
                            break;
                        }
                    }
                }
                else
                {
                    subList[detectableObjects.Length] = 1f;
                }

                perceptionBuffer.AddRange(subList);

                if (Application.isEditor)
                {
                    if (color != Color.black)
                    {
                        Debug.DrawRay(transform.position + new Vector3(0f, startOffset, 0f),
                        endPosition, color, 0.01f, true);
                    }
                }
            }

            return perceptionBuffer;
        }

        /// <summary>
        ///     Converts polar coordinate to cartesian coordinate.
        /// </summary>
        public static Vector3 PolarToCartesian(float radius, float angle)
        {
            float x = radius * Mathf.Cos(DegreeToRadian(angle));
            float z = radius * Mathf.Sin(DegreeToRadian(angle));
            return new Vector3(x, 0f, z);
        }

        /// <summary>
        ///     Converts degrees to radians.
        /// </summary>
        public static float DegreeToRadian(float degree)
        {
            return degree * Mathf.PI / 180f;
        }
    }
}