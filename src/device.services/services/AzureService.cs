﻿#region

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using forte.device.extensions;
using forte.device.models;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.MediaServices.Client.DynamicEncryption;

#endregion

namespace forte.device.services
{
    public class AzureService : Service
    {
        private string _mediaServicesEnableEncoding;

        private CloudMediaContext CreateContext()
        {
            var mediaServicesAccountName = AppState.Instance.AmsAccountName;
            var mediaServicesAccountKey = AppState.Instance.AmsAccountKey;
            _mediaServicesEnableEncoding = AppState.Instance.AmsEncoding;

            // Create and cache the Media Services credentials in a static class variable.
            var cachedCredentials = new MediaServicesCredentials(
                mediaServicesAccountName,
                mediaServicesAccountKey);

            // Used the cached credentials to create CloudMediaContext.
            var context = new CloudMediaContext(cachedCredentials);
            return context;
        }

        public AzureProgram CreateProgram()
        {
            var response = CreateProgramAsync();
            response.Wait();

            return new AzureProgram
            {
                AssetName = response.Result.Asset.Name,
                AssetUrl = response.Result.Asset.Uri.ToString(),
                Name = response.Result.Name,
                Id = response.Result.Id,
                AssetId = response.Result.Asset.Id
            };
        }

        private async Task<IProgram> CreateProgramAsync()
        {
            var context = CreateContext();
            var azureChannel = context.Channels.ToList().FirstOrDefault(c => c.Name == AppState.Instance.ChannelName);
            var trainerName = AppState.Instance.TrainerName.ToLower().Replace(" ", "");
            // Make sure asset is ready
            var assetName = $"{AppState.Instance.ClassStartTime.ToString("ddMMyy-HHmm")}-{trainerName}";
            var asset = await CreateAndConfigureAssetAsync(context, assetName);

            // Make sure program created
            var options = GetOptionsForProgramCreation(assetName, asset.Id);
            var program = context.Programs.ToList().FirstOrDefault(c => c.Name == options.Name) ??
                          await azureChannel.Programs.CreateAsync(options);

            var locator = CreateLocatorForAsset(context, program.Asset, program.ArchiveWindowLength);

            return program;
        }

        /// <summary>
        ///     Create an asset and configure asset delivery policies.
        /// </summary>
        /// <returns></returns>
        private async Task<IAsset> CreateAndConfigureAssetAsync(CloudMediaContext context, string assetName)
        {
            var asset = await context.Assets.CreateAsync(assetName, AssetCreationOptions.None, CancellationToken.None);

            var policy = await context.AssetDeliveryPolicies.CreateAsync(
                "Clear Policy",
                AssetDeliveryPolicyType.NoDynamicEncryption,
                AssetDeliveryProtocol.HLS | AssetDeliveryProtocol.SmoothStreaming | AssetDeliveryProtocol.Dash,
                null);

            asset.DeliveryPolicies.Add(policy);

            return asset;
        }

        private IProgram FindAzureProgram(CloudMediaContext context, string azureProgramId)
        {
            if (string.IsNullOrEmpty(azureProgramId)) return null;

            try
            {
                var program = context.Programs.ToList().FirstOrDefault(c => c.Id == azureProgramId);
                return program;
            }
            catch (Exception ex)
            {
                var exception = MediaServicesExceptionParser.Parse(ex);
                return null;
            }
        }

        /// <summary>
        ///     Create locators in order to be able to publish and stream the video.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="asset"></param>
        /// <param name="archiveWindowLength"></param>
        /// <returns></returns>
        private static ILocator CreateLocatorForAsset(CloudMediaContext context, IAsset asset,
            TimeSpan archiveWindowLength)
        {
            // You cannot create a streaming locator using an AccessPolicy that includes write or delete permissions.            
            var locator = context.Locators.CreateLocator
                (
                    LocatorType.OnDemandOrigin,
                    asset,
                    context.AccessPolicies.Create
                        (
                            "Live Stream Policy",
                            archiveWindowLength,
                            AccessPermissions.Read
                        )
                );

            return locator;
        }

        private static ProgramCreationOptions GetOptionsForProgramCreation(string programName, string assetId)
        {
            var options = new ProgramCreationOptions
            {
                Name = programName.Slugify(),
                Description = programName,
                // TODO: consider to make archive window length configurable 
                ArchiveWindowLength = TimeSpan.FromHours(2),
                AssetId = assetId
            };
            return options;
        }

        public bool VerifySettings()
        {
            try
            {
                var context = CreateContext();
                var channel = context.Channels.ToList().FirstOrDefault(ch => ch.Name == AppState.Instance.ChannelName);
                return channel != null;
            }
            catch (Exception exception)
            {
                Log($"ERROR: {exception.Message}");
                return false;
            }
        }

        public bool IsChannelRunning()
        {
            var context = CreateContext();
            var channel = context.Channels.ToList().FirstOrDefault(ch => ch.Name == AppState.Instance.ChannelName);
            return channel.State == ChannelState.Running;
        }

        public void StartChannel()
        {
            var context = CreateContext();
            var channel = context.Channels.ToList().FirstOrDefault(ch => ch.Name == AppState.Instance.ChannelName);
            if (channel.State == ChannelState.Stopped) channel.StartAsync().Wait();
        }

        public void StopChannel()
        {
            var context = CreateContext();
            var channel = context.Channels.ToList().FirstOrDefault(ch => ch.Name == AppState.Instance.ChannelName);
            if (channel.State == ChannelState.Running) channel.StopAsync().Wait();
        }

        public void StartProgram()
        {
            var context = CreateContext();
            var program = FindAzureProgram(context, AppState.Instance.CurrentProgram.Id);
            program.StartAsync().Wait();
        }

        public void StopProgram()
        {
            var context = CreateContext();
            var program = FindAzureProgram(context, AppState.Instance.CurrentProgram.Id);
            program.StopAsync().Wait();
        }
    }
}