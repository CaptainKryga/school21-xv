using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class VideoRecorder : MonoBehaviour
{
    //ffmpeg.exe -hwaccel cuda -hwaccel_output_format cuda -f gdigrab -framerate 60 -s 1920x1080 -i desktop -preset ultrafast -crf 0 out.mkv
    private bool SetRecordScreen = false;
    //private string persistentDataPath => Application.streamingAssetsPath + "/ScreenRecorder";
    private Process proc = null;
    public bool recording = false;
    public string savePath => "\"" + Application.streamingAssetsPath + "/ScreenRecorder";
    public string filePath => "/out_video.mp4\"";
    //public string fileName;

#if UNITY_STANDALONE_OSX
    [DllImport ("libc", EntryPoint = "chmod", SetLastError = true)]
    private static extern int sys_chmod (string path, uint mode);
 #endif

    void Start()
    {
        recording = false;
        SetRecordScreen = false;
        //persistentDataPath = Application.streamingAssetsPath + "/ScreenRecorder";

#if     UNITY_STANDALONE_OSX
        sys_chmod(Application.streamingAssetsPath + @"/FFMPEG/MAC/ffmpeg", 755);
#endif 
    }

    void OnApplicationQuit()
    {
        if (proc != null)
        {
#if UNITY_STANDALONE_OSX
                //proc.Close();
                UnityEngine.Debug.Log("Killed process on MAC");

                int IDstring = System.Convert.ToInt32(proc.Id.ToString());
                Process tempProc = Process.GetProcessById(IDstring);

                tempProc.CloseMainWindow();
                tempProc.WaitForExit();
#endif
                UnityEngine.Debug.Log("Stopped Recording!");
                proc = null;
                recording = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SetRecordScreen = !SetRecordScreen;
            if (recording == true && SetRecordScreen == false)
            {

#if UNITY_STANDALONE_OSX
                //proc.Close();
                UnityEngine.Debug.Log("Killed process on MAC");

                int IDstring = System.Convert.ToInt32(proc.Id.ToString());
                Process tempProc = Process.GetProcessById(IDstring);

                tempProc.CloseMainWindow();
                tempProc.WaitForExit();
#endif
                UnityEngine.Debug.Log("Stopped Recording!");
                proc = null;
                recording = false;
            }
        }

        RecordScreen();
    }

    public void RecordScreen()
    {
        if (SetRecordScreen == false || recording == true)
            return;

        UnityEngine.Debug.Log("Started Recording!");
        proc = new Process();
        proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        proc.StartInfo.CreateNoWindow = true;
        //proc.StartInfo.UseShellExecute = false;

        //string savePath = "\"" + Application.streamingAssetsPath + "/ScreenRecorder" + "/out_video.mp4\"";

#if UNITY_STANDALONE_OSX
        proc.StartInfo.FileName = Application.streamingAssetsPath + @"/FFMPEG/MAC/ffmpeg";
        proc.StartInfo.Arguments = " -y -f avfoundation -i 1 -pix_fmt yuv420p -framerate 10 -vcodec libx264 -preset ultrafast -vsync 2 " + savePath + filePath;
#endif

        UnityEngine.Debug.Log(proc.StartInfo.FileName + proc.StartInfo.Arguments);
        proc.Start();
        recording = true;
    }
}