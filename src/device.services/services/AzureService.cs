using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using forte.device.models;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.MediaServices.Client.DynamicEncryption;
using forte.device.extensions;

namespace forte.device.services
{
    public class AzureService
    {
        private readonly string _mediaServicesEnableEncoding;
        private readonly CloudMediaContext _context;

        public AzureService()
        {
            var mediaServicesAccountName = AppState.Instance.AmsAccountName;
            var mediaServicesAccountKey = AppState.Instance.AmsAccountKey;
            _mediaServicesEnableEncoding = AppState.Instance.AmsEncoding;

            // Create and cache the Media Services credentials in a static class variable.
            var cachedCredentials = new MediaServicesCredentials(
                mediaServicesAccountName,
                mediaServicesAccountKey);

            // Used the cached credentials to create CloudMediaContext.
            _context = new CloudMediaContext(cachedCredentials);
        }

        public async Task<IProgram> CreateProgramAsync()
        {
            var azureChannel = _context.Channels.ToList().FirstOrDefault(c => c.Name == AppState.Instance.ChannelName);
            var trainerName = AppState.Instance.TrainerName.ToLower().Replace(" ", "");
            // Make sure asset is ready
            var assetName = $"{DateTime.Now.ToString("ddMMyy-HHmm")}-{trainerName}";
            var asset = await CreateAndConfigureAssetAsync(assetName);

            // Make sure program created
            var options = GetOptionsForProgramCreation(assetName, asset.Id);
            var azureProgram = _context.Programs.ToList().FirstOrDefault(c => c.Name == options.Name) ??
                           await azureChannel.Programs.CreateAsync(options);

            // Make sure channel is running
            if (azureProgram.State != ProgramState.Running)
            {
                await azureProgram.StartAsync();
            }

            return azureProgram;
        }

        private async Task<IAsset> CreateAndConfigureAssetAsync(string assetName)
        {
            var asset = await _context.Assets.CreateAsync(assetName, AssetCreationOptions.None, CancellationToken.None);

            var policy = await _context.AssetDeliveryPolicies.CreateAsync(
                "Clear Policy",
                AssetDeliveryPolicyType.NoDynamicEncryption,
                AssetDeliveryProtocol.HLS | AssetDeliveryProtocol.SmoothStreaming | AssetDeliveryProtocol.Dash,
                null);

            // TODO: adding policy was commented - why? 
            // asset.DeliveryPolicies.Add(policy);

            return asset;
        }

        private IProgram FindAzureProgram(string azureProgramId)
        {
            if (string.IsNullOrEmpty(azureProgramId)) return null;

            try
            {
                var program = _context.Programs.ToList().FirstOrDefault(c => c.Id == azureProgramId);
                return program;
            }
            catch (Exception ex)
            {
                var exception = MediaServicesExceptionParser.Parse(ex);
                return null;
            }
        }

        private static ProgramCreationOptions GetOptionsForProgramCreation(string programName, string assetId)
        {
            var options = new ProgramCreationOptions
            {
                Name = programName.Slugify(),
                Description = programName,
                // TODO: consider to make archive window length configurable 
                ArchiveWindowLength = TimeSpan.FromHours(3),
                AssetId = assetId
            };
            return options;
        }
    }
}
