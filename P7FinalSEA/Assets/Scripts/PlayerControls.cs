using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    float left;
    float forward;
    public float fireRateInitial = 0.49f;
    public float fireRate;
    bool grounded = true;
    public GameObject[] projHit;
    public GameObject[] projTrails;
    public float[] reloadTimes;
    float weaponScrollWheel = 1;
    int actualScroll;
    public float parryTimer = 0f;
    float parryCooldown = 0f;
    float parryEffects = 2f;
    float dashCount = 0f;
    public float maxDashStamina;
    public GameObject parryTrail;
    public GameObject parryEffect;
    float dashStamina;
    bool dashing = false;
    float globalGravity = -0.9f;
    public float gravityScale = 1.0f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        fireRateInitial = reloadTimes[Mathf.FloorToInt(weaponScrollWheel)];
        dashStamina = maxDashStamina;
        fireRate = fireRateInitial;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dashing)
        {
            dashStamina += Time.deltaTime * (1f/3f);
        }
        if (dashStamina >= maxDashStamina)
        {
            dashStamina = maxDashStamina;
        }
        else if (dashStamina <= 0)
        {
            dashStamina = 0;
        }
        GameObject.Find("Dash Stamina").GetComponent<DashBar>().SetValue(dashStamina);
        BasicMovement();
        Shooting();
        AdvancedMovement();
        //Parrying();
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    void BasicMovement()
    {
        left = Input.GetAxis("Horizontal");
        forward = Input.GetAxis("Vertical");
        if (!dashing)
        {
            transform.Translate(Vector3.forward * 15f * forward * Time.deltaTime);
            transform.Translate(Vector3.right * 15f * left * Time.deltaTime);
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, GameObject.Find("PlayerCam").transform.localEulerAngles.y, 0));
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddRelativeForce(Vector3.up*10f, ForceMode.Impulse);
            grounded = false;
        }
    }

    void AdvancedMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !grounded)
        {
            if (dashStamina > 0)
            {
                dashStamina -= Time.deltaTime;
                dashing = true;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                transform.Translate(Vector3.forward * 30f * Time.deltaTime);
            }
        }
        else
        {
            dashing = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !grounded)
        {
            dashCount++;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !grounded && dashCount == 1 && !dashing)
        {
            rb.AddRelativeForce(Vector3.up*10f, ForceMode.Impulse);
            dashCount++;
            grounded = false;
        }
    }

    void UpdateReload(int weaponChosen)
    {
        fireRateInitial = reloadTimes[weaponChosen];
        fireRate = fireRateInitial;
    }

    void Shooting()
    {
        fireRate -= Time.deltaTime;
        weaponScrollWheel += Input.mouseScrollDelta.y;
        if (weaponScrollWheel < 0)
        {
            weaponScrollWheel = projTrails.Length;
        }
        actualScroll = Mathf.FloorToInt(Mathf.Abs(weaponScrollWheel) % projTrails.Length);
        if (Input.mouseScrollDelta.y != 0)
        {
            UpdateReload(actualScroll);
        }
        GameObject.Find("Reload Timer").GetComponent<ReloadTimer>().SetValue(fireRateInitial, fireRate);
        var inputFinder = Input.GetMouseButtonDown(0);
        var randomRange = UnityEngine.Random.Range(0f, 0f);
        if (projTrails[actualScroll].GetComponent<DamageField>().automatic)
        {
            inputFinder = Input.GetMouseButton(0);
            randomRange = UnityEngine.Random.Range(-0.035f, 0.035f);
        }
        else if (!projTrails[actualScroll].GetComponent<DamageField>().automatic)
        {
            inputFinder = Input.GetMouseButtonDown(0);
            randomRange = UnityEngine.Random.Range(0f, 0f);
        }
        if (inputFinder && fireRate <= 0)
        {
            Vector3 startRaycast = transform.position;
            RaycastHit hit;
            LayerMask mask = 1<<LayerMask.GetMask("TrailTrigger");
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + new Vector3 (randomRange * (UnityEngine.Random.Range(-1.0f, 1.0f)), Mathf.Tan(-GameObject.Find("PlayerCam").transform.localEulerAngles.x/180*Mathf.PI) + randomRange * (UnityEngine.Random.Range(-1.0f, 1.0f)))), out hit, Mathf.Infinity, mask))
            {
                fireRate = fireRateInitial;
                Instantiate(projHit[actualScroll], hit.point, Quaternion.identity);
                projTrails[actualScroll].GetComponent<ProjectileTrail>().SetPosition(hit.point, startRaycast);
                Damageable damaging = hit.collider.gameObject.GetComponent<Damageable>();
                if (damaging == null)
                {
                    return;
                }
                DamageField damageScript = projTrails[actualScroll].GetComponent<DamageField>();
                if (damageScript == null)
                {
                    return;
                }
                else
                {
                    float damageAmount = damageScript.damage;
                    damaging.Damaged(damageAmount);
                    if (damageScript.eleCannon)
                    {
                        damageAmount = 0;
                        GetComponent<Damageable>().damageTaken = 0;
                    }
                }
            }
        }
    }

    void Parrying()
    {
        parryEffects -= Time.deltaTime;
        parryTimer -= Time.deltaTime;
        parryCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E) && parryCooldown <= 0)
        {
            parryTimer = 0.15f;
            parryCooldown = 2f;
            parryEffects = 2f;
        }
        if (parryEffects > 0f)
        {
            parryEffect.SetActive(true);
        }
        else
        {
            parryEffect.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameObject.Find("Ground"))
        {
            grounded = true;
            dashCount = 0;
        }
        if (collision.gameObject.GetComponent<DamageField>() != null && parryTimer > 0)
        {
            float damageParried = collision.gameObject.GetComponent<DamageField>().damage;
            GameObject enthralled = GameObject.FindWithTag("Spark");
            if (enthralled != null)
            {
                enthralled = GameObject.Find("Kilosoult");
                enthralled.GetComponent<Damageable>().Damaged(damageParried);
                parryTrail.GetComponent<ProjectileTrail>().SetPosition(transform.position, enthralled.transform.position);
            }
            else
            {
                enthralled.GetComponent<Damageable>().Damaged(damageParried);
                parryTrail.GetComponent<ProjectileTrail>().SetPosition(transform.position, enthralled.transform.position);
            }
        }
    }
}
