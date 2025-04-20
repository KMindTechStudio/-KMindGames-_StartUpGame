using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("Assign Dash Objects")]
    public Button btn_Dash;
    public Image image_DashCoolDown;
    public TMP_Text text_DashCoolDown;

    [Header("Dash Variables")]
    public float dashCoolDown = 5f; // Cooldown time in seconds
    public float dashDuration = 0.5f; // Duration of the dash
    public float dashSpeed = 10f; // Speed of the dash
    private float dashCoolDownTimer = 0f; // Timer for cooldown
    private bool isCoolDown; 

    public VirtualJoystick joystick; // Reference to the joystick script

    void Start()
    {   
        joystick = FindAnyObjectByType<VirtualJoystick>();

        //Initialize the starting variables
        isCoolDown = false; //Ready to dash
        image_DashCoolDown.fillAmount = 0f;
        text_DashCoolDown.gameObject.SetActive(false);
        btn_Dash.onClick.AddListener(dashCoroutine);
    }

    void dashCoroutine()
    {
        Debug.Log("Dash Button Pressed");
        StartCoroutine(Dash());
    }
    void Update()
    {
        if (isCoolDown)
        {
            OnCooldown();
        }

        Debug.Log("playerPosisiton: " + transform.position);

    }
    void OnCooldown()
    {
        dashCoolDownTimer -= Time.deltaTime;
        if (dashCoolDownTimer <= 0f)
        {
            // If the cool down is over, make it be able to dash
            isCoolDown = false;
            text_DashCoolDown.gameObject.SetActive(false);
            image_DashCoolDown.fillAmount = 0f;
            btn_Dash.interactable = true;
        }
        else
        {
            // If the cooldown is not over, show the timer and make it not be able to dash
            text_DashCoolDown.gameObject.SetActive(true);
            text_DashCoolDown.text = Mathf.Ceil(dashCoolDownTimer).ToString();
            image_DashCoolDown.fillAmount = dashCoolDownTimer / dashCoolDown;
            btn_Dash.interactable = false;
        }
    }
    IEnumerator Dash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            if (isCoolDown)
            {
                //absolutely nothing
            }
            else
            {
                // Perform the dash action and set the cooldown be true
                if (joystick.moveDirection != null)
                {
                    if (transform.rotation.y == 0)
                    {
                        transform.position = new Vector3((joystick.moveDirection.x - dashSpeed * dashDuration) *Time.deltaTime, 0, transform.position.z);
                    }

                    if (transform.rotation.y == -180)
                    {
                        transform.position = new Vector3((joystick.moveDirection.x + dashSpeed * dashDuration) , 0, transform.position.z);
                    }
                }
                isCoolDown = true;
                text_DashCoolDown.gameObject.SetActive(true);
                dashCoolDownTimer = dashCoolDown;
            }
            yield return null;
        }
    }


}
