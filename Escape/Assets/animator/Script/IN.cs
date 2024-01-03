using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IN : MonoBehaviour
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

    public void OnTriggerEnter(Collider other)
    {
        animator.SetBool("In", true);

        if (other.tag == "Player")
        {
            animator.SetBool("IsOpen", true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("IsOpen", false);
        }

        animator.SetBool("In", false);
    }
}
