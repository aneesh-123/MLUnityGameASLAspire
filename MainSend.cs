using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Threading.Tasks; 
using UnityEngine.EventSystems;

public class MainSend : MonoBehaviour
{
    public ServerConnection serverConnection;
    public RawImage rawImage; // Reference to the RawImage component to display the image
    private WebCamTexture webcamTexture;
    private bool isPlaying = false;

    public GameObject canvasObject; // Reference to the GameObject containing the canvas
    public Text dataText; // Reference to the Text component on the canvas


    private void Start()
    {        
        // Find the Text component within the children of the canvasObject
        dataText = canvasObject.GetComponentInChildren<Text>();

        // Get available webcam devices
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            // Use the first available webcam device
            webcamTexture = new WebCamTexture(devices[0].name);

            // Start the webcam texture
            isPlaying = true;
            webcamTexture.Play();      

            // Start sending webcam frames to the server continuously
            StartCoroutine(SendWebcamFramesCoroutine());
            
        }
    }

    private System.Collections.IEnumerator SendWebcamFramesCoroutine()
    {
        while (isPlaying)
        {
            yield return new WaitForSeconds(0.5f);

            // Get the RawImage component attached to the game object
            Material material = new Material(Shader.Find("Sprites/Default"));
            material.mainTexture = webcamTexture;
            GetComponent<MeshRenderer>().material = material;

            // Capture the current frame from the webcam texture
            Texture2D texture2D = new Texture2D(webcamTexture.width, webcamTexture.height);
            texture2D.SetPixels(webcamTexture.GetPixels());
            texture2D.Apply();

            // Encode the Texture2D to JPG
            byte[] imageData = texture2D.EncodeToJPG();

            // Send the image byte array to the server
            serverConnection.SendByteArray(imageData);

            // Assign the webcam texture to the RawImage component
            if (rawImage != null)
            {
                rawImage.texture = webcamTexture;
            }

            // Wait for the next frame
            yield return null;
        }
    }

    private void OnDestroy()
    {
        // Stop the webcam texture
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
            isPlaying = false;
        }
    }

    public void HandleReceivedData(string message)
    {
        // Use Unity's main thread dispatcher to update the Text component on the canvas
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            dataText.text = "Received data: " + message;
        });
    }
}