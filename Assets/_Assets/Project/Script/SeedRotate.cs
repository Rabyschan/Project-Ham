using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 450f;
    [SerializeField] GameObject seed;

    Vector3 _origin;

    public bool _isEating = false;

    private void Awake()
    {
        _origin = transform.eulerAngles;
    }

    private void Update()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, _origin.z);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isEating = true;
            seed.SetActive(false);
        }   
    }
}
