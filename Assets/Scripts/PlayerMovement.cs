using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;
    private Renderer rend;
    private Color playerColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            // Pick a random color for this player
            playerColor = new Color(Random.value, Random.value, Random.value);

            // Apply locally
            rend.material.color = playerColor;

            // Send to all players (buffered so late joiners also see it)
            photonView.RPC("SetPlayerColor", RpcTarget.AllBuffered, playerColor.r, playerColor.g, playerColor.b);
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + move);
    }

    [PunRPC]
    void SetPlayerColor(float r, float g, float b)
    {
        playerColor = new Color(r, g, b);
        rend.material.color = playerColor;
    }
}
