using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject bala;

    [SerializeField]
    private float speed = 4f;
    

    private bool estadoEspada = true;
    
    private Rigidbody2D mRb;
    private Vector3 mDirection = Vector3.zero;
    private Animator mAnimator;
    private PlayerInput mPlayerInput;
    private Transform hitBox;
    private Vector2 lastDirection;

    private void Start()
    {
        mRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mPlayerInput = GetComponent<PlayerInput>();

        lastDirection = new Vector2(0,-1);

        hitBox = transform.Find("HitBox");

        ConversationManager.Instance.OnConversationStop += OnConversationStopDelegate;
        
    }

    private void OnConversationStopDelegate()
    {
        mPlayerInput.SwitchCurrentActionMap("Player");
    }

    private void Update()
    {
        if (mDirection != Vector3.zero)
        {
            mAnimator.SetBool("IsMoving", true);
            mAnimator.SetFloat("Horizontal", mDirection.x);
            mAnimator.SetFloat("Vertical", mDirection.y);
            lastDirection = new Vector2(mDirection.x,mDirection.y);
        }else
        {
            // Quieto
            mAnimator.SetBool("IsMoving", false);
        }
    }

    private void FixedUpdate()
    {
        mRb.MovePosition(
            transform.position + (mDirection * speed * Time.fixedDeltaTime)
        );
    }

    public void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>().normalized;
    }

    public void OnNext(InputValue value)
    {
        if (value.isPressed)
        {
            ConversationManager.Instance.NextConversation();
        }
    }

    public void OnCancel(InputValue value)
    {
        if (value.isPressed)
        {
            ConversationManager.Instance.StopConversation();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {   
            if(estadoEspada){
                mAnimator.SetTrigger("Attack");
                hitBox.gameObject.SetActive(true);
            }else{
                DisparoScript disparo = Instantiate(bala, transform.position,Quaternion.identity).GetComponent<DisparoScript>();
                disparo.Setup(lastDirection);
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Conversation conversation;
        if (other.transform.TryGetComponent<Conversation>(out conversation))
        {
            mPlayerInput.SwitchCurrentActionMap("Conversation");
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    public void DisableHitBox()
    {
        hitBox.gameObject.SetActive(false);
    }

    

    public void OnCambiarArma(InputValue value){
        if (value.isPressed)
        {
           if(estadoEspada){
            estadoEspada = false;
           }else{
            estadoEspada = true;
           }
        }
    }

    
}
