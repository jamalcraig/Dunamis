using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDunamis : MonoBehaviour {

    Rigidbody rb;
    public int type;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Hit() {
        Destroy(this.gameObject);
    }

    IEnumerator SelfDestruct() {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Bowl")) {
            gameObject.transform.parent = collision.gameObject.transform;
        } else if (!collision.gameObject.GetComponent<PipeDunamis>()) {
            StartCoroutine(SelfDestruct());
        }


    }
}
