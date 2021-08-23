using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour {

    public float speed = 1;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Move();
    }

    void Move() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, y, 0f);

        transform.position += movement * speed * Time.deltaTime;

        if (Input.GetButtonDown("PS4_C_R2_But") || Input.GetKeyDown(KeyCode.F)) {
            GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
    }

    
}
