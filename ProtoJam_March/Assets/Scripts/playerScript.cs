using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class playerScript : MonoBehaviour
{
    Rigidbody2D rbody;
    [SerializeField] Collider2D legcollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private ParticleSystem jumpEffect;

    [Space(10)] [SerializeField] float runMaxSpeed;
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
    float jumpedtime; //점프키 누른 타이밍
    private bool isJumpKeyEnd = false; //점프키를 땠는가?
    private float floatedtime; //공중에 떠있는 시간
    public float minimumJumpingTime; //점프시, 최소체공가능시간
    public float maximumJumpingTime; //점프시, 최대체공가능시간
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

    [SerializeField] AudioSource walkAudio;
    [SerializeField] AudioSource throwAudio;
    [SerializeField] AudioSource dropAudio;
    [SerializeField] AudioSource abilAudio;
    [SerializeField] AudioSource jumpAudio;

    public GameObject boxOverheadUI;
    public GameObject lockedBoxtarget;
    #region Singleton

    public static playerScript Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
        dropAudio.Play();
        GameObject box = Instantiate(pickupBoxPrefab, dropPos.position, Quaternion.identity); //떨구기
        box.GetComponent<pickupBox>().init();
        isholdingBox = false;
    }

    public void throwBox()
    {
        throwAudio.Play();
        GameObject box = Instantiate(pickupBoxPrefab, dropPos.position, Quaternion.identity); //던지기
        box.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * this.transform.localScale.x, 4.5f) * throwPower; //던지기
        box.GetComponent<pickupBox>().init();
        isholdingBox = false;
    }

    void Update()
    {
        //점프~착지 처리
        if (isOnGround())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            walkAudio.enabled = false;
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (spikehitRecent == false)
        {
            /*
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
            if (Input.GetKeyUp(KeyCode.W) || (longJump == true && Time.time - jumpedtime >= 0.2f))
            {
                longJump = false;
                coyoteTimeCounter = 0;
            }
            */
            if (!longJump)
            {
                if (Input.GetKeyDown(KeyCode.W) && coyoteTimeCounter >= 0f)
                {
                    jumpedtime = Time.time;
                    coyoteTimeCounter = -1f;
                    longJump = true;
                    floatedtime = 0f;
                    isJumpKeyEnd = false;
                    legcollider.enabled = false;

                    jumpAudio.Play();
                    Do_CreateDust();
                }
            }

            if (Input.GetKeyUp(KeyCode.W) && !isJumpKeyEnd)
            {
                isJumpKeyEnd = true;
            }

            if (longJump)
            {
                floatedtime += Time.deltaTime;
                float t = floatedtime / maximumJumpingTime;
                float _velocity = Mathf.Lerp(jumpPower, 1f, t);
                rbody.velocity = new Vector2(rbody.velocity.x, _velocity);
                //Debug.Log("t = " + t.ToString() + ", velocity = " + _velocity.ToString() + ", floatedTime = " + floatedtime.ToString());

                if (floatedtime > minimumJumpingTime)
                {
                    if (isJumpKeyEnd || floatedtime > maximumJumpingTime) //최대 점프 체공가능 시간이 다되면 컷
                    {
                        longJump = false;
                        isJumpKeyEnd = false;
                        floatedtime = 0f;
                        rbody.velocity = new Vector2(rbody.velocity.x, 0f);
                        legcollider.enabled = true;
                    }
                }
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
            if (isOnGround())
            {
                walkAudio.enabled = true;
            }
            else
            {
                walkAudio.enabled = false;
            }

            this.GetComponent<Animator>().SetBool("isWalking", true);
            direction = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (isOnGround())
            {
                walkAudio.enabled = true;
            }
            else
            {
                walkAudio.enabled = false;
            }

            this.GetComponent<Animator>().SetBool("isWalking", true);
            direction = 1;
        }
        else
        {
            walkAudio.enabled = false;
            this.GetComponent<Animator>().SetBool("isWalking", false);
            direction = 0;
        }

        if (direction != 0)
        {
            Turn(direction > 0);
        }


        //어빌리티(부스터) 입력처리
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //SmokyOnion -> R01ex :: 이 부분 함수로 추출했습니다. 스테이지 넘어갈 때마다 GameManager에서 접근해서 능력 사용인 상태로 넘어가면 해제되도록 하기 위해서...
            ActivateAbility();
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

        //(박스 들고 있을 경우) 박스 드랍 처리
        if (isholdingBox == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                dropBox();
                boxOverheadUI.SetActive(false);
                this.gameObject.GetComponent<Animator>().SetBool("isthrowing", true);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                throwBox();
                boxOverheadUI.SetActive(false);
                this.gameObject.GetComponent<Animator>().SetBool("isthrowing", true);
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

        if (Mathf.Abs(rbody.velocity.x) > Mathf.Abs(targetSpeed) &&
            Mathf.Sign(rbody.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
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

    public void ActivateAbility()
    {
        if (isAbilityActive == false)
        {
            if (remainingAbilityTime >= 0)
            {
                onAbilityActive.Invoke();
                //ability bar effect
                //backgrond, char effect
                abilAudio.enabled = true;
                abilAudio.Play();
                isAbilityActive = true;
                GameManager.instance.Do_AbilityEffectOn();
            }
        }
        else
        {
            //ability bar effect
            //backgrond, char effect
            abilAudio.enabled = false;
            isAbilityActive = false;
            onAbilityDeactive.Invoke();
            GameManager.instance.Do_AbilityEffectOff();;
        }
    }

    public void setIsthrowingOff()
    {
        this.gameObject.GetComponent<Animator>().SetBool("isthrowing", false);
    }
    public void setIspickingupfalse()
    {
        this.gameObject.GetComponent<Animator>().SetBool("ispickingup", false);
    }

    private void Do_CreateDust()
    {
        if (jumpEffect == null)
            return;
        jumpEffect.Play();
    }
}