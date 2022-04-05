using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conversa.Runtime;


public class DialogueTrigger : MonoBehaviour
{
	public Conversation conversation;
	public BeHerAlly.AvatarActor[] actors;
	public GameObject NarrationPanel;
	public GameObject DialoguePanel;
	public GameObject ChoicesPanel;
	public GameObject KnockButton;

	public static float cmStats;

	//public GameObject InspectorPanel;

	//Sound
	public AudioSource KnockDoor;


	

	public void Strat()
	{
		DialoguePanel.SetActive(false);
		ChoicesPanel.SetActive(false);
		//InspectorPanel.SetActive(false);
	}

	public void StartDialogue()
	{
		FindObjectOfType<DialogueManager>().OpenDialogue(conversation, actors);
	}

	public void ArriveAtPDButtonClicked()
	{
		//OpenDoorSound = GetComponent<AudioSource>();
		//OpenDoorSound.Play();
		NarrationPanel.SetActive(false);
		//InspectorPanel.SetActive(true);
		StartDialogue();
	}

    public void KnockButtonClicked()
	{
		KnockButton.SetActive(false);
		KnockDoor = GetComponent<AudioSource>();
		KnockDoor.Play();
		//InspectorPanel.SetActive(true);
		StartDialogue();
	}

}


[System.Serializable]
public class Message
{
	public int actorId;
	public string message;
}

//namespace BeHerAlly
//{
//	[CreateAssetMenu(fileName = "Actor", menuName = "BeHerAlly/Actor", order = 0)]
//	public class AvatarActor : Actor
//	{
//		public Sprite sprite;
//	}
//}
