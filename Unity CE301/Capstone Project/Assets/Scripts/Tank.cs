using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Player {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Move();
    }

    public override void Move() {
        base.Move();
        float forwardAxis = Input.GetAxis("Vertical");
		float turnAxis = Input.GetAxis("Horizontal");

		transform.Translate(0f, 0f, moveSpeed * forwardAxis * Time.deltaTime);
		transform.Rotate(0f, rotateSpeed * turnAxis * Time.deltaTime, 0f);

        findPath = Input.GetKeyDown(KeyCode.Q);
    }
}
