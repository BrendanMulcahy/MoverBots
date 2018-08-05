using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    // Update is called once per frame
    private void Update()
    {
        transform.position = _target.transform.position - _target.transform.forward * 20f + _target.transform.up * 10f;
        transform.LookAt(_target.transform);
    }
}