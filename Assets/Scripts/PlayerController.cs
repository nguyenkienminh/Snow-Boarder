using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float boostSpeed = 30f;
    [SerializeField] float baseSpeed = 20f;
    [SerializeField] float speedIncreaseRate = 0.5f;
    [SerializeField] float maxSpeed = 50f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] private float DefaultBaseSpeed;
    [SerializeField] private float DefaultBoostSpeed;
    [SerializeField] private float DefaultJumpForce;
    float currentSpeed = 0f;

    [Header("Score Multiply Display")]
    [SerializeField] TextMeshPro spinText;
    [SerializeField] Transform textPosition;

    [Header("Health")]
    [SerializeField] float startingHealth;
    public float currentHealth { get; private set; }
    public bool invulnerable;

    [Header("IsFrames")]
    [SerializeField] float iFrameDuration;
    [SerializeField] int numberOfFlash;

    [Header("IsFrames")]
    [SerializeField] SpriteRenderer[] spriteRenderers;

    Rigidbody2D rb2d;
    SurfaceEffector2D surfaceEffector2D;

    bool canMove = true;
    float elapsedTime = 0f;
    bool isSlowingDown = false;
    bool isGrounded = true;
    bool isBoosting = false;

    float lastRotation;
    float spinCount = 0f;
    float spinAccumulated = 0f;

    private bool isInvincible = false;

	public static PlayerController Instance;

	public bool hasCrashed { get; private set; } = false; // Mặc định chưa crash
    private string skateboardColor = "white";


    [SerializeField] ScoreManager ScoreManager;

    private void Awake()
    {
        DefaultBaseSpeed = baseSpeed;
        DefaultBoostSpeed = boostSpeed;
        DefaultJumpForce = jumpForce;
        Debug.Log("DefaultBaseSpeed: " + DefaultBaseSpeed);
        Debug.Log("DefaultBoostSpeed: " + DefaultBoostSpeed);
        Debug.Log("DefaultJumpForce: " + DefaultJumpForce);
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindFirstObjectByType<SurfaceEffector2D>();
        InvokeRepeating("AutoIncreaseScore", 1f, 1f);
        lastRotation = transform.eulerAngles.z;
        currentHealth = startingHealth;
        
        ShowAllBoards();
    }

    void Update()
    {
        if (canMove)
        {
            RotatePlayer();
            RespondToBoost();
            IncreaseSpeedOverTime();
            Jump();
        }

        if (!isGrounded)
        {
            if (spinText != null && !spinText.gameObject.activeSelf)
            {
                spinText.gameObject.SetActive(true);
            }

            CalculateSpin();
        }

        UpdateSpinTextPosition();

        float speed = rb2d.linearVelocity.magnitude;
        currentSpeed = speed;
        ScoreManager.AddSpeed(currentSpeed);
        ScoreManager.SetSpeedMultiplier(speed);

        if (currentSpeed > 0.1f)
        {
            SoundManager.instance.PlayMovementClip();
        }
        else
        {
            SoundManager.instance.StopMovementClip();
        }
    }

    void ShowAllBoards()
    {
        string purchasedSkateboards = PlayerPrefs.GetString("PurchasedSkateboards", "");

        if (!string.IsNullOrEmpty(purchasedSkateboards))
        {
            string[] skateboardNames = purchasedSkateboards.Split(',');

            bool foundActiveBoard = false;

            foreach (string skateboardName in skateboardNames)
            {
                bool isActive = PlayerPrefs.GetInt(skateboardName + "_active", 0) == 1;
                if (isActive)
                {
                    Debug.Log($"Ván trượt đang active: {skateboardName}");
                    skateboardColor = skateboardName;
                    SetBoarderColor(skateboardColor);
                    foundActiveBoard = true; 

                    if (skateboardName.Equals("red"))
                    {
                        baseSpeed = DefaultBaseSpeed + 5;
                        boostSpeed = DefaultBoostSpeed + 5;
                        jumpForce = DefaultJumpForce + 2;
                    }
                    else if (skateboardName.Equals("green"))
                    {
                        baseSpeed = DefaultBaseSpeed + 7;
                        boostSpeed = DefaultBoostSpeed + 7;
                        jumpForce = DefaultJumpForce + 3;
                    }
                    else if (skateboardName.Equals("black"))
                    {
                        baseSpeed = DefaultBaseSpeed + 10;
                        boostSpeed = DefaultBoostSpeed + 10;
                        jumpForce = DefaultJumpForce + 5;
                    }
                    break; 
                }
            }

            if (!foundActiveBoard)
            {
                baseSpeed = DefaultBaseSpeed;
                boostSpeed = DefaultBoostSpeed;
                jumpForce = DefaultJumpForce;
                Debug.Log("Không có ván trượt nào đang active, dùng giá trị mặc định.");
            }
        }
        else
        {
            Debug.Log("Không có ván trượt nào được mua!");
            baseSpeed = DefaultBaseSpeed;
            boostSpeed = DefaultBoostSpeed;
            jumpForce = DefaultJumpForce;
        }

        Debug.Log("baseSpeed: " + baseSpeed);
        Debug.Log("BoostSpeed: " + boostSpeed);
        Debug.Log("JumpForce: " + jumpForce);
    }


    void UpdateSpinTextPosition()
    {
        if (spinText != null && Camera.main != null)
        {
            Vector3 worldPosition = transform.position + new Vector3(0, 2, 0); // Cao hơn nhân vật

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            if (screenPosition.z > 0)
            {
                spinText.transform.position = worldPosition;
                spinText.transform.rotation = Quaternion.identity; // Giữ cho text không bị xoay
            }
        }
    }

    public void Crash()
    {
        if (!invulnerable)
        {
            if (hasCrashed) return;

            hasCrashed = true;
            DisableControls();
            StartCoroutine(HandleCrash());
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private IEnumerator HandleCrash()
    {
        AudioSource dieSound = SoundManager.instance.PlayDieClip();

        if (dieSound != null)
        {
            yield return new WaitForSeconds(Mathf.Min(dieSound.clip.length, .15f));

            if (dieSound.isPlaying)
            {
                dieSound.Stop();
            }
        }

        Time.timeScale = 0f;
        GameManager.Instance.MoveGameOver();
    }


    public void DisableControls()
    {
        canMove = false;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    void RespondToBoost()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            surfaceEffector2D.speed = boostSpeed;
            isSlowingDown = false;
            SoundManager.instance.PlayBoostSound();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            surfaceEffector2D.speed = 10;
            elapsedTime = 0;
            isSlowingDown = true;
            isBoosting = true;
        }
        else
        {
            surfaceEffector2D.speed = baseSpeed;
            isSlowingDown = false;
            isBoosting = false;

            SoundManager.instance.StopBoostSound();
        }
    }

    void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.AddTorque(torqueAmount * .9f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.AddTorque(-torqueAmount * .9f);
        }
    }

    void IncreaseSpeedOverTime()
    {
        if (isSlowingDown)
        {
            elapsedTime += Time.deltaTime;
            float newSpeed = 10 + elapsedTime * speedIncreaseRate;
            surfaceEffector2D.speed = Mathf.Clamp(newSpeed, 10, maxSpeed);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce);
            isGrounded = false;
            ScoreManager.AddScore(20);
            SoundManager.instance.PlayJumpClip();
        }
    }

    void AutoIncreaseScore()
    {

        int basePoints = 2; // Mặc định +2 điểm mỗi giây

        if (isBoosting)
        {
            basePoints *= 2; // Nếu boost, nhân đôi điểm
        }
        ScoreManager.AddScore(basePoints);
    }

    void CalculateSpin()
    {
        if (!isGrounded)
        {
            float currentRotation = transform.eulerAngles.z;
            float rotationChange = Mathf.DeltaAngle(lastRotation, currentRotation);
            spinAccumulated += Mathf.Abs(rotationChange);

            // Mỗi khi đủ 300 độ, tăng spinCount
            if (spinAccumulated >= 300f)
            {
                int fullSpins = Mathf.FloorToInt(spinAccumulated / 300f);
                spinCount += fullSpins;
                spinAccumulated -= fullSpins * 300f;


                if (SoundManager.instance != null && !SoundManager.instance.IsPlayingRotateSkyClip())
                {
                    SoundManager.instance.PlayRotateSkyClip();
                }
            }

            if (spinText != null)
            {
                spinText.text = $"x{spinCount}";
            }

            lastRotation = currentRotation;

        }
        else
        {
            SoundManager.instance.StopRotateSkyClip();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            surfaceEffector2D = collision.gameObject.GetComponent<SurfaceEffector2D>();

            isGrounded = true;
            SoundManager.instance?.StopRotateSkyClip();

            if (spinCount > 0)
            {
                int spinScore = Mathf.CeilToInt(100 * spinCount);
                ScoreManager.AddScore(spinScore);
            }

            if (spinText != null)
            {
                spinText.gameObject.SetActive(false);
                spinText.text = "";
            }

            spinCount = 0;
            spinAccumulated = 0;
            spinText.text = "";
            ScoreManager.ResetCombo();
        }

        if (collision.gameObject.CompareTag("Rock"))
        {
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            rb2d.AddForce(pushDirection * 15f, ForceMode2D.Impulse);

            if (!invulnerable)
            {
                TakeDame(1);
                surfaceEffector2D.speed = Mathf.Max(surfaceEffector2D.speed * 0.5f, baseSpeed);
            }
        }
    }

    public virtual void TakeDame(float _damage)
    {
        if (!Input.GetKey(KeyCode.Escape))
        {
            if (invulnerable)
            {
                return;
            }

            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

            if (currentHealth > 0)
            {
                ScoreManager.DecreaseScore(20);
                SoundManager.instance.PlayHurtClip();
                StartCoroutine(Invulnerability());
            }
            else
            {
                currentHealth = 0;
                SoundManager.instance.PlayDieClip();
                Crash();
            }
        }
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(7, 8, true);

        invulnerable = true;
        Physics2D.IgnoreLayerCollision(7, 8, true);

        for (int i = 0; i < numberOfFlash; i++)
        {
            foreach (var sr in spriteRenderers)
            {
                sr.color = new Color(1, 0, 0, 0.5f);
            }
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlash * 2));

            foreach (var sr in spriteRenderers)
            {
                sr.color = Color.white;
            }
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlash * 2));
        }

        Physics2D.IgnoreLayerCollision(7, 8, false);
        invulnerable = false;
    }

    private IEnumerator SuperInvulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(7, 8, true);

        float duration = 5f;
        float elapsedTime = 0f;
        float flashInterval = 0.1f;

        Color[] rainbowColors = new Color[]
        {
            Color.red,
            new Color(1f, 0.5f, 0f),
            Color.yellow,
            Color.green,
            Color.blue,
            new Color(0.29f, 0f, 0.51f),
            new Color(0.93f, 0.51f, 0.93f)
        };

        int currentColorIndex = 0;

        while (elapsedTime < duration)
        {
            foreach (var sr in spriteRenderers)
            {
                sr.color = rainbowColors[currentColorIndex];
            }
            currentColorIndex = (currentColorIndex + 1) % rainbowColors.Length;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;

        if (value)
        {
            StartCoroutine(SuperInvulnerability());
        }
        else
        {
            
            StopAllCoroutines();
            foreach (var sr in spriteRenderers)
            {
                sr.color = Color.white;
            }
            spriteRenderers[1].color = ColorHelper.GetColorFromName(skateboardColor);
            Physics2D.IgnoreLayerCollision(7, 8, false);
            invulnerable = false;
        }
    }

	public void SetBoarderColor(string colorName)
	{
		if (spriteRenderers != null && spriteRenderers.Length > 1)
		{
			SpriteRenderer boarderBottom = spriteRenderers[1];
			boarderBottom.color = ColorHelper.GetColorFromName(colorName);
            Debug.Log(boarderBottom.color);
		}
	}

	public static class ColorHelper
	{
		public static Color GetColorFromName(string colorName)
		{
			switch (colorName.ToLower())
			{
				case "red": return Color.red;
				case "green": return Color.green;
				case "blue": return Color.blue;
				case "yellow": return Color.yellow;
				case "black": return Color.black;
				case "White": return Color.white;
				case "cyan": return Color.cyan;
				case "magenta": return Color.magenta;
				case "gray": return Color.gray;
				default: return Color.white;
			}
		}
	}

}
