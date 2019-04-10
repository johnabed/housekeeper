using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private Animator anim;
    private Rigidbody2D myRigidbody;

    private bool playerMoving;
    private Vector2 lastMove;

    public Camera cam;
    public LayerMask movementMask;
    public Interactable focus;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        playerMoving = false;

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            RemoveFocus();

            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidbody.velocity.y);
            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            RemoveFocus();

            //transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
            playerMoving = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
        {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        }
        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
        }

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);

        //Check for rightmouse click on Interactables
        if (Input.GetMouseButtonDown(1))
        {   
            //Create a ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Check if ray hits
            if(Physics.Raycast(ray, out hit, 100))
            {
                print("rightclick hit"); //Todo: Remove
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                //Did we hit an interactable
                if(interactable != null)
                {
                    //Set our focus to the object
                    SetFocus(interactable);
                }
            }
        }
    }

    //Focus on a specific interactable object
    void SetFocus(Interactable newFocus)
    {
        print("SetFocus"); //Todo: Remove

        //if new focus
        if (newFocus != focus)
        {
            if(focus != null)
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
        print("RemoveFocus"); //Todo: Remove

        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }

    void FaceFocus()
    {
        //Todo: DEBUG CLOTHING NOT ROTATING BECAUSE THEY DONT SHARE lastMove VARIABLE!!!

        //get dist from object
        Vector2 direction = (focus.transform.position - transform.position).normalized;
        lastMove.x = direction.x;
        lastMove.y = direction.y; 
    }
}
