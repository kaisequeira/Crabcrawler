using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length;
    private float startPos;
    public GameObject mainCamera;
    [SerializeField] private float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = mainCamera.transform.position.x * (1 - parallaxEffect); 
        float distance = mainCamera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + distance, transform.position.y);

        if (temp > startPos + length) {
            startPos += length;
        } else if (temp < startPos - length) {
            startPos -= length;
        }
    }
}
