using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
	public static DatabaseManager i;

	public InputField _name;

	public InputField _email;

	public Text databaseName;

	public string userID;

	public DatabaseReference dbReference;

	private FirebaseApp app;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void CreateNewUser()
	{
	}

	private void HandleValueChanged(object sender, ValueChangedEventArgs args)
	{
	}
}
