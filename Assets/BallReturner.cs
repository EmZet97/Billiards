using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReturner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ball")
        {
            collision.collider.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            collision.collider.transform.position = new Vector3(0, 10, 0);
        }
    }
}
