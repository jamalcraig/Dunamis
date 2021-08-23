using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : Player {

    ScoreManager scoreManager;
    public float speed = 1;
    public float maxSpeed = 5;
    Rigidbody rb;
    [HideInInspector]
    public float rotY;
    [HideInInspector]
    public float rotX;
    public float turnSpeed = 10f;
    public float throwSpeed = 30f;

    public float grabDistance = 5f;
    public float destinationDistance;

    //Animator anim;
    //int velXHash = Animator.StringToHash("Vel X");
    //int velYHash = Animator.StringToHash("Vel Y");
    //int velZHash = Animator.StringToHash("Vel Z");
    int throwHash = Animator.StringToHash("Throw");
    bool isThrowing;

    public Camera cam;
    LayerMask enemyLM;
    LayerMask UILM;
    public Canvas pauseMenu;

    public Transform throwPosition;
    public GameObject rockCube;

    public int dunamisAmmo = 10;
    public int maxHealth = 10;
    public int health = 10;

    AudioSource audioSource;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip healthSound;
    public AudioClip pickUpGemSound;

    // Start is called before the first frame update
    void Start() {
        BIND();
        enemyLM = LayerMask.GetMask("Enemy");
        UILM = LayerMask.GetMask("UI");
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        rotY = 0;
        rotX = 0;
        scoreManager = FindObjectOfType<ScoreManager>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update() {
        Move();
        bindings.SelectBinding();
        bindings.BindingTest();

        findPath = bindings.ACTION;
        findPathToRockSpawner = bindings.L1;
        if (bindings.ACTION) {
            //Shoot();
        }

        if (bindings.START) {
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeInHierarchy);
        }

        if (bindings.L1) {

            //SelectOnUI();
        }

        if ((bindings.SHOOTBUT || bindings.SHOOT > 0.1f) && !isThrowing && dunamisAmmo > 0) {
            StartCoroutine(ThrowAnimation());
            StartCoroutine(ThrowRock());
        }

        if (bindings.R1) {
            Grab();
        }


        if (!bindings.R1HELD && grabbingTray) {
            print("R1 Held Detach");
            DetachFromTray();
        }

        if (bindings.RIGHTBUT) {

            audioSource.mute = !audioSource.mute;
        }

        if (bindings.TRIANGLEBUT) {
            health = 1000;
            scoreManager.UpdateHealth(health);
        }

        if (pathway != null && pathway.Count > 0) {
            //print("Distance away from path end: " + (int)Vector3.Distance(pathway[0].worldPos, transform.position));
            if (Vector3.Distance(pathway[0].worldPos, transform.position) < destinationDistance) {
                clearPath = true;
            } else {
                clearPath = false;
            }
        }


    }

    

    public override void Move() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        
        Vector3 movement = new Vector3(x, 0, z);

        rb.AddRelativeForce(movement * speed * Time.deltaTime);
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
//        Debug.Log(Input.GetAxis(UEJL));
        rotY += bindings.RSTICK_X * turnSpeed * Time.deltaTime;
        if (bindings.MapIsUnity()) {
            rotX -= bindings.RSTICK_Y * (turnSpeed/3) * Time.deltaTime;
            cam.transform.localRotation = Quaternion.Euler(rotX, cam.transform.rotation.y, cam.transform.rotation.z);
        }
        
        rb.rotation = Quaternion.Euler(0f, rotY, 0f);

        anim.SetFloat(velXHash, transform.InverseTransformDirection(rb.velocity).x);
        anim.SetFloat(velYHash, transform.InverseTransformDirection(rb.velocity).y);
        anim.SetFloat(velZHash, transform.InverseTransformDirection(rb.velocity).z);
    }

    IEnumerator ThrowAnimation() {
        Debug.Log("Throw");
        anim.SetBool(throwHash, true);
        isThrowing = true;
        yield return new WaitForSeconds(1f);
        anim.SetBool(throwHash, false);
        isThrowing = false;
    }

    IEnumerator ThrowRock() {
        Debug.Log("Throw Rock");
        yield return new WaitForSeconds(0.5f);
        //Instantiate(rockCube, throwPosition);
        audioSource.PlayOneShot(throwSound, 4);
        GameObject r = Instantiate(rockCube, throwPosition.position, cam.gameObject.transform.rotation);
        r.gameObject.GetComponent<RockCube>().Firee(RayCastFromCrosshairAll().point, throwSpeed);
        //Debug.DrawRay(throwPosition.position, RayCastFromCrosshairAll().point, Color.red, 5f);
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Debug.DrawRay(ray.origin, ray.direction.normalized * 10f, Color.blue, 5f);
        dunamisAmmo--;
        scoreManager.UpdateDunamisAmmoCount(dunamisAmmo);
        

    }

    RaycastHit RayCastFromCrosshairAll() {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            //print("RayCastFromCrosshairAll: I'm looking at " + hit.transform.name);
            

        } else {
            //print("RayCastFromCrosshairAll: I'm looking at nothing!");
        }
        //print("Hit Pos: " + hit.transform.position);
        
        return hit;
    }

    //I think I made this method to test raycasting
    //it also kills things in the enemy layer
    void Shoot() {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            print("I'm looking at " + hit.transform.name);
            if (hit.transform.gameObject.GetComponent<Renderer>()) {
                hit.transform.gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV();
            }

        } else {
            print("I'm looking at nothing!");
        }

        if (Physics.Raycast(ray, out hit, 100f, enemyLM)) {
            Destroy(hit.transform.parent.gameObject);
            
        }
        
    }

    void SelectOnUI() {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;



        if (Physics.Raycast(ray, out hit, 100f, UILM)) {
            hit.transform.GetComponent<Button>().onClick.Invoke();
            print("I'm looking at " + hit.transform.name);
        }
    }

    bool grabbingTray;
    Vector3 trayOffset;
    public GameObject objectBeingPushed;
    public Rigidbody crosshairRB;
    void Grab() {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit, grabDistance, grabableLM)) {
            print($"Grab Raycast hit {hit.transform.gameObject.name}");
            if (hit.transform.gameObject.GetComponent<RockCube>()) {
                hit.transform.gameObject.GetComponentInParent<RockSpawner>().RemoveRock(hit.transform.gameObject);
                //Destroy(hit.transform.gameObject);

                //scoreManager.PickedUpDunamis();
                //dunamisAmmo++;

                //scoreManager.UpdateDunamisAmmoCount(dunamisAmmo);
                AddDunamisAmmo(1);
            }
            if (hit.transform.gameObject.GetComponent<PipeDunamis>()) {

                if (hit.transform.gameObject.GetComponent<PipeDunamis>().type == 1) {
                    scoreManager.PickedUpPipeDunamis(1);
                } else {
                    AddDunamisAmmo(1);
                }
                hit.transform.gameObject.GetComponent<PipeDunamis>().Hit();
            }
            if (hit.transform.gameObject.GetComponent<PlayerClone>()){
                print("Grabbed Player Clone");
                hit.transform.gameObject.GetComponent<PlayerClone>().DunamisTaken();
            }

            
            if (hit.transform.gameObject.GetComponent<Tray>()) {
                if (objectBeingPushed == null) {
                    print("Grabbed Tray");
                    objectBeingPushed = hit.transform.gameObject;
                    grabbingTray = true;
                    objectBeingPushed.AddComponent<FixedJoint>();
                    objectBeingPushed.GetComponent<FixedJoint>().connectedBody = crosshairRB;
                    objectBeingPushed.GetComponent<Rigidbody>().mass = 0;
                    objectBeingPushed.GetComponent<Tray>().SetToGrabbedMat();
                    //trayOffset = cam.ViewportToWorldPoint(hit.transform.position)
                }

            } else {
                print("Grab didn't hit tray, so detach");
                DetachFromTray();
            }
        }

    }

    void GrabbingTray() {
        //if r1 still held down
        


    }

    void DetachFromTray() {
        grabbingTray = false;
        if (objectBeingPushed != null) {
            objectBeingPushed.GetComponent<Tray>().SetToUngrabbedMat();
            objectBeingPushed.GetComponent<Rigidbody>().mass = 1;
            objectBeingPushed.GetComponent<FixedJoint>().connectedBody = null;
            Destroy(objectBeingPushed.GetComponent<FixedJoint>());
            //objectBeingPushed.gameObject.transform.SetParent(null);
            objectBeingPushed = null;
        }
        print("DetachFromTray - " + Time.unscaledTime);
    }
    

    public void AddHealth() {
        if (health < maxHealth) {
            health++;
            scoreManager.UpdateHealth(health);
            audioSource.PlayOneShot(healthSound, 4);
        }
    }
    public void ReduceHealth(int damage) {
        audioSource.PlayOneShot(hitSound, 4);
        health -= damage;
        scoreManager.UpdateHealth(health);
        if (health <= 0) {
            SceneManager.LoadScene(0);
        }
    }

    public void AddDunamisAmmo(int ammo) {
        dunamisAmmo += ammo;
        scoreManager.PickedUpDunamis(ammo);
        scoreManager.UpdateDunamisAmmoCount(dunamisAmmo);

        audioSource.PlayOneShot(pickUpGemSound, 4);

    }

}
