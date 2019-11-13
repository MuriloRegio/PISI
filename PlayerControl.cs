using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	
	private Camera PlayerCam;			// Camera used by the player
	private GameManager _GameManager; 	// GameObject responsible for the management of the game
	private int _activePlayer;
	private bool _player1AI;
	private bool _player2AI;
	private kwRecognizer input;
	
	// Use this for initialization
	void Start () 
	{
		PlayerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); // Find the Camera's GameObject from its tag 

		_GameManager = gameObject.GetComponent<GameManager>();
		input        = gameObject.GetComponent<kwRecognizer>();

		_player1AI = _GameManager.player1AI;
		_player2AI = _GameManager.player2AI;
		
	}
	
	// Update is called once per frame
	void Update () {
		// Look for Mouse Inputs
		_activePlayer = _GameManager.activePlayer;
		if((_activePlayer == 1 && _player1AI == false) || (_activePlayer == -1 && _player2AI == false))
		{
			//Debug.Log ("Select");
			GetMouseInputs();
			GetVoiceInputs();
		}
	}

	// Detect Mouse Inputs
	void GetVoiceInputs() {
		
		_activePlayer     = _GameManager.activePlayer;
		var speechInput   = input.Buffer;
		
		// Select a piece if the gameState is 0 or 1
		if(_GameManager.gameState < 2)
		{
			if (speechInput.Count > 0){
				Debug.Log("RECEIVED");
				var piece = speechInput.Dequeue();

				// FROM origin
				GameObject target = _GameManager.Pieces[piece.fromRow, piece.fromCol];

				if (target.name.ToLower() != piece.name){
					Debug.Log("SHIET 1");
					Debug.Log(target.name);
					// TODO: Warn the user
					return;
				}

				if (target.tag == _activePlayer.ToString()){
					_GameManager.SelectPiece(target);
				}
				else
					Debug.Log("SHIET 2");

				// TO destination
				GameObject destination = _GameManager.Pieces[piece.toRow, piece.toCol];


				// If the ray hit a cube, move. If it hit a piece of the other player, eat it.
				if(destination == null)
				{
					Vector2 selectedCoord = new Vector2(piece.toRow, piece.toCol);
					_GameManager.MovePiece(selectedCoord);
				}
				else if(destination.tag == ((-1*_activePlayer).ToString()))
				{
					_GameManager.EatPiece(destination);
				}
				else
					Debug.Log("SHIET 3");
			}	
		}
	}
	
	// Detect Mouse Inputs
	void GetMouseInputs()
	{	
		_activePlayer = _GameManager.activePlayer;
		Ray _ray;
		RaycastHit _hitInfo;
		
		// Select a piece if the gameState is 0 or 1
		if(_GameManager.gameState < 2)
		{
			// On Left Click
			if(Input.GetMouseButtonDown(0))
			{
				_ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click
				
				// Raycast and verify that it collided
				if(Physics.Raycast (_ray,out _hitInfo))
				{
					// Select the piece if it has the good Tag
					if(_hitInfo.collider.gameObject.tag == (_activePlayer.ToString()))
					{
						
						_GameManager.SelectPiece(_hitInfo.collider.gameObject);
					}
				}
			}
		}
		
		// Move the piece if the gameState is 1
		if(_GameManager.gameState == 1)
		{
			Vector2 selectedCoord;
			
			// On Left Click
			if(Input.GetMouseButtonDown(0))
			{
				_ray = PlayerCam.ScreenPointToRay(Input.mousePosition); // Specify the ray to be casted from the position of the mouse click
				
				// Raycast and verify that it collided
				if(Physics.Raycast (_ray,out _hitInfo))
				{
					
					// If the ray hit a cube, move. If it hit a piece of the other player, eat it.
					if(_hitInfo.collider.gameObject.tag == ("Cube"))
					{
						selectedCoord = new Vector2(_hitInfo.collider.gameObject.transform.position.x,_hitInfo.collider.gameObject.transform.position.z);
						_GameManager.MovePiece(selectedCoord);
					}
					else if(_hitInfo.collider.gameObject.tag == ((-1*_activePlayer).ToString()))
					{
						_GameManager.EatPiece(_hitInfo.collider.gameObject);
					}
				}
			}	
		}
	}
}
