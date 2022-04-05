using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Conversa.Runtime;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using BeHerAlly;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
	public Image actorImage;
	public Text actorName;
	public Text messageText;
	public RectTransform backgroundBox;


    //Stats
	//public StatsUI statsUI;
	//public GameObject StatsPanel;
	////public Button ControlStatsPanel;
	//public Text sNameText;
	//public Text sNumberText;


	[Header("Choice")]
	public GameObject choiceGo;
	public GameObject choiceButtonPrefab;

    public Conversation currentConversation;

	Actor[] currentActors;
	public static bool isActive = false;
	ActorMessageEvent lastMessage;
	bool isChoice = false;

	ConversationRunner runner;

	// Start is called before the first frame update
	void Start()
	{
		//keep
		backgroundBox.transform.localScale = Vector3.zero;
	}

	public void OpenDialogue(Conversation conversation, Actor[] actors)
	{
		currentConversation = conversation;
		//test
		Debug.Log(currentConversation);

		currentActors = actors;
		isActive = true;
		SetupRunner();
		runner.Begin();
		backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
	}

	private void HandleConversationEvent(IConversationEvent e)
	{
		switch (e)
		{
			//case MessageEvent messageEvent:
			//	HandleMessage(messageEvent);
			//	break;
			//case ChoiceEvent choiceEvent:
			//	HandleChoice(choiceEvent);
			//	break;
			case ActorMessageEvent actorMessageEvent:
				DisplayMessage(actorMessageEvent);
				break;
			case ActorChoiceEvent actorChoiceEvent:
				DisplayChoice(actorChoiceEvent);
				break;
			case UserEvent userEvent:
				HandleUserEvent(userEvent);
				break;
			case EndEvent _:
				FinishConversation();
				break;
		}
	}

	void SetupRunner()
	{
		runner = new ConversationRunner(currentConversation);
		runner.OnConversationEvent.AddListener(HandleConversationEvent);
	}

	void DisplayMessage(ActorMessageEvent e)
	{
		isChoice = false;
		lastMessage = e;
		messageText.gameObject.SetActive(true);
		choiceGo.SetActive(false);

		messageText.text = e.Message;

		if (e.Actor is AvatarActor avatarActor)
		{
			actorName.text = avatarActor.DisplayName;
			actorImage.sprite = avatarActor.sprite;
		}

		AnimateTextColor();
	}

	void DisplayChoice(ActorChoiceEvent e)
	{
		isChoice = true;
		messageText.gameObject.SetActive(false);
		choiceGo.SetActive(true);

		foreach (Transform child in choiceGo.transform)
			Destroy(child.gameObject);

		e.Options.ForEach(option => {
			var instance = Instantiate(choiceButtonPrefab, choiceGo.transform);
			instance.GetComponentInChildren<Text>().text = option.Message;
			instance.GetComponent<Button>().onClick.AddListener(() => option.Advance());
		});
	}

	//Display Stats
	//public void DisplayStats(string statsNumber)
	//{
	//	var curstats = currentConversation.GetProperty<float>("BabyWellness");
	//	Debug.Log(curstats);
	//	statsNumber = curstats.ToString();
	//	sNumberText.text = statsNumber;
	//}


	//ChangeStatsEvents
	public void HandleUserEvent(UserEvent e)
	{
		switch (e.Name)
		{
			case "two":
				{
					var BW = runner.GetProperty<float>("One");
					Debug.Log(BW);
					runner.SetProperty<float>("One", BW + 1);
					return;
				}
			//case "BabyWellness-1":
			//	{
			//		var BW = currentConversation.GetProperty<float>("BabyWellness");
			//		currentConversation.SetProperty<float>("BabyWellness", BW - 1);
			//		return;
			//	}
			//case "ChristieMHL+1":
			//	{
			//		var CM = currentConversation.GetProperty<float>("ChristieMHL");
			//		currentConversation.SetProperty<float>("ChristieMHL", CM + 1);
			//		return;
			//	}
			//case "ChristieMHL-1":
			//	{
			//		var CM = currentConversation.GetProperty<float>("ChristieMHL");
			//		currentConversation.SetProperty<float>("ChristieMHL", CM - 1);
			//		return;
			//	}
			//case "JamieMHL+1":
			//	{
			//		var JM = currentConversation.GetProperty<float>("JamieMHL");
			//		currentConversation.SetProperty<float>("JamieMHL", JM + 1);
			//		return;
			//	}
			//case "JamieMHL-1":
			//	{
			//		var JM = currentConversation.GetProperty<float>("JamieMHL");
			//		currentConversation.SetProperty<float>("JamieMHL", JM - 1);
			//		return;
			//	}
			//case "JamieStamina-10":
			//	{
			//		var JS = currentConversation.GetProperty<float>("JamieStamina");
			//		currentConversation.SetProperty<float>("JamieStamina", JS - 10);
			//		return;
			//	}
			//case "JamieSS+1":
			//	{
			//		var SS = currentConversation.GetProperty<float>("JamieSS");
			//		currentConversation.SetProperty<float>("JamieSS", SS + 1);
			//		return;
			//	}
			//case "JamieSS-1":
			//	{
			//		var SS = currentConversation.GetProperty<float>("JamieSS");
			//		currentConversation.SetProperty<float>("JamieSS", SS - 1);
			//		return;
			//	}
		}
	}


	void AnimateTextColor()
	{
		LeanTween.textAlpha(messageText.rectTransform, 0, 0);
		LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
	}

	void FinishConversation()
	{
		Debug.Log("Conversation eneded!");
		backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
		isActive = false;
        if(SceneManager.GetActiveScene().name == "Scene1-3")
		{
			SceneManager.UnloadSceneAsync("Scene1-3");
		}
        else
		{

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

		}
		
	}



	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && isActive == true && isChoice == false)
		{
			lastMessage.Advance();
		}
	}
}
