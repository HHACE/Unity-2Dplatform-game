using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class player : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _sprite;

    [SerializeField] private float movespeed;
    [SerializeField] private float jumpHigh;
    [SerializeField] private float isGrounded;
    [SerializeField] private bool canMove;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector2 _groundCheckSize;

    [SerializeField] private Transform _wallCheckLeft;
    [SerializeField] private Vector2 _wallCheckSizeLeft;
    [SerializeField] private Transform _wallCheckRight;
    [SerializeField] private Vector2 _wallCheckSizeRight;

    //Dashing
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;

    private TrailRenderer _tr;


    //collision
    [SerializeField] private GameObject _spike;

    private float inputhorizontal;
    private float inputvertical;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _tr = GetComponent<TrailRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        if (isDashing) {
            Physics2D.IgnoreCollision(_spike.GetComponent<CompositeCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), true);
            return;
        }
        inputhorizontal = Input.GetAxisRaw("Horizontal");
        inputvertical = Input.GetAxisRaw("Vertical");
        Walk();
 
        Dashing();
    }

    private void Walk() {
        if (canMove) {
            if (inputhorizontal == 1 ) {
                _anim.SetBool("aWalk", true);
                _sprite.flipX = false;
            } else if (inputhorizontal ==-1)
            {
                _anim.SetBool("aWalk", true);
                _sprite.flipX = true;
            }
            else 
            {
                _anim.SetBool("aWalk", false);
            }

        _rb.velocity = new Vector2(inputhorizontal * movespeed, _rb.velocity.y);
        }


    }


    private void Jump() {

        if (Input.GetKeyDown(KeyCode.Z) && groundedCheck())
        {
            _anim.SetBool("aJump", true);
            _rb.gravityScale = 1.5f;
            _rb.AddForce(new Vector2(0, jumpHigh));
        }
        else if (Input.GetKeyDown(KeyCode.Z) && walledCheckedRight())
        {
            Physics2D.IgnoreCollision(_spike.GetComponent<CompositeCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), false);
            isDashing = false;
            _anim.SetBool("aJump", true);
            canMove = false;
            _rb.gravityScale = 1.5f;
            _rb.AddForce(new Vector2(-jumpHigh * 2, jumpHigh));
            Invoke("CanMove", 0.25f);
        }
        else if (Input.GetKeyDown(KeyCode.Z) && walledCheckedLeft())
        {
            isDashing = false;
            Physics2D.IgnoreCollision(_spike.GetComponent<CompositeCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), false);
            _anim.SetBool("aJump", true);
            canMove = false;
            _rb.gravityScale = 1.5f;
            _rb.AddForce(new Vector2(jumpHigh * 2 , jumpHigh));
            Invoke("CanMove", 0.25f);
        }

        else if (!Input.GetKey(KeyCode.Z) && isDashing == false || _rb.velocity.y < 0 && isDashing == false) {
            _anim.SetBool("aJump", false);
            _anim.SetBool("aFall", true);
            _rb.gravityScale = 3;
            if (groundedCheck())
            {
               
                _anim.SetBool("aFall", false);
            }
        }

            

    }

    private bool groundedCheck()
    {
        return Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0, LayerMask.GetMask("Ground"));
    }
    private bool walledCheckedLeft()
    {
        return Physics2D.OverlapBox(_wallCheckLeft.position, _wallCheckSizeLeft, 0, LayerMask.GetMask("Ground")) && Input.GetKey(KeyCode.LeftArrow);
    }
    private bool walledCheckedRight() {
        return Physics2D.OverlapBox(_wallCheckRight.position, _wallCheckSizeRight, 0, LayerMask.GetMask("Ground")) && Input.GetKey(KeyCode.RightArrow);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_groundCheck.position, _groundCheckSize);
        Gizmos.DrawWireCube(_wallCheckLeft.position, _wallCheckSizeLeft);
        Gizmos.DrawWireCube(_wallCheckRight.position, _wallCheckSizeRight);
    }




    private void CanMove()
    {
        canMove = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "spike")
        {
            canMove = false;
            _rb.simulated = false;
            _anim.SetTrigger("Death");
            Invoke("LoadcurrentScene",0.4f);
        }
    }

    private void LoadcurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void Dashing() {
        if (groundedCheck()) { canDash = true; }

        if (Input.GetKeyDown(KeyCode.X) && canDash) {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float ogGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _tr.emitting = true;
        if (inputhorizontal != 0 || inputvertical != 0)
        {
            _rb.velocity = new Vector2(inputhorizontal * dashingPower, inputvertical*dashingPower);
        }
        else {
            if (_sprite.flipX == true)
            {
                _rb.velocity = new Vector2(-1 * dashingPower, 0f);
            }
            else {
                _rb.velocity = new Vector2(1 * dashingPower, 0f);
            }
        }
        yield return new WaitForSeconds(dashingTime);
        _tr.emitting = false;
        _rb.gravityScale = ogGravity;
        isDashing = false;
        Physics2D.IgnoreCollision(_spike.GetComponent<CompositeCollider2D>(), gameObject.GetComponent<BoxCollider2D>(), false);
    }


}
