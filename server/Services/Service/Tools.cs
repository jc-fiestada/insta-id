using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace InstaId.Services.Service;

public class Tools
{
    public async Task<byte[]> OptimizeImage(IFormFile image)
    {
        using var stream = image.OpenReadStream();
        using var img = await Image.LoadAsync(stream);

        img.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(300, 300),
            Mode = ResizeMode.Crop
        }));

        using var output = new MemoryStream();

        await img.SaveAsJpegAsync(output, new JpegEncoder
        {
            Quality = 70
        });

        return output.ToArray();
    }
}