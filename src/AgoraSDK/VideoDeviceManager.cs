using System;
using System.Runtime.InteropServices;

namespace AgoraSDK
{
    public abstract class IVideoDeviceManager : AgoraRtcEngineNative
    {

        public abstract bool CreateAVideoDeviceManager();

        public abstract int ReleaseAVideoDeviceManager();

        public abstract int StartVideoDeviceTest(IntPtr hwnd);

        public abstract int StopVideoDeviceTest();

        public abstract int GetVideoDeviceCount();

        public abstract int GetVideoDevice(int index, ref string deviceName, ref string deviceId);

        public abstract int SetVideoDevice(string deviceId);

        public abstract int GetCurrentVideoDevice(ref string deviceId);
    }

    /** The definition of the VideoDeviceManager. */
    public sealed class VideoDeviceManager : IVideoDeviceManager
    {
        private RtcEngine _rtcEngine = null;
        private static VideoDeviceManager _videoDeviceManagerInstance = null;

        private VideoDeviceManager(RtcEngine rtcEngine)
        {
            _rtcEngine = rtcEngine;
		}

        public static VideoDeviceManager GetInstance(RtcEngine rtcEngine)
        {
            if (_videoDeviceManagerInstance == null)
            {
                _videoDeviceManagerInstance = new VideoDeviceManager(rtcEngine);
            }
            return _videoDeviceManagerInstance;
        }

        public static void ReleaseInstance()
        {
            _videoDeviceManagerInstance = null;
        }

        /** Create a VideoDeviceManager instance.
        *
        * @note Ensure that you call {@link agora_gaming_rtc.VideoDeviceManager.ReleaseAVideoDeviceManager ReleaseAVideoDeviceManager} to release this instance after calling this method.
        * 
        * @return 
        * - true: Success.
        * - false: Failure.
        */
        public override bool CreateAVideoDeviceManager()
        {
            if (_rtcEngine == null)
                return false;

            return createAVideoDeviceManager();
        }

        /** Release a VideoDeviceManager instance.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int ReleaseAVideoDeviceManager()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return releaseAVideoDeviceManager();
        }

        /** Starts the video recording device test.
        * 
        * This method tests whether the video recording device works properly. Before calling this method, ensure that you have already called the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} method, and the window handle (`hwnd`) parameter is valid.
        *  
        * @note 
        * Ensure that you call {@link agora_gaming_rtc.VideoDeviceManager.StopVideoDeviceTest StopVideoDeviceTest} after calling this method.
        * 
        * @param hwnd The window handle used to display the screen.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int StartVideoDeviceTest(IntPtr hwnd)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return startVideoDeviceTest(hwnd);
        }

        /** Stops the video recording device test.
        * 
        * @note Ensure that you call this method to stop the test after calling {@link agora_gaming_rtc.VideoDeviceManager.StartVideoDeviceTest StartVideoDeviceTest}.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int StopVideoDeviceTest()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return stopVideoDeviceTest();
        }

        /** Retrieves the total number of the indexed video recording devices in the system.
        * 
        * @return Total number of the indexed video recording devices.
        */
        public override int GetVideoDeviceCount()
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return getVideoDeviceCollectionCount();
        }

        /** Retrieves the video recording device associated with the index.
        *         
        * After calling this method, the SDK retrieves the device name and device ID according to the index.
        * 
        * @note Call {@link agora_gaming_rtc.VideoDeviceManager.GetVideoDeviceCount GetVideoDeviceCount} before this method.
        * 
        * @param index The index of the recording device in the system. The value of `index` is associated with the number of the recording device which is retrieved from `GetVideoDeviceCount`. For example, when the number of recording devices is 3, the value range of `index` is [0,2].
        * @param deviceName The name of the recording device for the corresponding index.
        * @param deviceId The ID of the recording device for the corresponding index.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int GetVideoDevice(int index, ref string deviceName, ref string deviceId)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            //if (index >= 0 && index < GetVideoDeviceCount())
            {
                System.IntPtr videoDeviceName = Marshal.AllocHGlobal(512);
                System.IntPtr videoDeviceId = Marshal.AllocHGlobal(512);
                int ret = getVideoDeviceCollectionDevice(index, videoDeviceName, videoDeviceId);
                deviceName = Marshal.PtrToStringAnsi(videoDeviceName);
                deviceId = Marshal.PtrToStringAnsi(videoDeviceId);
                Marshal.FreeHGlobal(videoDeviceName);
                Marshal.FreeHGlobal(videoDeviceId);
                return ret;
            }
            //else
            //{
            //    return (int)ERROR_CODE.ERROR_INVALID_ARGUMENT;
            //}
        }

        /** Retrieves the device ID of the current video recording device.
        * 
        * @param deviceId The device ID of the current video recording device.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int GetCurrentVideoDevice(ref string deviceId)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            if (GetVideoDeviceCount() > 0)
            {
                System.IntPtr videoDeviceId = Marshal.AllocHGlobal(512);
                int ret = getCurrentVideoDevice(videoDeviceId);
                deviceId = Marshal.PtrToStringAnsi(videoDeviceId);
                Marshal.FreeHGlobal(videoDeviceId);
                return ret;
            }
            else
            {
                return (int)ERROR_CODE.ERROR_NO_DEVICE_PLUGIN;
            }
        }

        /** Sets the video recording device using the device ID.
        * 
        * @note 
        * - Call {@link agora_gaming_rtc.VideoDeviceManager.GetVideoDevice GetVideoDevice} before this method.
        * - Plugging or unplugging the video device does not change the device ID.
        * 
        * @param deviceId Device ID of the video recording device, retrieved by calling `GetVideoDevice`.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
        public override int SetVideoDevice(string deviceId)
        {
            if (_rtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            return setVideoDeviceCollectionDevice(deviceId);
        }
    }
}
