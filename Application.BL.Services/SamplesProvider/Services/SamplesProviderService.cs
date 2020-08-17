using Application.BL.Services.SamplesProvider.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Application.BL.Services.SamplesProvider.Services
{
    public class SamplesProviderService
    {
        private const string AddonDescName = "AddonDesc.(UIAddon).xdb";

        private readonly string samplesPath =
            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Samples";

        public SamplesProviderService(ILogger<SamplesProviderService> logger)
        {
            Samples = new ObservableCollection<SampleModel>();
            var previousDirectory = Directory.GetCurrentDirectory();
            try
            {
                foreach (var path in Directory.GetDirectories(samplesPath))
                {
                    var addonDesc = $"{path}{Path.DirectorySeparatorChar}{AddonDescName}";

                    if (!File.Exists(addonDesc))
                        continue;

                    var name = Directory.GetParent(addonDesc).Name;

                    Samples.Add(new SampleModel
                    {
                        Name = name,
                        FullPath = addonDesc
                    });
                }
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception.Message);
            }

            Directory.SetCurrentDirectory(previousDirectory);
        }

        public ObservableCollection<SampleModel> Samples { get; set; }
    }
}