using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public enum Type {A, B};
    public Type zombieType;
    public int maxHealth;
    public int curHealth;
    public Transform target;
    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;
    public bool isRun;
    public AudioSource zombieSound;

    Rigidbody rigid;
    CapsuleCollider capCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();   
        anim = GetComponent<Animator>();
        zombieSound = GetComponent<AudioSource>();

        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }
    void Update()
    {
        if(nav.enabled){                            // 목표물 설정 
            nav.SetDestination(target.position);    // 도착할 목표 위치 지정 함수 
            nav.isStopped = !isChase;
        } 
    }

    void Targetting()
    {
        float targetRadius = 0;  //shpereCast 반지름, 길이 
        float targetRange = 0;

        switch(zombieType){
            case Type.A:
                targetRadius = 1.5f;
                targetRange = 3f;
                break;
            case Type.B:
                targetRadius = 2f;
                targetRange = 9f;
                break;
        }
        
        RaycastHit[] rayHits = 
            Physics.SphereCastAll(transform.position,
                                  targetRadius,
                                  transform.forward, 
                                  targetRange,
                                  LayerMask.GetMask("Player"));

        if(rayHits.Length > 0 && !isAttack){
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        switch(zombieType){
            case Type.A:
                isChase = false;
                isAttack = true;
                anim.SetBool("isAttack", true);

                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                 isChase = true;
                isAttack = false;
                anim.SetBool("isAttack", false);

                break;
            case Type.B:
                anim.SetBool("isRun", true);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(2f);
                meleeArea.enabled = false;
                
                anim.SetBool("isRun", false);
                
                break;
        }
    }

    void FreezeVelocity()
    {
        if(isChase){
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sound")
        {
            zombieSound.Play();
        }
    }

    void FixedUpdate()
    {
        Targetting();
        FreezeVelocity();
    }
}
