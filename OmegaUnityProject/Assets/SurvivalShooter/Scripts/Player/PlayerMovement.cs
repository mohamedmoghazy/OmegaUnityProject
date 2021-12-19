
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	float camRayLength = 100f;

	void Awake()
	{
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		float h = VariableJoystick.Instance.Horizontal;
		float v = VariableJoystick.Instance.Vertical;

		Move (h, v);
		Animating (h, v);
	}

	void Move(float h, float v)
	{
		movement.Set (h, 0f, v);

		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);

		if (movement == Vector3.zero)
		{
			return;
		}
		
		Quaternion newRotation = Quaternion.LookRotation (movement);
		playerRigidbody.MoveRotation (newRotation);
	}

	void Animating (float h, float v){
		bool walking = h != 0f || v != 0f;
		anim.SetBool("IsWalking", walking);
	}
}
