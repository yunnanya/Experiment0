using System;
using System.IO;
using System.Text;
using UnityEngine;
using ViveSR.anipal.Eye;
using System.Runtime.InteropServices;
using UnityEngine.Assertions;
using System.Globalization;

namespace ViveSR.anipal.Eye
{
    public class SRanipal_GazeRaySample_v2 : MonoBehaviour
    {
        public int LengthOfRay = 25;
        [SerializeField] private LineRenderer GazeRayRenderer;
        private static EyeData_v2 eyeData = new EyeData_v2();
        private bool eye_callback_registered = false;

        // 眼动数据参数
        private float pupilDiameterLeft, pupilDiameterRight;
        private Vector2 pupilPositionLeft, pupilPositionRight;
        private float eyeOpenLeft, eyeOpenRight;

        // 文件记录参数
        private string datasetFilePath;
        private StreamWriter datasetFileWriter;
        private const string TimeFormat = "yyyy-MM-dd_HH:mm:ss.ffff";

        public event Action<Vector3> CollisionPointEvent;

        void Start()
        {
            if (!SRanipal_Eye_Framework.Instance.EnableEye)
            {
                enabled = false;
                return;
            }
            Assert.IsNotNull(GazeRayRenderer);

            // 文件路径与PlayerTracker一致
            string directory = Path.Combine(Application.dataPath, "../PlayerTraces");
            Directory.CreateDirectory(directory);

            string fileName = $"Eye_Tracking_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.csv";
            datasetFilePath = Path.Combine(directory, fileName);

            datasetFileWriter = new StreamWriter(datasetFilePath, false, new UTF8Encoding(true))
            {
                AutoFlush = true
            };

            // CSV表头
            datasetFileWriter.WriteLine(
                "TimeStamp," +
                "碰撞物体名称," +
                "碰撞点X,碰撞点Y,碰撞点Z," +
                "左瞳孔直径(mm),右瞳孔直径(mm)," +
                "左瞳孔X,左瞳孔Y," +
                "右瞳孔X,右瞳孔Y," +
                "左眼睁眼度,右眼睁眼度"
            );

            Debug.Log($"眼动数据文件已创建: {datasetFilePath}");
        }

        void Update()
        {
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT)
                return;

            UpdateEyeCallback();
            ProcessGazeData();
        }

        private void UpdateEyeCallback()
        {
            bool shouldRegister = SRanipal_Eye_Framework.Instance.EnableEyeDataCallback;
            if (shouldRegister != eye_callback_registered)
            {
                if (shouldRegister)
                {
                    SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(
                        Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                }
                else
                {
                    SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(
                        Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                }
                eye_callback_registered = shouldRegister;
            }
        }

        private void ProcessGazeData()
        {
            if (!GetGazeRay(out Vector3 origin, out Vector3 direction)) return;

            UpdateRayVisualization(direction);
            GetEyeMetrics();
            CheckCollision(direction);
        }

        private bool GetGazeRay(out Vector3 origin, out Vector3 direction)
        {
            if (eye_callback_registered)
            {
                return SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out origin, out direction, eyeData) ||
                       SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out origin, out direction, eyeData) ||
                       SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out origin, out direction, eyeData);
            }
            return SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out origin, out direction) ||
                   SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out origin, out direction) ||
                   SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out origin, out direction);
        }

        private void UpdateRayVisualization(Vector3 direction)
        {
            Vector3 worldDir = Camera.main.transform.TransformDirection(direction);
            GazeRayRenderer.SetPosition(0, Camera.main.transform.position);
            GazeRayRenderer.SetPosition(1, Camera.main.transform.position + worldDir * LengthOfRay);
        }

        private void GetEyeMetrics()
        {
            var left = eyeData.verbose_data.left;
            var right = eyeData.verbose_data.right;

            pupilDiameterLeft = left.pupil_diameter_mm;
            pupilDiameterRight = right.pupil_diameter_mm;
            pupilPositionLeft = left.pupil_position_in_sensor_area;
            pupilPositionRight = right.pupil_position_in_sensor_area;
            eyeOpenLeft = left.eye_openness;
            eyeOpenRight = right.eye_openness;
        }

        private void CheckCollision(Vector3 direction)
        {
            RaycastHit hit;
            Vector3 worldDir = Camera.main.transform.TransformDirection(direction).normalized;
            bool isHit = Physics.SphereCast(
                Camera.main.transform.position,
                0.1f,
                worldDir,
                out hit,
                LengthOfRay
            );

            string timestamp = DateTime.Now.ToString(TimeFormat);

            if (isHit)
            {
                HandleCollision(hit, timestamp);
            }
            else
            {
                HandleNoCollision(timestamp);
            }
        }

        private void HandleCollision(RaycastHit hit, string timestamp)
        {
            Vector3 point = hit.point;
            CollisionPointEvent?.Invoke(point);

            WriteDataLine(
                timestamp,
                hit.collider.name,
                point.x, point.y, point.z
            );
        }

        private void HandleNoCollision(string timestamp)
        {
            WriteDataLine(
                timestamp,
                "无碰撞",
                float.NaN, float.NaN, float.NaN
            );
        }

        private void WriteDataLine(string time, string name, float x, float y, float z)
        {
            datasetFileWriter.WriteLine(string.Join(",",
                FormatField(time),
                FormatField(name),
                FormatNumber(x),
                FormatNumber(y),
                FormatNumber(z),
                FormatNumber(pupilDiameterLeft),
                FormatNumber(pupilDiameterRight),
                FormatNumber(pupilPositionLeft.x),
                FormatNumber(pupilPositionLeft.y),
                FormatNumber(pupilPositionRight.x),
                FormatNumber(pupilPositionRight.y),
                FormatNumber(eyeOpenLeft),
                FormatNumber(eyeOpenRight)
            ));
        }

        private string FormatField(string value)
        {
            if (string.IsNullOrEmpty(value)) return "\"\"";
            return value.Contains(",") ? $"\"{value.Replace("\"", "\"\"")}\"" : value;
        }

        private string FormatNumber(float value)
        {
            return value.ToString("F4", CultureInfo.InvariantCulture);
        }

        private void ReleaseEyeCallback()
        {
            if (eye_callback_registered)
            {
                SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(
                    Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }
        }

        private static void EyeCallback(ref EyeData_v2 eye_data)
        {
            eyeData = eye_data;
        }

        void OnDestroy()
        {
            datasetFileWriter?.Close();
            ReleaseEyeCallback();
        }
    }
}