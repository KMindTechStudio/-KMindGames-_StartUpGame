using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public Button btn_Dash;
    public Image image_DashCoolDown;
    public TMP_Text text_DashCoolDown;
    public float dashDistance = 5f;  // Distance to dash
    private float dashCoolDown = 5f; // Cooldown time in seconds
    private float dashCoolDownTimer = 0f; // Timer for cooldown
    private bool isCoolDown; 


    void Start()
    {   
        //Initialize the starting variables
        isCoolDown = false; //Ready to dash
        image_DashCoolDown.fillAmount = 0f;
        text_DashCoolDown.gameObject.SetActive(false);
        btn_Dash.onClick.AddListener(Dash);
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
    void Dash()
    {
        if(isCoolDown)
        {
            //absolutely nothing
        }
        else
        {
            // Perform the dash action and set the cooldown be true
            transform.position += transform.forward * dashDistance;
            isCoolDown = true;
            text_DashCoolDown.gameObject.SetActive(true);
            dashCoolDownTimer = dashCoolDown;
        }
    }

    void Update()
    {
        if (isCoolDown)
        {
            OnCooldown();
        }
    }
}
