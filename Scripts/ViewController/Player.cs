using System;
using QFramework;
using UnityEngine;

[Serializable]
public class Player : MonoBehaviour, IController
{
	public Animator animator;

	private Rigidbody2D rb;

	public Gun gun;

	private GameObject dieExplosionEffect;

	private bool isInventorySwitch = false;

	private bool isSettingPanelSwitch = false;

	private NPC npc;

	public IPlayerModel playerModel;

	public bool isMove = true;

	public bool isShoot = true;

	public float moveDir;

	public bool isGrounded;

	public float ReveiveTime = 2.5f;

	private Trigger2DCheck groundCheck;

	private AudioSource audioSource;

	public static Player Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		playerModel = this.GetModel<IPlayerModel>();
		rb = GetComponent<Rigidbody2D>();
		groundCheck = (transform.Find("GroundCheck")).GetComponent<Trigger2DCheck>();
		dieExplosionEffect = Resources.Load<GameObject>("ExplosionEffect");
		npc = FindObjectOfType<NPC>();
		audioSource = transform.GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{
		isGrounded = groundCheck.Triggered;
		if (isMove && isGrounded)
		{
			Move();
			Dash();
			rb.velocity = new Vector2(moveDir * (float)playerModel.ActualSpeed, rb.velocity.y);
			Flip();
			if (rb.velocity.x > Mathf.Epsilon)
			{
				GameManager.Instance.PlaySound(audioSource);
			}
			else
			{
				GameManager.Instance.StopSound(audioSource);
			}
		}
	}

	private void Update()
	{
		moveDir = Input.GetAxis("Horizontal");
		PlayerInput();
		if (playerModel.HP.Value <= 0)
		{
			Die();
		}
	}

	private void LateUpdate()
	{
		if (!isMove && !isShoot)
		{
			Invoke("InvokeMove", 0.25f);
		}
	}

	private void PlayerInput()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			animator.SetTrigger("isShooting");
			gun.Attack();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			gun.Reload();
		}
		ShiftWeaponInput();
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			GameManager.Instance.PlayClikAudio();
			isInventorySwitch = !isInventorySwitch;
			GameUIManager.Instance.InventorySwitch();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameManager.Instance.PlayClikAudio();
			isSettingPanelSwitch = !isSettingPanelSwitch;
			GameUIManager.Instance.SettingPanelSwitch();
		}
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			GameManager.Instance.PlayJumpSound();
			animator.SetTrigger("isJumping");
			rb.velocity = new Vector2(rb.velocity.x * moveDir, (float)playerModel.JumpForce);
		}
		if (Input.GetKeyDown(KeyCode.E) && npc.isShowInteract)
		{
			GameManager.Instance.PlayClikAudio();
			GameUIManager.Instance.DialoguePanel.UpdateDialogue();
		}
	}

	public void Move()
	{
		playerModel.ActualSpeed.Value = playerModel.MoveSpeed;
		animator.SetBool("isWalking", Mathf.Abs(rb.velocity.x) > Mathf.Epsilon);
	}

	public void Dash()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			playerModel.ActualSpeed.Value = playerModel.MoveSpeed * playerModel.SprintSpeedMultiplier;
			animator.SetBool("isRunning", animator.GetBool("isWalking"));
		}
		else
		{
			playerModel.ActualSpeed.Value = playerModel.MoveSpeed;
			animator.SetBool("isRunning", false);
		}
	}

	public void Flip()
	{
		if (rb.velocity.x > 0.1f)
		{
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		if (rb.velocity.x < -0.1f)
		{
			transform.rotation = Quaternion.Euler(0f, 180f, 0f);
		}
	}

	private void ShiftWeaponInput()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.SendCommand(new ShiftGunCommand(1));
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.SendCommand(new ShiftGunCommand(2));
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((collision.transform).CompareTag("EnemyBullet") || (collision.transform).CompareTag("Enemy"))
		{
			GameManager.Instance.PlayHurtAudio();
			isMove = false;
			isShoot = false;
			rb.velocity = Vector2.zero;
			animator.SetBool("isHurt", true);
		}
		else
		{
			animator.SetBool("isHurt", false);
		}
	}

	private void InvokeMove()
	{
		isMove = true;
		isShoot = true;
		animator.SetBool("isHurt", false);
		rb.velocity = new Vector2(moveDir * playerModel.ActualSpeed.Value, rb.velocity.y);
	}

	private void Die()
	{
		GameObject explosion = Instantiate(dieExplosionEffect, transform.position, Quaternion.identity);
		GameManager.Instance.PlayerExplosionAudio();
		Invoke("Reveive", ReveiveTime);
		gameObject.SetActive(false);
		Destroy(explosion, 1f);
	}

	public void Reveive()
	{
		playerModel.HP.Value = playerModel.MaxHp;
		gameObject.SetActive(true);
		gameObject.transform.position = new Vector3(-2f, 0f, 0f);
	}

	public IArchitecture GetArchitecture()
	{
		return Architecture<Game2D>.Interface;
	}
}
