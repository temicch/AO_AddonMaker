﻿using System.IO;
using Application.BL.Files;
using Application.BL.Services.SamplesProvider.Services;
using Application.BL.Widgets;
using Xunit;
using static Application.Tests.InitData;

namespace Application.Tests
{
    public class FileManagerTests : IClassFixture<InitFixture>
    {
        public FileManagerTests(InitFixture initFixture)
        {
            InitFixture = initFixture;
        }

        public InitFixture InitFixture { get; }

        public IFileManager FileManager => InitFixture.FileManager;
        public SamplesProviderService SamplesProvider => InitFixture.SamplesProvider;

        [Fact]
        public void Count_Of_Files_Must_Be_Equal_To_Predefined_Values()
        {
            foreach (var file in InitFixture.UiAddons)
            {
                FileManager.Clear();
                FileManager.Load(file);
                var addonName = Path.GetFileName(Path.GetDirectoryName(file));
                Assert.Equal(AddonFilesCount[addonName], FileManager.Count);
            }
        }

        [Fact]
        public void Count_Of_UiElements_Must_Be_Equal_To_Predefined_Values()
        {
            foreach (var file in InitFixture.UiAddons)
            {
                FileManager.Clear();
                FileManager.Load(file);
                var addonName = Path.GetFileName(Path.GetDirectoryName(file));
                Assert.Equal(AddonUiElementsCount[addonName], (FileManager.RootFile as IUIElement).ChildrenCount);
            }
        }
    }
}