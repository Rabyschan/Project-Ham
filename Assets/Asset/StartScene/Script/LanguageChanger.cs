using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject[] english_text;
    [SerializeField] private GameObject[] korean_text;

    [SerializeField] private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown.onValueChanged.AddListener(OnDropdownEvent);
    }
    public void OnDropdownEvent(int index)
    {
        if (index == 0)
        {
            for (int i = 0; i < english_text.Length; i++) english_text[i].SetActive(true);
            for (int i = 0; i < korean_text.Length; i++) korean_text[i].SetActive(false);
        }
        if (index == 1)
        {
            for (int i = 0; i < english_text.Length; i++) english_text[i].SetActive(false);
            for (int i = 0; i < korean_text.Length; i++) korean_text[i].SetActive(true);
        }
    }
    
}
