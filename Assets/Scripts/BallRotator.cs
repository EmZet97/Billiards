using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRotator : MonoBehaviour
{
    public GameObject BallInstance;
    private Transform child;
    // Start is called before the first frame update
    void Start()
    {
        // child = Instantiate(BallInstance, transform.position, transform.rotation, transform).transform;
        // child.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var velocity = GetComponent<Rigidbody>().velocity;

        child.Rotate(new Vector3(velocity.z * 5, 0, 0), Space.World);
        child.Rotate(new Vector3(0, 0, -velocity.x * 5), Space.World);

        Debug.DrawLine(transform.position, transform.position + (velocity * 5), Color.cyan);
    }
}
