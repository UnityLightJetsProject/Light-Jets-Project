using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public Canvas exitMenu;
	public Button startText;
	public Button exitText;

	void Start ()
	{
		exitMenu = exitMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		exitMenu.enabled = false;
	}

	public void ExitConfirmationMenu ()
	{
		exitMenu.enabled = true;
		startText.enabled = false;
		exitText.enabled = false;
	}

	public void NoPress ()
	{
		exitMenu.enabled = false;
		startText.enabled = true;
		exitText.enabled = true;
	}

	public void StartGame ()
	{
		SceneManager.LoadScene (1);
	}

	public void QuitGame ()
	{
		Application.Quit ();
	}
}
