using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //Animation smoothing
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;

    public float moveSpeed = 20;

    private Animator anim;
    private Rigidbody2D myRigidbody;

    private bool playerMoving;
    private Vector2 currMove;
    private Vector2 lastMove;

    public Camera cam;
    public LayerMask movementMask;
    public Interactable focus;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        playerMoving = false;

        //Check for movement keys
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f
            || Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            playerMoving = true;
            currMove = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, Input.GetAxisRaw("Vertical") * moveSpeed);
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            RemoveFocus();
        }

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);

        //Avoids clicking through UI overlays like Inventory (picking up something behind it or something)
        //Note: Place all mouseclick events below here
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //Check for rightmouse click on Interactables
        if (Input.GetMouseButtonDown(1))
        {
            //Create a ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Check if ray hits
            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                //Did we hit an interactable
                if (interactable != null)
                {
                    //Set our focus to the object
                    SetFocus(interactable);
                }
            }
        }
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


    //Focus on a specific interactable object
    void SetFocus(Interactable newFocus)
    {
        //if new focus
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDefocused();
            }

            focus = newFocus;
        }

        newFocus.OnFocused(transform);
        FaceFocus();
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }

    void FaceFocus()
    {
        lastMove = (focus.transform.position - transform.position).normalized;
    }
}
