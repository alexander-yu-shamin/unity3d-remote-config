using System;
using Unity.RemoteConfig;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TestConfig : MonoBehaviour
{
	Text Text;


	public struct userAttributes
	{
		// Optionally declare variables for any custom user attributes; if none keep an empty struct:
		public bool expansionFlag;
	}

	public struct appAttributes
	{
		// Optionally declare variables for any custom app attributes; if none keep an empty struct:
		public int level;
		public int score;
		public string appVersion;
	}

	// Start is called before the first frame update
	void Start()
	{
		Text = GetComponent<Text>();
		Text.text = "Start";
		//ConfigManager.SetCustomUserID("some-user-id");
		ConfigManager.FetchCompleted += ApplyRemoteSettings;
		//UpdateSettings();

	}

	[ContextMenu("UpdateSettings")]
	void UpdateSettings()
	{
		// Fetch configuration setting from the remote service: 
		ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());

	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			UpdateSettings();
		}
	}

	void ApplyRemoteSettings(ConfigResponse configResponse)
	{
		// Conditionally update settings, depending on the response's origin:
		switch (configResponse.requestOrigin)
		{
			case ConfigOrigin.Default:
				Debug.Log("No settings loaded this session; using default values.");
				break;
			case ConfigOrigin.Cached:
				Debug.Log("No settings loaded this session; using cached values from a previous session.");
				break;
			case ConfigOrigin.Remote:
				Debug.Log("New settings loaded this session; update values accordingly.");
				Text.text = ConfigManager.appConfig.GetString("ui_string", "nothing");
			break;
		}
	}

	private void OnDestroy()
	{
		ConfigManager.FetchCompleted -= ApplyRemoteSettings;
	}

}
