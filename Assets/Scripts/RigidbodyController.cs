using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyController : MonoBehaviour
{
    // Public variable to control movements behaviour
    public float speed = 10.0f;
    public float jumpForce = 6.0f;


    // RigidBody of current GameObject
    private Rigidbody rigidBody;

    // Ground detection
    private List<GameObject> groundGameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        bool isGround = groundGameObjects.Count != 0;

        Vector3 newVelocity = Vector3.zero;

        // x/z Movement
        newVelocity.x = Keyboard.current.dKey.isPressed ? speed : Keyboard.current.aKey.isPressed ? -speed : 0;
        newVelocity.z = Keyboard.current.wKey.isPressed ? speed : Keyboard.current.sKey.isPressed ? -speed : 0;

        // y Movement / Jump / Gravity
        if (isGround)
        {
            // Allow Jump on Ground and disable Gravity
            newVelocity.y = Keyboard.current.spaceKey.isPressed && isGround ? jumpForce : 0;
            rigidBody.useGravity = false;
        }
        else
        {
            // Don't allow Jump in Air and enable Gravity
            newVelocity.y = rigidBody.velocity.y;
            rigidBody.useGravity = true;
        }


        // Apply Movement
        rigidBody.velocity = newVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ground detection
        if (collision.gameObject.CompareTag("Ground"))
            if (!groundGameObjects.Contains(collision.gameObject))
                groundGameObjects.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        // Ground detection
        if (collision.gameObject.CompareTag("Ground"))
            groundGameObjects.RemoveAll(obj => obj == collision.gameObject);
    }
}
