using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashForce = 30f; //Dash Duration and Force: dash become quicker/ slower
    public float dashDuration = 0.12f;
    public float dashCooldown = 1.5f; //Time between dashes
    private float dashCooldownTimer = 0f;
    private bool isDashing = false;
    private bool canDash = true;
    private Rigidbody2D rb;
    private GameObject player;
    private Vector2 dashDirection;
    private Animator anim;

    [Header("UI Settings")]
    public Image img_DashCooldown;
    public TextMeshProUGUI txt_DashCooldown;
    public Button btn_Dash;

    private void Start()
    {
        img_DashCooldown.fillAmount = 1f;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
        anim = player.GetComponent<Animator>();
    }


    public void Dash()
    {
        if (!canDash || isDashing)
        {
            return;
        }

        StartCoroutine(DashCourotine());
    }

    private void Update()
    {
        //Debug.Log(player.transform.rotation.y);
        if (!canDash)
        {
            OnCoolDown();
        }
    }


    private void OnCoolDown()
    {
        //Calculate the cooldown timer
        dashCooldownTimer -= Time.deltaTime;
        if (dashCooldownTimer > 0)
        {
            //Set the text, the image to be in cooldown
            txt_DashCooldown.gameObject.SetActive(true);
            img_DashCooldown.fillAmount = dashCooldownTimer/dashCooldown;
            txt_DashCooldown.text = Mathf.Ceil(dashCooldownTimer).ToString();
            btn_Dash.interactable = false;
        }
        else
        {
            txt_DashCooldown.gameObject.SetActive(false);
            btn_Dash.interactable = true;
        }
    }
    IEnumerator DashCourotine()
    {
        //Assign dashing variables
        isDashing = true;
        canDash = false;

        //Assign dashCooldownTimer start and duration
        dashCooldownTimer = dashCooldown;

        // Sure, if the rotation is like this, it means the player is facing left
        if (player.transform.rotation.y <0.5f)
        {
            dashDirection = Vector2.left;
        }
        //I don't fucking know why I write this, but it works
        if (player.transform.rotation.y >0.5f || player.transform.rotation.y <0f)
        {
            dashDirection = Vector2.right;
        }
        //Dash player forward
        rb.linearVelocity = dashDirection.normalized * dashForce;
        anim.SetBool("isDashing", true);
        yield return new WaitForSeconds(dashDuration);

        //Stop dashing after dashDuration
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        anim.SetBool("isDashing", false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
