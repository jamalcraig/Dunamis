using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : BasicNPC {

    ScoreManager scoreManager;
    public int racerInst;
    Rigidbody rb;
    int distFromPlayerHash = Animator.StringToHash("Dist from player");
    int hitHash = Animator.StringToHash("Hit");
    int tauntHash = Animator.StringToHash("Taunt");
    int escapeHash = Animator.StringToHash("Escape");

    public enum State { Neutral, Chasing, Attack, Dead, Taunt, Escape, Reverse }
    public State currentState = State.Neutral;

    float distanceFromPlayer;
    public float minDistFromPlayer;
    public float attackRadius;
    public float meleeRange;

    public bool waitToReCalcPath;
    public bool waitToUppercut;
    public bool waitToCalcExit;
    public bool findPathToExit;

    public Transform healthOrbPos;
    public GameObject healthOrb;
    public Transform healthOrbSpawnParent;
    public GameObject spawnedHealthOrb;
    
    void Start() {
        thisInst = ++inst;
        BIND();
        scoreManager = FindObjectOfType<ScoreManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        player = FindObjectOfType<Player>();
        currentPos = transform.position;
    }

    void Update() {

        //if (currentState == State.Chasing)
        //    findPathToPlayer = true;

        //if (currentState != State.Chasing && currentState != State.Attack)
        //    findPathToPlayer = false;

        findPathToPlayer = false;
        findPathToExit = false;

        distanceFromPlayer = Vector3.Distance(transform.position, player.gameObject.transform.position);
        anim.SetFloat(distFromPlayerHash, distanceFromPlayer);

        switch (currentState) {
        case State.Neutral:
            break;

        case State.Chasing:
            if (!waitToReCalcPath) {
                //print("ReCalcPathBack() (Coroutine)");
                StartCoroutine(ReCalcPathBack());
            }
            if (distanceFromPlayer < attackRadius) {
                currentState = State.Attack;
                pathway.Clear();
                current = 0;
                //print($"Racer - {Vector3.Distance(transform.position, player.gameObject.transform.position)} < {attackRadius} = {Vector3.Distance(transform.position, player.gameObject.transform.position) < attackRadius}");
            }
            if (pathway.Count == 0 || current == 0) {
                MoveWithLOS();
            }
            break;

        case State.Attack:
            if (!waitToReCalcPath) {
                //print("ReCalcPathBack() (Coroutine)");
                StartCoroutine(ReCalcPathBack());
            }
            if (distanceFromPlayer > attackRadius * 1.5) {
                currentState = State.Chasing;
            }
            if (!waitToUppercut) {
                StartCoroutine(Hit());
            }

            if (pathway.Count == 0 || current == 0) {
                MoveWithLOS();
            } else {
                RotateTowardsPlayer();
            }
            break;

        case State.Taunt:
            anim.SetBool(tauntHash, true);
            break;

        case State.Escape:
            anim.SetBool(escapeHash, true);
            if (!waitToCalcExit) {
                StartCoroutine(CalcPathToExit());
            }
            
            break;

        case State.Reverse:
            if (!reversing) {
                Reverse();
            }

            break;

        case State.Dead:
            pathway.Clear();
            current = 0;
            break;
        }

        if (currentState != State.Taunt)
            anim.SetBool(tauntHash, false);

        if ((currentState == State.Chasing || currentState == State.Attack) && distanceFromPlayer < attackRadius && distanceFromPlayer > minDistFromPlayer) {
            MoveWithLOS();
            //print($"Moving Directly Towards Player - dist: {distanceFromPlayer}");
        } else {
            UpdateCheckpoint();
        }
        MovementAnimation();
        timeTilLOSCheck -= Time.deltaTime;
    }
    

    public float maxSpeed;
    public float forceSpeed;
    public float forceRotateSpeed;
    private void FixedUpdate() {
        //UpdateCheckpoint();

        /*
            if (rb.velocity.magnitude > maxSpeed ) {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        */
    }

    public float timeSinceLastCheckpoint;
    public override void UpdateCheckpoint() {

        /*
        if (currentState == State.Reverse) {
            print("Reversing");
            transform.position = Vector3.MoveTowards(transform.position, behind, Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, behind) < 0.5f) {
                print("Got to behind pos");
                if (lastState == State.Escape) {
                    waitToCalcExit = false;
                    StartCoroutine(CalcPathToExit());
                    print("CalcPathToExit from Reversing State");
                }
                if (lastState == State.Attack || lastState == State.Chasing) {
                    waitToReCalcPath = false;
                    StartCoroutine(ReCalcPathBack());
                    print("ReCalcPathBack from Reversing State");
                }
                currentState = lastState;
                reversing = false;
            }
        } else { 
        */
            
            if (pathway.Count > 0 && current < pathway.Count && Vector3.Distance(pathway[current].worldPos, transform.position) < rad) {
                //print($"Next Checkpoint {current} - Racer ({thisInst})");
                if (current <= pathway.Count) {
                    current++;
                    timeSinceLastCheckpoint = Time.time;
                }

                if (current >= pathway.Count) {
                    //print("dumb");
                    pathway.Clear();
                    current = 0;
                    switch (currentState) {

                    case State.Attack:

                        break;
                    case State.Chasing:
                        currentState = State.Attack;
                        break;

                    case State.Neutral:


                        break;

                    case State.Escape:
                        //Destroy(GetComponentInParent<Transform>().gameObject.GetComponentInParent<Transform>().gameObject);
                        break;

                    case State.Dead:

                        break;

                    }
                }

                
            }
            /*
            print($"Timeeeee {Time.time - timeSinceLastCheckpoint}");
            if (Time.time - timeSinceLastCheckpoint > 3f) {

                lastState = currentState;
                currentState = State.Reverse;
                timeSinceLastCheckpoint = Time.time;
            } */


            if (currentState != State.Attack && pathway.Count > 0 && current <= pathway.Count - 1) {
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

            /*
            //rigidbody way
            if (currentState != State.Attack && pathway.Count > 0 && current <= pathway.Count - 1) {
                Vector3 t2 = pathway[current].worldPos;
                t2.y = transform.position.y;
                var targetRotation2 = Quaternion.LookRotation(t2 - transform.position);

                // Smoothly rotate towards the target point.
                //targetRotation = Quaternion.Euler(0f, targetRotation.y, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation2, forceRotateSpeed * Time.deltaTime);

                rb.AddForce(rb.transform.forward * forceSpeed * Time.deltaTime);
            } */

        //}

            
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<RockCube>()) {
            if (collision.gameObject.GetComponent<RockCube>().fired) {
                
                StartCoroutine(Die());
                collision.gameObject.GetComponent<RockCube>().Hit();
            }
        }

        /*
        if (collision.gameObject.layer == LayerMask.GetMask("BlockedPath")){
            print("COLLIDING WITH BLOCKED");
            StartCoroutine(CollidingWithBlocked(collision));
        } */
    }

    void RotateTowardsPlayer() {
        Vector3 p = player.gameObject.transform.position;
        p.y = currentPos.y;
        var targetRotation = Quaternion.LookRotation(p - transform.position);

        // Smoothly rotate towards the target point.
        //targetRotation = Quaternion.Euler(0f, targetRotation.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public void SetToEscape() {
        currentState = State.Escape;
        

    }

    public void SpawnHealthOrb() {
        spawnedHealthOrb = Instantiate(healthOrb, healthOrbPos.position, healthOrbPos.rotation, healthOrbSpawnParent);
        spawnedHealthOrb.transform.localScale = new Vector3(0.25f, 0.5f, 0.25f);
    }

    IEnumerator Die() {
        //makes sure that an health orb is still spawned if killed during taunt animation
        if (currentState == State.Taunt) {
            SpawnHealthOrb();
            print("Spawned orb during taunt");
        }

        currentState = State.Dead;
        anim.SetBool(deadHash, true);
        rb.isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = false;

        
        if (spawnedHealthOrb != null) {
            spawnedHealthOrb.GetComponent<HealthOrb>().StartHovering();
            print("Health orb hovering now");
        }

        scoreManager.KilledRacer();
        yield return new WaitForSeconds(3);
        GetComponentInParent<EnemySpawner>().RemoveEnemy(GetComponentInParent<VariableGrid>().gameObject);
    }

    public void Despawn() {

        GetComponentInParent<EnemySpawner>().RemoveEnemy(GetComponentInParent<VariableGrid>().gameObject);
    }

    IEnumerator ReCalcPathBack() {
        waitToReCalcPath = true;
        if (currentState == State.Chasing || currentState == State.Attack)
            findPathToPlayer = true;
        else
            findPathToPlayer = false;

        yield return new WaitForSeconds(0.476f);
        waitToReCalcPath = false;
    }

    IEnumerator CalcPathToExit() {
        waitToCalcExit = true;
        if (currentState == State.Escape)
            findPathToExit = true;
        else
            findPathToExit = false;

        yield return new WaitForSeconds(0.476f);
        waitToCalcExit = false;
    }

    IEnumerator Hit() {
        waitToUppercut = true;
        anim.SetBool(hitHash, true);
        StartCoroutine(HitDetection());
        yield return new WaitForSeconds(1f);
        anim.SetBool(hitHash, false);
        waitToUppercut = false;
        
    }

    IEnumerator HitDetection() {
        yield return new WaitForSeconds(0.8f/2f);
        Debug.DrawRay(transform.position + new Vector3(0, 2f, 0), transform.forward * meleeRange, Color.magenta);
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 2, 0), transform.forward);
        bool tp = Physics.Raycast(ray, out hit, meleeRange);
        //Debug.Log("Hit Timer: " + (Time.time - st));
        if (tp) {
            if (hit.transform.CompareTag("Player")) {
                hit.transform.gameObject.GetComponent<PlayerController>().ReduceHealth(1);
                print("HIT PLAYER");
                currentState = State.Taunt;
            }
        }
    }

    IEnumerator CollidingWithBlocked(Collision collision) {
        yield return new WaitForSeconds(1f);
        Debug.DrawRay(transform.position + new Vector3(0, 2f, 0), transform.forward * meleeRange, Color.magenta);
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 2, 0), transform.forward);
        bool tp = Physics.Raycast(ray, out hit, meleeRange);
        //Debug.Log("Hit Timer: " + (Time.time - st));
        if (tp) {
            print("TP");
            print($"Racer Raycast hit {hit.transform.name}");
            if (hit.collider == collision.collider) {
                print("Changing to Reverse");
                lastState = currentState;
                currentState = State.Reverse;
            }
        }
    }

    public bool reversing;
    public float reverseDistanceBack = 1f;
    public Vector3 behind;
    public State lastState;
    void Reverse() {
        reversing = true;
        behind = transform.forward * reverseDistanceBack;
     
    }


    //-----Line of Sight Stuff
    public bool playerInLOS;
    public float LOSRange = 20f;
    public float LOSCheckInterval = 0.3f;
    public float timeTilLOSCheck;
    public void MoveWithLOS() {
        Vector3 t = player.transform.position;
        t.y = transform.position.y;
        transform.LookAt(t);
        
        if (timeTilLOSCheck <= 0) {
            playerInLOS = CheckLOS();
            timeTilLOSCheck = LOSCheckInterval;
        }
        if (playerInLOS) {
            MoveToPlayer();
        }
    }

    public bool CheckLOS() {
        Debug.DrawRay(transform.position + new Vector3(0, 1f, 0), transform.forward * LOSRange, Color.magenta);
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
        bool tp = Physics.Raycast(ray, out hit, LOSRange);
        if (tp) {
            //print($"Racer Raycast hit {hit.transform.name}");
            if (hit.transform.gameObject.GetComponent<PlayerController>()) {
                //print($"Player in LOS for {gameObject.name} ({thisInst})");
                return true;
            }
        }

        return false;
    }
    //-----End of Line of Sight Stuff
}
