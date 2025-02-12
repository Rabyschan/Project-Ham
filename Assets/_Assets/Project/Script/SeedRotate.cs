using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

    public class Seed : MonoBehaviour, IItem
    {
        public int score = 1; // 증가할 점수

        public void Use(GameObject target)
        {
            //GameManager의 AddScore 실행.
            GameManager.instance.CollectSeed(score);

            Destroy(this.gameObject);
        }
    }
}
