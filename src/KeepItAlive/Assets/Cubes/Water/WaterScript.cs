using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    private float timePointer;
    private float initialY;

    void Start()
    {
        initialY = transform.localPosition.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timePointer += Time.deltaTime * 2;

        var offset = Mathf.Sin(timePointer + transform.localPosition.x + transform.localPosition.z) / 4 - 0.3f;

        transform.localPosition = new Vector3(transform.localPosition.x, initialY + offset, transform.localPosition.z);
    }
}
