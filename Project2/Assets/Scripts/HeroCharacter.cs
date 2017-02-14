using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacter : MonoBehaviour {

    public Hero HeroType;

    public bool isEnemy = false;

    public int showHealth;
    private int fullHeath;
    private SpriteRenderer spriteShow;
    private Hero myAgent;
    private float timer;
    private Animator myAni;
    private Character[] myEnemy;
    public SpriteRenderer myHealthBar;
    // Use this for initialization
    void Start()
    {
        myAgent = ScriptableObject.Instantiate(HeroType);
        myAni = gameObject.GetComponent<Animator>();
        spriteShow = gameObject.GetComponent<SpriteRenderer>();
        spriteShow.sprite = myAgent.mySprite;
        timer = Random.Range(0, myAgent.atkTime);
        myEnemy = FindObjectsOfType<Character>() as Character[];
        fullHeath = myAgent.health;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > myAgent.atkTime)
        {
            attack();
            timer = 0;
            Debug.Log("Fire");
        }
    }

    public void attack()
    {
        myAni.SetTrigger("atk");
        myEnemy[Random.Range(0,myEnemy.Length-1)].getHit(myAgent.atk);
    }

    public void getHit(int atkAmt)
    {
        myAgent.health -= atkAmt;
        if (myAgent.health <= 0)
        {
            myAgent.health = fullHeath;
        }
        showHealth = myAgent.health;
        myHealthBar.transform.localScale = new Vector3 ((float)myAgent.health / (float)fullHeath,1,1);
    }
}