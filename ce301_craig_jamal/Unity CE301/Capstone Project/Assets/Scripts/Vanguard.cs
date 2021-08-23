using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanguard : BasicNPC {

    Rigidbody rb;
    public Vector3 m_EulerAngleVelocity;
    //Animator anim;
    //int velXHash = Animator.StringToHash("Vel X");
    //int velYHash = Animator.StringToHash("Vel Y");
    //int velZHash = Animator.StringToHash("Vel Z");
    public Vector3 maxVel = new Vector3(10f, 10f, 10f);
    float speedModifier = 1f;

    // now inherited from BasicNPC
    /*
    Vector3 currentPos;
    Vector3 currentVelocity;
    */

    // Start is called before the first frame update
    void Start() {
        BIND();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //m_EulerAngleVelocity = new Vector3(0, 100, 0);
        currentPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        //findPath = Input.GetKeyDown(KeyCode.T);
        findPath = bindings.ACTION;
        //findPath = Input.GetButtonDown("PS4_C_Triangle");
        UpdateCheckpoint2();
        MovementAnimation();
    }

    //now inherited from BasicNPC
    /*
    public void MovementAnimation() {



        //Vector3 vel = rb.velocity;
        //if (vel.magnitude > maxVel.x * speedModifier) {
        //    rb.velocity = vel.normalized * maxVel.x;
        //}
        //anim.SetFloat(velXHash, transform.InverseTransformDirection(vel).x);
        //anim.SetFloat(velYHash, transform.InverseTransformDirection(vel).y);
        //anim.SetFloat(velZHash, transform.InverseTransformDirection(vel).z);



        Vector3 lastPos = currentPos;
        currentPos = transform.position;
        currentVelocity = (currentPos - lastPos) / Time.deltaTime;
        anim.SetFloat(velXHash, transform.InverseTransformDirection(currentVelocity).x);
        anim.SetFloat(velYHash, transform.InverseTransformDirection(currentVelocity).y);
        anim.SetFloat(velZHash, transform.InverseTransformDirection(currentVelocity).z);
        //Debug.Log(currentVelocity.magnitude);
    }*/

    void SmoothLookAt(Vector3 newDirection) {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), Time.deltaTime);
    }

    public void UpdateCheckpoint2() {

        if (current > pathway.Count) {
            current = 0;
        }
        if (pathway.Count > 0 && Vector3.Distance(pathway[current].worldPos, transform.position) < rad) {
            current++;
            if (current >= pathway.Count) {
                current = 0;
            }
        }


        if (pathway.Count > 0) {
            transform.position = Vector3.MoveTowards(transform.position, pathway[current].worldPos, Time.deltaTime * moveSpeed);
            //transform.LookAt(pathway[current].worldPos);
            //SmoothLookAt(pathway[current].worldPos);
            var targetRotation = Quaternion.LookRotation(pathway[current].worldPos - transform.position);

            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        }




        //Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        ////rb.MoveRotation(rb.rotation * deltaRotation);

        //if (pathway.Count > 0) {
        //    rb.AddForce(rb.transform.forward * moveSpeed * Time.deltaTime);
        //    //rb.MoveRotation(Quaternion.Euler(pathway[current].worldPos)/* * deltaRotation*/);
        //    //rb.MoveRotation(Quaternion.Euler(new Vector3(0, pathway[current].worldPos.y, pathway[current].worldPos.z))/* * deltaRotation*/);
        //    //rb.MoveRotation(Quaternion.Euler(transform.InverseTransformPoint(pathway[current].worldPos))/* * deltaRotation*/);


        //    Vector3 p = pathway[current].worldPos;
        //    p.y = transform.position.y;
        //    Vector3 direction = p - transform.position;
        //    Debug.DrawLine(transform.position, pathway[current].worldPos, Color.cyan);
        //    rb.MoveRotation(Quaternion.LookRotation(direction));
        //} 
    }
}
