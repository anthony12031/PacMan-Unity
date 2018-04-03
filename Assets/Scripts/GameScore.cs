using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameScore : MonoBehaviour {

	public static int puntaje = 0;
	public Text puntajeT;
	public  Text playerId;
	public Text playerTopPuntaje;
	public Text[] topPuntajes;
	static bool isGameOver = false;
	public Button playAB;

	[System.Serializable]
	public class Puntaje{
		public int valor; 
		public string playerId;
		public Puntaje[] puntajes;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(getTopPuntajes());
		if (PlayerPrefs.GetString ("playerId").Equals (""))
			StartCoroutine (getUniqueId ());
		else {
			playerId.text = PlayerPrefs.GetString ("playerId");
			StartCoroutine (getPuntaje ());
		}
			

	}

	public static void gameOver(){
		Debug.Log ("game over");
		isGameOver = true;
	}



	IEnumerator almacenarPuntaje() {
		WWWForm form = new WWWForm();
		form.AddField("puntaje", puntaje);
		UnityWebRequest www = UnityWebRequest.Post("http://localhost:8181/"+playerId.text+"/puntaje", form);
		yield return www.Send();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Form upload complete!");
		}
	}

	IEnumerator modificarPuntaje() {
		WWWForm form = new WWWForm();
		form.AddField("puntaje", puntaje);
		UnityWebRequest www = UnityWebRequest.Put("http://localhost:8181/"+playerId.text+"/puntaje", puntaje+"");
		yield return www.Send();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Form upload complete!");
		}
	}

	IEnumerator BorrarPuntaje() {
		WWWForm form = new WWWForm();
		form.AddField("puntaje", puntaje);
		UnityWebRequest www = UnityWebRequest.Delete("http://localhost:8181/"+playerId.text+"/puntaje");
		yield return www.Send();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Form upload complete!");
		}
	}

	IEnumerator getTopPuntajes()
	{
		UnityWebRequest www = UnityWebRequest.Get ("http://localhost:8181/puntaje");
		yield return www.Send();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log (www.downloadHandler.text);
			string response = www.downloadHandler.text;
			Puntaje p = JsonUtility.FromJson<Puntaje>(response);
			for(int i=0;i<p.puntajes.Length;i++){
				Debug.Log (p.puntajes[i].valor);
				topPuntajes[i].text = p.puntajes[i].playerId+":   "+ p.puntajes[i].valor+""; 
			}
		}
	}

	IEnumerator getUniqueId()
	{
		UnityWebRequest www = UnityWebRequest.Get ("http://localhost:8181/uniqueId");
		yield return www.Send();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log (www.downloadHandler.text);
			string response = www.downloadHandler.text;
			PlayerPrefs.SetString("playerId", response);
			playerId.text = response;
			StartCoroutine (getPuntaje ());
		}
	}

	IEnumerator getPuntaje()
	{
		UnityWebRequest www = UnityWebRequest.Get ("http://localhost:8181/"+playerId.text+"/puntaje");
		yield return www.Send();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log (www.downloadHandler.text);
			string response = www.downloadHandler.text;
			playerTopPuntaje.text = response;
		}
	}

	public void callBorrarPuntaje(){
		playerTopPuntaje.text = "-";
		StartCoroutine(BorrarPuntaje());
	}

	public void callCambiarPlayerId(){
		StartCoroutine(getUniqueId());
	}
	
	// Update is called once per frame
	void Update () {
		puntajeT.text = puntaje + "";
		if (isGameOver) {
			//reproducir sonido de muerte de pacman
			GetComponent<AudioSource> ().Play ();
			//almacenar puntaje en ell servidor
			if (playerTopPuntaje.text.Equals ("-"))
				//Debug.Log ("post puntaje");
				StartCoroutine (almacenarPuntaje ());
			else {
				//Debug.Log ("put puntaje");
				if (puntaje > int.Parse (playerTopPuntaje.text))
					StartCoroutine (modificarPuntaje ());
			}
				
				
			isGameOver = false;
			playAB.gameObject.SetActive (true);
		}
	}

	public void playAgain(){
		puntaje = 0;
		SceneManager.LoadScene( SceneManager.GetActiveScene().name );
	}

}
