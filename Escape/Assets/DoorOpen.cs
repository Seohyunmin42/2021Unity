using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && animator.GetInteger("aa") == 1)
        {
            animator.SetBool("IsOpen", true);
            animator.SetInteger("aa", 2);
        }
        else if(other.tag == "Player" && animator.GetInteger("aa") == 2)
        {
            animator.SetBool("IsOpen", true);
            animator.SetInteger("aa", 1);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && animator.GetInteger("aa") == 1)
        {
            animator.SetBool("IsOpen", false);
            animator.SetInteger("aa", 2);
        }
        else if (other.tag == "Player" && animator.GetInteger("aa") == 2)
        {
            animator.SetBool("IsOpen", false);
            animator.SetInteger("aa", 1);
        }
    }
}
