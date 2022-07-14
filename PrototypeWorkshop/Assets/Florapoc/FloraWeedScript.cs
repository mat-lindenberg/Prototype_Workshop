using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraWeedScript : MonoBehaviour
{
    // Start is called before the first frame update

    public float timer;
    public float timeBetweenGrowthStages;

    public int growthStage;
    public int finalStage;

    public Transform stem;
    public Transform bulb;

    public GameObject seedPrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < timeBetweenGrowthStages)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            timeBetweenGrowthStages = Random.Range(.850f, 1.115f);

            if (growthStage < finalStage)
            {
                growthStage++;

                transform.localScale *= 1.1f;
            }
            else
            {
                Burst();
                growthStage = 0;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("what the fuck");
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.tag == "PlayerBullet")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    public void Burst()
    {
       

        int rando = Random.Range(1, 3);

        for (int i = 0; i < rando; i++)
        {
            GameObject g = Instantiate(seedPrefab);
            g.transform.position = bulb.transform.position + new Vector3(Random.Range(-1f, 1),1, Random.Range(-1, 1));
            Vector3 v = g.transform.position;
            v += new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
            v = v.normalized;
            v.y = 1;
            g.GetComponent<Rigidbody>().AddForce(v * 4f, ForceMode.Impulse);
        }
    }
}
