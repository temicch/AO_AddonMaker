using System.Collections.Generic;
using System.IO;
using Application.BL.Texture;
using Xunit;

namespace Application.Tests;

public class TextureTests : IClassFixture<InitFixture>
{
    public TextureTests(InitFixture initFixture)
    {
        InitFixture = initFixture;
    }

    public InitFixture InitFixture { get; }

    public IList<string> BinFiles => InitFixture.BinFiles;

    [Fact]
    public void All_Samples_Textures_Must_Be_Created_Successfully()
    {
        foreach (var filePath in BinFiles)
        {
            Texture texture;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                texture = new Texture(stream, 10, 10, Format.DXT5);
                Assert.NotNull(texture);
            }

            Assert.NotNull(texture.Bitmap);
        }
    }

    [Fact]
    public void Wrong_Texture_Must_Throw_An_Exception()
    {
        using var stream = new MemoryStream(new byte[1024]);
        Assert.NotNull(Record.Exception(() => new Texture(stream, 0, 0, Format.DXT5)));
    }
}
