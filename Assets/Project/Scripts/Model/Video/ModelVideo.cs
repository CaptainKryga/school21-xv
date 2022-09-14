using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ModelVideo : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text textPath;
    [SerializeField] private GameObject imgRecord;
    
    private bool isSetRecordScreen;
    private Process proc;
    private bool isRecording;
    private string savePath = "\"" + Application.streamingAssetsPath + "/ScreenRecorder";
    private string filePath = "/out_video.mp4\"";

    [DllImport ("libc", EntryPoint = "chmod", SetLastError = true)]
    private static extern int sys_chmod (string path, uint mode);

    void Start()
    {
        isRecording = false;
        isSetRecordScreen = false;

        //chmod 777 полный доступ
        sys_chmod(Application.streamingAssetsPath + @"/ffmpeg", 755);
        Debug.Log(Application.streamingAssetsPath);

        textPath.text = Application.streamingAssetsPath + "/ScreenRecorder";
    }

    void OnApplicationQuit()
    {
        if (proc != null)
        {
            KillProcess();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isSetRecordScreen = !isSetRecordScreen;
            if (isRecording && !isSetRecordScreen)
                KillProcess();
        }

        RecordScreen();
    }

    private void KillProcess()
    {
        int IDstring = System.Convert.ToInt32(proc.Id.ToString());
        Process tempProc = Process.GetProcessById(IDstring);
        tempProc.CloseMainWindow();
        tempProc.WaitForExit();
        proc = null;
        isRecording = false;
        
        imgRecord.SetActive(false);
        Debug.Log("STOP RECORDING");
    }

    public void RecordScreen()
    {
        if (isSetRecordScreen == false || isRecording)
            return;
        
        proc = new Process();
        proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        proc.StartInfo.CreateNoWindow = true;

        proc.StartInfo.FileName = Application.streamingAssetsPath + @"/ffmpeg";
        proc.StartInfo.Arguments = 
            " -y -f avfoundation -i 1 -pix_fmt yuv420p -framerate 10 -vcodec libx264 -preset ultrafast -vsync 2 " + 
            savePath + filePath;
        Debug.Log(proc.StartInfo.FileName + proc.StartInfo.Arguments);
        
        proc.Start();
        isRecording = true;
        
        imgRecord.SetActive(true);
        Debug.Log("START RECORD");
    }
}