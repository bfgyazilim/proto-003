using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
	//public GameObject testUnits, testMonsters;
	//public GameObject flashbackTimeline, stormTimeline, aicommandTimeline;
	//public GameObject dialoguePanel;

	public GameObject[] timelines;

	void Update ()
	{
		/*
		//Gameplay
		if(Input.GetKeyDown(KeyCode.Alpha0))
		{
			testUnits.SetActive(true);
			testMonsters.SetActive(true);

			flashbackTimeline.SetActive(false);
			stormTimeline.SetActive(false);
			aicommandTimeline.SetActive(false);

			dialoguePanel.SetActive(false);
		}

		//Flashback timeline
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			testUnits.SetActive(false);
			testMonsters.SetActive(false);

			flashbackTimeline.SetActive(true);
			stormTimeline.SetActive(false);
			aicommandTimeline.SetActive(false);

			//dialoguePanel.SetActive(false);
		}

		//Storm Timeline
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			testUnits.SetActive(false);
			testMonsters.SetActive(false);

			flashbackTimeline.SetActive(false);
			stormTimeline.SetActive(true);
			aicommandTimeline.SetActive(false);

			dialoguePanel.SetActive(false);
		}
		*/


		//Timeline 2 Active
		if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Space))
		{
			timelines[1].SetActive(true);
			Debug.Log("Timeline active");
		}

		//Timeline 3 Active
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			timelines[2].SetActive(true);
			Debug.Log("Timeline active");
		}

		//Timeline 4 Active
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			timelines[3].SetActive(true);
			Debug.Log("Timeline active");
		}

		//Timeline 5 Active
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			timelines[4].SetActive(true);
			Debug.Log("Timeline active");
		}

		//Timeline 6 Active
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			timelines[5].SetActive(true);
			Debug.Log("Timeline active");
		}

		//Timeline 7 Active
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			timelines[6].SetActive(true);
			Debug.Log("Timeline active");
		}

		//Timeline 8 Active
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			timelines[7].SetActive(true);
			Debug.Log("Timeline active");
		}

	}
}
