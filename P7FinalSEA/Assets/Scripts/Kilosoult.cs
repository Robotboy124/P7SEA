using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kilosoult : MonoBehaviour
{
    public GameObject spark;
    public GameObject dashTarget;
    public GameObject phaseDash;
    public GameObject dashTrail;
    public GameObject buildUp;
    public GameObject lightningX;
    public GameObject rotatingLightning;
    public GameObject phaseTwoLightning;
    public GameObject bossModel;
    Animator bossAnim;
    public Transform playerCam;
    public Transform roof;
    int phaseMulti = 1;
    bool teleporting = true;
    bool souled = false;
    public float teleportTimer;
    public float attackTimer;
    public int soulTimer = 0;
    int xLightningTimer;
    Light lighting;
    Damageable damagin;

    // Start is called before the first frame update
    void Start()
    {
        damagin = GetComponent<Damageable>();
        StartCoroutine(AttackCoroutine());
        StartCoroutine(TeleportCoroutine());
        lighting = gameObject.GetComponent<Light>();
        bossAnim = bossModel.GetComponent<Animator>();
        bossAnim.SetTrigger("isIdle");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GameObject.Find("PlayerCam").transform.position);
        PhaseCheck();
    }

    void PhaseCheck()
    {
        if(damagin.health <= damagin.initialHealth*0.6f)
        {
            phaseMulti = 2;
            phaseTwoLightning.SetActive(true);
        }
        else
        {
            phaseMulti = 1;
            phaseTwoLightning.SetActive(false);
        }
    }

    IEnumerator AttackCoroutine()
    {
        if (phaseMulti == 1)
        {
            yield return new WaitForSeconds(attackTimer + Random.Range(-0.5f, 0.5f));
        }
        else if (phaseMulti == 2)
        {
            yield return new WaitForSeconds((attackTimer + Random.Range(-0.5f, 0.5f))/phaseMulti*1.5f);
        }
        teleporting = false;
        StopCoroutine(TeleportCoroutine());
        if (soulTimer >= (2*phaseMulti) && GameObject.FindWithTag("Spark") != null)
        {
            StartCoroutine(ExplodeCoroutine());
        }
        else
        {
            if (damagin.health <= damagin.initialHealth *0.15f && GameObject.FindWithTag("Spark") == null)
            {
                StartCoroutine(FranticCoroutine());
            }
            else if (damagin.health > damagin.initialHealth * 0.15f)
            {
                var randomCoroutine = Random.Range(0, 10);
                if (randomCoroutine <= 4)
                {
                    StartCoroutine(XLightningCoroutine());
                }
                else if (randomCoroutine <= 6 && randomCoroutine > 4)
                {
                    StartCoroutine(SoulCoroutine());
                }
                else if (randomCoroutine > 6)
                {
                    StartCoroutine(DashCoroutine());
                }
            }
        }
        StopCoroutine(AttackCoroutine());
    }

    IEnumerator TeleportCoroutine()
    {
        if (teleporting)
        {
            Vector3 randomTeleport = new Vector3(Random.Range(-19f, 19f), Random.Range(1.75f, roof.position.y-1), Random.Range(-19f, 19f));
            GameObject teleportLocation = Instantiate(dashTarget, randomTeleport, Quaternion.identity);
            if (phaseMulti == 1)
            {
                yield return new WaitForSeconds(teleportTimer);
            }
            else if (phaseMulti == 2)
            {
                yield return new WaitForSeconds(teleportTimer/phaseMulti*1.5f);
            }

            transform.position = randomTeleport;
            Destroy(teleportLocation);
            StartCoroutine(TeleportCoroutine());
        }
    }

    IEnumerator XLightningCoroutine()
    {
        bossAnim.ResetTrigger("isIdle");
        bossAnim.SetTrigger("bigAttack");
        yield return new WaitForSeconds(1f);

        for(int i = 0; i<(1*phaseMulti); i++)
        {
            GameObject instantiating = Instantiate(lightningX, new Vector3(Random.Range(-18f, 18f), 0.5f, Random.Range(-18f, 18f)), Quaternion.identity);
            instantiating.GetComponent<InstantiatedAttack>().objectSpawnedThis = gameObject;
            bossAnim.SetTrigger("isIdle");
            bossAnim.ResetTrigger("bigAttack");
        }

        if (souled == true)
        {
            soulTimer += 1;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(AttackCoroutine());
        teleporting = true;
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(XLightningCoroutine());
    }

    IEnumerator SoulCoroutine()
    {
        bossAnim.ResetTrigger("isIdle");
        bossAnim.SetTrigger("sparkAttack");
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 3; i ++)
        {
            Instantiate(spark, transform.position + Vector3.right * Random.Range(-3.0f, 3.0f), Quaternion.identity);
        }
        bossAnim.SetTrigger("isIdle");
        bossAnim.ResetTrigger("sparkAttack");
        yield return new WaitForSeconds(0.75f);
        StartCoroutine(AttackCoroutine());
        teleporting = true;
        souled = true;
        soulTimer += 1;
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(SoulCoroutine());
    }
    
    IEnumerator ExplodeCoroutine()
    {
        soulTimer = 0;
        if (GameObject.FindWithTag("Spark") == null)
        {
            StartCoroutine(AttackCoroutine());
            teleporting = true;
            souled = false;
            StartCoroutine(TeleportCoroutine());
            StopCoroutine(ExplodeCoroutine());
        }
        else
        {
            bossAnim.ResetTrigger("isIdle");
            bossAnim.SetTrigger("sparkAttack");
            yield return new WaitForSeconds(2f);

            GameObject[] sparks = GameObject.FindGameObjectsWithTag("Spark");
            for (int i = 0; i < sparks.Length; i++)
            {
                sparks[i].GetComponent<FollowPlayer>().SparkExplode();
            }
            StartCoroutine(AttackCoroutine());
            teleporting = true;
            souled = false;
            bossAnim.ResetTrigger("sparkAttack");
            bossAnim.SetTrigger("isIdle");
            StartCoroutine(TeleportCoroutine());
            StopCoroutine(ExplodeCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        int phaseCheck = phaseMulti;
        buildUp.SetActive(true);
        lighting.range = 24f;
        lighting.intensity = 8f;
        Vector3[] targets;
        transform.position = new Vector3(transform.position.x, GameObject.Find("Player").transform.position.y, transform.position.z);
        Vector3 raycastInitial = transform.position;
        targets = new Vector3[6];
        if (phaseMulti == 1)
        {
            targets[0] = new Vector3(Random.Range(-19f, 19f), Random.Range(1.75f, roof.position.y-1), Random.Range(-19f, 19f));
            targets[1] = new Vector3(Random.Range(-19f, 19f), Random.Range(1.75f, roof.position.y-1), Random.Range(-19f, 19f));           
            targets[2] = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z);
            targets[3] = targets[2];
            targets[4] = targets[2];
            targets[5] = targets[2];
        }
        else if (phaseMulti == 2)
        {
            targets[0] = new Vector3(Random.Range(-19f, 19f), Random.Range(1.75f, roof.position.y-1), Random.Range(-19f, 19f));
            targets[1] = new Vector3(Random.Range(-19f, 19f), Random.Range(1.75f, roof.position.y-1), Random.Range(-19f, 19f)); 
            targets[2] = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z); 
            targets[3] = new Vector3(Random.Range(-19f, 19f), Random.Range(1.75f, roof.position.y-1), Random.Range(-19f, 19f));
            targets[4] = new Vector3(Random.Range(-19f, 19f), Random.Range(1.75f, roof.position.y-1), Random.Range(-19f, 19f));
            Instantiate(dashTarget, targets[0], Quaternion.identity); 
            Instantiate(dashTarget, targets[1], Quaternion.identity); 
            Instantiate(dashTarget, targets[2], Quaternion.identity); 
            Instantiate(dashTarget, targets[3], Quaternion.identity); 
            Instantiate(dashTarget, targets[4], Quaternion.identity);
        }
        dashTrail.SetActive(true);
        if (phaseMulti == 2)
        {
            phaseDash.SetActive(true);
        }
        yield return new WaitForSeconds(0.75f);
        if (phaseMulti == 2)
        {
            targets[5] = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z);
        }
        for (int i = 0; i < (3*phaseMulti); i++)
        {
            yield return new WaitForSeconds(teleportTimer / 4f);
            transform.position = targets[i];
            RaycastHit hit;
            if (i == 0)
            {
                if (Physics.Raycast(raycastInitial, targets[i]-raycastInitial, out hit, Vector3.Distance(targets[i],raycastInitial)))
                {
                    if (hit.collider.gameObject == GameObject.Find("Player"))
                    {
                        hit.collider.gameObject.GetComponent<Damageable>().Damaged(50);
                    }
                }
            }
            else if (i > 0)
            {
                if (Physics.Raycast(targets[i-1], targets[i]-targets[i-1], out hit, Vector3.Distance(targets[i], targets[i-1])))
                {
                    if (hit.collider.gameObject == GameObject.Find("Player"))
                    {
                        hit.collider.gameObject.GetComponent<Damageable>().Damaged(50);
                    }                    
                }   
            }
        }
        if (souled == true)
        {
            soulTimer += 1;
        }
        yield return new WaitForSeconds(0.5f);
        GameObject[] dashers = GameObject.FindGameObjectsWithTag("DashTarget");
        for (int i = 0; i < dashers.Length; i++)
        {
            Destroy(dashers[i]);
        }
        StartCoroutine(AttackCoroutine());
        teleporting = true;
        lighting.range = 12f;
        lighting.intensity = 4f;
        dashTrail.SetActive(false);
        phaseDash.SetActive(false);
        buildUp.SetActive(false);
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(DashCoroutine());
    }

    IEnumerator FranticCoroutine()
    {
        Vector3 randomTeleport = new Vector3(Random.Range(-19f, 19f), Random.Range(1.75f, roof.position.y - 1), Random.Range(-19f, 19f));
        GameObject teleportLocation = Instantiate(dashTarget, randomTeleport, Quaternion.identity);
        yield return new WaitForSeconds(teleportTimer / 1.5f);
        xLightningTimer += 1;
        transform.position = randomTeleport;
        Destroy(teleportLocation);
        if (xLightningTimer >= 4)
        {
            int randomRange = Random.Range(0, 4);
            if (randomRange < 3)
            {
                Instantiate(lightningX, new Vector3(Random.Range(-19f, 19f), 0.5f, Random.Range(-19f, 19f)), Quaternion.identity);
            }
            else
            {
                Instantiate(rotatingLightning, new Vector3(Random.Range(-19f, 19f), 0.5f, Random.Range(-19f, 19f)), Quaternion.identity);
            }
            xLightningTimer = 0;
        }
        StartCoroutine(FranticCoroutine());
    }
}
