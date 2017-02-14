using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public Enemies EnemyType;

    public bool isEnemy = false;

    public int showHealth;
    private int fullHeath;
    private SpriteRenderer spriteShow;
    private Enemies myAgent;
    private float timer;
    private Animator myAni;
    private HeroCharacter myHero;
    public SpriteRenderer myHealthBar;
    // Use this for initialization
    void Start () {
        //Very important we instantiate a copy as we do not want to alter the source file
        myAgent = ScriptableObject.Instantiate(EnemyType);
        //Grab all our component
        myAni = gameObject.GetComponent<Animator>();
        spriteShow = gameObject.GetComponent<SpriteRenderer>();
        spriteShow.sprite = myAgent.mySprite;
        timer = Random.Range(0, myAgent.atkTime);
        //Grab hero
        myHero = FindObjectOfType<HeroCharacter>();
        fullHeath = myAgent.health;
    }
	
	// Update is called once per frame
	void Update () {
        //Loop through and if we exceed timer we call and attack and reset
        timer += Time.deltaTime;
		if(timer > myAgent.atkTime)
        {
            attack();
            timer = 0;
            Debug.Log("Fire");
        }
	}

    public void attack()
    {
        //Call atk animation and hit
        myAni.SetTrigger("atk");
        myHero.getHit(myAgent.atk);
    }

    public void getHit(int atkAmt)
    {
        //Take Damage
        myAgent.health -= atkAmt;
        if(myAgent.health<=0)
        {
            myAgent.health = EnemyType.health;
        }
        showHealth = myAgent.health;
        myHealthBar.transform.localScale = new Vector3((float)myAgent.health / (float)fullHeath, 1, 1);
    }
}
