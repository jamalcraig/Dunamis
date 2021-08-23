using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrb : MonoBehaviour {
    Vector3 startPos;
    Vector3 endPos;
    // Start is called before the first frame update
    void Start() {
        
    }

    public float hoverSpeed;
    public float startingHeight;
    public float height;
    float t;
    public bool hover;
    // Update is called once per frame
    void Update() {
        if (hover) {
            t += Time.deltaTime * hoverSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);


            if (transform.position.y <= startPos.y || transform.position.y >= endPos.y) {
                hoverSpeed *= -1;
            }
        }

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<PlayerController>() && transform.parent == null) {

            other.gameObject.GetComponent<PlayerController>().AddHealth();
            Destroy(this.gameObject);
        }
    }

    public void StartHovering() {
        transform.parent = null;
        transform.position = new Vector3(transform.position.x, startingHeight, transform.position.z);
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        startPos = transform.position;
        endPos = startPos + new Vector3(0f, height, 0f);
        
        hover = true;
    }


}
