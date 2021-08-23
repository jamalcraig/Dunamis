using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNPC : GameCharacter {

    //public List<Transform> checkpoints = new List<Transform>();



    //  void FixedUpdate() {

    //      if (current > pathway.Count) {
    //	current = 0;
    //}
    //if (pathway.Count > 0 && Vector3.Distance(pathway[current].worldPos, transform.position) < rad) {
    //	current++;
    //	if (current >= pathway.Count) {
    //		current = 0;
    //	}
    //}


    //if (pathway.Count > 0)
    //	transform.position = Vector3.MoveTowards(transform.position, pathway[current].worldPos, Time.deltaTime * moveSpeed);
    //  }

    protected Player player;

    private void Update() {
        UpdateCheckpoint();
    }

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
    }

    public void MoveToPlayer() {
        Vector3 t = player.transform.position;
        t.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, t, Time.deltaTime * moveSpeed);
    }
}
