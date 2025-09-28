using TMPro;
using UnityEngine;

public class CoinSystems : MonoBehaviour
{
    public static int coins = 0;
    public TextMeshProUGUI coin_text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coin_text.text = coins.ToString();
    }
    public static void AddCoins()
    {
        coins+= 10;
    }
}
