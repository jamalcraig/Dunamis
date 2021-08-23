using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour {

    public static int inst = 0;
    public int thisInst;

    //[HideInInspector]
    public Bindings bindings;
    public float moveSpeed;
    public float rotateSpeed;
    public bool findPath;
    public bool findPathToRockSpawner;
    public bool findPathToPlayer;
    public bool clearPath;
    public List<Node> pathway = new List<Node>();
    public int current = 0;
    public float rad = 5f;

    public LayerMask rockCubeLM;
    public LayerMask grabableLM;

    protected Animator anim;
    protected int velXHash = Animator.StringToHash("Vel X");
    protected int velYHash = Animator.StringToHash("Vel Y");
    protected int velZHash = Animator.StringToHash("Vel Z");
    protected int deadHash = Animator.StringToHash("Dead");
    protected Vector3 currentPos;
    protected Vector3 currentVelocity;


    protected void BIND() {
        bindings = FindObjectOfType<Bindings>();
    }


    void Update() {
        //UpdateCheckpoint();
    }

    public virtual void UpdateCheckpoint() {

        if (current > pathway.Count) {
            current = 0;
        }
        if (pathway.Count > 0 && Vector3.Distance(pathway[current].worldPos, transform.position) < rad) {
            current++;
            if (current >= pathway.Count) {
                current = 0;
            }
        }


        if (pathway.Count > 0)
            transform.position = Vector3.MoveTowards(transform.position, pathway[current].worldPos, Time.deltaTime * moveSpeed);
    }
}
