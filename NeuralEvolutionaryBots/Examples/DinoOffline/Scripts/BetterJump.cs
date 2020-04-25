using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour {
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2;
	Rigidbody2D rb;
	DinoPlayer dp;

	void Awake () {
		dp = GetComponent<DinoPlayer> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	public void ApplyBetterJump () {
		if (rb.velocity.y < 0) {
			rb.gravityScale = fallMultiplier;
		} else if (rb.velocity.y > 0 && !dp.JumpArrow) {
			rb.gravityScale = lowJumpMultiplier;
		} else {
			rb.gravityScale = 1;
		}
	}
}