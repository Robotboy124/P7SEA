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
    public GameObject franticLightning;
    public GameObject phaseTwoLightning;
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
    }

    // Update is called once per frame
    void Update()
    {
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
            if (damagin.health <= damagin.initialHealth *0.15f && soulTimer == 0)
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
            Vector3 randomTeleport = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f));
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
        yield return new WaitForSeconds(1f);

        for(int i = 0; i<(1*phaseMulti); i++)
        {
            Instantiate(lightningX, new Vector3(Random.Range(-18f, 18f), 0.5f, Random.Range(-18f, 18f)), Quaternion.identity);
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
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 3; i ++)
        {
            Instantiate(spark, transform.position + Vector3.right * Random.Range(-3.0f, 3.0f), Quaternion.identity);
        }
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
            yield return new WaitForSeconds(2f);

            GameObject[] sparks = GameObject.FindGameObjectsWithTag("Spark");
            for (int i = 0; i < sparks.Length; i++)
            {
            sparks[i].GetComponent<FollowPlayer>().SparkExplode();
            }
            StartCoroutine(AttackCoroutine());
            teleporting = true;
            souled = false;
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
            int u = Random.Range(1, targets.Length - 3);
            targets[0] = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f));
            targets[1] = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f));           
            targets[2] = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f));
            targets[3] = targets[2];
            targets[4] = targets[2];
            targets[5] = targets[2];
            targets[u-1] = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z);
            Instantiate(dashTarget, targets[u-1], Quaternion.identity);
            Instantiate(dashTarget, targets[0], Quaternion.identity);
            Instantiate(dashTarget, targets[1], Quaternion.identity);
            Instantiate(dashTarget, targets[2], Quaternion.identity);
        }
        else if (phaseMulti == 2)
        {
            int u = Random.Range(1, targets.Length);
            targets[0] = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f));
            targets[1] = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f)); 
            targets[2] = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f)); 
            targets[3] = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f));
            targets[4] = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f));
            targets[5] = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f));
            targets[u-1] = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, GameObject.Find("Player").transform.position.z);
            Instantiate(dashTarget, targets[u-1], Quaternion.identity);
            Instantiate(dashTarget, targets[0], Quaternion.identity); 
            Instantiate(dashTarget, targets[1], Quaternion.identity); 
            Instantiate(dashTarget, targets[2], Quaternion.identity); 
            Instantiate(dashTarget, targets[3], Quaternion.identity); 
            Instantiate(dashTarget, targets[4], Quaternion.identity); 
            Instantiate(dashTarget, targets[5], Quaternion.identity); 
        }
        dashTrail.SetActive(true);
        if (phaseMulti == 2)
        {
            phaseDash.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < (3*phaseMulti); i++)
        {
            yield return new WaitForSeconds(teleportTimer / 3f);
            transform.position = targets[i];
            RaycastHit hit;
            if (i == 0)
            {
                if (Physics.Raycast(raycastInitial, targets[i]-raycastInitial, out hit, Vector3.Distance(targets[i],raycastInitial)))
                {
                    if (hit.collider.gameObject == GameObject.Find("Player"))
                    {
                        hit.collider.gameObject.GetComponent<Damageable>().Damaged(40);
                    }
                }
            }
            else if (i > 0)
            {
                if (Physics.Raycast(targets[i-1], targets[i]-targets[i-1], out hit, Vector3.Distance(targets[i], targets[i-1])))
                {
                    if (hit.collider.gameObject == GameObject.Find("Player"))
                    {
                        hit.collider.gameObject.GetComponent<Damageable>().Damaged(40);
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
        Destroy(dashers[0]);
        Destroy(dashers[1]);
        Destroy(dashers[2]);
        if (phaseMulti == 2 && phaseCheck == 2)
        {
            Destroy(dashers[3]);
            Destroy(dashers[4]);
            Destroy(dashers[5]);
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
<<<<<<< Updated upstream
        if (damagin.health > damagin.initialHealth * 0.15f)
        {
            StartCoroutine(AttackCoroutine());
            teleporting = true;
            StartCoroutine(TeleportCoroutine());
            StopCoroutine(FranticCoroutine());
        }
        else
        {
            Vector3 randomTeleport = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y - 1), Random.Range(-19f, 19f));
            GameObject teleportLocation = Instantiate(dashTarget, randomTeleport, Quaternion.identity);
            yield return new WaitForSeconds(teleportTimer / 1.5f);
            xLightningTimer += 1;
            transform.position = randomTeleport;
            Destroy(teleportLocation);
            if (xLightningTimer >= 4)
            {
                Instantiate(lightningX, new Vector3(Random.Range(-19f, 19f), 0.5f, Random.Range(-19f, 19f)), Quaternion.identity);
=======
        if (damagin.health > damagin.initialHealth*0.15f)
        {
            StartCoroutine(AttackCoroutine());
        }
        else
        {
            Vector3 randomTeleport = new Vector3(Random.Range(-19f, 19f), Random.Range(1f, roof.position.y-1), Random.Range(-19f, 19f));
            GameObject teleportLocation = Instantiate(dashTarget, randomTeleport, Quaternion.identity);
            yield return new WaitForSeconds(teleportTimer/1.5f);
            xLightningTimer += 1;
            transform.position = randomTeleport;
            Destroy(teleportLocation);
            if (xLightningTimer >= 2)
            {
                Instantiate(lightningX, new Vector3 (Random.Range(-19f, 19f), 0.5f, Random.Range(-19f, 19f)), Quaternion.identity);
>>>>>>> Stashed changes
                xLightningTimer = 0;
            }
            StartCoroutine(FranticCoroutine());
        }
    }
}
