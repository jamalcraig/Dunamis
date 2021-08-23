using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMarker : MonoBehaviour {

    public GameObject target;
    public GameCharacter gameCharacter;
    Vector3 gameCharacterPos;
    MeshRenderer meshRenderer;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start() {

        if (GetComponent<MeshRenderer>())
            meshRenderer = GetComponent<MeshRenderer>();

        if (GetComponent<SpriteRenderer>())
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (target != null && gameCharacter != null) {
            if (Vector3.Distance(target.transform.position, transform.position) > Vector3.Distance(gameCharacter.gameObject.transform.position, target.transform.position)) {
                if (meshRenderer != null)
                    meshRenderer.enabled = false;

                if (spriteRenderer != null)
                    spriteRenderer.enabled = false;
            } else {
                if (meshRenderer != null)
                    meshRenderer.enabled = true;

                if (spriteRenderer != null)
                    spriteRenderer.enabled = true;
            }
        }
    }
}
