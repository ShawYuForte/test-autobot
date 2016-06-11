#region

using System;
using System.Collections;
using System.Collections.Generic;
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
        private CloudMediaContext CreateContext()
        {
            var mediaServicesAccountName = AppSettings.Instance.AmsAccountName;
            var mediaServicesAccountKey = AppSettings.Instance.AmsAccountKey;

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

            return response.Result;
        }

        public AzureProgram FetchProgram()
        {
            var name = "live-stream-test";
            var context = CreateContext();
            var azureChannel = context.Channels.ToList().FirstOrDefault(c => c.Name == AppSettings.Instance.ChannelName);
            var program = context.Programs.ToList().FirstOrDefault(c => c.Name == name);
            var asset = program.Asset;

            return new AzureProgram
            {
                Name = program.Name,
                AssetId = program.AssetId,
                AssetName = program.Asset.Name,
                AssetUrl = program.Asset.Uri.ToString(),
                Id = program.Id,
                Running = program.State == ProgramState.Running
            };
        }

        private async Task<AzureProgram> CreateProgramAsync()
        {
            var context = CreateContext();
            var azureChannel = context.Channels.ToList().FirstOrDefault(c => c.Name == AppSettings.Instance.ChannelName);
            var trainerName = AppState.Instance.TrainerName.ToLower().Replace(" ", "");
            // Make sure asset is ready
            var assetName = $"{AppState.Instance.ClassStartTime.ToString("MMdd-HHmm")}-{trainerName}";
            var asset = await CreateAndConfigureAssetAsync(context, assetName);

            // Make sure program created
            var program = await azureChannel.Programs.CreateAsync(assetName, TimeSpan.FromHours(2), asset.Id);

            var locator = CreateLocatorForAsset(context, program.Asset, program.ArchiveWindowLength);
            var urls = GetLocatorsInAllStreamingEndpoints(context, asset);
            string publishUrl = null;
            if (urls != null)
            {
                publishUrl = urls.FirstOrDefault()?.ToString();
            }

            return new AzureProgram
            {
                AssetName = asset.Name,
                AssetUrl = asset.Uri.ToString(),
                Name = program.Name,
                Id = program.Id,
                AssetId = program.Asset.Id,
                PublishUrl = publishUrl
            };
        }

        public static IList<Uri> GetLocatorsInAllStreamingEndpoints(CloudMediaContext context, IAsset asset)
        {
            var locators = asset.Locators.Where(l => l.Type == LocatorType.OnDemandOrigin);
            var ismFile = asset.AssetFiles.AsEnumerable().FirstOrDefault(a => a.Name.EndsWith(".ism"));
            var template = new UriTemplate("{contentAccessComponent}/{ismFileName}/manifest");
            var urls = locators.SelectMany(l =>
                        context
                            .StreamingEndpoints
                            .AsEnumerable()
                            .Where(se => se.State == StreamingEndpointState.Running)
                            .Select(
                                se =>
                                    template.BindByPosition(new Uri("http://" + se.HostName),
                                    l.ContentAccessComponent,
                                        ismFile.Name)))
                        .ToArray();
            return urls;
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

            //asset.DeliveryPolicies.Add(policy);

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
                var channel = context.Channels.ToList().FirstOrDefault(ch => ch.Name == AppSettings.Instance.ChannelName);
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
            var channel = context.Channels.ToList().FirstOrDefault(ch => ch.Name == AppSettings.Instance.ChannelName);
            return channel.State == ChannelState.Running;
        }

        public void StartChannel()
        {
            var context = CreateContext();
            var channel = context.Channels.ToList().FirstOrDefault(ch => ch.Name == AppSettings.Instance.ChannelName);
            if (channel.State == ChannelState.Stopped) channel.StartAsync().Wait();
        }

        public bool StopChannel()
        {
            var context = CreateContext();
            var channel = context.Channels.ToList().FirstOrDefault(ch => ch.Name == AppSettings.Instance.ChannelName);
            if (channel.State == ChannelState.Running)
            {
                if (channel.Programs.ToList().Count(program => program.State != ProgramState.Stopped) > 0)
                {
                    Log("Can't shut down your channel, there are other programs running on it!");
                    return false;
                } 
                channel.StopAsync().Wait();
            }
            return true;
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

        public bool ThereAreProgramsRunning()
        {
            var context = CreateContext();
            var channel = context.Channels.ToList().FirstOrDefault(ch => ch.Name == AppSettings.Instance.ChannelName);
            return channel.Programs.ToList().Count(program => program.State != ProgramState.Stopped) > 0;
        }
    }
}