using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemBox : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemNum;
        
        public void Initalize(Sprite sprite, int num)
        {
            itemImage.sprite = sprite;
            itemNum.text = (num + 1).ToString();
        }
    }
}
