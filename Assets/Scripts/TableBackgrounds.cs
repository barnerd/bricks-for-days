using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBackgrounds : MonoBehaviour
{
    public Sprite[] sprites;

    private void Start()
    {
        SetBackground();
    }

    public void SetRandomBackground()
    {
        SetBackground(sprites[Random.Range(0, sprites.Length)]);
    }

    public void SetBackground(Sprite _sprite = null)
    {
        if (_sprite == null)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = _sprite;
        }
    }
}
