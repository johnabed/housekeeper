using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float lookRadius = 6f;
    float stoppingDistance = 1f;

    Transform target;

    //Animation smoothing
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;

    public float moveSpeed = 20;

    private Animator anim;
    private Rigidbody2D myRigidbody;

    private bool isMoving;
    private Vector2 currMove;
    private Vector2 lastMove;
    private bool isAttacking;

    private float timeBetweenRandomMoveCounter;
    private float movementTimeCounter;
    Vector2 randomMove;
    bool randomMoving;
    
    CharacterCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        anim = GetComponentInChildren<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        combat = GetComponent<CharacterCombat>();

        timeBetweenRandomMoveCounter = Random.Range(3f, 7f);
        movementTimeCounter = Random.Range(0f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        isAttacking = false;

        Vector2 distance = target.position - transform.position;

        if(distance.magnitude <= lookRadius)
        {
            //moving
            if(distance.magnitude >= stoppingDistance) {
                isMoving = true;

                currMove = new Vector2(distance.normalized.x * moveSpeed, distance.normalized.y * moveSpeed);
                lastMove = new Vector2(distance.normalized.x, distance.normalized.y);
            }
            //attacking
            else
            {
                isAttacking = true;

                //Attack target
                AttackTarget();

                //Face target
                FaceTarget();
            }
            
            //Reset random movement
            randomMoving = false;
            timeBetweenRandomMoveCounter = Random.Range(3f, 7f);
        }
        //Random movement
        else
        {
            if (randomMoving)
            {
                isMoving = true;
                movementTimeCounter -= Time.deltaTime;
                currMove = randomMove;
                lastMove = new Vector2(currMove.normalized.x, currMove.normalized.y);;

                if (movementTimeCounter < 0f)
                {
                    randomMoving = false;
                    timeBetweenRandomMoveCounter = Random.Range(3f, 7f);
                }
            }
            else { 
                timeBetweenRandomMoveCounter -= Time.deltaTime;
                if (timeBetweenRandomMoveCounter < 0f)
                {
                    randomMoving = true;
                    movementTimeCounter = Random.Range(0f, 1f);
                
                    randomMove = new Vector2(Random.Range(-1f,1f) * moveSpeed, Random.Range(-1f,1f) * moveSpeed);
                }
            }
        }

        anim.SetFloat("MoveX", currMove.x);
        anim.SetFloat("MoveY", currMove.y);
        anim.SetBool("IsMoving", isMoving);
        anim.SetBool("IsAttacking", isAttacking);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    private void FixedUpdate()
    {
        // Adjust for deltatime
        currMove = currMove * Time.fixedDeltaTime;

        // Move the character by finding the target velocity
        Vector3 targetVelocity = currMove * 10f;
        // And then smoothing it out and applying it to the character
        myRigidbody.velocity = Vector3.SmoothDamp(myRigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    void AttackTarget()
    {
        CharacterStats targetStats = target.GetComponent<CharacterStats>();
        if(targetStats != null)
        {
            combat.Attack(targetStats);
        }
    }

    void FaceTarget()
    {
        lastMove = (target.position - transform.position).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}