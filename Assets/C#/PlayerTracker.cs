using System;
using System.IO;
using System.Text;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    private static PlayerTracker _instance;
    public static PlayerTracker Instance => _instance ??= FindObjectOfType<PlayerTracker>();

    [Header("VR Player Settings")]
    [SerializeField] private Transform vrPlayer;  // 手动拖拽VR摄像机/控制器到此字段
    [SerializeField] private string playerID = "VR_Player_01";

    [Header("Recording Settings")]
    private StreamWriter _playerWriter;
    private StringBuilder _buffer = new StringBuilder();
    private string _filePath;

    private const string FileHeader = "TimeStamp,PlayerID,PosX,PosY,PosZ";
    private const string FilePrefix = "Player_Trajectory_";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeRecorder();
    }

    private void InitializeRecorder()
    {
        string directory = Path.Combine(Application.dataPath, "../PlayerTraces");
        Directory.CreateDirectory(directory);

        string timestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        _filePath = Path.Combine(directory, $"{FilePrefix}{timestamp}.csv");

        _playerWriter = new StreamWriter(_filePath, true);
        _playerWriter.WriteLine(FileHeader);
        Debug.Log($"Player trace file created: {_filePath}");
    }

    private void Update()
    {
        RecordPosition(); // 每帧直接记录
    }

    private void RecordPosition()
    {
        if (vrPlayer == null) return;

        Vector3 position = vrPlayer.position;
        _buffer.Append(DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.ffff"))
              .Append(",")
              .Append(playerID)
              .Append(",")
              .Append(position.x.ToString("F3"))
              .Append(",")
              .Append(position.y.ToString("F3"))
              .Append(",")
              .Append(position.z.ToString("F3"))
              .Append("\n");

        _playerWriter.Write(_buffer.ToString());
        _buffer.Clear();
    }

    private void OnDestroy()
    {
        _buffer?.Clear();
        _playerWriter?.Flush();
        _playerWriter?.Close();
        Debug.Log("Player tracker resources released");
    }

    // 编辑器扩展：自动查找VR玩家对象
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (vrPlayer == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) vrPlayer = playerObj.transform;
        }
    }
#endif
}