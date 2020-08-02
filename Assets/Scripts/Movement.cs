using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    private Collision coll;
    [HideInInspector]
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Space]
    [Header("Stats")]
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public float horizontalMoveInput;
    public float verticalMoveInput;
    public float dashSpeed = 50f;
    public float dashTime = 0.4f;
    public float inControl = 1;
    public float floatCooldown = 0.1f;

    public int side = 1;

    [Space]
    [Range(0.0f, 1.0f)]
    public float deadZone;

    [Space]

    [Header("Booleans")]
    public bool canMove = true;
    public bool canControlX = true;
    public bool groundTouch;
    public bool hasDashed;
    public bool isDashing;
    public bool isFloating;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        horizontalMoveInput = Input.GetAxis("Horizontal");
        verticalMoveInput = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(xRaw, yRaw);

        //Jump
        if (Input.GetButtonDown("Jump") && groundTouch)
            Jump(Vector2.up);
        
        //Float
        if(Input.GetButton("Jump") && !groundTouch && rb.velocity.y < 0)
        {
            isFloating = true;
            floatCooldown = 0.1f;
            rb.drag = 5;
        }
        if ( floatCooldown < 0)
        {
            rb.drag = 0;
            isFloating = false;
        }
        else
            floatCooldown -= Time.deltaTime;

        //pseudo Friction
        if (dir == Vector2.zero && !isDashing)
            rb.velocity = new Vector2(0, rb.velocity.y);

        //Move
        else if (canMove && canControlX)
            Move(dir);

        //FlipSprite
        if (rb.velocity.x > 0)
            side = 1;
        else if (rb.velocity.x < 0)
            side = -1;
        Flip(side);

        //Dash
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.RightControl)) && !hasDashed)
        {
            Dash(xRaw, yRaw);
        }

        //Manage GroundTouch
        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }
        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

    }

    private void Move(Vector2 dir)
    {
        if(!groundTouch)
            rb.velocity = new Vector2(dir.x * moveSpeed * inControl  *1.2f, rb.velocity.y);
        else
            rb.velocity = new Vector2(dir.x * moveSpeed * inControl, rb.velocity.y);
    }

    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }

    private void Dash (float x ,float y)
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.1f, .3f, 10, 90, false, true);
        //FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
        hasDashed = true;
        if (x == 0 && y == 0)
            x = sr.flipX == false ? 1 : -1;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);
        //Debug.Log(dir.normalized);
        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, dashTime + 0.2f /*.8f*/, RigidbodyDrag);

        //dashParticle.Play();

        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        //wallJumped = true;
        isDashing = true;
        canControlX = false;
        inControl = 0;

        yield return new WaitForSeconds(dashTime - 0.15f);
        canControlX = true;
        inControl = 1f;

        yield return new WaitForSeconds(0.15f);
        inControl = 1;

        //dashParticle.Stop();
        rb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        //wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (groundTouch)
            hasDashed = false;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void GroundTouch()
    {
        canControlX = true;
        hasDashed = false;
        isDashing = false;
        isFloating = false;
        rb.drag = 0;

        //side = sr.flipX ? -1 : 1;

        //jumpParticle.Play();
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    private void Flip(int side)
    {
        bool state = (side == 1) ? false : true;
        sr.flipX = state;
    }
}
