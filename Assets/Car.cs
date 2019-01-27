using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform[] target;
    public float speed;

    private int current;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != target[current].position)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target[current].position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else current = (current + 1) % target.Length;

        if (transform.rotation.y != target[current].rotation.y)
        {
            var step = speed * Time.deltaTime;
            var carAngle = transform.rotation.eulerAngles.y;
            carAngle = Mathf.LerpAngle(carAngle, target[current].rotation.eulerAngles.y, 3*Time.deltaTime);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, carAngle, transform.rotation.eulerAngles.z);
        }

        
    }
}
