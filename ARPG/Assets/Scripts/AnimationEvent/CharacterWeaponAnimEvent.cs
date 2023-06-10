using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponAnimEvent : MonoBehaviour
{
    [SerializeField] private Transform hipGreatSowrd;   //后背大刀
    [SerializeField] private Transform handGreatSowrd;  //手上大刀
    [SerializeField] private Transform hipKatana;  //后背武士刀
    [SerializeField] private Transform handKatana;  //手上武士刀

    private void ShowGS()
    {
        if (!handGreatSowrd.gameObject.activeSelf)
        {
            handGreatSowrd.gameObject.SetActive(true);
            hipKatana.gameObject.SetActive(true);
            hipGreatSowrd.gameObject.SetActive(false);
            handKatana.gameObject.SetActive(false);
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
            
        }
    }
}
