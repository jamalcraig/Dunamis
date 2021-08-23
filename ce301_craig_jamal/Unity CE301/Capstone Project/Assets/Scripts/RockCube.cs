using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCube : MonoBehaviour {

    Rigidbody rb;
    public float speed = 10f;
    public bool fired = false;
    // Start is called before the first frame update
    void Start() {

    }

    public void Firee(Vector3 direction, float _speed) {
        fired = true;
        rb = GetComponent<Rigidbody>();
        //direction -= transform.position;
        direction.Normalize();
        
        //rb.AddRelativeForce(direction * _speed, ForceMode.Impulse);
        rb.AddRelativeForce(Vector3.forward * _speed, ForceMode.Impulse);
        StartCoroutine(selfDestruct());
    }

    IEnumerator selfDestruct() {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    public void Hit() {
        Destroy(this.gameObject);
    }
}
