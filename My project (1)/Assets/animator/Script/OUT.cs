using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OUT : MonoBehaviour
{
    public Animator animator;  
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        animator.SetBool("In", false);

        if (other.tag == "Player")
        {
            animator.SetBool("IsOpen", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("IsOpen", false);
        }
    }
}
