using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    float left;
    float forward;
    public float speed;
    public float minimumY;
    public float fireRateInitial = 0.49f;
    public float fireRate;
    public Transform checkpoint;
    public bool grounded = true;
    public bool slamming = false;
    public GameObject[] projHit;
    public GameObject[] projTrails;
    public float[] reloadTimes;
    float weaponScrollWheel = 1;
    int actualScroll;
    public float dashCount = 0f;
    public float maxDashStamina;
    public GameObject slamEffect;
    public GameObject tutorialText;
    public GameObject burnOutText;
    float dashStamina;
    bool dashing = false;
    public bool respawning = true;
    bool dasher = false;
    bool jumpReady = true;
    bool automatic = false;
    int burstFire = 0;
    public int maxAutoBurnOut;
    int burstCount;
    int updatingBurnOut;
    float dashChargeMulti = 1;
    float globalGravity = -0.9f;
    public float gravityScale = 1.0f;
    public float jumpForce = 13f;
    Rigidbody rb;
    public GameObject parryBlock;
    float parryRegen = 2.5f;
    public float maxParry = 3;
    public float currentParry;
    // Start is called before the first frame update
    void Start()
    {
        currentParry = maxParry;
        updatingBurnOut = maxAutoBurnOut;
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
            dashStamina += Time.deltaTime * (1f/3f) * dashChargeMulti;
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
        Parrying();
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
        if (transform.position.y < minimumY)
        {
            GetComponent<Damageable>().Damaged(100);
        }
        if (grounded)
        {
            dashChargeMulti = 2.5f;
        }
        else
        {
            dashChargeMulti = 1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            tutorialText.GetComponent<TMP_Text>().text = "Your starting weapon is a basic semi-auto. The other is an (unnecessarily complicated) automatic.";
        }
        if (grounded)
        {
            respawning = false;
        }
    }

    void BasicMovement()
    {
        left = Input.GetAxis("Horizontal");
        forward = Input.GetAxis("Vertical");
        if (!dashing && !slamming && !respawning)
        {
            transform.Translate(Vector3.forward * speed * forward * Time.deltaTime);
            transform.Translate(Vector3.right * speed * left * Time.deltaTime);
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, GameObject.Find("PlayerCam").transform.localEulerAngles.y, 0));
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddRelativeForce(Vector3.up*jumpForce, ForceMode.Impulse);
            grounded = false;
        }
        if (transform.position.y < minimumY)
        {
            transform.position = checkpoint.position;
        }
    }

    void AdvancedMovement()
    {
        if (!respawning)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !grounded && dasher)
            {
                if (dashStamina > 0)
                {
                    dashStamina -= Time.deltaTime;
                    dashing = true;
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    transform.Translate(Vector3.forward * speed * 2f * Time.deltaTime);
                }
            }
            else
            {
                dashing = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) && !grounded)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.down * 80f, ForceMode.Impulse);
                slamming = true;
            }
            if (Input.GetKey(KeyCode.LeftControl) && !grounded)
            {
                rb.AddForce(Vector3.down*80f*Time.deltaTime, ForceMode.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && !grounded)
            {
                dasher = true;
                dashCount++;
            }
            if (Input.GetKeyDown(KeyCode.Space) && !grounded && dashCount > 0 && dashing && jumpReady)
            {
                dasher = false;
                rb.AddRelativeForce(Vector3.up*jumpForce, ForceMode.Impulse);
                dashCount++;
                jumpReady = false;
            }
            if (grounded)
            {
                jumpReady = true;
            }
        }
    }

    void UpdateReload(int weaponChosen)
    {
        fireRateInitial = reloadTimes[weaponChosen];
        fireRate = fireRateInitial;
    }

    void Shooting()
    {
        if (!respawning)
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
                randomRange = UnityEngine.Random.Range(-0.0525f, 0.0525f);
                automatic = true;
            }
            else if (!projTrails[actualScroll].GetComponent<DamageField>().automatic)
            {
                inputFinder = Input.GetMouseButtonDown(0);
                randomRange = UnityEngine.Random.Range(0f, 0f);
                automatic = false;
                burstFire = 0;
            }
            if (Input.GetMouseButtonUp(0) && automatic && burstFire < updatingBurnOut)
            {
                updatingBurnOut = burstFire;
                burstFire = 0;
                burstCount++;
            }
            if (!automatic || burstFire < maxAutoBurnOut || burstCount < 3)
            {
                burnOutText.SetActive(false);
            }
            if (!automatic)
            {
                updatingBurnOut = maxAutoBurnOut;
                burstCount = 0;
            }
            if (inputFinder && fireRate <= 0 && !dashing)
            {
                if (automatic && (burstFire == updatingBurnOut || burstCount >= 3))
                {
                    burnOutText.SetActive(true);
                }
                else
                {
                    Vector3 startRaycast = transform.position;
                    RaycastHit hit;
                    LayerMask mask = 1<<LayerMask.GetMask("TrailTrigger");
                    if (Physics.Raycast(GameObject.Find("PlayerCam").transform.position, transform.TransformDirection(Vector3.forward + new Vector3 (randomRange * (UnityEngine.Random.Range(-1.0f, 1.0f)), Mathf.Tan(-GameObject.Find("PlayerCam").transform.localEulerAngles.x/180*Mathf.PI) + randomRange * (UnityEngine.Random.Range(-1.0f, 1.0f)))), out hit, Mathf.Infinity, mask))
                    {
                        fireRate = fireRateInitial;
                        Instantiate(projHit[actualScroll], hit.point, Quaternion.identity);
                        projTrails[actualScroll].GetComponent<ProjectileTrail>().SetPosition(hit.point, startRaycast);
                        Damageable damaging = hit.collider.gameObject.GetComponent<Damageable>();
                        if (hit.collider.gameObject.CompareTag("Parry") && (!projTrails[actualScroll].GetComponent<DamageField>().eleCannon && projTrails[actualScroll].GetComponent<DamageField>().playerProj))
                        {
                            hit.collider.gameObject.GetComponent<ParryBlock>().Explode(projTrails[actualScroll].GetComponent<DamageField>().damage * 3f, projTrails[actualScroll].GetComponent<DamageField>().damage * 0.5f);
                        }
                        if (automatic)
                        {
                            burstFire++;
                        }
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
        }
    }

    void Parrying()
    {
        if (currentParry != maxParry)
        {
            parryRegen -= Time.deltaTime;
        }    
        if (parryRegen <= 0)
        {
            currentParry++;
            parryRegen = 5.0f;
        }
        if (currentParry > maxParry)
        {
            currentParry = maxParry;
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentParry > 0 && !dashing)
        {
            GameObject objectSpawned = Instantiate(parryBlock, GameObject.Find("Parry Aim").transform.position - Vector3.up*0.45f, Quaternion.identity);
            Rigidbody parryRb = objectSpawned.GetComponent<Rigidbody>();
            parryRb.AddTorque(Vector3.right*45f);
            parryRb.AddRelativeForce(((GameObject.Find("Parry Aim").transform.position - transform.position)*0.75f + Vector3.up*0.5f) * 10f, ForceMode.Impulse);
            currentParry--;
        }
        GameObject.Find("Parry Slider").GetComponent<WoodMeter>().UpdateValue(currentParry);
    }

    public void Respawn()
    {
        GetComponent<Damageable>().health = 100;
        transform.position = checkpoint.position;
        rb.velocity = Vector3.zero;
        grounded = false;
        respawning = true;
        GetComponent<Damageable>().damageTaken = 0;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ground>() != null)
        {
            grounded = true;
        }
    }
}
