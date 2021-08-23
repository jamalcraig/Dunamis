using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : BasicNPC {

    ScoreManager scoreManager;
    Rigidbody rb;
    public Vector3 m_EulerAngleVelocity;

    //now inherited from BasicNPC
    //Animator anim;
    //int velXHash = Animator.StringToHash("Vel X");
    //int velYHash = Animator.StringToHash("Vel Y");
    //int velZHash = Animator.StringToHash("Vel Z");

    int givingHash = Animator.StringToHash("Giving");
    float speedModifier = 1f;

    //now inherited from BasicNPC
    //Vector3 currentPos; 
    //Vector3 currentVelocity;

    public float grabRadius = 2.5f;

    public enum State { Neutral, FindingGem, Returning, Giving };
    public State currentState = State.Giving;
    public bool waitToReCalcPath = false;
    //Player player; // now inherited from BasicNPC

    GameObject dunamisInHand;

    // Start is called before the first frame update
    void Start() {
        BIND();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        player = FindObjectOfType<Player>();
        dunamisInHand = GetComponentInChildren<RockCube>().gameObject;
        //m_EulerAngleVelocity = new Vector3(0, 100, 0);
        currentPos = transform.position;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update() {
        //findPath = Input.GetKeyDown(KeyCode.T);

        if (currentState == State.Neutral) {
            findPathToRockSpawner = bindings.L2BUT;
        } else {
            findPathToRockSpawner = false;
        }



        if (findPathToRockSpawner)
            currentState = State.FindingGem;

        //findPath = Input.GetButtonDown("PS4_C_Triangle");
        UpdateCheckpoint();
        MovementAnimation();
       
        switch (currentState) {
        case State.Returning:
            if (!waitToReCalcPath) {
                print("ReCalcPathBack() (Coroutine)");
                StartCoroutine(ReCalcPathBack());
            }
            if (Vector3.Distance(transform.position, player.gameObject.transform.position) < givePlayerRadius) {
                currentState = State.Giving;
                pathway.Clear();
                current = 0;
                print($"KLKLK {Vector3.Distance(transform.position, player.gameObject.transform.position)} < {givePlayerRadius} = {Vector3.Distance(transform.position, player.gameObject.transform.position) < givePlayerRadius}");
            }
            dunamisInHand.SetActive(true);
            break;

        case State.Giving:
            anim.SetBool(givingHash, true);
            findPathToPlayer = false;
            if (Vector3.Distance(transform.position, player.gameObject.transform.position) > givePlayerRadius*1.5) {
                currentState = State.Returning;
            }
            var targetRotation = Quaternion.LookRotation(player.gameObject.transform.position - transform.position);

            // Smoothly rotate towards the target point.
            //targetRotation = Quaternion.Euler(0f, targetRotation.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            dunamisInHand.SetActive(true);
            break;

        case State.FindingGem:
            findPathToPlayer = false;
            break;

        case State.Neutral:
            dunamisInHand.SetActive(false);
            findPathToPlayer = false;
            break;
        }

        if (currentState != State.Giving)
            anim.SetBool(givingHash, false);


        //print($"Distance away from player: {Vector3.Distance(transform.position, player.gameObject.transform.position)}");
    }
    public float givePlayerRadius = 10f;

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

    public override void UpdateCheckpoint() {

        
        
        if (pathway.Count > 0 && current < pathway.Count && Vector3.Distance(pathway[current].worldPos, transform.position) < rad) {
            if (current <= pathway.Count)
                current++;

            if (current >= pathway.Count) {
                print("dumb");
                pathway.Clear();
                current = 0;
                switch (currentState) {

                case State.Returning:
                    currentState = State.Giving;
                    break;
                case State.Giving:
                    break;

                case State.FindingGem:

                    GrabGemAtSpawner();
                    break;

                }
            }
        }


        if (currentState != State.Giving && pathway.Count > 0 && current <= pathway.Count - 1) {
            Vector3 t = pathway[current].worldPos;
            t.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, t, Time.deltaTime * moveSpeed);
            //transform.LookAt(pathway[current].worldPos);
            //SmoothLookAt(pathway[current].worldPos);
            var targetRotation = Quaternion.LookRotation(t - transform.position);

            // Smoothly rotate towards the target point.
            //targetRotation = Quaternion.Euler(0f, targetRotation.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        } 
    }

    int dunamisCarryCount = 0;
    void GrabGemAtSpawner() {
        Collider[] c = Physics.OverlapSphere(transform.position, grabRadius, rockCubeLM);
        for (int i = 0; i < c.Length; i++) {

            print($"Destroyed {c[i].transform.gameObject.name}");
            c[i].transform.gameObject.GetComponentInParent<RockSpawner>().RemoveRock(c[i].transform.gameObject);
            Destroy(c[i].transform.gameObject);
            dunamisCarryCount++;
        }
        currentState = State.Returning;
        print($"Clone grabbed {dunamisCarryCount} dunamis");
    }

    IEnumerator ReCalcPathBack() {
        waitToReCalcPath = true;
        if (currentState == State.Returning)
            findPathToPlayer = true;
        else
            findPathToPlayer = false;

        yield return new WaitForSeconds(0.1f);
        waitToReCalcPath = false;
    }

    public void DunamisTaken() {
        currentState = State.Neutral;
        pathway.Clear();
        //print($"Pathway.count: {pathway.Count}");

        player.GetComponent<PlayerController>().AddDunamisAmmo(dunamisCarryCount);
        dunamisCarryCount = 0;
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(0,25,0,10);
        //Gizmos.DrawWireSphere(this.transform.position, grabRadius);
    }
}
