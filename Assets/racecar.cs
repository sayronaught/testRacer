using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class racecar : MonoBehaviour
{
    public Vector2 wheel;
    public int gear;
    private float gearchange = 0;
    private Rigidbody myRB = null;
    public Vector2 breakpedal;

    public void wheelchange(InputAction.CallbackContext ctx)
    {
        wheel = ctx.ReadValue<Vector2>();
        wheel.y = wheel.y * -0.5f + 0.5f;
        wheel.y = Mathf.Clamp(wheel.y, 0, 1);
    }
    public void gearuUp(InputAction.CallbackContext ctx)
    {
        if (gearchange > 0) return;
        gear = Mathf.Clamp(gear +1 , 0 ,6);
        gearchange = 0.25f;
    }
    public void gearDown(InputAction.CallbackContext ctx)
    {
        if (gearchange > 0) return;
        gear = Mathf.Clamp(gear - 1, 0, 6);
        gearchange = 0.25f;
    }
    public void pushBreak(InputAction.CallbackContext ctx)
    {
        breakpedal = ctx.ReadValue<Vector2>();
        breakpedal.y = breakpedal.y * -0.5f + 0.5f; 
    }
    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        gearchange -= Time.deltaTime;
        transform.Rotate(Vector3.up, wheel.x * Time.deltaTime * 200);
        if (gear == 0)
        {
            myRB.AddForce(transform.forward * wheel.y * Time.deltaTime * -700);
        }
        else
        myRB.AddForce(transform.forward * wheel.y * Time.deltaTime * (700 + 200 * gear));
        if (breakpedal.y > 0 && breakpedal.y != 0.5f)
        {
            //myRB.angularVelocity *= breakpedal.y * Time.deltaTime;
            //myRB.velocity *= breakpedal.y * Time.deltaTime;
            myRB.velocity -= myRB.velocity * Time.deltaTime *  breakpedal.y;
        }

    }
}
