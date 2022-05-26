using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private float speedFollow;
    [SerializeField] private Animator anim;
    private Transform currentTarget;

    private bool isDead;

    private void Start()
    {
        ChangeRagdoll(true, false);
        GetComponent<Rigidbody>().isKinematic = false;

        GetComponent<BoxCollider>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;

    }

    private void Update()
    {
        if(currentTarget && !isDead)
        {
            anim.SetBool("isRun", true);

            transform.LookAt(currentTarget);

            if(Vector2.Distance(transform.position, currentTarget.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speedFollow * Time.deltaTime);
            }
        }
    }

    public void Dead()
    {
        isDead = true;
        anim.enabled = false;

        GetComponent<BoxCollider>().enabled = false;

        ChangeRagdoll(false, true);

        
    }

    private void ChangeRagdoll(bool isActive, bool collidersActive)
    {
        Rigidbody[] rbAll = GetComponentsInChildren<Rigidbody>();
        Collider[] colliders = GetComponentsInChildren<Collider>();


        foreach (Rigidbody rb in rbAll)
        {
            rb.isKinematic = isActive;
        }
        foreach (Collider coll in colliders)
        {
            coll.enabled = collidersActive;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        SoloPlayerController player = other.GetComponent<SoloPlayerController>();

        if(other.CompareTag("AddPlayer") && !currentTarget && !player.currentTarget)
        {
            currentTarget = other.transform;
            player.currentTarget = transform;

            other.transform.SetParent(null);

            player.isAdded = false;
            
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("AddPlayer"))
        {
            if(!isDead && other.gameObject == currentTarget.gameObject)
            {
                this.Dead();
                other.gameObject.GetComponent<SoloPlayerController>().Dead();                
            }

        }
    }
        
}
