using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{
    public Camera cameraToCapture;

    public int screenshotWidth = 720;
    public int screenshotHeight = 1280;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        RenderTexture rt = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        cameraToCapture.targetTexture = rt;
        Texture2D screenshot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);

        cameraToCapture.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        cameraToCapture.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenshot.EncodeToPNG();
        string filename = Application.dataPath + "/HighResScreenshot.png";
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log($"Screenshot saved to: {filename}");
    }
}
