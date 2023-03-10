using System;
using System.Runtime.InteropServices;

namespace AgoraSDK
{
    public class AgoraRtcEngineNative
    {
		#region events
		/**
        EngineEvent is only for engine, not for user,Please do not call this function.
        */
		protected delegate void EngineEventOnCaptureVideoFrame(int videoFrameType, int width, int height, int yStride, IntPtr yBuffer, int rotation, long renderTimeMs);

        protected delegate void EngineEventOnRenderVideoFrame(uint uid, int videoFrameType, int width, int height, int yStride, IntPtr yBuffer, int rotation, long renderTimeMs);

        protected delegate void EngineEventOnRecordAudioFrame(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnPlaybackAudioFrame(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnMixedAudioFrame(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnPlaybackAudioFrameBeforeMixing(uint uid, int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnPullAudioFrameHandler(int type, int samples, int bytesPerSample, int channels, int samplesPerSec, IntPtr buffer, long renderTimeMs, int avsync_type);

        protected delegate void EngineEventOnLeaveChannelHandler(uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage);

        protected delegate void EngineEventOnUserOfflineHandler(uint uid, int offLineReason);

        protected delegate void EngineEventOnAudioVolumeIndicationHandler(string volumeInfo, int speakerNumber, int totalVolume);

        protected delegate void EngineEventOnRtcStatsHandler(uint duration, uint txBytes, uint rxBytes, uint txAudioBytes, uint txVideoBytes, uint rxAudioBytes, uint rxVideoBytes, ushort txKBitRate, ushort rxKBitRate, ushort rxAudioKBitRate, ushort txAudioKBitRate, ushort rxVideoKBitRate, ushort txVideoKBitRate, ushort lastmileDelay, ushort txPacketLossRate, ushort rxPacketLossRate, uint userCount, double cpuAppUsage, double cpuTotalUsage);

        protected delegate void EngineEventOnAudioRouteChangedHandler(int route);

        protected delegate void EngineEventOnLocalVideoStatsHandler(int sentBitrate, int sentFrameRate, int encoderOutputFrameRate, int rendererOutputFrameRate, int targetBitrate, int targetFrameRate, int qualityAdaptIndication, int encodedBitrate, int encodedFrameWidth, int encodedFrameHeight, int encodedFrameCount, int codecType);

        protected delegate void EngineEventOnRemoteVideoStatsHandler(uint uid, int delay, int width, int height, int receivedBitrate, int decoderOutputFrameRate, int rendererOutputFrameRate, int remoteVideoStreamType, int packetLossRate, int totalFrozenTime, int frozenRate);

        protected delegate void EngineEventOnRemoteAudioStatsHandler(uint uid, int quality, int networkTransportDelay, int jitterBufferDelay, int audioLossRate, int numChannels, int receivedSampleRate, int receivedBitrate, int totalFrozenTime, int frozenRate);

        protected delegate void EngineEventOnAudioDeviceVolumeChangedHandler(int deviceType, int volume, bool muted);

        protected delegate void EngineEventOnAudioMixingStateChangedHandler(int state, int errorCode);

        protected delegate void EngineEventOnRtmpStreamingStateChangedHandler(string url, int state, int errCode);

        protected delegate void EngineEventOnNetworkTypeChangedHandler(int type);

        protected delegate void EngineEventOnLastmileProbeResultHandler(int state, uint upLinkPacketLossRate, uint upLinkjitter, uint upLinkAvailableBandwidth, uint downLinkPacketLossRate, uint downLinkJitter, uint downLinkAvailableBandwidth, uint rtt);

        protected delegate void EngineEventOnUserInfoUpdatedHandler(uint uid, uint userUid, string userAccount);

        protected delegate void EngineEventOnLocalAudioStateChangedHandler(int state, int error);

        protected delegate void EngineEventOnRemoteAudioStateChangedHandler(uint uid, int state, int reason, int elapsed);

        protected delegate void EngineEventOnLocalAudioStatsHandler(int numChannels, int sentSampleRate, int sentBitrate);

        protected delegate void EngineEventOnChannelMediaRelayStateChangedHandler(int state, int code);

        protected delegate void EngineEventOnChannelMediaRelayEventHandler(int events);

        protected delegate bool EngineEventOnReceiveAudioPacketHandler(IntPtr buffer, IntPtr size);

        protected delegate bool EngineEventOnReceiveVideoPacketHandler(IntPtr buffer, IntPtr size);

        protected delegate bool EngineEventOnSendAudioPacketHandler(IntPtr buffer, IntPtr size);

        protected delegate bool EngineEventOnSendVideoPacketHandler(IntPtr buffer, IntPtr size);

        protected delegate void EngineEventOnMediaMetaDataReceived(uint uid, uint size, IntPtr buffer, long timeStampMs);

        protected delegate void EngineEventOnConnectionStateChanged(int state, int reason);

        protected delegate bool EngineEventOnReadyToSendMetadata();

        protected delegate int EngineEventOnGetMaxMetadataSize();

        protected delegate void EngineEventOnClientRoleChanged(int oldRole, int newRole);

        protected delegate void EngineEventOnRemoteVideoStateChanged(uint uid, int state, int reason, int elapsed);
        // audio and video raw data

        protected delegate void EngineEventOnLocalVideoStateChanged(int localVideoState, int error);

		#endregion

		#region DllImport

		public const string MyLibName = "agoraSdkCWrapper";

        // standard sdk api
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern bool createEngine(string appId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern bool deleteEngine();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getSdkVersion();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int joinChannel(string channelKey, string channelName, string info, uint uid);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoicePitch(double pitch);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteVoicePosition(uint uid, double pan, double gain);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setVoiceOnlyMode(bool enable);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int leaveChannel();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableLastmileTest();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int disableLastmileTest();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableVideo();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int disableVideo();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableLocalVideo(bool enabled);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableLocalAudio(bool enabled);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setupLocalVideo(int hwnd, int renderMode, uint uid, IntPtr priv);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setupRemoteVideo(int hwnd, int renderMode, uint uid, IntPtr priv);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLocalRenderMode(int renderMode);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteRenderMode(uint userId, int renderMode);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVideoMirrorMode(int mirrorMode);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startPreview();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopPreview();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableAudio();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int disableAudio();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setParameters(string options);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getCallId();
        // caller free the returned char * (through freeObject)
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int rate(string callId, int rating, string desc);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int complain(string callId, string desc);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setEnableSpeakerphone(bool enabled);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern bool isSpeakerphoneEnabled();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultAudioRoutetoSpeakerphone(bool enabled);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableAudioVolumeIndication(int interval, int smooth, bool report_vad);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startAudioRecording(string filePath, int quality);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startAudioRecording2(string filePath, int sampleRate, int quality);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioRecording();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startAudioMixing(string filePath, bool loopBack, bool replace, int cycle);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioMixing();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int pauseAudioMixing();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int resumeAudioMixing();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int adjustAudioMixingVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingDuration();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingCurrentPosition();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalAudioStream(bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int muteAllRemoteAudioStreams(bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteAudioStream(uint uid, bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int switchCamera();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setVideoProfile(int profile, bool swapWidthAndHeight);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int muteLocalVideoStream(bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int muteAllRemoteVideoStreams(bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int muteRemoteVideoStream(uint uid, bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLogFile(string filePath);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int renewToken(string token);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setChannelProfile(int profile);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setClientRole(int role);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableDualStreamMode(bool enabled);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionMode(string encryptionMode);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setEncryptionSecret(string secret);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startRecordingService(string recordingKey);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopRecordingService(string recordingKey);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int refreshRecordingServiceStatus();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int createDataStream(bool reliable, bool ordered);
        // TODO! supports general data later. now only string is supported
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int sendStreamMessage(int streamId, string data, Int64 length);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setSpeakerphoneVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int adjustRecordingSignalVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int adjustPlaybackSignalVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setHighQualityAudioParametersWithFullband(int fullband, int stereo, int fullBitrate);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableInEarMonitoring(bool enabled);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableWebSdkInteroperability(bool enabled);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setVideoQualityParameters(bool preferFrameRateOverImageQuality);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startEchoTest();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startEchoTest2(int intervalInSeconds);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopEchoTest();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteVideoStreamType(uint uid, int streamType);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setMixedAudioFrameParameters(int sampleRate, int samplesPerCall);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setAudioMixingPosition(int pos);
        // setLogFilter: deprecated
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLogFilter(uint filter);
        // video texture stuff (extension for gaming)
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableVideoObserver();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int disableVideoObserver();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int generateNativeTexture();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int updateTexture(int tex, IntPtr data, uint uid);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int deleteTexture(int tex);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int updateVideoRawData(IntPtr data, uint uid);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int addUserVideoInfo(uint userId, uint textureId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int removeUserVideoInfo(uint userId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setPlaybackDeviceVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getEffectsVolume();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setEffectsVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int playEffect(int soundId, string filePath, int loopCount, double pitch, double pan, int gain, bool publish);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopEffect(int soundId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopAllEffects();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int preloadEffect(int soundId, string filePath);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int unloadEffect(int soundId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int pauseEffect(int soundId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int pauseAllEffects();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int resumeEffect(int soundId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int resumeAllEffects();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultMuteAllRemoteAudioStreams(bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultMuteAllRemoteVideoStreams(bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void freeObject(IntPtr obj);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getConnectionState();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setAudioProfile(int audioProfile, int scenario);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setVideoEncoderConfiguration(int width, int height, int frameRate, int minFrameRate, int bitrate, int minBitrate, int orientationMode, int degradationPreference);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int adjustAudioMixingPlayoutVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int adjustAudioMixingPublishVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setVolumeOfEffect(int soundId, int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setRecordingAudioFrameParameters(int sampleRate, int channel, int mode, int samplesPerCall);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setPlaybackAudioFrameParameters(int sampleRate, int channel, int mode, int samplesPerCall);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLocalPublishFallbackOption(int option);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteSubscribeFallbackOption(int option);
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteDefaultVideoStreamType(int remoteVideoStreamType);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int addPublishStreamUrl(string url, bool transcodingEnabled);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int removePublishStreamUrl(string url);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getErrorDescription(int code);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLiveTranscoding(int width, int height, int videoBitrate, int videoFramerate, bool lowLatency, int videoGroup, int video_codec_profile, uint backgroundColor, uint userCount, uint transcodingUserUid, int transcodingUsersX, int transcodingUsersY, int transcodingUsersWidth, int transcodingUsersHeight, int transcodingUsersZorder, double transcodingUsersAlpha, int transcodingUsersAudioChannel, string transcodingExtraInfo, string metaData, string watermarkRtcImageUrl, int watermarkRtcImageX, int watermarkRtcImageY, int watermarkRtcImageWidth, int watermarkRtcImageHeight, string backgroundImageRtcImageUrl, int backgroundImageRtcImageX, int backgroundImageRtcImageY, int backgroundImageRtcImageWidth, int backgroundImageRtcImageHeight, int audioSampleRate, int audioBitrate, int audioChannels, int audioCodecProfile);
        // video manager
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern bool createAVideoDeviceManager();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int releaseAVideoDeviceManager();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startVideoDeviceTest(IntPtr hwnd);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopVideoDeviceTest();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getVideoDeviceCollectionCount();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getVideoDeviceCollectionDevice(int index, IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setVideoDeviceCollectionDevice(string deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentVideoDevice(IntPtr deviceId);
        // audio recording device manager
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern bool creatAAudioRecordingDeviceManager();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int releaseAAudioRecordingDeviceManager();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioRecordingDeviceCount();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioRecordingDevice(int index, IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setAudioRecordingDevice(string deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setAudioRecordingDeviceVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioRecordingDeviceVolume();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setAudioRecordingDeviceMute(bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern bool isAudioRecordingDeviceMute();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentRecordingDeviceInfo(IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentRecordingDevice(IntPtr deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startAudioRecordingDeviceTest(int indicationInterval);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioRecordingDeviceTest();

        //audio playback device manager
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioPlaybackDeviceCount();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern bool creatAAudioPlaybackDeviceManager();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int releaseAAudioPlaybackDeviceManager();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioPlaybackDevice(int index, IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setAudioPlaybackDevice(string deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setAudioPlaybackDeviceVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioPlaybackDeviceVolume();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setAudioPlaybackDeviceMute(bool mute);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern bool isAudioPlaybackDeviceMute();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startAudioPlaybackDeviceTest(string testAudioFilePath);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopAudioPlaybackDeviceTest();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentPlaybackDeviceInfo(IntPtr deviceName, IntPtr deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getCurrentPlaybackDevice(IntPtr deviceId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int pushVideoFrame(int type, int format, byte[] videoBuffer, int stride, int height, int cropLeft, int cropTop, int cropRight, int cropBottom, int rotation, long timestamp);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setExternalVideoSource(bool enable, bool useTexture);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setExternalAudioSource(bool enabled, int sampleRate, int channels);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int pushAudioFrame_(int audioFrameType, int samples, int bytesPerSample, int channels, int samplesPerSec, byte[] buffer, long renderTimeMs, int avsync_type);

        // [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        // protected static extern int pushAudioFrame2_(int mediaSourceType, int audioFrameType, int samples, int bytesPerSample, int channels, int samplesPerSec, byte[] buffer, long renderTimeMs, int avsync_type, bool wrap);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int registerVideoRawDataObserver();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int unRegisterVideoRawDataObserver();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int registerAudioRawDataObserver();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int unRegisterAudioRawDataObserver();
        // render
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setRenderMode(int renderMode);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingPlayoutVolume();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getAudioMixingPublishVolume();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoiceChanger(int voiceChanger);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoiceReverbPreset(int audioReverbPreset);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableSoundPositionIndication(bool enabled);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoiceEqualization(int bandFrequency, int bandGain);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLocalVoiceReverb(int reverbKey, int value);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setCameraCapturerConfiguration(int cameraCaptureConfiguration, int cameraDirection);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setRemoteUserPriority(uint uid, int userPriority);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setLogFileSize(uint fileSizeInKBytes);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setExternalAudioSink(bool enabled, int sampleRate, int channels);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int pullAudioFrame_(IntPtr audioBuffer, int type, int samples, int bytesPerSample, int channels, int samplesPerSec, long renderTimeMs, int avsync_type);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startLastmileProbeTest(bool probeUplink, bool probeDownlink, uint expectedUplinkBitrate, uint expectedDownlinkBitrate);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopLastmileProbeTest();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int configPublisher(int width, int height, int framerate, int bitrate, int defaultLayout, int lifecycle, bool owner, int injectStreamWidth, int injectStreamHeight, string injectStreamUrl, string publishUrl, string rawStreamUrl, string extraInfo);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int addVideoWatermark(string url, int x, int y, int width, int height);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int addVideoWatermark2(string watermarkUrl, bool visibleInPreview, int positionInLandscapeX, int positionInLandscapeY, int positionInLandscapeWidth, int positionInLandscapeHeight, int positionInPortraitX, int positionInPortraitY, int positionInPortraitWidth, int positionInPortraitHeight);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int clearVideoWatermarks();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int registerLocalUserAccount(string appId, string userAccount);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int joinChannelWithUserAccount(string token, string channelId, string userAccount);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int getUserInfoByUserAccount(string userAccount);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern IntPtr getUserInfoByUid(uint uid);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setBeautyEffectOptions(bool enabled, int lighteningContrastLevel, float lighteningLevel, float smoothnessLevel, float rednessLevel);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setInEarMonitoringVolume(int volume);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startScreenCaptureByDisplayId(uint displayId, int x, int y, int width, int height, int screenCaptureVideoDimenWidth, int screenCaptureVideoDimenHeight, int screenCaptureFrameRate, int screenCaptureBitrate, bool screenCaptureCaptureMouseCursor);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startScreenCaptureByScreenRect(int screenRectX, int screenRectY, int screenRectWidth, int screenRectHeight, int regionRectX, int regionRectY, int regionRectWidth, int regionRectHeight, int screenCaptureVideoDimenWidth, int screenCaptureVideoDimenHeight, int screenCaptureFrameRate, int screenCaptureBitrate, bool screenCaptureCaptureMouseCursor);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setScreenCaptureContentHint(int videoContentHint);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int updateScreenCaptureParameters(int screenCaptureVideoDimenWidth, int screenCaptureVideoDimenHeight, int screenCaptureFrameRate, int screenCaptureBitrate, bool screenCaptureCaptureMouseCursor);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int updateScreenCaptureRegion(int x, int y, int width, int height);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopScreenCapture();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int addInjectStreamUrl(string url, int width, int height, int videoGop, int videoFramerate, int videoBitrate, int audioSampleRate, int audioBitrate, int audioChannels);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int removeInjectStreamUrl(string url);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int enableLoopbackRecording(bool enabled, string deviceName);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setAudioSessionOperationRestriction(int restriction);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int switchChannel(string token, string channelId);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startChannelMediaRelay(string srcChannelName, string srcToken, uint srcUid, string destChannelName, string destToken, uint destUid, int destCount);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int updateChannelMediaRelay(string srcChannelName, string srcToken, uint srcUid, string destChannelName, string destToken, uint destUid, int destCount);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int stopChannelMediaRelay();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int sendMetadata(uint uid, uint size, byte[] buffer, long timeStampMs);

        // [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        // protected static extern int sendAudioPacket(byte[] buffer, uint size);

        // [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        // protected static extern int sendVideoPacket(byte[] buffer, uint size);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int registerPacketObserver();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int unRegisterPacketObserver();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int registerMediaMetadataObserver(int metaDataType);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int unRegisterMediaMetadataObserver();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setMirrorApplied(bool wheatherApply);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int sendMetadata(uint uid, uint size, string buffer, long timeStamps);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int startScreenCaptureByWindowId(int windowId, int regionRectX, int regionRectY, int regionRectWidth, int regionRectHeight, int screenCaptureVideoDimenWidth, int screenCaptureVideoDimenHeight, int screenCaptureFrameRate, int screenCaptureBitrate, bool screenCaptureCaptureMouseCursor);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern int setDefaultEngineSettings();

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnEngineCallback(
			OnJoinChannelSuccessHandler OnJoinChannelSuccess,
            OnReJoinChannelSuccessHandler OnReJoinChannelSuccess,
            OnConnectionLostHandler OnConnectionLost,
            EngineEventOnLeaveChannelHandler OnLeaveChannel,
            OnConnectionInterruptedHandler OnConnectionInterrupted,
            OnRequestTokenHandler OnRequestToken,
            OnUserJoinedHandler OnUserJoined,
            EngineEventOnUserOfflineHandler OnUserOffline,
            EngineEventOnAudioVolumeIndicationHandler OnAudioVolumeIndication,
            OnUserMutedAudioHandler OnUserMuteAudio,
            OnSDKWarningHandler OnSDKWarning,
            OnSDKErrorHandler OnSDKError,
            EngineEventOnRtcStatsHandler OnRtcStats,
            OnAudioMixingFinishedHandler OnAudioMixingFinished,
            EngineEventOnAudioRouteChangedHandler OnAudioRouteChanged,
            OnFirstRemoteVideoDecodedHandler OnFirstRemoteVideoDecoded,
            OnVideoSizeChangedHandler OnVideoSizeChanged,
            EngineEventOnClientRoleChanged onClientRolteChanged,
            OnUserMuteVideoHandler OnUserMuteVideo,
            OnMicrophoneEnabledHandler OnMicrophoneEnabled,
            OnApiExecutedHandler OnApiExecuted,
            OnFirstLocalAudioFrameHandler OnFirstLocalAudioFrame,
            OnFirstRemoteAudioFrameHandler OnFirstRemoteAudioFrame,
            OnLastmileQualityHandler OnLastmileQuality,
            OnAudioQualityHandler onAudioQuality,
            OnStreamInjectedStatusHandler onStreamInjectedStatus,
            OnStreamUnpublishedHandler onStreamUnpublished,
            OnStreamPublishedHandler onStreamPublished,
            OnStreamMessageErrorHandler onStreamMessageError,
            OnStreamMessageHandler onStreamMessage,
            OnConnectionBannedHandler onConnectionBanned,
            OnVideoStoppedHandler OnVideoStopped,
            OnTokenPrivilegeWillExpireHandler onTokenPrivilegeWillExpire,
            OnNetworkQualityHandler onNetworkQuality,
            EngineEventOnLocalVideoStatsHandler onLocalVideoStats,
            EngineEventOnRemoteVideoStatsHandler onRemoteVideoStats,
            EngineEventOnRemoteAudioStatsHandler onRemoteAudioStats,
            OnFirstLocalVideoFrameHandler OnFirstLocalVideoFrame,
            OnFirstRemoteVideoFrameHandler OnFirstRemoteVideoFrame,
            OnUserEnableVideoHandler OnUserEnableVideo,
            OnAudioDeviceStateChangedHandler onAudioDeviceStateChanged,
            OnCameraReadyHandler onCameraReady,
            OnCameraFocusAreaChangedHandler onCameraFocusAreaChanged,
            OnCameraExposureAreaChangedHandler onCameraExposureAreaChanged,
            OnRemoteAudioMixingBeginHandler onRemoteAudioMixingBegin,
            OnRemoteAudioMixingEndHandler onRemoteAudioMixingEnd,
            OnAudioEffectFinishedHandler onAudioEffectFinished,
            OnVideoDeviceStateChangedHandler onVideoDeviceStateChanged,
            EngineEventOnRemoteVideoStateChanged OnRemoteVideoStateChanged,
            OnUserEnableLocalVideoHandler OnUserEnableLocalVideo,
            OnLocalPublishFallbackToAudioOnlyHandler OnLocalPublishFallbackToAudioOnly,
            OnRemoteSubscribeFallbackToAudioOnlyHandler onRemoteSubscribeFallbackToAudioOnly,
            EngineEventOnConnectionStateChanged onConnectionStateChanged,
            OnRemoteVideoTransportStatsHandler onRemoteVideoTransportStats,
            OnRemoteAudioTransportStatsHandler onRemoteAudioTransportStats,
            OnTranscodingUpdatedHandler onTranscodingUpdated,
            EngineEventOnAudioDeviceVolumeChangedHandler onAudioDeviceVolumeChanged,
            OnActiveSpeakerHandler onActiveSpeaker,
            OnMediaEngineStartCallSuccessHandler onMediaEngineStartCallSuccess,
            OnMediaEngineLoadSuccessHandler onMediaEngineLoadSuccess,
            EngineEventOnAudioMixingStateChangedHandler onAudioMixingStateChanged,
            OnFirstRemoteAudioDecodedHandler onFirstRemoteAudioDecoded,
            EngineEventOnLocalVideoStateChanged onLocalVideoStateChanged,
            EngineEventOnRtmpStreamingStateChangedHandler onRtmpStreamingStateChanged,
            EngineEventOnNetworkTypeChangedHandler onNetworkTypeChanged,
            EngineEventOnLastmileProbeResultHandler onLastmileProbeResult,
            OnLocalUserRegisteredHandler onLocalUserRegistered,
            EngineEventOnUserInfoUpdatedHandler onUserInfoUpdated,
            EngineEventOnLocalAudioStateChangedHandler onLocalAudioStateChanged,
            EngineEventOnRemoteAudioStateChangedHandler onRemoteAudioStateChanged,
            EngineEventOnLocalAudioStatsHandler onLocalAudioStats,
            EngineEventOnChannelMediaRelayStateChangedHandler onChannelMediaRelayStateChanged,
            EngineEventOnChannelMediaRelayEventHandler onChannelMediaRelayEvent);

        // audio and video raw data
        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnCaptureVideoFrame(EngineEventOnCaptureVideoFrame onCaptureVideoFrame);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnRenderVideoFrame(EngineEventOnRenderVideoFrame onRenderVideoFrame);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnRecordAudioFrame(EngineEventOnRecordAudioFrame onRecordAudioFrame);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnPlaybackAudioFrame(EngineEventOnPlaybackAudioFrame onPlaybackAudioFrame);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnMixedAudioFrame(EngineEventOnMixedAudioFrame onMixedAudioFrame);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnPlaybackAudioFrameBeforeMixing(EngineEventOnPlaybackAudioFrameBeforeMixing onPlaybackAudioFrameBeforeMixing);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnPullAudioFrame(EngineEventOnPullAudioFrameHandler onPullAudioFrame);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnPacketCallback(EngineEventOnReceiveAudioPacketHandler onReceiveAudioPacket, EngineEventOnReceiveVideoPacketHandler onReceiveVideoPacket, EngineEventOnSendAudioPacketHandler onSendAudioPacket, EngineEventOnSendVideoPacketHandler onSendVideoPacket);

        [DllImport(MyLibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        protected static extern void initEventOnMetaDataCallback(EngineEventOnMediaMetaDataReceived onMetadataReceived, EngineEventOnReadyToSendMetadata onReadyToSendMetadata, EngineEventOnGetMaxMetadataSize onGetMaxMetadataSize);
        #endregion engine callbacks   
    }
}
