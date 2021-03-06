﻿using System;
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
    private CharacterCombat myCombat;

    private bool isMoving;
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
        myCombat = GetComponent<CharacterCombat>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;

        //Check for movement keys
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f
                                                  || Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            isMoving = true;
            currMove = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, Input.GetAxisRaw("Vertical") * moveSpeed);
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            RemoveFocus();
        }
        
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("IsMoving", isMoving);

        //Avoids clicking through UI overlays like Inventory (picking up something behind it or something)
        //Note: Place all mouseclick events below here
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        //Check for leftmouse click for Attacking
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AttackAnimation(0.5f));
            
            //Raycast Cone in front of Player
            Vector2 ray = new Vector2(transform.position.x, transform.position.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray, lastMove, 1f);
            
            List<CharacterStats> targets = new List<CharacterStats>();
            //Cycle through hits
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != null)
                {
                    Hitbox hitbox = hits[i].collider.GetComponent<Hitbox>();
                    
                    //Was it a hitbox and was Player within Enemy radius?
                    if (hitbox != null && hitbox.WithinRadius(transform))
                    {
                        Transform target = hitbox.transform.parent;
                        Debug.Log("Raycast Left-click Hitbox hit " + target.name);
                        CharacterStats targetStats = target.GetComponent<CharacterStats>();
                        targets.Add(targetStats);
                    }
                }
            }
            if(targets.Count > 0)
            {
                myCombat.AttackGroup(targets);
            }
        }

        //Check for rightmouse click on Interactables
        if (Input.GetMouseButtonDown(1))
        {   
            //Create Ray
            Vector2 ray = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100);

            //check if ray hits (num is distance)
            if (hit.collider != null)
            {
                Debug.Log("Raycast Right-click Hit: " + hit.collider.gameObject.name);

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
        LookAt(focus.transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }

    void LookAt(Transform target)
    {
        lastMove = (target.position - transform.position).normalized;
    }
    
    
    IEnumerator AttackAnimation(float delay)
    {
        anim.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(delay);
        anim.SetBool("IsAttacking", false);
    }
}