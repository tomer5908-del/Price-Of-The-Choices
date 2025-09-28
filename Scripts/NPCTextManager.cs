using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCTextManager : MonoBehaviour
{
    private GameObject king;
    private float range = 3f;
    private GameObject text;
    private GameObject message_box;
    private TextMeshProUGUI inner_text;
    private bool seen_before = false;
    public GameObject decision_1_box;
    public GameObject decision_2_box;
    public TextMeshProUGUI decision_1;
    public TextMeshProUGUI decision_2;
    public TextMeshProUGUI decision_1_bravery;
    public TextMeshProUGUI decision_2_bravery;
    public TextMeshProUGUI decision_1_loyalty;
    public TextMeshProUGUI decision_2_loyalty;
    public TextMeshProUGUI decision_1_helpfulness;
    public TextMeshProUGUI decision_2_helpfulness;
    public NPCDialog decision_p;
    private bool decided = false;
    private List<string> original_messages;
    private int currentMessageIndex = 0; // Track which message is being shown
    private int latest_selection = 1;

    private void Awake()
    {
        text = transform.Find("tc").Find("t").gameObject;
        message_box = transform.Find("tc").Find("Image").gameObject;
        inner_text = message_box.transform.Find("it").gameObject.GetComponent<TextMeshProUGUI>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        king = Movement.king_public;
        message_box.SetActive(false);
        decision_1_box.SetActive(false);
        decision_2_box.SetActive(false);
        original_messages = new List<string>(decision_p.message_box_texts);
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckNearPlayers() && !seen_before)
        {
            text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && !seen_before)
            {
                seen_before = true;
                Destroy(text);
                StartCoroutine(StartMessageBoxAnimation());
            }
        }
        else if (!seen_before) text.SetActive(false);
        
    }
    private IEnumerator StartMessageBoxAnimation()
    {
        // Show messages in sequence
        while (currentMessageIndex < original_messages.Count)
        {
            string message = original_messages[currentMessageIndex];
            float typingSpeed = 0.05f;

            // Show the message box
            message_box.SetActive(true);
            inner_text.text = "";  // Clear previous message

            // Animate typing effect for the message
            yield return new WaitForSeconds(0.5f);  // Optional delay before typing starts
            for (int i = 0; i < message.Length; i++)
            {
                inner_text.text = message.Substring(0, i + 1); // Show the message one character at a time
                yield return new WaitForSeconds(typingSpeed);  // Wait before showing the next character
            }

            // Show decision options only after the first message
            if (currentMessageIndex == 0) // First message only triggers decisions
            {
                decision_1_box.SetActive(true);
                decision_2_box.SetActive(true);
                PrepareDecisions();
                yield return new WaitForSeconds(0.5f);  // Wait briefly for the player to see the options
            }

            // Wait for the player to decide
            yield return new WaitUntil(() => decided);

            // After decision is made, hide decision boxes and move to the next message
            decision_1_box.SetActive(false);
            decision_2_box.SetActive(false);

            // Based on the decision, decide which next message to show
            if (decided)
            {
                // If decision is odd (1), show odd indexed messages (1st, 3rd, 5th...)
                // If decision is even (2), show even indexed messages (2nd, 4th, 6th...)
                if (decided && currentMessageIndex % 2 == 0)  // Odd indexed message (1st, 3rd, etc.)
                {
                    if (currentMessageIndex + 2 < original_messages.Count)  // Check if there's an odd message to show
                    {
                        currentMessageIndex += latest_selection;  // Skip to the next odd index message
                    }
                }
                else if (decided)  // Even indexed message (2nd, 4th, etc.)
                {
                    if (currentMessageIndex + 2 < original_messages.Count)  // Check if there's an even message to show
                    {
                        currentMessageIndex += latest_selection;  // Skip to the next even index message
                    }
                }

                decided = false;  // Reset the decision flag for next loop
            }

            // Optional delay before showing next message
            yield return new WaitForSeconds(1f);
        }

        // Destroy the message box after all messages are shown
        Destroy(message_box);
    }





    public void MakeDecision(int id)
    {
        latest_selection = id;
        // Hide decision boxes once decision is made
        decision_1_box.SetActive(false);
        decision_2_box.SetActive(false);
        decided = true;  // Set flag that decision is made

        // Apply the effects of the decision
        if (id == 1)
        {
            Movement.parameters.bravery += decision_p.decision_1_bravery_effect[0];
            Movement.parameters.loyalty += decision_p.decision_1_loyalty_effect[0];
            Movement.parameters.helpfulness += decision_p.decision_1_helpfulness_effect[0];
        }
        else if (id == 2)
        {
            Movement.parameters.bravery += decision_p.decision_2_bravery_effect[0];
            Movement.parameters.loyalty += decision_p.decision_2_loyalty_effect[0];
            Movement.parameters.helpfulness += decision_p.decision_2_helpfulness_effect[0];
        }
    }




    public void PrepareDecisions()
    {
        // Decision 1 Text
        decision_1.text = decision_p.decision_1_text[0];  // Display the first text option for Decision 1

        // Bravery effect for decision 1
        decision_1_bravery.text = "Bravery: " + decision_p.decision_1_bravery_effect[0].ToString();
        if (decision_p.decision_1_bravery_effect[0] > 0)
        {
            decision_1_bravery.color = Color.green;  // Positive effect color
        }
        else if (decision_p.decision_1_bravery_effect[0] < 0)
        {
            decision_1_bravery.color = Color.red;  // Negative effect color
        }
        else
        {
            decision_1_bravery.color = Color.white;  // Neutral color for no effect
        }

        // Loyalty effect for decision 1
        decision_1_loyalty.text = "Loyalty: " + decision_p.decision_1_loyalty_effect[0].ToString();
        if (decision_p.decision_1_loyalty_effect[0] > 0)
        {
            decision_1_loyalty.color = Color.green;  // Positive effect color
        }
        else if (decision_p.decision_1_loyalty_effect[0] < 0)
        {
            decision_1_loyalty.color = Color.red;  // Negative effect color
        }
        else
        {
            decision_1_loyalty.color = Color.white;  // Neutral color for no effect
        }

        // Helpfulness effect for decision 1
        decision_1_helpfulness.text = "Helpfulness: " + decision_p.decision_1_helpfulness_effect[0].ToString();
        if (decision_p.decision_1_helpfulness_effect[0] > 0)
        {
            decision_1_helpfulness.color = Color.green;  // Positive effect color
        }
        else if (decision_p.decision_1_helpfulness_effect[0] < 0)
        {
            decision_1_helpfulness.color = Color.red;  // Negative effect color
        }
        else
        {
            decision_1_helpfulness.color = Color.white;  // Neutral color for no effect
        }

        // Decision 2 Text
        decision_2.text = decision_p.decision_2_text[0];  // Display the first text option for Decision 2

        // Bravery effect for decision 2
        decision_2_bravery.text = "Bravery: " + decision_p.decision_2_bravery_effect[0].ToString();
        if (decision_p.decision_2_bravery_effect[0] > 0)
        {
            decision_2_bravery.color = Color.green;  // Positive effect color
        }
        else if (decision_p.decision_2_bravery_effect[0] < 0)
        {
            decision_2_bravery.color = Color.red;  // Negative effect color
        }
        else
        {
            decision_2_bravery.color = Color.white;  // Neutral color for no effect
        }

        // Loyalty effect for decision 2
        decision_2_loyalty.text = "Loyalty: " + decision_p.decision_2_loyalty_effect[0].ToString();
        if (decision_p.decision_2_loyalty_effect[0] > 0)
        {
            decision_2_loyalty.color = Color.green;  // Positive effect color
        }
        else if (decision_p.decision_2_loyalty_effect[0] < 0)
        {
            decision_2_loyalty.color = Color.red;  // Negative effect color
        }
        else
        {
            decision_2_loyalty.color = Color.white;  // Neutral color for no effect
        }

        // Helpfulness effect for decision 2
        decision_2_helpfulness.text = "Helpfulness: " + decision_p.decision_2_helpfulness_effect[0].ToString();
        if (decision_p.decision_2_helpfulness_effect[0] > 0)
        {
            decision_2_helpfulness.color = Color.green;  // Positive effect color
        }
        else if (decision_p.decision_2_helpfulness_effect[0] < 0)
        {
            decision_2_helpfulness.color = Color.red;  // Negative effect color
        }
        else
        {
            decision_2_helpfulness.color = Color.white;  // Neutral color for no effect
        }
    }



    private bool CheckNearPlayers()
    {
        float distance = Vector2.Distance(transform.position, king.transform.position);
        if (distance < range) return true;
        else return false;
    }
}
