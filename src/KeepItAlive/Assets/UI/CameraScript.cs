using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float WantedZ { get; set; }
    void Start()
    {
        WantedZ = Camera.main.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            WantedZ--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            WantedZ++;
        }

        var pos = Camera.main.transform.position;
        var wanted = new Vector3(pos.x, pos.y, WantedZ);
        Camera.main.transform.position = Vector3.Lerp(pos, wanted, 0.8f);
    }
}
