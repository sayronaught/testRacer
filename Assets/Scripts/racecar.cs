using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class racecar : MonoBehaviour
{
    public Vector2 wheel;
    public int gear;
    public Vector2 breakpedal;
    public bool grounded;

    public float SSS;
    private float gearchange = 0;
    private Rigidbody myRB = null;
    private PlayerInput myPI = null;
    private AudioSource myAS = null;


    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myPI = GetComponent<PlayerInput>();
        myAS = GetComponent<AudioSource>();
    }

    public void wheelchange(InputAction.CallbackContext ctx)
    {
        wheel = ctx.ReadValue<Vector2>();
        if (myPI.currentActionMap.name == "SteeringWheel") wheel.y = wheel.y * -0.5f + 0.5f;
        wheel.y = Mathf.Clamp(wheel.y, 0, 1);
    }

    public void gearUp(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (gearchange > 0) return;
            gear = Mathf.Clamp(gear + 1, 0, 6);
            gearchange = 0.25f;
        }
    }
    public void gearDown(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (gearchange > 0) return;
            gear = Mathf.Clamp(gear - 1, 0, 6);
            gearchange = 0.25f;
        }
    }
    public void pushBreak(InputAction.CallbackContext ctx)
    {
        breakpedal = ctx.ReadValue<Vector2>();
        breakpedal.y = breakpedal.y * -0.5f + 0.5f; 
    }
    public void changeSceen(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            int nextscene = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextscene > SceneManager.sceneCount) nextscene = 0;
            SceneManager.LoadScene(nextscene);
        }
    }
    public void changeActionMap(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (myPI.currentActionMap.name == "SteeringWheel") myPI.SwitchCurrentActionMap("Keyboard");
            else if (myPI.currentActionMap.name == "Keyboard") myPI.SwitchCurrentActionMap("SteeringWheel");
            else return;
        }
    }

    private void Update()
    {
        
        gearchange -= Time.deltaTime;
        if (grounded)
        {
            transform.Rotate(transform.up, wheel.x * Time.deltaTime * 200);
            if (gear == 0)
            {
                myRB.AddForce(transform.forward * wheel.y * Time.deltaTime * -700);
            }
            else
                myRB.AddForce(transform.forward * wheel.y * Time.deltaTime * (900 + 300 * gear));
            myAS.pitch = wheel.y * SSS + 0.5f;
            if (breakpedal.y > 0 && breakpedal.y != 0.5f)
                myRB.velocity -= myRB.velocity * Time.deltaTime * breakpedal.y;
        }
       // if (transform.up.y < transform.position.y)
       // {
       //     transform.LookAt(transform.forward);
       // }

    }
    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
