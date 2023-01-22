using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public Animator playerAnim;
    public GameObject player;

    //Movement
    private float ru_speed = 5;
    private float gravity = -9.81f;
    private float jumpHeight = 2;
    private bool walking;

    //Ground Check
    public Transform groundCheck;
    public LayerMask floorMask;
    private float groundDistance = 0.4f;
    Vector3 velocity;
    public bool grounded;

    //Player Health/Stamina
    public float playerhp = 100;
    private float playersp = 100;
    public float playerCurrentsp;
    public Slider PlayerHealthBar;
    public Slider PlayerStaminaBar;
    public bool isBlocking = true;
    public float blockMit = 0.6f;

    //Powerups
    public bool increaseDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentsp = playersp;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHealthBar.value = playerhp;
        PlayerStaminaBar.value = playerCurrentsp;

        if (!isBlocking)
        {
        playerCurrentsp += 0.02f;
        }

        if (playerCurrentsp >= 100)
        {
            playerCurrentsp = 100;
        }

        if (playerhp < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Player died!");
        }

        if (playerhp > 100)
        {
         playerhp = 100;
        }

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, floorMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float z = Input.GetAxis("Horizontal");
        float x = Input.GetAxis("Vertical");

        playerAnim.SetFloat("forward", x);
        playerAnim.SetFloat("strafe", z);

        Vector3 move = -transform.right * x + transform.forward * z;

        if(!isBlocking)
        {
        controller.Move(move * ru_speed * Time.deltaTime);
        }

        IsSprinting();

        if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.LeftShift) && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (grounded)
        {
            playerAnim.SetBool("jump", false);
        }
        else
        {
            playerAnim.SetBool("jump", true);
        }

        if(Input.GetKey(KeyCode.Mouse1) && playerCurrentsp >=3 && grounded)
        {
            playerAnim.SetBool("blocking", true);
            isBlocking = true;
        }
        else
        {
            playerAnim.SetBool("blocking", false);
            isBlocking = false;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
        grounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
        Debug.Log("You died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (other.gameObject.CompareTag("IncHealth"))
        {
            Debug.Log("HP replenished!");
            playerhp += 100;
        }
    }

    private void IsSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetButton("Jump") && grounded && Input.GetKey("w") && playerCurrentsp >= 3 && !isBlocking)
        {
            TakeStamina(0.05f);
            ru_speed = 8.5f;
            playerAnim.SetBool("sprint", true);
        }
        else
        {
            ru_speed = 5;
            playerAnim.SetBool("sprint", false);
        }
    }

    public void TakeStamina(float amount)
    {
            playerCurrentsp -= amount;
            PlayerStaminaBar.value = playerCurrentsp;
    }

}
