using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public Button btn_Dash;
    public Image image_DashCooldown;
    public TMP_Text text_Dashcooldown;
    public float dashDistance = 5f;
    private float dashCooldown = 5f;
    private float dashCooldownTimer = 0f;
    private bool isCooldown;


    void Start()
    {
        isCooldown = false;
        image_DashCooldown.fillAmount = 0f;
        text_Dashcooldown.gameObject.SetActive(false);
        btn_Dash.onClick.AddListener(Dash);
    }

    void OnCooldown()
    {
        dashCooldownTimer -= Time.deltaTime;
        if (dashCooldownTimer <= 0f)
        {
            isCooldown = false;
            text_Dashcooldown.gameObject.SetActive(false);
            image_DashCooldown.fillAmount = 0f;
            btn_Dash.interactable = true;
        }
        else
        {
            text_Dashcooldown.gameObject.SetActive(true);
            text_Dashcooldown.text = Mathf.Ceil(dashCooldownTimer).ToString();
            image_DashCooldown.fillAmount = dashCooldownTimer / dashCooldown;
            btn_Dash.interactable = false;
        }
    }
    void Dash()
    {
        if(isCooldown)
        {
        }
        else
        {
            transform.position += transform.forward * dashDistance;
            isCooldown = true;
            text_Dashcooldown.gameObject.SetActive(true);
            dashCooldownTimer = dashCooldown;
        }
    }

    void Update()
    {
        if (isCooldown)
        {
            OnCooldown();
        }
    }
}
