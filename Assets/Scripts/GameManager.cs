using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int rows = 2;
    public int columns = 3;
    public GameObject cardPrefab;
    public Transform cardParent;
    public Text scoreText;

    

    private int score = 0;
    private List<Card> cards = new List<Card>();
    private Card firstCard, secondCard;
    private bool isFlipping = false;

    void Start() {
        GenerateCards();
        UpdateScore(0);
    }

    void GenerateCards() {
        // Clear existing cards
        foreach (Transform child in cardParent) {
            Destroy(child.gameObject);
        }

        // Create new cards
        List<int> cardValues = GenerateCardValues(rows*columns);
        for (int i = 0; i<cardValues.Count; i++) {
            GameObject cardObject = Instantiate(cardPrefab, cardParent);
            Card card = cardObject.GetComponent<Card>();
            card.SetCardValue(cardValues[i]);
            cards.Add(card);
        }

        AdjustCardLayout();
    }

    void AdjustCardLayout() {
        GridLayoutGroup grid = cardParent.GetComponent<GridLayoutGroup>();
        grid.constraint=GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount=rows;
    }

    List<int> GenerateCardValues(int totalCards) {
        List<int> values = new List<int>();
        for (int i = 0; i<totalCards/2; i++) {
            values.Add(i);
            values.Add(i);
        }

        // Shuffle values
        for (int i = 0; i<values.Count; i++) {
            int randomIndex = Random.Range(0, values.Count);
            (values[i], values[randomIndex])=(values[randomIndex], values[i]);
        }

        return values;
    }

    public void CardClicked(Card clickedCard) {
        if (isFlipping||clickedCard.IsMatched)
            return;

        if (firstCard==null) {
            firstCard=clickedCard;
            firstCard.Flip();
        }
        else {
            secondCard=clickedCard;
            secondCard.Flip();
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch() {
        isFlipping=true;

        yield return new WaitForSeconds(1f);

        if (firstCard.CardValue==secondCard.CardValue) {
            firstCard.SetMatched();
            secondCard.SetMatched();
            UpdateScore(10);
        }
        else {
            firstCard.FlipBack();
            secondCard.FlipBack();
            UpdateScore(-5);
        }

        firstCard=null;
        secondCard=null;
        isFlipping=false;
    }

    void UpdateScore(int points) {
        score+=points;
        scoreText.text="Score: "+score;
    }

    public void SaveProgress() {
        PlayerPrefs.SetInt("Score", score);
    }

    public void LoadProgress() {
        score=PlayerPrefs.GetInt("Score", 0);
        scoreText.text="Score: "+score;
    }
}
