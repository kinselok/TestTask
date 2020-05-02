using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Element : MonoBehaviour
{
    public int ID { get { return id; } }

    #region Editor
    [SerializeField]
    private Sprite bluredSprite;
    [SerializeField]
    private int id;
    #endregion

    private Sprite defaultSprite;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        SlotInput.BaseInput.Play += OnMove;
    }

    private void OnMove()
    {
        spriteRenderer.sprite = bluredSprite;
    }

    public void OnMovementStop()
    {
        spriteRenderer.sprite = defaultSprite;
    }
    private void OnDestroy()
    {
        SlotInput.BaseInput.Play -= OnMove;
    }
}

