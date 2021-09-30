using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension  : CinemachineExtension
{
    [SerializeField]
    private float horizontalSpeed = 10f;
    [SerializeField]
    private float verticalSpeed = 10f;
    [SerializeField]
    private float clampAngle = 80f;

    private Vector3 startingRotation;

    // Joystick controls
    Vector3 joystickVector;
    [SerializeField]
    protected VariableJoystick joystick;
    // button
    protected JoyButton joyButton1;
    public float mouseSensitivity = 50f;

    protected override void Awake()
    {
        startingRotation = transform.localRotation.eulerAngles;
        base.Awake();
        //Cursor.visible = false;
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (joystick != null)
                {
                    //Debug.Log("Joystick not null");
                    // get joystick values
                    //float h = joystick.Horizontal * 1f + Input.GetAxis("Horizontal") * 1f;
                    //float v = joystick.Vertical * 1f + Input.GetAxis("Vertical") * 1f;
                    //weapon.transform.Rotate(new Vector3(joystick.Vertical * 2f + Input.GetAxis("Vertical") * 2f, joystick.Horizontal * 2f + +Input.GetAxis("Horizontal") * 2f, 0));

                    // Get Input 
                    float mouseX = (joystick.Horizontal + Input.GetAxis("Mouse X")) * horizontalSpeed * Time.deltaTime;
                    float mouseY = (joystick.Vertical + Input.GetAxis("Mouse Y")) * verticalSpeed * Time.deltaTime;

                    // apply to rotation, and process (clamp) etc.
                    startingRotation.x += mouseX;
                    startingRotation.y += mouseY;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                    state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);

                    // Shoot code, add later
                    /*
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        shoot = true;
                    }
                    // Get touch, so you can shoot, while using Joystick(aiming), independently from each other, native Unity Code...
                    if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
                    {
                        Shoot();
                        if (shootTimePassed < shootInterval)
                        {
                            shootTimePassed += Time.deltaTime;
                        }
                        else
                        {
                            shoot = true;
                            Shoot();
                            shootTimePassed = 0;
                        }
                    }
                    */
                }
                else
                {
                    Debug.Log("Joystick null");
                }

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
