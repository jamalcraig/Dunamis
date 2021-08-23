using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bindings : MonoBehaviour {
    public static Bindings bindingsClass;

    public GameObject c, c2;


    private void Awake() {
        if (bindingsClass == null) {
            DontDestroyOnLoad(this);
            bindingsClass = this;
        } else if (bindingsClass != this) {
            Destroy(this);
        }
        
    }

    private void Start() {
        SetBindingToIOS();
        StartCoroutine(waitToFindC2());
    }



    ControllerMap b;
    public void SelectBinding() {
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetButtonDown(UECircle)) {
            SetBindingToUnity();
        }
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetButtonDown(Circle)) {
            SetBindingToIOS();
        }

        BindButtons(b);
    }
    public void SetBindingToUnity() {
        b = ControllerMap.Unity;
    }
    public void SetBindingToIOS() {
        b = ControllerMap.IOS;
    }
    public ControllerMap GetControllerMap() {
        return b;
    }
    public bool MapIsUnity() {
        return b == ControllerMap.Unity;
    }
    public bool MapIsIOS() {
        return b == ControllerMap.IOS;
    }

    public enum ControllerMap { IOS, Unity };
    public enum InputMap { Action, Shoot, LStick, RStick, Start };

    [HideInInspector]
    public float SHOOT, RSTICK_X, RSTICK_Y;
    [HideInInspector]
    public bool ACTION, SHOOTBUT, START, L1, L2BUT, R1, R1HELD, R1RELEASE, XBUT, TRIANGLEBUT, CIRCLEBUT, CIRCLEHELD, LEFTBUT, RIGHTBUT;

    public const string L1B = "PS4_C_L1_But";
    public const string L1A = "PS4_C_L1_Axis";
    public const string L2B = "PS4_C_L2_But";
    public const string L2A = "PS4_C_L2_Axis";
    public const string L3 = "PS4_C_L3";
    public const string R1B = "PS4_C_R1_But";
    public const string R1A = "PS4_C_R1_Axis";
    public const string R2B = "PS4_C_R2_But";
    public const string R2A = "PS4_C_R2_Axis";
    public const string R3 = "PS4_C_R3";
    public const string Triangle = "PS4_C_Triangle";
    public const string Circle = "PS4_C_Circle";
    public const string Square = "PS4_C_Square";
    public const string X = "PS4_C_X";
    public const string StartHold = "PS4_C_Options_Hold";
    public const string OptionsHold = "PS4_C_Options_Hold";
    public const string StartPress = "PS4_C_Options_Press";
    public const string OptionsPress = "PS4_C_Options_Press";
    public const string RStickX = "PS4_C_RStick_X";
    public const string RStickY = "PS4_C_RStick_Y";
    public const string LDPadPress = "PS4_C_DPad_Left_But";
    public const string RDPadPress = "PS4_C_DPad_Right_But";

    public const string UEShoot = "UEShoot";
    public const string UEAction = "UEAction";
    public const string UERStickX = "UERStick_X";
    public const string UERStickY = "UERStick_Y";
    public const string UEStart = "UEStart";
    public const string UEJL = "UEJL";
    public const string UEIK = "UEIK";
    public const string UEL1 = "UEL1";
    public const string UEL2 = "UEL2";
    public const string UER1 = "UER1";
    public const string UEX = "UEX";
    public const string UETriangle = "UETriangle";
    public const string UECircle = "UECircle";
    public const string UE1 = "UE1";


    public void BindButtons(ControllerMap t) {
        switch (t) {
        case ControllerMap.IOS:
            ACTION = Input.GetButtonDown(Square);
            SHOOT = Input.GetAxis(R2A);
            SHOOTBUT = Input.GetButtonDown(R2B);
            RSTICK_X = Input.GetAxis(RStickX);
            RSTICK_Y = Input.GetAxis(RStickY);
            START = Input.GetButtonDown(StartPress);
            L1 = Input.GetButtonDown(L1B);
            L2BUT = Input.GetButtonDown(L2B);
            R1 = Input.GetButtonDown(R1B);
            R1HELD = Input.GetButton(R1B);
            R1RELEASE = Input.GetButtonUp(R1B);
            XBUT = Input.GetButtonDown(X);
            TRIANGLEBUT = Input.GetButtonDown(Triangle);
            CIRCLEBUT = Input.GetButtonDown(Circle);
            CIRCLEHELD = Input.GetButton(Circle);
            LEFTBUT = Input.GetButtonDown(LDPadPress);
            RIGHTBUT = Input.GetButtonDown(RDPadPress);
            break;
        case ControllerMap.Unity:
            ACTION = Input.GetButtonDown(UEAction);
            SHOOT = Input.GetAxis(UEShoot) + Input.GetAxis(UE1);
            SHOOTBUT = Input.GetButtonDown(UE1);
            RSTICK_X = Input.GetAxis(UERStickX) + Input.GetAxis(UEJL);
            //Debug.Log("Right Stick X: " + RSTICK_X);
            RSTICK_Y = Input.GetAxis(UERStickY) + Input.GetAxis(UEIK);
            START = Input.GetButtonDown(UEStart);
            L1 = Input.GetButtonDown(UEL1);
            L2BUT = Input.GetButtonDown(UEL2);
            R1 = Input.GetButtonDown(UER1);
            R1HELD = Input.GetButton(UER1);
            R1RELEASE = Input.GetButtonUp(UER1);
            XBUT = Input.GetButtonDown(UEX);
            TRIANGLEBUT = Input.GetButtonDown(UETriangle);
            CIRCLEBUT = Input.GetButtonDown(UECircle);
            CIRCLEHELD = Input.GetButton(UECircle);
            LEFTBUT = Input.GetKeyDown(KeyCode.LeftShift);
            RIGHTBUT = Input.GetKeyDown(KeyCode.LeftControl);
            break;
        }


    }

    IEnumerator waitToFindC2() {
        Debug.Log("Started Wait To Find C2");
        yield return new WaitForSeconds(1f);

        c2 = GameObject.FindGameObjectWithTag("Test Cube");
    }

    public void BindingTest() {

        if (ACTION) {
            //c2.GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("Action");
        }

        if (L1) {
            Debug.Log("L1");
        }

        if (L2BUT) {
            Debug.Log("L2BUT");
        }

        if (R1) {
            Debug.Log("R1 - " + Time.unscaledTime);
        }

        if (R1HELD) {
            Debug.Log("R1HELD - " + Time.unscaledTime);
        }

        if (R1RELEASE) {
            Debug.Log("R1RELEASED - " + Time.unscaledTime );
        }

        if (XBUT) {
            Debug.Log("XBUT");
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                SceneManager.LoadScene(1);
            }
             
        }

        if (LEFTBUT) {
            Debug.Log("LeftDPAD");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (TRIANGLEBUT) {
            print("TRIANGLEBUT");
        }

        if (CIRCLEBUT) {
            print("CIRCLEBUT");
            
        }

        if (SHOOT > 0) {
            //c2.GetComponent<Renderer>().material.color = Color.blue;
            Debug.Log("Shoot");
        }

        if (SHOOTBUT) {
            //c2.GetComponent<Renderer>().material.color = Color.blue;
            Debug.Log("SHOOTBUT");
        }

        if (START) {
            //c2.GetComponent<Renderer>().material.color = Color.black;
            Debug.Log("Start");
        }
            

    }

    void trigger() {

        c.GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    int index = 0;
    public void findingBut() {
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
        /*        text.text = "Input: " + index + " --- " + name + " : " + x;
                if (x > 0) {
                    trigger();
                } */
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
