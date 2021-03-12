using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Robot : MonoBehaviour
{
    public AIHelpers.MovementBehaviors activeMovementBehavior = AIHelpers.MovementBehaviors.None;
    public GameObject attackObject;
    public GameObject seekObject;
    public bool isAlive = false;
    public bool isChasingPlayer = false;
    public bool isIdeal = false;
    public bool isAttackCooldown = false;

    public Transform playerTarget;
    public Transform wanderObject;
    
    public float maxSpeed = 3.0f;
    public AttackCooldownState cooldown;

    Rigidbody rigidBody;

    Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isAlive = true;
        isChasingPlayer = false;
        isIdeal = true;
        isAttackCooldown = false;
}

    // Update is called once per frame
    void Update()
    {
        AIHelpers.InputParameters inputData = new AIHelpers.InputParameters(gameObject.transform, attackObject.transform, Time.deltaTime, maxSpeed);
        AIHelpers.MovementResult movementResult = new AIHelpers.MovementResult();

        switch (activeMovementBehavior)
        {
            case AIHelpers.MovementBehaviors.Idle:
                AIHelpers.Idle(inputData, ref movementResult);
                break;
            case AIHelpers.MovementBehaviors.Attack:
                AIHelpers.Attack(inputData, ref movementResult);
                break;
            case AIHelpers.MovementBehaviors.AttackCooldown:

                AIHelpers.AttackCooldown(inputData, ref movementResult);

                break;
            default:
                //AIHelpers.SeekKinematic(inputData, ref movementResult);
                movementResult.newPosition = transform.position;
                break;
        }

        gameObject.transform.position = movementResult.newPosition;

    }

    private void FixedUpdate()
    {
     

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackCooldown"))
        {
            goFromTarget(playerTarget, 4);
        }
        
    }
   

    
    public void ActivateAttackCooldown()
    {
       if(isAttackCooldown)
        {
            animator.ResetTrigger("AttackTrigger");
            animator.SetTrigger("AttackCooldown");
           // activeMovementBehavior = AIHelpers.MovementBehaviors.AttackCooldown;
        }
       
    }

    public void ActivateAttack()
    {
        //attackObject = seekObject;

       // AIHelpers.InputParameters inputData = new AIHelpers.InputParameters(gameObject.transform, seekObject.transform, Time.deltaTime, maxSpeed);
       // AIHelpers.MovementResult movementResult = new AIHelpers.MovementResult();
       // activeMovementBehavior = AIHelpers.MovementBehaviors.Attack;
    }

    public void ActivateIdle()
    {
       if (isIdeal)
        {
            activeMovementBehavior = AIHelpers.MovementBehaviors.Idle;
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isIdeal = false;
            isAttackCooldown = true;

        }
       

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetTrigger("AttackTrigger");
            isIdeal = false;
            isChasingPlayer = true;
            AttackPlayer(playerTarget, 4f);
            //activeMovementBehavior = AIHelpers.MovementBehaviors.AttackCooldown;
           
        }

        //if (other.gameObject.CompareTag("Player") && isAlive)
        //{
        //    if (transform.position.y < playerTarget.position.y)
        //    {
                
        //        Debug.Log("OnTop");
        //        isAlive = false;
        //        Vector3 enemySize = transform.localScale;
        //        //transform.localScale = new Vector3(enemySize.x, enemySize.y - );
        //    }
        //}


    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //animator.ResetTrigger("AttackTrigger");
            animator.ResetTrigger("AttackCooldownTrigger");
            animator.SetTrigger("IdleTrigger");
            isIdeal = true;
            isChasingPlayer = false;
            isAttackCooldown = false;
            Debug.Log("Player Left");
        }
    }

    private void AttackPlayer(Transform targetObject, float maxSpeed)
    {
        isIdeal = false;
        Vector3 direction = targetObject.position - transform.position;
        direction.Normalize();
        transform.LookAt(new Vector3(targetObject.position.x, .5f, targetObject.position.z));
        transform.position = transform.position + (direction * maxSpeed * Time.fixedDeltaTime);
    }

    private void goFromTarget(Transform targetObject, float maxSpeed)
    {
       
        Vector3 direction = transform.position - targetObject.position;
        direction.Normalize();
        transform.LookAt(new Vector3(targetObject.position.x, .5f, targetObject.position.z));
        transform.Rotate(0, 180, 0);
        transform.position = transform.position + (direction * maxSpeed * Time.fixedDeltaTime);
    }

   
  



}
