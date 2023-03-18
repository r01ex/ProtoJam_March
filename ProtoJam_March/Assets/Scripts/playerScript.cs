using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class playerScript : MonoBehaviour
{
    Rigidbody2D rbody;
    [SerializeField] Collider2D legcollider;
    [SerializeField] LayerMask groundLayer;

    [Space(10)]
    [SerializeField] float runMaxSpeed;
    [SerializeField] float runAccelAmount;
    [SerializeField] float runDeccelAmount;
    int direction;
    bool isFacingRight = true;

    bool longJump;
    public float defaultgrav;
    [SerializeField] float gravityMult;
    [SerializeField] float jumpPower;
    [SerializeField] float coyoteTime;
    [SerializeField] float baseJumpVelocity;
    float jumpedtime;   //점프키 누른 타이밍
    public float coyoteTimeCounter;

    public bool spikehitRecent = false;

    public bool isholdingBox = false;
    [SerializeField] GameObject pickupBoxPrefab;
    [SerializeField] Transform dropPos;
    [SerializeField] float throwPower;


    public bool isAbilityActive { get; private set; }
    public float remainingAbilityTime;
    public UnityEvent onAbilityActive;
    public UnityEvent onAbilityDeactive;

    #region Singleton
    public static playerScript Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    #endregion
    
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        defaultgrav = rbody.gravityScale;
    }
    public bool isOnGround()
    {
        bool isTouchingGround = legcollider.IsTouchingLayers(groundLayer);
        this.GetComponent<Animator>().SetBool("isOnGround", isTouchingGround);
        return isTouchingGround;
    }
    public void dropBox()
    {
        //GameObject box = Instantiate(pickupBoxPrefab, dropPos.position, Quaternion.identity); //떨구기

        GameObject box = Instantiate(pickupBoxPrefab, dropPos.position, Quaternion.identity); //던지기
        box.GetComponent<Rigidbody2D>().velocity = new Vector2(3*this.transform.localScale.x, 1) * throwPower; //던지기

        box.GetComponent<pickupBox>().init();
        isholdingBox = false;
    }
    void Update()
    {
        if (isOnGround())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (spikehitRecent == false)
        {
            if (Input.GetKeyDown(KeyCode.W) && coyoteTimeCounter > 0)
            {
                jumpedtime = Time.time;
                rbody.velocity = new Vector2(rbody.velocity.x, baseJumpVelocity);
                longJump = true;
                coyoteTimeCounter = 0;
            }
            else if (Input.GetKey(KeyCode.W) && (rbody.velocity.y > 0) && longJump == true)
            {
                rbody.AddForce(new Vector2(0, Time.deltaTime * jumpPower));
            }
            if (Input.GetKeyUp(KeyCode.W) || (longJump == true && Time.time - jumpedtime >= 0.2f))  //0.2f 값은 착지-재점프 까지의 딜레이?
            {
                longJump = false;
                coyoteTimeCounter = 0;
            }
        }
        if (rbody.velocity.y < 0)
        {
            this.GetComponent<Animator>().SetFloat("playerYvelocity", rbody.velocity.y);
            rbody.gravityScale = defaultgrav * gravityMult;
        }
        else
        {
            this.GetComponent<Animator>().SetFloat("playerYvelocity", rbody.velocity.y);
            rbody.gravityScale = defaultgrav;
        }

        //좌-우 이동 + 애니메이션 처리
        if (Input.GetKey(KeyCode.A))
        {
            this.GetComponent<Animator>().SetBool("isWalking", true);           
            direction = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.GetComponent<Animator>().SetBool("isWalking", true);
            direction = 1;
        }
        else
        {
            this.GetComponent<Animator>().SetBool("isWalking", false);
            direction = 0;
        }
        if (direction != 0)
        {
            Turn(direction > 0);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isAbilityActive == false)
            {
                if (remainingAbilityTime >= 0)
                {
                    onAbilityActive.Invoke();
                    //ability bar effect
                    //backgrond, char effect
                    isAbilityActive = true;
                }
            }
            else
            {
                //ability bar effect
                //backgrond, char effect
                isAbilityActive = false;
                onAbilityDeactive.Invoke();
            }
        }
        if (isAbilityActive == true)
        {
            if (remainingAbilityTime >= 0)
            {
                remainingAbilityTime -= Time.deltaTime;
                isAbilityActive = true;
            }
            else
            {
                //ability bar effect
                //backgrond, char effect
                onAbilityDeactive.Invoke();
                isAbilityActive = false;
            }
        }

        if (isholdingBox == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                dropBox();
            }
        }

    }

    private void FixedUpdate()
    {
        if (spikehitRecent == false)
        {
            Run();
        }
    }
    private void Run()
    {
        float targetSpeed = direction * runMaxSpeed;

        #region Calculate AccelRate
        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        #endregion      

        #region Conserve Momentum
        if (Mathf.Abs(rbody.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rbody.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelRate = 0;
        }
        #endregion

        float speedDif = targetSpeed - rbody.velocity.x;

        float movement = speedDif * accelRate;

        rbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    private void Turn(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight && rbody.bodyType != RigidbodyType2D.Static)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }
    }
}
