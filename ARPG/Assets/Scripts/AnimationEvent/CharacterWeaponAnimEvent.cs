using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponAnimEvent : MonoBehaviour
{
    protected int equipSWeaponID = Animator.StringToHash("EquipSWeapon");	//指示当前动画是否持着大刀
    
    [SerializeField] private Transform hipGreatSowrd;   //后背大刀
    [SerializeField] private Transform handGreatSowrd;  //手上大刀
    [SerializeField] private Transform hipKatana;  //后背武士刀
    [SerializeField] private Transform handKatana;  //手上武士刀
    private Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    private void ShowGS()
    {
        if (!handGreatSowrd.gameObject.activeSelf)
        {
            handGreatSowrd.gameObject.SetActive(true);
            hipKatana.gameObject.SetActive(true);
            hipGreatSowrd.gameObject.SetActive(false);
            handKatana.gameObject.SetActive(false);
            animator.SetBool(equipSWeaponID,true);
        }

    }
    
    private void HideGS()
    {
        if (handGreatSowrd.gameObject.activeSelf)
        {
            handGreatSowrd.gameObject.SetActive(false);
            hipGreatSowrd.gameObject.SetActive(true);
            handKatana.gameObject.SetActive(true);
            hipKatana.gameObject.SetActive(false);
            animator.SetBool(equipSWeaponID,false);
        }
    }
}
