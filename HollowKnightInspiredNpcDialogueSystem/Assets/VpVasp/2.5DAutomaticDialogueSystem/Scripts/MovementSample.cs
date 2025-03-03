using UnityEngine;

public class MovementSample : MonoBehaviour
{
    public float speed = 5.0f;
    private CharacterController controller;

    void Start()
    {
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalMovement, 0, 0);
        moveDirection *= speed;

        controller.Move(moveDirection * Time.deltaTime);

        if (horizontalMovement < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (horizontalMovement > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (moveDirection.x != 0)
        {
            controller.height = 1.3f;
        }
        else
        {
            controller.height = 1;
        }
    }
 }
