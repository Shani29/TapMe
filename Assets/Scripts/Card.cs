using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int CardValue { get; private set; }
    public bool IsMatched { get; private set; }

    [Header("Card Components")]
    public Image frontImage;
    public Image backImage;

    private bool isFlipped = false;

  
    public void SetCardValue(int value) {
        CardValue=value;
        frontImage.sprite=GetCardSprite(value); // Assuming you have a method for sprites
        IsMatched=false;
    }

    public void Flip() {
        isFlipped=true;
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
    }

    public void FlipBack() {
        isFlipped=false;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
    }

    public void SetMatched() {
        IsMatched=true;
    }

    private void OnMouseDown() {
        if (!IsMatched&&!isFlipped) {
            FindObjectOfType<GameManager>().CardClicked(this);
        }
    }

   
    private Sprite GetCardSprite(int value) {
        return Resources.Load<Sprite>($"Sprites/Cards{value}");
    }
}
