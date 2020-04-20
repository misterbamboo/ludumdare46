using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharetteWeelRotationScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] weels;

    [SerializeField]
    private float rotationSpeed;

    void Start()
    {

    }

    void Update()
    {
        var rotationToAdd = rotationSpeed * Time.deltaTime * 360;
        foreach (var weel in weels)
        {
            var rotation = weel.transform.rotation.eulerAngles;
            rotation = new Vector3(rotation.x, rotation.y, rotation.z - rotationToAdd);
            weel.transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
