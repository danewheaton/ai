using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float speed = 5;

	void Update ()
    {
        transform.Rotate(Vector3.left * speed * Time.deltaTime);
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
