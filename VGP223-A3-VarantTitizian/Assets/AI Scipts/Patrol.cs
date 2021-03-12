using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public GameObject[] waypoints;
    public Transform playerTarget;
    public Transform wanderObject;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        
    }

    private void AttackPlayer(Transform targetObject, float maxSpeed)
    {
        Vector3 direction = targetObject.position - transform.position;
        direction.Normalize();
        transform.LookAt(new Vector3(targetObject.position.x, .5f, targetObject.position.z));
        transform.position = transform.position + (direction * maxSpeed * Time.fixedDeltaTime);
    }
}
