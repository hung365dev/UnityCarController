using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float RaceForce;
    public Transform Wheel_FL, Wheel_FR, Wheel_BL, Wheel_BR;
    public float TurnRate;
    public float WheelMaxAngle;
    
    private Rigidbody rigidBody;
    private Rigidbody wheel_fl_rb, wheel_fr_rb, wheel_bl_rb, wheel_br_rb;
    private ConfigurableJoint wheel_fl_cj, wheel_fr_cj;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        wheel_fl_rb = Wheel_FL.GetComponent<Rigidbody>();
        wheel_fr_rb = Wheel_FR.GetComponent<Rigidbody>();
        wheel_bl_rb = Wheel_BL.GetComponent<Rigidbody>();
        wheel_br_rb = Wheel_BR.GetComponent<Rigidbody>();

        wheel_fl_cj = Wheel_FL.GetComponent<ConfigurableJoint>();
        wheel_fr_cj = Wheel_FR.GetComponent<ConfigurableJoint>();
               
    }


    void Update()
    {
        Controls();
    }

    private float forwardAngleL = 0f;
    private float forwardAngleR = 0f;
    private void Controls()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigidBody.AddForce(-transform.right * RaceForce, ForceMode.Impulse);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rigidBody.AddForce(transform.right * RaceForce, ForceMode.Impulse);
        }

    }    
    
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            float delta = TurnRate * Time.fixedDeltaTime;
            if (forwardAngleR + delta > WheelMaxAngle)
            {
                forwardAngleR = WheelMaxAngle;
            }
            else
            {
                forwardAngleR += delta;
            }
            if (forwardAngleL + delta > WheelMaxAngle)
            {
                forwardAngleL = WheelMaxAngle;
            }
            else
            {
                forwardAngleL += delta;
            }

            float angle = Vector3.SignedAngle(Wheel_FL.up, transform.up, -transform.forward);
            Wheel_FL.RotateAround(Wheel_FL.position, transform.forward, forwardAngleL - angle);

            angle = Vector3.SignedAngle(Wheel_FR.up, transform.up, -transform.forward);
            Wheel_FR.RotateAround(Wheel_FR.position, transform.forward, forwardAngleR - angle);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            float delta = -TurnRate * Time.fixedDeltaTime;
            if (forwardAngleR + delta < -WheelMaxAngle)
            {
                forwardAngleR = -WheelMaxAngle;
            }
            else
            {
                forwardAngleR += delta;
            }
            if (forwardAngleL + delta < -WheelMaxAngle)
            {
                forwardAngleL = -WheelMaxAngle;
            }
            else
            {
                forwardAngleL += delta;
            }
            float angle = Vector3.SignedAngle(Wheel_FL.up, transform.up, -transform.forward);
            Wheel_FL.RotateAround(Wheel_FL.position, transform.forward, forwardAngleL - angle);
            angle = Vector3.SignedAngle(Wheel_FR.up, transform.up, -transform.forward);
            Wheel_FR.RotateAround(Wheel_FR.position, transform.forward, forwardAngleR - angle);
        }
        Vector3 newUp = (Wheel_FL.up - Vector3.Project(Wheel_FL.up, transform.forward)).normalized;
        Vector3 newForw = (Wheel_FL.forward - Vector3.Project(Wheel_FL.forward, newUp)).normalized;
        Wheel_FL.rotation = Quaternion.LookRotation(newForw, newUp);

        newUp = (Wheel_FR.up - Vector3.Project(Wheel_FR.up, transform.forward)).normalized;
        newForw = (Wheel_FR.forward - Vector3.Project(Wheel_FR.forward, newUp)).normalized;
        Wheel_FR.rotation = Quaternion.LookRotation(newForw, newUp);

        forwardAngleL = Vector3.SignedAngle(Wheel_FL.up, transform.up, -transform.forward);
        forwardAngleR = Vector3.SignedAngle(Wheel_FR.up, transform.up, -transform.forward);
        wheel_fl_cj.axis = new Vector3(0, 1, 0);
        wheel_fr_cj.axis = new Vector3(0, 1, 0);
    }

}
