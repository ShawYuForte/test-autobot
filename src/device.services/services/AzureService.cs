#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using forte.device.extensions;
using forte.device.models;
using Microsoft.WindowsAzure.MediaServices.Client;

#endregion

namespace forte.device.services
{
    public class AzureService : Service
    {
        private const double DaysInTenYears = 3650;

        public static AzureService Instance { get; } = new AzureService();

        private AzureService() : base("Azure")
        {
        }

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
            // Make sure asset is ready
            var studioAcronym = ToAcronym(AppSettings.Instance.StudioName);
            var className = AppState.Instance.ClassName.Replace(" ", "");
            var assetName = $"{AppState.Instance.ClassStartTime.ToString("MMdd-HHmm")}-{studioAcronym}{className}";
            var asset = await CreateAndConfigureAssetAsync(context, assetName);

            // Make sure program created
            var program = await azureChannel.Programs.CreateAsync(asset.Name, TimeSpan.FromHours(2), asset.Id);

            var locator = CreateLocatorForAsset(context, program.Asset, TimeSpan.FromDays(DaysInTenYears));
            var publishUrl = GetFirstLocatorUrl(context, asset);

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

        private static string ToAcronym(string input)
        {
            var buffer = new StringBuilder();

            var words = input.Split(' ');

            foreach (var word in words)
            {
                if (string.IsNullOrWhiteSpace(word)) continue;
                buffer.Append(word.ToUpper().Substring(0, 1));
            }

            return buffer.ToString();
        }

        public static string GetFirstLocatorUrl(CloudMediaContext context, IAsset asset)
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
            return urls.FirstOrDefault()?.ToString();
        }

        /// <summary>
        ///     Create an asset and configure asset delivery policies.
        /// </summary>
        /// <returns></returns>
        private async Task<IAsset> CreateAndConfigureAssetAsync(CloudMediaContext context, string assetName)
        {
            var safeAssetName = assetName;
            var counter = 1;
            while (context.Assets.ToList().Any(a => a.Name == safeAssetName))
            {
                safeAssetName = $"{assetName}-{counter++}";
            }

            var asset =
                await context.Assets.CreateAsync(safeAssetName, AssetCreationOptions.None, CancellationToken.None);

            //var policy = await context.AssetDeliveryPolicies.CreateAsync(
            //    "Clear Policy",
            //    AssetDeliveryPolicyType.NoDynamicEncryption,
            //    AssetDeliveryProtocol.HLS | AssetDeliveryProtocol.SmoothStreaming | AssetDeliveryProtocol.Dash,
            //    null);

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
                var channel = context.Channels.ToList()
                    .FirstOrDefault(ch => ch.Name == AppSettings.Instance.ChannelName);
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
            // ReSharper disable once RemoveToList.2
            return channel.Programs.ToList().Count(program => program.State != ProgramState.Stopped) > 0;
        }

        /// <summary>
        ///     Process generated asset to multiple bitrates (adaptive)
        /// </summary>
        /// <returns></returns>
        public bool ProcessAsset()
        {
            var context = CreateContext();
            IMediaProcessor processor = null;
            IAsset asset = null;
            IJob job = null;
            var processorName = "Media Encoder Standard";

            if (!TryExecute(() => processor = context.MediaProcessors.Where(p => p.Name == processorName).
                ToList().OrderBy(p => new Version(p.Version)).LastOrDefault())) return false;

            if (!TryExecute(() => asset =
                    context.Assets.ToList()
                        .FirstOrDefault(a => a.Id == AppState.Instance.CurrentProgram.AssetId)))
                return false;

            var jobName = $"{processorName} processing of {asset.Name}";
            if (!TryExecute(() => job = context.Jobs.Create(jobName, 10))) return false;

            string taskName = $"{processorName} v{processor.Version} processing {asset.Name}";

            var task = job.Tasks.AddNew(taskName, processor, "H264 Multiple Bitrate 720p", TaskOptions.None);
            task.InputAssets.Add(asset);

            // Add an output asset to contain the results of the job.  
            string assetName = $"{asset.Name}-Adaptive";
            task.OutputAssets.AddNew(assetName, AssetCreationOptions.None);

            if (!TryExecute(() => job.Submit(), "Submitting Azure Job")) return false;

            TryExecute(() =>
            {
                var outputAsset = context.Assets.ToList().First(a => a.Name == assetName);
                CreateLocatorForAsset(context, outputAsset, TimeSpan.FromDays(DaysInTenYears));
                //AppState.Instance.CurrentProgram.PublishUrl = GetFirstLocatorUrl(context, outputAsset);
            }, "Creating publish url for adaptive asset.");

            return true;
        }
    }
}