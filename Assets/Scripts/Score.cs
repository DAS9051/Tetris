using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Score : MonoBehaviour
{
    
    public TMP_Text scoreText;
    

    void Update(){

        scoreText.text = Board.TotalScore.ToString();
        
    }
}
