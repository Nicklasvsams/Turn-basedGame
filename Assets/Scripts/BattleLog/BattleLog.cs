using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour
{
    [SerializeField]
    private int maxMessages = 50;

    [SerializeField]
    private GameObject battleLog, textObject;

    [SerializeField]
    private List<Message> messageList = new List<Message>();

    public void SendMessageToChat(string text)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        if (text.Contains("Player"))
        {
            // 15
            var indexOfPlayer = text.IndexOf("Player");

            Debug.Log("Pindex: " + indexOfPlayer + " - TLength: " + text.Length);
            Debug.Log(text);

            // 34
            string tempText;


            if(indexOfPlayer == 0)
            {
                tempText = text.Substring(indexOfPlayer + 6, text.Length - "Player".Length);

                tempText = tempText.Insert(0, "You"); 
            }
            else
            {
                Debug.Log(text.Length - (indexOfPlayer + 6) - text.Substring(0, indexOfPlayer).Length);
                tempText = string.Format("{0}{1}", text.Substring(0, indexOfPlayer), text.Substring(indexOfPlayer+6));

                tempText = tempText.Insert(indexOfPlayer, "you");
            }

            text = tempText;
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, battleLog.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
