using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;
    public Vector3 spawnPoint;

    CharacterController controller;
    Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (spawnPoint == Vector3.zero)
            spawnPoint = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        float x = 0f;
        float z = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) z -= 1f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) z += 1f;

        Vector3 move = new Vector3(x, 0f, z);
        if (move.sqrMagnitude > 1f) move.Normalize();
        move *= moveSpeed;

        if (controller.isGrounded)
        {
            velocity.y = -1f;
            if (Input.GetKeyDown(KeyCode.Space))
                velocity.y = jumpSpeed;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move((move + new Vector3(0f, velocity.y, 0f)) * Time.deltaTime);

        if (transform.position.y < -12f)
            Respawn();
    }

    public void Respawn()
    {
        controller.enabled = false;
        transform.position = spawnPoint;
        velocity = Vector3.zero;
        controller.enabled = true;
    }
}
