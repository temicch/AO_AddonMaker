using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.BL.Files;
using Application.BL.Services.SamplesProvider.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests;

public class InitFixture : IDisposable
{
    private readonly string _samplesFolder;

    public InitFixture()
    {
        _samplesFolder = SamplesProviderService.SamplesDirectoryPath;

        SamplesProvider = new SamplesProviderService(new Mock<ILogger<SamplesProviderService>>().Object);

        XdbFiles = new List<string>(Directory.GetFiles(_samplesFolder, "*.xdb", SearchOption.AllDirectories));
        BinFiles = new List<string>(Directory.GetFiles(_samplesFolder, "*.bin", SearchOption.AllDirectories));
        UiAddons = XdbFiles.Select(x => x).Where(x => x.EndsWith("(UIAddon).xdb")).ToList();

        FileManager = new FileManager(new Mock<ILogger<FileManager>>().Object);
    }

    public IList<string> XdbFiles { get; }
    public IList<string> BinFiles { get; }

    public IList<string> UiAddons { get; }

    public IFileManager FileManager { get; }

    public SamplesProviderService SamplesProvider { get; }

    public void Dispose()
    {
        XdbFiles.Clear();
        BinFiles.Clear();
    }
}
