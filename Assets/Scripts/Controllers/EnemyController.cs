using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float lookRadius = 6f;
    float lookRadiusMin = 1f;

    Transform target;

    //Animation smoothing
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;

    public float moveSpeed = 20;

    private Animator anim;
    private Rigidbody2D myRigidbody;

    private bool enemyMoving;
    private Vector2 currMove;
    private Vector2 lastMove;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        anim = GetComponentInChildren<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyMoving = false;

        Vector2 distance = target.position - transform.position;

        if(distance.magnitude <= lookRadius && distance.magnitude >= lookRadiusMin)
        {
            enemyMoving = true;

            //Make sure enemy not trying to move to handle small range
            float moveX = Mathf.Abs(distance.x) < 0.1f ? 0f : Mathf.Sign(distance.x);
            float moveY = Mathf.Abs(distance.y) < 0.1f ? 0f : Mathf.Sign(distance.y);

            currMove = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
            lastMove = new Vector2(moveX, moveY);
        }

        anim.SetFloat("MoveX", currMove.x);
        anim.SetFloat("MoveY", currMove.y);
        anim.SetBool("EnemyMoving", enemyMoving);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
