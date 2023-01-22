using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationAttackScript : MonoBehaviour
{
    private Animator SwordAnim;
    private bool canAttack = true;
    public float AttackCooldown = 1;
    public bool isLightAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
        SwordAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(canAttack)
            {
                LightAttack();
            }
        }
    }   

    public void LightAttack()
    {
        isLightAttacking = true;
        canAttack = false;
        SwordAnim.SetTrigger("lightAttack");
        StartCoroutine(ResetAttackCooldown());
        
    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetLightAttack());
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
    }

    IEnumerator ResetLightAttack()
    {
        yield return new WaitForSeconds(1.05f);
        isLightAttacking = false;

    }
}
