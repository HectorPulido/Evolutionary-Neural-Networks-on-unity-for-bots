using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoPlayer : MonoBehaviour {
    [Header ("Control")]
    public float jumpSpeed;

    [Header ("GroundCheck")]
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize;
    public LayerMask mask;

    private Rigidbody2D rb;
    private Animator animator;
    private BetterJump bt;

    private bool IsGround {
        get {
            var cc = Physics2D.BoxCast (groundCheckPosition.position,
                groundCheckSize,
                0,
                Vector2.zero,
                mask);

            if (cc.collider == null) {
                return false;
            }

            if (cc.collider.gameObject == gameObject) {
                return false;
            }
            if (cc.collider.CompareTag ("Player")) {
                return false;
            }
            if (cc.collider.CompareTag ("Obstacle")) {
                return false;
            }

            return true;
        }
    }

    // Start is called before the first frame update
    void Awake () {
        bt = GetComponent<BetterJump> ();
        animator = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody2D> ();
    }

    public bool DownArrow;
    public bool JumpArrow;

    private bool jumpRequest;
    private bool downRequest;

    // Update is called once per frame
    void Update () {
        if (IsGround) {
            if (JumpArrow) {
                jumpRequest = true;
            }

            animator.SetBool ("sneaking", DownArrow);
        } else {
            if (DownArrow) {
                downRequest = true;
            }
        }

    }

    void FixedUpdate () {
        bt.ApplyBetterJump ();
        if (jumpRequest && IsGround) {
            jumpRequest = false;

            rb.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Impulse);
        }
        if (downRequest && !IsGround) {
            downRequest = false;
            rb.AddForce (new Vector2 (0, -jumpSpeed), ForceMode2D.Impulse);
        }
    }
    void OnDrawGizmosSelected () {
        if (!groundCheckPosition)
            return;
        Gizmos.DrawWireCube ((Vector3) groundCheckPosition.position, (Vector3) groundCheckSize);
    }

}