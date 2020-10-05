using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    private int lives = 5;
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float jumpForce = 15f;
    [SerializeField]
    private float additionalJumps = 1f;

    public int Lives { set { lives += value; } get { return lives; } }

    private float dir = 1f;
    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;
    private float startBullet;

    private bool isGrounded = false;

    private Bullet bullet;
    
    public CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
        startBullet = Time.time;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded)
        {
            State = CharState.Idle;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Time.time - startBullet > 0.5f)
        {
            Shoot();
            startBullet = Time.time;
        }

        if (Input.GetButton("Horizontal"))
        {
            Run();
        }

        if ((isGrounded || additionalJumps > 0f) && Input.GetButtonDown("Jump"))
        {
            Jump();
            additionalJumps--;
        }

        if (lives == 0)
        {
            Destroy(gameObject);
            Application.Quit();
        }
    }

    private void Run()
    {
        dir = Input.GetAxis("Horizontal");
        Vector3 direction = transform.right * dir;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0f;

        if (isGrounded)
        {
            State = CharState.Run;
        }
    }

    private void Jump()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0); 
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        Vector3 position = transform.position;
        position.y += 0.8f;


        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.CharacterBullet = true;
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0f : 1.0f);
    }

    public override void RecieveDamage(float opponentPosition = 1.0f)
    {
        lives--;
        Debug.Log(lives);

        speed = 0f;
        jumpForce = 0f;

        Invoke(nameof(setDefault), 0.28f);

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.right * (opponentPosition < 0f ? -1f : 1f) * 4f, ForceMode2D.Impulse);
    }

    private void setDefault()
    {
        speed = 3f;
        jumpForce = 15f;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded)
        {
            State = CharState.Jump;
        }
        else
        {
            additionalJumps = 1f;
        }
    }

    public override void GetHeart()
    {
        lives++;
        Debug.Log(lives);
    }
}


public enum CharState
{
    Idle,
    Run,
    Jump
}
