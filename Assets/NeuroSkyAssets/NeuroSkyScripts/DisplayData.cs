                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayData : MonoBehaviour
{
	public Texture2D[] signalIcons;
	public Text medValue;
	public Text medSeconds;
	

	
	
	private int indexSignalIcons = 1;
	private float timeMed = 0;
	private Rigidbody gameBody;

	private float medDur;
	private int medTot;
	
	TGCConnectionController controller;
	
	
	private int poorSignal1;
	//private int attention1;
	public int meditation1;
	
	
	//private float delta;
	
	void Start()
	{
		//playerprefs
		medDur = PlayerPrefs.GetFloat ("medDur");
		medTot = PlayerPrefs.GetInt ("medTot");

		//find and connect controller
		controller = GameObject.Find("NeuroSkyTGCController").GetComponent<TGCConnectionController>();

		controller.Connect ();

		//update signals
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;
		//controller.UpdateAttentionEvent += OnUpdateAttention;
		controller.UpdateMeditationEvent += OnUpdateMeditation;
		
		//controller.UpdateDeltaEvent += OnUpdateDelta;
		
	}
	

	
	//assigns signal icon based on value of signal
	void OnUpdatePoorSignal(int value)
	{
		poorSignal1 = value;
		if(value < 25){
			indexSignalIcons = 0;
		}else if(value >= 25 && value < 51){
			indexSignalIcons = 4;
		}else if(value >= 51 && value < 78){
			indexSignalIcons = 3;
		}else if(value >= 78 && value < 107){
			indexSignalIcons = 2;
		}else if(value >= 107){
			indexSignalIcons = 1;
		}
	}
	/*
	void OnUpdateAttention(int value){
		attention1 = value;
	}
	*/
	void OnUpdateMeditation(int value){
		meditation1 = value;
		
	}
	/*
	void OnUpdateDelta(float value){
		delta = value;
	}
	*/
	void Update ()
	{
		//add to time spent
		medDur += Time.deltaTime;

		//meditation requirement at 75
		if ((meditation1 >= 75) )
		{
			//ad to meditation seconds
			timeMed += Time.deltaTime;
			//update text
			medSeconds.text = Mathf.RoundToInt(timeMed).ToString() + "s";
			
		}

		//Update med value
		medValue.text = meditation1.ToString ();
	}
	
	void OnDisable()
	{
		//disconnect controller
		controller.Disconnect ();

		//calculate results
		int results = Mathf.RoundToInt(timeMed);
		GameMaster.instance.AddToAmmo (results);

		//update player preferences
		PlayerPrefs.SetInt ("medTot", medTot + Mathf.RoundToInt(timeMed));
		PlayerPrefs.SetFloat ("medDur", medDur);

	}


	//set up GUI
	void OnGUI()
	{
		GUILayout.Space (50);
		
		GUILayout.BeginHorizontal();

		GUILayout.Space (500);

		//connecction buttons
        if (GUILayout.Button("Connect"))
        {
            controller.Connect();
        }
        if (GUILayout.Button("DisConnect"))
        {
            controller.Disconnect();
			indexSignalIcons = 1;
        }



		//update signal icon
		GUILayout.Space(Screen.width-50);
		GUILayout.Label(signalIcons[indexSignalIcons]);
		GUI.DrawTexture(new Rect(0,200,100,100), signalIcons [indexSignalIcons], ScaleMode.ScaleToFit);
		
		GUILayout.EndHorizontal();



		/*
        GUILayout.Label("PoorSignal1:" + poorSignal1);
        GUILayout.Label("Attention1:" + attention1);
        GUILayout.Label("Meditation1:" + meditation1);
		GUILayout.Label("Delta:" + delta);
		*/
		
	}
}