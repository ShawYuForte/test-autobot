using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace AgoraSDK
{
    public sealed class RtcEngine : AgoraRtcEngineNative
    {
        private readonly VideoDeviceManager _videoDeviceManager;
        private readonly Thread _callbackQueueThread;

        private RtcEngine(string appId)
        {
            InitEngineCallback();
            //Looks like returns false if everything is OK
            bool hasErrors = createEngine(appId);
            if (!hasErrors)
            {
				//_videoDeviceManager = VideoDeviceManager.GetInstance(this);

				_callbackQueueThread = new Thread(() => 
				{
					while (true)
					{
						AgoraCallbackQueue.Current.Update();
					}
				});
                _callbackQueueThread.Start();
            }
        }

        private static RtcEngine instance = null;

        private class AgoraCallbackQueue
        {
            private static Queue<Action> queue = new Queue<Action>();
            private static AgoraCallbackQueue _current;
            private int _queueTimeout = 500;
            private int _queueSize = 250;

            public static AgoraCallbackQueue Current
            {
                get
                {
                    if (_current == null)
                    {
                        _current = new AgoraCallbackQueue();
                    }
                    return _current;
                }
            }

            public void ClearQueue()
            {
                lock (queue)
                {
                    queue.Clear();
                }
            }

            public void EnQueue(Action action)
            {
                lock (queue)
                {
                    if (queue.Count >= _queueSize)
                    {
                        queue.Dequeue();
                    }
                    queue.Enqueue(action);
                }
            }

            private Action DeQueue()
            {
                lock (queue)
                {
                    Action action = queue.Dequeue();
                    return action;
                }
            }

            void Awake()
            {
                _current = this;
            }

            public void Update()
            {
                if (queue.Count > 0)
                {
                    var action = DeQueue();
                    action();
                    Thread.Sleep(_queueTimeout);
                }
            }

            public void OnDestroy()
            {
                _current = null;
            }
        }

		#region Callbacks

		#region Stored Callbacks

		//use these delegates to avoid GC destroying these objects prematurely

		public OnJoinChannelSuccessHandler OnJoinChannelSuccessCallbackStored = new OnJoinChannelSuccessHandler(OnJoinChannelSuccessCallback);
		public OnSDKWarningHandler OnSDKWarningCallbackStored = new OnSDKWarningHandler(OnSDKWarningCallback);
		public OnSDKErrorHandler OnSDKErrorCallbackStored = new OnSDKErrorHandler(OnSDKErrorCallback);
		public OnStreamMessageErrorHandler OnStreamMessageErrorCallbackStored = new OnStreamMessageErrorHandler(OnStreamMessageErrorCallback);

		#endregion

		private void InitEngineCallback()
        {
			initEventOnEngineCallback(
				OnJoinChannelSuccessCallbackStored, null
				/*OnReJoinChannelSuccessCallback*/, null
				/*OnConnectionLostCallback*/, null
				/*OnLeaveChannelCallback*/, null
				/*OnConnectionInterruptedCallback*/, null
				/*OnRequestTokenCallback*/, null
				/*OnUserJoinedCallback*/, null
				/*OnUserOfflineCallback*/, null
				/*OnAudioVolumeIndicationCallback*/, null
				/*OnUserMuteAudioCallback*/,
				OnSDKWarningCallbackStored, 
				OnSDKErrorCallbackStored, null
				/*OnRtcStatsCallback*/, null
				/*OnAudioMixingFinishedCallback*/, null
				/*OnAudioRouteChangedCallback*/, null
				/*OnFirstRemoteVideoDecodedCallback*/, null
				/*OnVideoSizeChangedCallback*/, null
				/*OnClientRoleChangedCallback*/, null
				/*OnUserMuteVideoCallback*/, null
				/*OnMicrophoneEnabledCallback*/, null
				/*OnApiExecutedCallback*/, null
				/*OnFirstLocalAudioFrameCallback*/, null
				/*OnFirstRemoteAudioFrameCallback*/, null
				/*OnLastmileQualityCallback*/, null
				/*OnAudioQualityCallback*/, null
				/*OnStreamInjectedStatusCallback*/, null
				/*OnStreamUnpublishedCallback*/, null
				/*OnStreamPublishedCallback*/,
				OnStreamMessageErrorCallbackStored, null
				/*OnStreamMessageCallback*/, null
				/*OnConnectionBannedCallback*/, null
				/*OnVideoStoppedCallback*/, null
				/*OnTokenPrivilegeWillExpireCallback*/, null
				/*OnNetworkQualityCallback*/, null
				/*OnLocalVideoStatsCallback*/, null
				/*OnRemoteVideoStatsCallback*/, null
				/*OnRemoteAudioStatsCallback*/, null
				/*OnFirstLocalVideoFrameCallback*/, null
				/*OnFirstRemoteVideoFrameCallback*/, null
				/*OnUserEnableVideoCallback*/, null
				/*OnAudioDeviceStateChangedCallback*/, null
				/*OnCameraReadyCallback*/, null
				/*OnCameraFocusAreaChangedCallback*/, null
				/*OnCameraExposureAreaChangedCallback*/, null
				/*OnRemoteAudioMixingBeginCallback*/, null
				/*OnRemoteAudioMixingEndCallback*/, null
				/*OnAudioEffectFinishedCallback*/, null
				/*OnVideoDeviceStateChangedCallback*/, null
				/*OnRemoteVideoStateChangedCallback*/, null
				/*OnUserEnableLocalVideoCallback*/, null
				/*OnLocalPublishFallbackToAudioOnlyCallback*/, null
				/*OnRemoteSubscribeFallbackToAudioOnlyCallback*/, null
				/*OnConnectionStateChangedCallback*/, null
				/*OnRemoteVideoTransportStatsCallback*/, null
				/*OnRemoteAudioTransportStatsCallback*/, null
				/*OnTranscodingUpdatedCallback*/, null
				/*OnAudioDeviceVolumeChangedCallback*/, null
				/*OnActiveSpeakerCallback*/, null
				/*OnMediaEngineStartCallSuccessCallback*/, null
				/*OnMediaEngineLoadSuccessCallback*/, null
				/*OnAudioMixingStateChangedCallback*/, null
				/*OnFirstRemoteAudioDecodedCallback*/, null
				/*OnLocalVideoStateChangedCallback*/, null
				/*OnRtmpStreamingStateChangedCallback*/, null
				/*OnNetworkTypeChangedCallback*/, null
				/*OnLastmileProbeResultCallback*/, null
				/*OnLocalUserRegisteredCallback*/, null
				/*OnUserInfoUpdatedCallback*/, null
				/*OnLocalAudioStateChangedCallback*/, null
				/*OnRemoteAudioStateChangedCallback*/, null
				/*OnLocalAudioStatsCallback*/, null
				/*OnChannelMediaRelayStateChangedCallback*/, null
				/*OnChannelMediaRelayEventCallback*/
				);
        }

		private static void OnJoinChannelSuccessCallback(string channel, uint uid, int elapsed)
        {
            if (instance != null && instance.OnJoinChannelSuccess != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnJoinChannelSuccess != null)
                    {
                        instance.OnJoinChannelSuccess(channel, uid, elapsed);
                    }
                });
            }
        }

        private static void OnLeaveChannelCallback(uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage)
        {
            if (instance != null && instance.OnLeaveChannel != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnLeaveChannel != null)
                    {
                        RtcStats rtcStats = new RtcStats();
                        rtcStats.duration = duration;
                        rtcStats.txBytes = txBytes;
                        rtcStats.rxBytes = rxBytes;
                        rtcStats.txAudioBytes = txAudioBytes;
                        rtcStats.txVideoBytes = txVideoBytes;
                        rtcStats.rxAudioBytes = rxAudioBytes;
                        rtcStats.rxVideoBytes = rxVideoBytes;
                        rtcStats.txKBitRate = txKBitRate;
                        rtcStats.rxKBitRate = rxKBitRate;
                        rtcStats.rxAudioKBitRate = rxAudioKBitRate;
                        rtcStats.txAudioKBitRate = txAudioKBitRate;
                        rtcStats.rxVideoKBitRate = rxVideoKBitRate;
                        rtcStats.txVideoKBitRate = txVideoKBitRate;
                        rtcStats.lastmileDelay = lastmileDelay;
                        rtcStats.txPacketLossRate = txPacketLossRate;
                        rtcStats.rxPacketLossRate = rxPacketLossRate;
                        rtcStats.userCount = userCount;
                        rtcStats.cpuAppUsage = cpuAppUsage;
                        rtcStats.cpuTotalUsage = cpuTotalUsage;
                        instance.OnLeaveChannel(rtcStats);
                    }
                });
            }
        }

        private static void OnReJoinChannelSuccessCallback(string channelName, uint uid, int elapsed)
        {
            if (instance != null && instance.OnReJoinChannelSuccess != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnReJoinChannelSuccess != null)
                    {
                        instance.OnReJoinChannelSuccess(channelName, uid, elapsed);
                    }
                });
            }
        }

        private static void OnConnectionLostCallback()
        {
            if (instance != null && instance.OnConnectionLost != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnConnectionLost != null)
                    {
                        instance.OnConnectionLost();
                    }
                });
            }
        }

        private static void OnConnectionInterruptedCallback()
        {
            if (instance != null && instance.OnConnectionInterrupted != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnConnectionInterrupted != null)
                    {
                        instance.OnConnectionInterrupted();
                    }
                });
            }
        }

        private static void OnRequestTokenCallback()
        {
            if (instance != null && instance.OnRequestToken != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRequestToken != null)
                    {
                        instance.OnRequestToken();
                    }
                });
            }
        }

        private static void OnUserJoinedCallback(uint uid, int elapsed)
        {
            if (instance != null && instance.OnUserJoined != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnUserJoined != null)
                    {
                        instance.OnUserJoined(uid, elapsed);
                    }
                });
            }
        }

        private static void OnUserOfflineCallback(uint uid, int reason)
        {
            if (instance != null && instance.OnUserOffline != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnUserOffline != null)
                    {
                        instance.OnUserOffline(uid, (USER_OFFLINE_REASON)reason);
                    }
                });
            }
        }

        private static void OnAudioVolumeIndicationCallback(string volumeInfo, int speakerNumber, int totalVolume)
        {
            if (instance != null && instance.OnVolumeIndication != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnVolumeIndication != null)
                    {
                        string[] sArray = volumeInfo.Split('\t');
                        int j = 1;
                        AudioVolumeInfo[] infos = new AudioVolumeInfo[speakerNumber];
                        if (speakerNumber > 0)
                        {
                            for (int i = 0; i < speakerNumber; i++)
                            {
                                uint uids = (uint)int.Parse(sArray[j++]);
                                uint vol = (uint)int.Parse(sArray[j++]);
                                uint vad = (uint)int.Parse(sArray[j++]);
                                infos[i].uid = uids;
                                infos[i].volume = vol;
                                infos[i].vad = vad;
                            }
                        }
                        instance.OnVolumeIndication(infos, speakerNumber, totalVolume);
                    }
                });
            }
        }

        private static void OnUserMuteAudioCallback(uint uid, bool muted)
        {
            if (instance != null && instance.OnUserMutedAudio != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnUserMutedAudio != null)
                    {
                        instance.OnUserMutedAudio(uid, muted);
                    }
                });
            }
        }

        private static void OnSDKWarningCallback(int warn, string msg)
        {
            if (instance != null && instance.OnWarning != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnWarning != null)
                    {
                        instance.OnWarning(warn, msg);
                    }
                });
            }
        }

        private static void OnSDKErrorCallback(int error, string msg)
        {
            if (instance != null && instance.OnError != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnError != null)
                    {
                        instance.OnError(error, msg);
                    }
                });
            }
        }

        private static void OnRtcStatsCallback(uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage)
        {
            if (instance != null && instance.OnRtcStats != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRtcStats != null)
                    {
                        RtcStats rtcStats = new RtcStats();
                        rtcStats.duration = duration;
                        rtcStats.txBytes = txBytes;
                        rtcStats.rxBytes = rxBytes;
                        rtcStats.txAudioBytes = txAudioBytes;
                        rtcStats.txVideoBytes = txVideoBytes;
                        rtcStats.rxAudioBytes = rxAudioBytes;
                        rtcStats.rxVideoBytes = rxVideoBytes;
                        rtcStats.txKBitRate = txKBitRate;
                        rtcStats.rxKBitRate = rxKBitRate;
                        rtcStats.rxAudioKBitRate = rxAudioKBitRate;
                        rtcStats.txAudioKBitRate = txAudioKBitRate;
                        rtcStats.rxVideoKBitRate = rxVideoKBitRate;
                        rtcStats.txVideoKBitRate = txVideoKBitRate;
                        rtcStats.lastmileDelay = lastmileDelay;
                        rtcStats.txPacketLossRate = txPacketLossRate;
                        rtcStats.rxPacketLossRate = rxPacketLossRate;
                        rtcStats.userCount = userCount;
                        rtcStats.cpuAppUsage = cpuAppUsage;
                        rtcStats.cpuTotalUsage = cpuTotalUsage;
                        instance.OnRtcStats(rtcStats);
                    }
                });
            }
        }

        private static void OnAudioMixingFinishedCallback()
        {
            if (instance != null && instance.OnAudioMixingFinished != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnAudioMixingFinished != null)
                    {
                        instance.OnAudioMixingFinished();
                    }
                });
            }
        }

        private static void OnAudioRouteChangedCallback(int route)
        {
            if (instance != null && instance.OnAudioRouteChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnAudioRouteChanged != null)
                    {
                        instance.OnAudioRouteChanged((AUDIO_ROUTE)route);
                    }
                });
            }
        }

        private static void OnFirstRemoteVideoDecodedCallback(uint uid, int width, int height, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteVideoDecoded != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnFirstRemoteVideoDecoded != null)
                    {
                        instance.OnFirstRemoteVideoDecoded(uid, width, height, elapsed);
                    }
                });
            }
        }

        private static void OnVideoSizeChangedCallback(uint uid, int width, int height, int rotation)
        {
            if (instance != null && instance.OnVideoSizeChanged != null && AgoraCallbackQueue.Current != null)
            {

            }
        }

        private static void OnClientRoleChangedCallback(int oldRole, int newRole)
        {
            if (instance != null && instance.OnClientRoleChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnClientRoleChanged != null)
                    {
                        instance.OnClientRoleChanged((CLIENT_ROLE_TYPE)oldRole, (CLIENT_ROLE_TYPE)newRole);
                    }
                });
            }
        }

        private static void OnUserMuteVideoCallback(uint uid, bool muted)
        {
            if (instance != null && instance.OnUserMuteVideo != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnUserMuteVideo != null)
                    {
                        instance.OnUserMuteVideo(uid, muted);
                    }
                });
            }
        }

        private static void OnMicrophoneEnabledCallback(bool isEnabled)
        {
            if (instance != null && instance.OnMicrophoneEnabled != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnMicrophoneEnabled != null)
                    {
                        instance.OnMicrophoneEnabled(isEnabled);
                    }
                });
            }
        }

        private static void OnApiExecutedCallback(int err, string api, string result)
        {
            if (instance != null && instance.OnApiExecuted != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnApiExecuted != null)
                    {
                        instance.OnApiExecuted(err, api, result);
                    }
                });
            }
        }

        private static void OnFirstLocalAudioFrameCallback(int elapsed)
        {
            if (instance != null && instance.OnFirstLocalAudioFrame != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnFirstLocalAudioFrame != null)
                    {
                        instance.OnFirstLocalAudioFrame(elapsed);
                    }
                });
            }
        }

        private static void OnFirstRemoteAudioFrameCallback(uint userId, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteAudioFrame != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnFirstRemoteAudioFrame != null)
                    {
                        instance.OnFirstRemoteAudioFrame(userId, elapsed);
                    }
                });
            }
        }

        private static void OnLastmileQualityCallback(int quality)
        {
            if (instance != null && instance.OnLastmileQuality != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnLastmileQuality != null)
                    {
                        instance.OnLastmileQuality(quality);
                    }
                });
            }
        }

        private static void OnAudioQualityCallback(uint userId, int quality, ushort delay, ushort lost)
        {
            if (instance != null && instance.OnAudioQuality != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnAudioQuality != null)
                    {
                        instance.OnAudioQuality(userId, quality, delay, lost);
                    }
                });
            }
        }

        private static void OnStreamInjectedStatusCallback(string url, uint userId, int status)
        {
            if (instance != null && instance.OnStreamInjectedStatus != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnStreamInjectedStatus != null)
                    {
                        instance.OnStreamInjectedStatus(url, userId, status);
                    }
                });
            }
        }

        private static void OnStreamUnpublishedCallback(string url)
        {
            if (instance != null && instance.OnStreamUnpublished != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnStreamUnpublished != null)
                    {
                        instance.OnStreamUnpublished(url);
                    }
                });
            }
        }

        private static void OnStreamPublishedCallback(string url, int error)
        {
            if (instance != null && instance.OnStreamPublished != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnStreamPublished != null)
                    {
                        instance.OnStreamPublished(url, error);
                    }
                });
            }
        }

        private static void OnStreamMessageErrorCallback(uint userId, int streamId, int code, int missed, int cached)
        {
            if (instance != null && instance.OnStreamMessageError != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnStreamMessageError != null)
                    {
                        instance.OnStreamMessageError(userId, streamId, code, missed, cached);
                    }
                });
            }
        }

        private static void OnStreamMessageCallback(uint userId, int streamId, string data, int length)
        {
            if (instance != null && instance.OnStreamMessage != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnStreamMessage != null)
                    {
                        instance.OnStreamMessage(userId, streamId, data, length);
                    }
                });
            }
        }

        private static void OnConnectionBannedCallback()
        {
            if (instance != null && instance.OnConnectionBanned != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnConnectionBanned != null)
                    {
                        instance.OnConnectionBanned();
                    }
                });
            }
        }

        private static void OnConnectionStateChangedCallback(int state, int reason)
        {
            if (instance != null && instance.OnConnectionStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnConnectionStateChanged != null)
                    {
                        instance.OnConnectionStateChanged((CONNECTION_STATE_TYPE)state, (CONNECTION_CHANGED_REASON_TYPE)reason);
                    }
                });
            }
        }

        private static void OnTokenPrivilegeWillExpireCallback(string token)
        {
            if (instance != null && instance.OnTokenPrivilegeWillExpire != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnTokenPrivilegeWillExpire != null)
                    {
                        instance.OnTokenPrivilegeWillExpire(token);
                    }
                });
            }
        }

        private static void OnActiveSpeakerCallback(uint uid)
        {
            if (instance != null && instance.OnActiveSpeaker != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnActiveSpeaker != null)
                    {
                        instance.OnActiveSpeaker(uid);
                    }
                });
            }
        }

        private static void OnVideoStoppedCallback()
        {
            if (instance != null && instance.OnVideoStopped != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnVideoStopped != null)
                    {
                        instance.OnVideoStopped();
                    }
                });
            }
        }

        private static void OnFirstLocalVideoFrameCallback(int width, int height, int elapsed)
        {
            if (instance != null && instance.OnFirstLocalVideoFrame != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnFirstLocalVideoFrame != null)
                    {
                        instance.OnFirstLocalVideoFrame(width, height, elapsed);
                    }
                });
            }
        }

        private static void OnFirstRemoteVideoFrameCallback(uint uid, int width, int height, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteVideoFrame != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnFirstRemoteVideoFrame != null)
                    {
                        instance.OnFirstRemoteVideoFrame(uid, width, height, elapsed);
                    }
                });
            }
        }

        private static void OnUserEnableVideoCallback(uint uid, bool enabled)
        {
            if (instance != null && instance.OnUserEnableVideo != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnUserEnableVideo != null)
                    {
                        instance.OnUserEnableVideo(uid, enabled);
                    }
                });
            }
        }

        private static void OnUserEnableLocalVideoCallback(uint uid, bool enabled)
        {
            if (instance != null && instance.OnUserEnableLocalVideo != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnUserEnableLocalVideo != null)
                    {
                        instance.OnUserEnableLocalVideo(uid, enabled);
                    }
                });
            }
        }

        private static void OnRemoteVideoStateChangedCallback(uint uid, int state, int reason, int elapsed)
        {
            if (instance != null && instance.OnRemoteVideoStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRemoteVideoStateChanged != null)
                    {
                        instance.OnRemoteVideoStateChanged(uid, (REMOTE_VIDEO_STATE)state, (REMOTE_VIDEO_STATE_REASON)reason, elapsed);
                    }
                });
            }
        }

        private static void OnLocalPublishFallbackToAudioOnlyCallback(bool isFallbackOrRecover)
        {
            if (instance != null && instance.OnLocalPublishFallbackToAudioOnly != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnLocalPublishFallbackToAudioOnly != null)
                    {
                        instance.OnLocalPublishFallbackToAudioOnly(isFallbackOrRecover);
                    }
                });
            }
        }

        private static void OnRemoteSubscribeFallbackToAudioOnlyCallback(uint uid, bool isFallbackOrRecover)
        {
            if (instance != null && instance.OnRemoteSubscribeFallbackToAudioOnly != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRemoteSubscribeFallbackToAudioOnly != null)
                    {
                        instance.OnRemoteSubscribeFallbackToAudioOnly(uid, isFallbackOrRecover);
                    }
                });
            }
        }

        private static void OnNetworkQualityCallback(uint uid, int txQuality, int rxQuality)
        {
            if (instance != null && instance.OnNetworkQuality != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnNetworkQuality != null)
                    {
                        instance.OnNetworkQuality(uid, txQuality, rxQuality);
                    }
                });
            }
        }

        private static void OnLocalVideoStatsCallback(int sentBitrate, int sentFrameRate, int encoderOutputFrameRate, int rendererOutputFrameRate, int targetBitrate, int targetFrameRate, int qualityAdaptIndication, int encodedBitrate, int encodedFrameWidth, int encodedFrameHeight, int encodedFrameCount, int codecType)
        {
            if (instance != null && instance.OnLocalVideoStats != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnLocalVideoStats != null)
                    {
                        LocalVideoStats localVideoStats = new LocalVideoStats();
                        localVideoStats.sentBitrate = sentBitrate;
                        localVideoStats.sentFrameRate = sentFrameRate;
                        localVideoStats.encoderOutputFrameRate = encoderOutputFrameRate;
                        localVideoStats.rendererOutputFrameRate = rendererOutputFrameRate;
                        localVideoStats.targetBitrate = targetBitrate;
                        localVideoStats.targetFrameRate = targetFrameRate;
                        localVideoStats.qualityAdaptIndication = (QUALITY_ADAPT_INDICATION)qualityAdaptIndication;
                        localVideoStats.encodedBitrate = encodedBitrate;
                        localVideoStats.encodedFrameWidth = encodedFrameWidth;
                        localVideoStats.encodedFrameHeight = encodedFrameHeight;
                        localVideoStats.encodedFrameCount = encodedFrameCount;
                        localVideoStats.codecType = (VIDEO_CODEC_TYPE)codecType;
                        instance.OnLocalVideoStats(localVideoStats);
                    }
                });
            }
        }

        private static void OnRemoteVideoStatsCallback(uint uid, int delay, int width, int height, int receivedBitrate, int decoderOutputFrameRate, int rendererOutputFrameRate, int remoteVideoStreamType, int packetLossRate, int totalFrozenTime, int frozenRate)
        {
            if (instance != null && instance.OnRemoteVideoStats != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRemoteVideoStats != null)
                    {
                        RemoteVideoStats remoteVideoStats = new RemoteVideoStats();
                        remoteVideoStats.uid = uid;
                        remoteVideoStats.delay = delay;
                        remoteVideoStats.width = width;
                        remoteVideoStats.height = height;
                        remoteVideoStats.receivedBitrate = receivedBitrate;
                        remoteVideoStats.decoderOutputFrameRate = decoderOutputFrameRate;
                        remoteVideoStats.rendererOutputFrameRate = rendererOutputFrameRate;
                        remoteVideoStats.rxStreamType = (REMOTE_VIDEO_STREAM_TYPE)remoteVideoStreamType;
                        remoteVideoStats.packetLossRate = packetLossRate;
                        remoteVideoStats.totalFrozenTime = totalFrozenTime;
                        remoteVideoStats.frozenRate = frozenRate;
                        instance.OnRemoteVideoStats(remoteVideoStats);
                    }
                });
            }
        }

        private static void OnRemoteAudioStatsCallback(uint uid, int quality, int networkTransportDelay, int jitterBufferDelay, int audioLossRate, int numChannels, int receivedSampleRate, int receivedBitrate, int totalFrozenTime, int frozenRate)
        {
            if (instance != null && instance.OnRemoteAudioStats != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRemoteAudioStats != null)
                    {
                        RemoteAudioStats remoteAudioStats = new RemoteAudioStats();
                        remoteAudioStats.uid = uid;
                        remoteAudioStats.quality = quality;
                        remoteAudioStats.networkTransportDelay = networkTransportDelay;
                        remoteAudioStats.jitterBufferDelay = jitterBufferDelay;
                        remoteAudioStats.audioLossRate = audioLossRate;
                        remoteAudioStats.numChannels = numChannels;
                        remoteAudioStats.receivedSampleRate = receivedSampleRate;
                        remoteAudioStats.receivedBitrate = receivedBitrate;
                        remoteAudioStats.totalFrozenTime = totalFrozenTime;
                        remoteAudioStats.frozenRate = frozenRate;
                        instance.OnRemoteAudioStats(remoteAudioStats);
                    }
                });
            }
        }

        private static void OnAudioDeviceStateChangedCallback(string deviceId, int deviceType, int deviceState)
        {
            if (instance != null && instance.OnAudioDeviceStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnAudioDeviceStateChanged != null)
                    {
                        instance.OnAudioDeviceStateChanged(deviceId, deviceType, deviceState);
                    }
                });
            }
        }

        private static void OnCameraReadyCallback()
        {
            if (instance != null && instance.OnCameraReady != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnCameraReady != null)
                    {
                        instance.OnCameraReady();
                    }
                });
            }
        }

        private static void OnCameraFocusAreaChangedCallback(int x, int y, int width, int height)
        {
            if (instance != null && instance.OnCameraFocusAreaChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnCameraFocusAreaChanged != null)
                    {
                        instance.OnCameraExposureAreaChanged(x, y, width, height);
                    }
                });
            }
        }

        private static void OnCameraExposureAreaChangedCallback(int x, int y, int width, int height)
        {
            if (instance != null && instance.OnCameraExposureAreaChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnCameraExposureAreaChanged != null)
                    {
                        instance.OnCameraExposureAreaChanged(x, y, width, height);
                    }
                });
            }
        }

        private static void OnRemoteAudioMixingBeginCallback()
        {
            if (instance != null && instance.OnRemoteAudioMixingBegin != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRemoteAudioMixingBegin != null)
                    {
                        instance.OnRemoteAudioMixingBegin();
                    }
                });
            }
        }

        private static void OnRemoteAudioMixingEndCallback()
        {
            if (instance != null && instance.OnRemoteAudioMixingEnd != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRemoteAudioMixingEnd != null)
                    {
                        instance.OnRemoteAudioMixingEnd();
                    }
                });
            }
        }

        private static void OnAudioEffectFinishedCallback(int soundId)
        {
            if (instance != null && instance.OnAudioEffectFinished != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnAudioEffectFinished != null)
                    {
                        instance.OnAudioEffectFinished(soundId);
                    }
                });
            }
        }

        private static void OnVideoDeviceStateChangedCallback(string deviceId, int deviceType, int deviceState)
        {
            if (instance != null && instance.OnVideoDeviceStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnVideoDeviceStateChanged != null)
                    {
                        instance.OnVideoDeviceStateChanged(deviceId, deviceType, deviceState);
                    }
                });
            }
        }

        private static void OnRemoteVideoTransportStatsCallback(uint uid, ushort delay, ushort lost, ushort rxKBitRate)
        {
            if (instance != null && instance.OnRemoteVideoTransportStats != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRemoteVideoTransportStats != null)
                    {
                        instance.OnRemoteVideoTransportStats(uid, delay, lost, rxKBitRate);
                    }
                });
            }
        }

        private static void OnRemoteAudioTransportStatsCallback(uint uid, ushort delay, ushort lost, ushort rxKBitRate)
        {
            if (instance != null && instance.OnRemoteAudioTransportStats != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRemoteAudioTransportStats != null)
                    {
                        instance.OnRemoteAudioTransportStats(uid, delay, lost, rxKBitRate);
                    }
                });
            }
        }

        private static void OnTranscodingUpdatedCallback()
        {
            if (instance != null && instance.OnTranscodingUpdated != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnTranscodingUpdated != null)
                    {
                        instance.OnTranscodingUpdated();
                    }
                });
            }
        }

        private static void OnAudioDeviceVolumeChangedCallback(int deviceType, int volume, bool muted)
        {
            if (instance != null && instance.OnAudioDeviceVolumeChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnAudioDeviceVolumeChanged != null)
                    {
                        instance.OnAudioDeviceVolumeChanged((MEDIA_DEVICE_TYPE)deviceType, volume, muted);
                    }
                });
            }
        }

        private static void OnMediaEngineStartCallSuccessCallback()
        {
            if (instance != null && instance.OnMediaEngineStartCallSuccess != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnMediaEngineStartCallSuccess != null)
                    {
                        instance.OnMediaEngineStartCallSuccess();
                    }
                });
            }
        }

        private static void OnMediaEngineLoadSuccessCallback()
        {
            if (instance != null && instance.OnMediaEngineLoadSuccess != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnMediaEngineLoadSuccess != null)
                    {
                        instance.OnMediaEngineLoadSuccess();
                    }
                });
            }
        }

        private static void OnAudioMixingStateChangedCallback(int audioMixingStateType, int audioMixingErrorType)
        {
            if (instance != null && instance.OnAudioMixingStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnAudioMixingStateChanged != null)
                    {
                        instance.OnAudioMixingStateChanged((AUDIO_MIXING_STATE_TYPE)audioMixingStateType, (AUDIO_MIXING_ERROR_TYPE)audioMixingErrorType);
                    }
                });
            }
        }

        private static void OnFirstRemoteAudioDecodedCallback(uint uid, int elapsed)
        {
            if (instance != null && instance.OnFirstRemoteAudioDecoded != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnFirstRemoteAudioDecoded != null)
                    {
                        instance.OnFirstRemoteAudioDecoded(uid, elapsed);
                    }
                });
            }
        }

        private static void OnLocalVideoStateChangedCallback(int localVideoState, int error)
        {
            if (instance != null && instance.OnLocalVideoStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnLocalVideoStateChanged != null)
                    {
                        instance.OnLocalVideoStateChanged((LOCAL_VIDEO_STREAM_STATE)localVideoState, (LOCAL_VIDEO_STREAM_ERROR)error);
                    }
                });
            }
        }

        private static void OnRtmpStreamingStateChangedCallback(string url, int state, int errCode)
        {
            if (instance != null && instance.OnRtmpStreamingStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRtmpStreamingStateChanged != null)
                    {
                        instance.OnRtmpStreamingStateChanged(url, (RTMP_STREAM_PUBLISH_STATE)state, (RTMP_STREAM_PUBLISH_ERROR)errCode);
                    }
                });
            }
        }

        private static void OnNetworkTypeChangedCallback(int networkType)
        {
            if (instance != null && instance.OnNetworkTypeChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnNetworkTypeChanged != null)
                    {
                        instance.OnNetworkTypeChanged((NETWORK_TYPE)networkType);
                    }
                });
            }
        }

        private static void OnLastmileProbeResultCallback(int state, uint upLinkPacketLossRate, uint upLinkjitter, uint upLinkAvailableBandwidth, uint downLinkPacketLossRate, uint downLinkJitter, uint downLinkAvailableBandwidth, uint rtt)
        {
            if (instance != null && instance.OnLastmileProbeResult != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnLastmileProbeResult != null)
                    {
                        LastmileProbeResult lastmileProbeResult = new LastmileProbeResult();
                        lastmileProbeResult.state = (LASTMILE_PROBE_RESULT_STATE)state;
                        lastmileProbeResult.uplinkReport.packetLossRate = upLinkPacketLossRate;
                        lastmileProbeResult.uplinkReport.jitter = upLinkjitter;
                        lastmileProbeResult.uplinkReport.availableBandwidth = upLinkAvailableBandwidth;
                        lastmileProbeResult.downlinkReport.packetLossRate = downLinkPacketLossRate;
                        lastmileProbeResult.downlinkReport.jitter = downLinkJitter;
                        lastmileProbeResult.downlinkReport.availableBandwidth = downLinkAvailableBandwidth;
                        lastmileProbeResult.rtt = rtt;
                        instance.OnLastmileProbeResult(lastmileProbeResult);
                    }
                });
            }
        }

        private static void OnUserInfoUpdatedCallback(uint uid, uint userUid, string userAccount)
        {
            if (instance != null && instance.OnUserInfoUpdated != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnUserInfoUpdated != null)
                    {
                        UserInfo userInfo = new UserInfo();
                        userInfo.uid = userUid;
                        userInfo.userAccount = userAccount;
                        instance.OnUserInfoUpdated(uid, userInfo);
                    }
                });
            }
        }

        private static void OnLocalUserRegisteredCallback(uint uid, string userAccount)
        {
            if (instance != null && instance.OnLocalUserRegistered != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnLocalUserRegistered != null)
                    {
                        instance.OnLocalUserRegistered(uid, userAccount);
                    }
                });
            }
        }

        private static void OnLocalAudioStateChangedCallback(int state, int error)
        {
            if (instance != null && instance.OnLocalAudioStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnLocalAudioStateChanged != null)
                    {
                        instance.OnLocalAudioStateChanged((LOCAL_AUDIO_STREAM_STATE)state, (LOCAL_AUDIO_STREAM_ERROR)error);
                    }
                });
            }
        }

        private static void OnRemoteAudioStateChangedCallback(uint uid, int state, int reason, int elapsed)
        {
            if (instance != null && instance.OnRemoteAudioStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnRemoteAudioStateChanged != null)
                    {
                        instance.OnRemoteAudioStateChanged(uid, (REMOTE_AUDIO_STATE)state, (REMOTE_AUDIO_STATE_REASON)reason, elapsed);
                    }
                });
            }
        }

        private static void OnLocalAudioStatsCallback(int numChannels, int sentSampleRate, int sentBitrate)
        {
            if (instance != null && instance.OnLocalAudioStats != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnLocalAudioStats != null)
                    {
                        LocalAudioStats localAudioStats = new LocalAudioStats();
                        localAudioStats.numChannels = numChannels;
                        localAudioStats.sentSampleRate = sentSampleRate;
                        localAudioStats.sentBitrate = sentBitrate;
                        instance.OnLocalAudioStats(localAudioStats);
                    }
                });
            }
        }

        private static void OnChannelMediaRelayStateChangedCallback(int state, int code)
        {
            if (instance != null && instance.OnChannelMediaRelayStateChanged != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnChannelMediaRelayStateChanged != null)
                    {
                        instance.OnChannelMediaRelayStateChanged((CHANNEL_MEDIA_RELAY_STATE)state, (CHANNEL_MEDIA_RELAY_ERROR)code);
                    }
                });
            }
        }

        private static void OnChannelMediaRelayEventCallback(int events)
        {
            if (instance != null && instance.OnChannelMediaRelayEvent != null && AgoraCallbackQueue.Current != null)
            {
                AgoraCallbackQueue.Current.EnQueue(() => {
                    if (instance != null && instance.OnChannelMediaRelayEvent != null)
                    {
                        instance.OnChannelMediaRelayEvent((CHANNEL_MEDIA_RELAY_EVENT)events);
                    }
                });
            }
        }

		public OnJoinChannelSuccessHandler OnJoinChannelSuccess;

        public OnReJoinChannelSuccessHandler OnReJoinChannelSuccess;

        public OnConnectionLostHandler OnConnectionLost;

        public OnConnectionInterruptedHandler OnConnectionInterrupted;

        public OnRequestTokenHandler OnRequestToken;

        public OnUserJoinedHandler OnUserJoined;

        public OnUserOfflineHandler OnUserOffline;

        public OnLeaveChannelHandler OnLeaveChannel;

        public OnVolumeIndicationHandler OnVolumeIndication;

        public OnUserMutedAudioHandler OnUserMutedAudio;

        public OnSDKWarningHandler OnWarning;

        public OnSDKErrorHandler OnError;

        public OnRtcStatsHandler OnRtcStats;

        public OnAudioMixingFinishedHandler OnAudioMixingFinished;

        public OnAudioRouteChangedHandler OnAudioRouteChanged;

        public OnFirstRemoteVideoDecodedHandler OnFirstRemoteVideoDecoded;

        public OnVideoSizeChangedHandler OnVideoSizeChanged;

        public OnClientRoleChangedHandler OnClientRoleChanged;

        public OnUserMuteVideoHandler OnUserMuteVideo;

        public OnMicrophoneEnabledHandler OnMicrophoneEnabled;

        public OnFirstRemoteAudioFrameHandler OnFirstRemoteAudioFrame;

        public OnFirstLocalAudioFrameHandler OnFirstLocalAudioFrame;

        public OnApiExecutedHandler OnApiExecuted;

        public OnLastmileQualityHandler OnLastmileQuality;

        public OnAudioQualityHandler OnAudioQuality;

        public OnStreamInjectedStatusHandler OnStreamInjectedStatus;

        public OnStreamUnpublishedHandler OnStreamUnpublished;

        public OnStreamPublishedHandler OnStreamPublished;

        public OnStreamMessageErrorHandler OnStreamMessageError;

        public OnStreamMessageHandler OnStreamMessage;

        public OnConnectionBannedHandler OnConnectionBanned;

        public OnConnectionStateChangedHandler OnConnectionStateChanged;

        public OnTokenPrivilegeWillExpireHandler OnTokenPrivilegeWillExpire;

        public OnActiveSpeakerHandler OnActiveSpeaker;

        public OnVideoStoppedHandler OnVideoStopped;

        public OnFirstLocalVideoFrameHandler OnFirstLocalVideoFrame;

        public OnFirstRemoteVideoFrameHandler OnFirstRemoteVideoFrame;

        public OnUserEnableVideoHandler OnUserEnableVideo;

        public OnUserEnableLocalVideoHandler OnUserEnableLocalVideo;

        public OnRemoteVideoStateChangedHandler OnRemoteVideoStateChanged;

        public OnLocalPublishFallbackToAudioOnlyHandler OnLocalPublishFallbackToAudioOnly;

        public OnRemoteSubscribeFallbackToAudioOnlyHandler OnRemoteSubscribeFallbackToAudioOnly;

        public OnNetworkQualityHandler OnNetworkQuality;

        public OnLocalVideoStatsHandler OnLocalVideoStats;

        public OnRemoteVideoStatsHandler OnRemoteVideoStats;

        public OnRemoteAudioStatsHandler OnRemoteAudioStats;

        public OnAudioDeviceStateChangedHandler OnAudioDeviceStateChanged;

        public OnCameraReadyHandler OnCameraReady;

        public OnCameraFocusAreaChangedHandler OnCameraFocusAreaChanged;

        public OnCameraExposureAreaChangedHandler OnCameraExposureAreaChanged;

        public OnRemoteAudioMixingBeginHandler OnRemoteAudioMixingBegin;

        public OnRemoteAudioMixingEndHandler OnRemoteAudioMixingEnd;

        public OnAudioEffectFinishedHandler OnAudioEffectFinished;

        public OnVideoDeviceStateChangedHandler OnVideoDeviceStateChanged;

        public OnRemoteVideoTransportStatsHandler OnRemoteVideoTransportStats;

        public OnRemoteAudioTransportStatsHandler OnRemoteAudioTransportStats;

        public OnTranscodingUpdatedHandler OnTranscodingUpdated;

        public OnAudioDeviceVolumeChangedHandler OnAudioDeviceVolumeChanged;

        public OnMediaEngineStartCallSuccessHandler OnMediaEngineStartCallSuccess;

        public OnMediaEngineLoadSuccessHandler OnMediaEngineLoadSuccess;

        public OnAudioMixingStateChangedHandler OnAudioMixingStateChanged;

        public OnFirstRemoteAudioDecodedHandler OnFirstRemoteAudioDecoded;

        public OnLocalVideoStateChangedHandler OnLocalVideoStateChanged;

        public OnRtmpStreamingStateChangedHandler OnRtmpStreamingStateChanged;

        public OnNetworkTypeChangedHandler OnNetworkTypeChanged;

        public OnLastmileProbeResultHandler OnLastmileProbeResult;

        public OnLocalUserRegisteredHandler OnLocalUserRegistered;

        public OnUserInfoUpdatedHandler OnUserInfoUpdated;

        public OnLocalAudioStateChangedHandler OnLocalAudioStateChanged;

        public OnRemoteAudioStateChangedHandler OnRemoteAudioStateChanged;

        public OnLocalAudioStatsHandler OnLocalAudioStats;

        public OnChannelMediaRelayEventHandler OnChannelMediaRelayEvent;

        public OnChannelMediaRelayStateChangedHandler OnChannelMediaRelayStateChanged;

        #endregion

        public static RtcEngine GetEngine(string appId)
        {
            if (instance == null)
            {
                instance = new RtcEngine(appId);
            }
            return instance;
        }

        public int JoinChannel(string channelName, string info, uint uid)
        {
            return JoinChannelByKey(null, channelName, info, uid);
        }

        public int JoinChannelByKey(string channelKey, string channelName, string info, uint uid)
        {
            return joinChannel(channelKey, channelName, info, uid);
        }

        public int LeaveChannel()
        {
            return leaveChannel();
        }

        /** Sets the video encoder configuration.
        *
        * Each video encoder configuration corresponds to a set of video parameters, including the resolution, frame rate, bitrate, and video orientation.
        *
        * The parameters specified in this method are the maximum values under ideal network conditions. If the video engine cannot render the video using the specified parameters due to poor network conditions, the parameters further down the list are considered until a successful configuration is found.
        * 
        * @note If you do not need to set the video encoder configuration after joining the channel, you can call this method before the {@link agora_gaming_rtc.IRtcEngine.EnableVideo EnableVideo} method to reduce the render time of the first video frame.
        *
        * @param configuration Sets the local video encoder configuration. See VideoEncoderConfiguration.
        * 
        * @return
        * - 0: Success.
        * - < 0: Failure.
        */
		public int SetVideoEncoderConfiguration(VideoEncoderConfiguration configuration)
        {
            return setVideoEncoderConfiguration(configuration.dimensions.width, configuration.dimensions.height, (int)configuration.frameRate, configuration.minFrameRate, configuration.bitrate, configuration.minBitrate, (int)configuration.orientationMode, (int)configuration.degradationPreference);
        }

        public int EnableVideo()
        {
            setChannelProfile((int)CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);
            setClientRole((int)CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

            //for agora logs
            getAudioRecordingDeviceVolume();
            //internal device manager
            creatAAudioRecordingDeviceManager();

			/*
			2.2.* versions:
			10 - gain, stereo
			11 - no gain, no stereo
			12 - no gain, stereo
			13 - no gain, no stereo, gaming
			14 - no gain, stereo, gaming
			*/

            //setAudioRecordingDeviceVolume(255);
            //adjustRecordingSignalVolume(400);

            setParameters("{\"che.audio.enable.agc\":false}");
			setAudioProfile((int) AUDIO_PROFILE_TYPE.AUDIO_PROFILE_MUSIC_HIGH_QUALITY_STEREO, (int) AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_GAME_STREAMING);

			enableVideo();
            enableAudio();
            enableDualStreamMode(true);

            return 0;
        }

		public string GetSdkVersion()
        {
            string s = null;
            var res = getSdkVersion();
            if (res != IntPtr.Zero)
            {
                s = Marshal.PtrToStringAnsi(res);
                //freeObject(res);
            }
            return s;
        }

        public string GetUserInfoByUid(uint uid)
        {
            string s = null;
            var res = getUserInfoByUid(uid);
            if (res != IntPtr.Zero)
            {
                s = Marshal.PtrToStringAnsi(res);
                //freeObject(res);
            }
            return s;
        }

        public string GetErrorDescription(int code)
        {
            string s = null;
            var res = getErrorDescription(code);
            if (res != IntPtr.Zero)
            {
                s = Marshal.PtrToStringAnsi(res);
                //freeObject(res);
            }
            return s;
        }

        public int EnableVideoObserver()
        {
            return enableVideoObserver();
        }

        public int GetVideoDeviceCount()
        {
            return _videoDeviceManager.GetVideoDeviceCount();
        }

        public void ReleaseQueue()
        {
			AgoraCallbackQueue.Current.OnDestroy();
			_callbackQueueThread.Abort();
        }
    }
}
