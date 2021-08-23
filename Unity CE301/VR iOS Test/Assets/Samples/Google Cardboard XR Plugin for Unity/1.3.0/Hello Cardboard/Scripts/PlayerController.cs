using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed = 1;
    public float maxSpeed = 5;
    Rigidbody rb;
    public float rot;
    public float turnSpeed = 10f;
    public GameObject c, c2;
    public Text text;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rot = 0;
        BindButtons(ControllerMap.IOS);
    }

    // Update is called once per frame
    void Update() {
        Move();
        findingBut();
      

        SelectBinding();
        BindingTest();
        
    }

    ControllerMap b;
    void SelectBinding() {
        if (Input.GetKeyDown(KeyCode.Equals)) {
            b = ControllerMap.Unity;
        }
        if (Input.GetKeyDown(KeyCode.Minus)) {
            b = ControllerMap.IOS;
        }

        BindButtons(b);
    }

    void Move() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        
        Vector3 movement = new Vector3(x, 0, z);

        rb.AddRelativeForce(movement * speed * Time.deltaTime);
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
//        Debug.Log(Input.GetAxis(UEJL));
        rot += RSTICK_X * turnSpeed * Time.deltaTime;
        rb.rotation = Quaternion.Euler(0f, rot, 0f);
    }

    void trigger() {
        
        c.GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    void BindingTest() {

        if (ACTION)
            c2.GetComponent<Renderer>().material.color = Color.red;

        if (SHOOT > 0)
            c2.GetComponent<Renderer>().material.color = Color.blue;

        if (START)
            c2.GetComponent<Renderer>().material.color = Color.black;
        
    }

    public enum ControllerMap { IOS, Unity };
    public enum InputMap { Action, Shoot, LStick, RStick, Start };

    float SHOOT, RSTICK_X, RSTICK_Y;
    bool ACTION, START;

    string L1B = "PS4_C_L1_But";
    string L1A = "PS4_C_L1_Axis";
    string L2B = "PS4_C_L2_But";
    string L2A = "PS4_C_L2_Axis";
    string L3 = "PS4_C_L3";
    string R1B = "PS4_C_R1_But";
    string R1A = "PS4_C_R1_Axis";
    string R2B = "PS4_C_R1_But";
    string R2A = "PS4_C_R2_Axis";
    string R3 = "PS4_C_R3";
    string Triangle = "PS4_C_Triangle";
    string Circle = "PS4_C_Circle";
    string Square = "PS4_C_Square";
    string X = "PS4_C_X";
    string StartHold = "PS4_C_Options_Hold";
    string OptionsHold = "PS4_C_Options_Hold";
    string StartPress = "PS4_C_Options_Press";
    string OptionsPress = "PS4_C_Options_Press";
    string RStickX = "PS4_C_RStick_X";
    string RStickY = "PS4_C_RStick_Y";

    string UEShoot = "UEShoot";
    string UEAction = "UEAction";
    string UERStickX = "UERStick_X";
    string UERStickY = "UERStick_Y";
    string UEStart = "UEStart";
    string UEJL = "UEJL";


    void BindButtons(ControllerMap t) {
        switch (t) {
        case ControllerMap.IOS:
            ACTION = Input.GetButton(Square);
            SHOOT = Input.GetAxis(R2A);
            RSTICK_X = Input.GetAxis(RStickX);
            RSTICK_Y = Input.GetAxis(RStickY);
            START = Input.GetButtonDown(StartPress);
            break;
        case ControllerMap.Unity:
            ACTION = Input.GetButton(UEAction);
            SHOOT = Input.GetAxis(UEShoot);
            RSTICK_X = Input.GetAxis(UERStickX) + Input.GetAxis(UEJL);
            Debug.Log("RSX: " + RSTICK_X);
            RSTICK_Y = Input.GetAxis(UERStickY);
            START = Input.GetButtonDown(UEStart);
            break;
        }

        
    }

    int index = 0;
    void findingBut() {
        if (Input.GetButtonDown("PS4_C_Options_Press")) {
            index++;
            if (index > 28) {
                index = 0;
            }
        }
        float x = 0;
        string name = "";
        switch (index) {
        case 0:
            x = Input.GetAxis("PS4_C_L1_But");
            name = "PS4_C_L1_But";
            break;
        case 1:
            x = Input.GetAxis("PS4_C_L1_Axis");
            name = "PS4_C_L1_Axis";
            break;
        case 2:
            x = Input.GetAxis("PS4_C_L2_But");
            name = "PS4_C_L2_But";
            break;
        case 3:
            x = Input.GetAxis("PS4_C_L2_Axis");
            name = "PS4_C_L2_Axis";
            break;
        case 4:
            x = Input.GetAxis("PS4_C_L3");
            name = "PS4_C_L3";
            break;
        case 5:
            x = Input.GetAxis("PS4_C_R1_But");
            name = "PS4_C_R1_But";
            break;
        case 6:
            x = Input.GetAxis("PS4_C_R1_Axis");
            name = "PS4_C_R1_Axis";
            break;
        case 7:
            x = Input.GetAxis("PS4_C_R2_But");
            name = "PS4_C_R2_But";
            break;
        case 8:
            x = Input.GetAxis("PS4_C_R2_Axis");
            name = "PS4_C_R2_Axis";
            break;
        case 9:
            x = Input.GetAxis("PS4_C_R3");
            name = "PS4_C_R3";
            break;
        case 10:
            x = Input.GetAxis("PS4_C_Triangle");
            name = "PS4_C_Triangle";
            break;
        case 11:
            x = Input.GetAxis("PS4_C_Circle");
            name = "PS4_C_Circle";
            break;
        case 12:
            x = Input.GetAxis("PS4_C_X");
            name = "PS4_C_X";
            break;
        case 13:
            x = Input.GetAxis("PS4_C_Square");
            name = "PS4_C_Square";
            break;
        case 14:
            x = Input.GetAxis("PS4_C_Options_Hold");
            name = "PS4_C_Options_Hold";
            break;
        case 15:
            x = Input.GetAxis("PS4_C_DPad_Up_But");
            name = "PS4_C_DPad_Up_But";
            break;
        case 16:
            x = Input.GetAxis("PS4_C_DPad_Up_Axis");
            name = "PS4_C_DPad_Up_Axis";
            break;
        case 17:
            x = Input.GetAxis("PS4_C_DPad_Down_But");
            name = "PS4_C_DPad_Down_But";
            break;
        case 18:
            x = Input.GetAxis("PS4_C_DPad_Down_Axis");
            name = "PS4_C_DPad_Down_Axis";
            break;
        case 19:
            x = Input.GetAxis("PS4_C_DPad_Left_But");
            name = "PS4_C_DPad_Left_But";
            break;
        case 20:
            x = Input.GetAxis("PS4_C_DPad_Left_Axis");
            name = "PS4_C_DPad_Left_Axis";
            break;
        case 21:
            x = Input.GetAxis("PS4_C_DPad_Right_But");
            name = "PS4_C_DPad_Right_But";
            break;
        case 22:
            x = Input.GetAxis("PS4_C_DPad_Right_Axis");
            name = "PS4_C_DPad_Right_Axis";
            break;
        case 23:
            x = Input.GetAxis("PS4_C_RStick_X");
            name = "PS4_C_RStick_X";
            break;
        case 24:
            x = Input.GetAxis("PS4_C_RStick_Y");
            name = "PS4_C_RStick_Y";
            break;
        case 25:
            x = Input.GetAxis("PS4_C_Options_Press");
            name = "PS4_C_Options_Press";
            break;
        case 26:
            x = Input.GetAxis("jb0");
            name = "jb0";
            break;
        case 27:
            x = Input.GetAxis("jb1");
            name = "jb1";
            break;
        case 28:
            x = Input.GetAxis("jb3");
            name = "jb3";
            break;


        }
        text.text = "Input: " + index + " --- " + name + " : " + x;
        if (x > 0) {
            trigger();
        }
    }





    //void findingBut() {
    //    if (Input.GetAxis("PS4_C_X") > 0) {
    //        index++;
    //        if (index > 32) {
    //            index = 0;
    //        }
    //    }
    //    float x = 0;
    //    string name = "";
    //    switch (index) {
    //    case 0:
    //        x = Input.GetAxis("PS4_C_L1");
    //        name = "PS4_C_L1";
    //        break;
    //    case 1:
    //        x = Input.GetAxis("PS4_C_L2");
    //        name = "PS4_C_L2";
    //        break;
    //    case 2:
    //        x = Input.GetAxis("PS4_C_L3");
    //        name = "PS4_C_L3";
    //        break;
    //    case 3:
    //        x = Input.GetAxis("PS4_C_R1");
    //        name = "PS4_C_R1";
    //        break;
    //    case 4:
    //        x = Input.GetAxis("PS4_C_R2");
    //        name = "PS4_C_R2";
    //        break;
    //    case 5:
    //        x = Input.GetAxis("PS4_C_R3");
    //        name = "PS4_C_R3";
    //        break;
    //    case 6:
    //        x = Input.GetAxis("PS4_C_Square");
    //        name = "PS4_C_Square";
    //        break;
    //    case 7:
    //        x = Input.GetAxis("PS4_C_X");
    //        name = "PS4_C_X";
    //        break;
    //    case 8:
    //        x = Input.GetAxis("PS4_C_Circle");
    //        name = "PS4_C_Circle";
    //        break;
    //    case 9:
    //        x = Input.GetAxis("PS4_C_Triangle");
    //        name = "PS4_C_Triangle";
    //        break;
    //    case 10:
    //        x = Input.GetAxis("PS4_C_LDPad");
    //        name = "PS4_C_LDPad";
    //        break;
    //    case 11:
    //        x = Input.GetAxis("PS4_C_RDPad");
    //        name = "PS4_C_RDPad";
    //        break;
    //    case 12:
    //        x = Input.GetAxis("PS4_C_UDPad");
    //        name = "PS4_C_UDPad";
    //        break;
    //    case 13:
    //        x = Input.GetAxis("PS4_C_DDPad");
    //        name = "PS4_C_DDPad";
    //        break;
    //    case 14:
    //        x = Input.GetAxis("PS4_C_Options");
    //        name = "PS4_C_Options";
    //        break;
    //    case 15:
    //        x = Input.GetAxis("PS4_C_Share");
    //        name = "PS4_C_Share";
    //        break;
    //    case 16:
    //        x = Input.GetAxis("PS4_C_Touchpad");
    //        name = "PS4_C_Touchpad";
    //        break;
    //    case 17:
    //        x = Input.GetAxis("PS4_C_RStick_X");
    //        name = "PS4_C_RStick_X";
    //        break;
    //    case 18:
    //        x = Input.GetAxis("PS4_C_RStick_Y");
    //        name = "PS4_C_RStick_Y";
    //        break;
    //    case 19:
    //        x = Input.GetAxis("PS4_C_PS");
    //        name = "PS4_C_PS";
    //        break;
    //    case 20:
    //        x = Input.GetAxis("jb10");
    //        name = "jb10";
    //        break;
    //    case 21:
    //        x = Input.GetAxis("jb11");
    //        name = "jb11";
    //        break;
    //    case 22:
    //        x = Input.GetAxis("jb14");
    //        name = "jb14";
    //        break;
    //    case 23:
    //        x = Input.GetAxis("jb15");
    //        name = "jb15";
    //        break;
    //    case 24:
    //        x = Input.GetAxis("jb16");
    //        name = "jb16";
    //        break;
    //    case 25:
    //        x = Input.GetAxis("jb17");
    //        name = "jb17";
    //        break;
    //    case 26:
    //        x = Input.GetAxis("5th");
    //        name = "5th";
    //        break;
    //    case 27:
    //        x = Input.GetAxis("8th");
    //        name = "8th";
    //        break;
    //    case 28:
    //        x = Input.GetAxis("11th");
    //        name = "11th";
    //        break;
    //    case 29:
    //        x = Input.GetAxis("12th");
    //        name = "12th";
    //        break;
    //    case 30:
    //        x = Input.GetAxis("jb18");
    //        name = "jb18";
    //        break;
    //    case 31:
    //        x = Input.GetAxis("jb19");
    //        name = "jb19";
    //        break;
    //    case 32:
    //        x = Input.GetAxis("13th");
    //        name = "13th";
    //        break;
    //    case 33:
    //        x = Input.GetAxis("14th");
    //        name = "14th";
    //        break;
    //    }
    //    text.text = "Input: " + index + " --- " + name + " : " + x;
    //    if (x > 0) {
    //        trigger();
    //    }
    //}
}
