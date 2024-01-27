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
    public GameObject groundLightning;
    public GameObject slamIndicate;
    public GameObject slamLightning;
    public GameObject bossModel;
    Animator bossAnim;
    public Transform playerCam;
    public Transform roof;
    int phaseMulti = 1;
    bool teleporting = true;
    bool souled = false;
    bool flyUp = false;
    public float teleportTimer;
    public float attackTimer;
    public int soulTimer = 0;
    int xLightningTimer;
    float teleportDivisor = 1f;
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

        if (flyUp)
        {
            transform.Translate(Vector3.up * 84 * Time.deltaTime, Space.World);
        }
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
            if (damagin.health <= damagin.initialHealth *0.2f)
            {
                StartCoroutine(FranticCoroutine());
            }
            else if (damagin.health > damagin.initialHealth * 0.2f)
            {
                var randomCoroutine = Random.Range(0, 10);
                if ((randomCoroutine <= 5 && phaseMulti == 1) || (randomCoroutine <= 4 && phaseMulti == 2))
                {
                    StartCoroutine(XLightningCoroutine());
                }
                else if ((randomCoroutine <= 7 && randomCoroutine > 5 && phaseMulti == 1))
                {
                    StartCoroutine(SoulCoroutine());
                }
                else if ((randomCoroutine > 4 && randomCoroutine <= 7 && phaseMulti == 2) || (randomCoroutine > 7 && phaseMulti == 1))
                {
                    StartCoroutine(DashCoroutine());
                }
                else if (randomCoroutine > 7 || phaseMulti == 2)
                {
                    StartCoroutine(SlamCoroutine());
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
        yield return new WaitForSeconds(1f / (Mathf.Pow(phaseMulti, 0.85f)));

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
        yield return new WaitForSeconds(1f / (Mathf.Pow(phaseMulti, 0.75f)));
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

    IEnumerator SlamCoroutine()
    {
        buildUp.SetActive(true);
        lighting.range = 24f;
        lighting.intensity = 8f;
        lighting.color = new Color((14f/255f), (14f/255f), 204f / 255f, 1f);
        yield return new WaitForSeconds(0.5f);
        Instantiate(slamIndicate, transform.position, Quaternion.identity);
        flyUp = true;
        yield return new WaitForSeconds(2.75f);
        Vector3 summonPos = GameObject.Find("Player").transform.position - Vector3.up * GameObject.Find("Player").transform.position.y;
        transform.position = summonPos - Vector3.up * 10f;
        yield return new WaitForSeconds((1f)/(4f));
        flyUp = false;
        Instantiate(slamLightning, summonPos, Quaternion.identity);
        lighting.range = 12f;
        lighting.intensity = 4f;
        lighting.color = new Color(253f / 255f, 229f / 255f, 8f / 255f, 255f / 255f);
        yield return new WaitForSeconds(0.75f);
        teleporting = true;
        StartCoroutine(AttackCoroutine());
        buildUp.SetActive(false);
        StartCoroutine(TeleportCoroutine());
        StopCoroutine(SlamCoroutine());
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
        yield return new WaitForSeconds(0.8f / Mathf.Pow(phaseMulti, 1.2f));
        if (phaseMulti == 2)
        {
            targets[5] = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z);
        }
        for (int i = 0; i < (3*phaseMulti); i++)
        {
            yield return new WaitForSeconds(teleportTimer / 5.5f);
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
        yield return new WaitForSeconds(0.5f / (Mathf.Pow(phaseMulti, 0.75f)));
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
        if (damagin.health >= damagin.initialHealth * 0.2f)
        {
            StartCoroutine(AttackCoroutine());
            StartCoroutine(TeleportCoroutine());
            teleportDivisor = 1;
            phaseMulti = 1;
            StopCoroutine(FranticCoroutine());
        }
        else
        {
            teleportDivisor += 0.001f;
            Vector3 randomTeleport = new Vector3(Random.Range(-19f, 19f), Random.Range(1.75f, roof.position.y - 1), Random.Range(-19f, 19f));
            GameObject teleportLocation = Instantiate(dashTarget, randomTeleport, Quaternion.identity);
            yield return new WaitForSeconds(teleportTimer / (teleportDivisor * 1.4f));
            xLightningTimer += 1;
            transform.position = randomTeleport;
            Destroy(teleportLocation);
            if (xLightningTimer >= 3)
            {
                int randomRange = Random.Range(0, 9);
                if (randomRange < 7)
                {
                    Instantiate(lightningX, new Vector3(Random.Range(-19f, 19f), 0.5f, Random.Range(-19f, 19f)), Quaternion.identity);
                }
                else if (randomRange == 7)
                {
                    Instantiate(rotatingLightning, new Vector3(Random.Range(-19f, 19f), 0.5f, Random.Range(-19f, 19f)), Quaternion.identity);
                }
                else if (randomRange > 7)
                {
                    GameObject instantiatedBigBolt = Instantiate(slamLightning, GameObject.Find("Player").transform.position - Vector3.up * GameObject.Find("Player").transform.position.y, Quaternion.identity);
                    instantiatedBigBolt.GetComponent<LightningSummon>().lightningTimer = 1.0f;
                }
                xLightningTimer = 0;
            }
            StartCoroutine(FranticCoroutine());
        }
    }
}
