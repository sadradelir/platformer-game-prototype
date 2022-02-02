using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    // status 
    [SerializeField]
    private bool jumping;

    [SerializeField]
    private bool grounded;

    public bool touchingMovingPlatform;
    public Transform groundingObjectTransform;

    private float xSpeed;
    private float ySpeed;

    [SerializeField]
    private bool rightWalled;

    [SerializeField]
    private bool leftWalled;

//  [SerializeField] private float lastNonZeroXSpeed;
    [SerializeField]
    private float ledgeTime;

    private bool doubleJumped = false;

    // balance parameters
    public float jumpSpeed;
    public float gravity;
    public float initialSpeed;
    public float fallSpeed;
    public float accelaration;
    public Transform bottomPosition;
    public Transform rightPosition;
    public Transform leftPosition;
    public Transform topPosition;

    // sensing 
    [SerializeField]
    private float distanceToGround;

    [SerializeField]
    private float distanceToCeiling;

    [SerializeField]
    private float distanceToWallRight;

    [SerializeField]
    private float distanceToWallLeft;

    public BoxCollider2D collider;

    // misc
    private Coroutine jumpRoutine;
    public bool jumpButtonDown;
    public bool rightButtonDown;
    public bool leftButtonDown;

    // animation
    public SpriteRenderer sprite;
    public Animator anim;


    // Update is called once per frame
    void Update()
    {
        GetInput();
        CheckDistanceToGround();
        CheckDistanceToCeiling();
        CheckDistanceToWallRight();
        CheckDistanceToWallLeft();
        ActMovement();
        ActToMovingPlatform();
    }


    private void LateUpdate()
    {
        UpdateCharacterFacing();
        UpdateAnitmation();
    }


    private void CheckDistanceToCeiling()
    {
        for (int i = -1; i <= 1; i++)
        {
            var origin = topPosition.position + 0.2f * i * Vector3.right;
            Debug.DrawRay(origin, Vector3.up, Color.white);
            var hit = Physics2D.Raycast(origin,
                Vector2.up, 0.5f, LayerMask.GetMask("Ground"));
            if (hit)
            {
                distanceToCeiling = hit.distance;
                return;
            }
        }

        distanceToCeiling = Single.PositiveInfinity;
    }

    private void CheckDistanceToGround()
    {
        distanceToGround = Single.PositiveInfinity;
        bool aRayHit = false;
        for (int i = -1; i <= 1; i++)
        {
            var origin = bottomPosition.position + 0.2f * i * Vector3.right;
            Debug.DrawRay(origin, Vector3.down, Color.red);
            var hit = Physics2D.Raycast(origin,
                Vector2.down, 1f, LayerMask.GetMask("Ground", "Slabs"));
            if (hit && hit.distance < distanceToGround)
            {
                distanceToGround = hit.distance;
                groundingObjectTransform = hit.transform;
                aRayHit = true;
            }
        }

        if (grounded)
        {
            ledgeTime = Time.time;
        }

        if (!aRayHit || distanceToGround > 0.1f)
        {
            grounded = false;
        }
    }

    private void CheckDistanceToWallRight()
    {
        distanceToWallRight = Single.PositiveInfinity;
        bool aRayHit = false;
        for (int i = -2; i <= 1; i++)
        {
            if (grounded && i == -2)
            {
                continue;
            }

            var origin = rightPosition.position + 0.25f * 0.99f * i * Vector3.up;
            Debug.DrawRay(origin, Vector3.right, Color.red);
            var hit = Physics2D.Raycast(origin,
                Vector2.right, 1f, LayerMask.GetMask("Ground"));
            if (hit && hit.distance < distanceToWallRight)
            {
                distanceToWallRight = hit.distance;
                aRayHit = true;
            }
        }

        if (!aRayHit || distanceToWallRight > 0.2f)
        {
            rightWalled = false;
        }
    }

    private void CheckDistanceToWallLeft()
    {
        distanceToWallLeft = Single.PositiveInfinity;
        bool aRayHit = false;
        for (int i = -2; i <= 1; i++)
        {
            if (grounded && i == -2)
            {
                continue;
            }

            var origin = leftPosition.position + 0.25f * 0.99f * i * Vector3.up;
            Debug.DrawRay(origin, Vector3.left, Color.red);
            var hit = Physics2D.Raycast(origin,
                Vector2.left, 1f, LayerMask.GetMask("Ground"));
            if (hit && hit.distance < distanceToWallLeft)
            {
                distanceToWallLeft = hit.distance;
                aRayHit = true;
            }
        }

        if (!aRayHit || distanceToWallLeft > 0.2f)
        {
            leftWalled = false;
        }
    }


    public void SetButtonDown(string button)
    {
        if (button == "right")
        {
            rightButtonDown = true;
        }
        else if (button == "left")
        {
            leftButtonDown = true;
        }
        else if (button == "jump")
        {
            jumpButtonDown = true;
        }
    }

    public void SetButtonUp(string button)
    {
        if (button == "right")
        {
            rightButtonDown = false;
        }
        else if (button == "left")
        {
            leftButtonDown = false;
        }
        else if (button == "jump")
        {
            jumpButtonDown = false;
        }
    }


    private void GetInput()
    {
        float inputaxis = GetInputAxis();
        float acc = accelaration;
        xSpeed =
            Mathf.MoveTowards(xSpeed,
                initialSpeed * inputaxis,
                acc * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space) || jumpButtonDown)
        {
            jumpButtonDown = false;
            jumping = true;
            if (jumpRoutine != null)
            {
                StopCoroutine(jumpRoutine);
            }

            jumpRoutine = StartCoroutine(falseJumping());
        }
    }

    private float GetInputAxis()
    {
        var horizontalAxisInput = Input.GetAxisRaw("Horizontal");
        horizontalAxisInput += rightButtonDown ? 1 : 0;
        horizontalAxisInput += leftButtonDown ? -1 : 0;
        return horizontalAxisInput;
    }

    private IEnumerator falseJumping()
    {
        yield return new WaitForSeconds(0.2f);
        jumping = false;
    }

    private void ActMovement()
    {
        var ledgeTimeJump = Time.time - ledgeTime < 0.1f;
        if (jumping && (!doubleJumped || grounded || ledgeTimeJump))
        {
            if (!grounded && !ledgeTimeJump)
            {
                doubleJumped = true;
            }

            jumping = false;
            grounded = false;
            ySpeed = jumpSpeed;
        }

        //wall jump
        // if (jumping && rightWalled)
        // {
        //     xSpeed = -initialSpeed;
        //     rightWalled = false;
        //     jumping = false;
        //     ySpeed = jumpSpeed;
        // }

        // //
        // if (jumping && leftWalled)
        // {
        //     xSpeed = initialSpeed;
        //     leftWalled = false;
        //     jumping = false;
        //     ySpeed = jumpSpeed;
        // }

        //y 

        ySpeed -= gravity * Time.deltaTime;
        ySpeed = Mathf.Max(fallSpeed, ySpeed);

        // wall falling 
        // if ((rightWalled || leftWalled) && ySpeed < 0)
        // {
        //     ySpeed = Mathf.Clamp(ySpeed, -fallSpeed, 0);
        // }

        var amountToFall = ySpeed * Time.deltaTime;
        if (ySpeed < 0 && Mathf.Abs(amountToFall) > distanceToGround)
        {
            amountToFall = -distanceToGround;
            grounded = true;
            doubleJumped = false;
            ySpeed = 0;
        }

        if (ySpeed > 0 && amountToFall > distanceToCeiling)
        {
            amountToFall = distanceToCeiling;
            ySpeed = 0;
        }

        //x
        var amountToMove = xSpeed * Time.deltaTime;
        if (xSpeed > 0 && amountToMove > distanceToWallRight)
        {
            amountToMove = distanceToWallRight;
            rightWalled = true;
            xSpeed = 0;
        }

        if (xSpeed < 0 && Mathf.Abs(amountToMove) > distanceToWallLeft)
        {
            amountToMove = -distanceToWallLeft;
            leftWalled = true;
            xSpeed = 0;
        }

        transform.Translate(amountToMove, amountToFall, 0);
    }


    private void ActToMovingPlatform()
    {
        if (grounded && groundingObjectTransform.CompareTag("MovingPlatform"))
        {
            transform.SetParent(groundingObjectTransform);
        }
        else
        {
            transform.SetParent(null);
        }
    }

    private void UpdateCharacterFacing()
    {
        if (xSpeed < 0)
        {
            sprite.flipX = true;
        }
        else if (xSpeed > 0)
        {
            sprite.flipX = false;
        }
    }

    private void UpdateAnitmation()
    {
        if (!grounded)
        {
            if (ySpeed > 0)
            {
                anim.Play("jump");
            }
            else
            {
                anim.Play("fall");
            }
        }
        else if (xSpeed == 0)
        {
            anim.Play("idle");
        }
        else
        {
            anim.Play("run");
        }
    }

    public void SpringJump()
    {
        ySpeed = jumpSpeed * 1.3f;
    }
}
