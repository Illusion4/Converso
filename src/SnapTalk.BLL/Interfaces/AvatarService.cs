using SkiaSharp;
using SnapTalk.BLL.Helpers;

namespace SnapTalk.BLL.Interfaces;

public class AvatarService(IBlobService blobService) : IAvatarService
{
    public byte[] GenerateAvatar(string avatarText, string color)
    {
        byte[] avatarBytes = GenerateAvatar(avatarText, color, 100);
        var uniqueFileName = FileNameHelper.CreateUniqueFileName($"{avatarText}.png");

        return avatarBytes;
    }
    
    private byte[] GenerateAvatar(string avatarText, string color, int size)
    {
        // Ensure text is uppercase and max 2 characters
        string text = avatarText.ToUpper().Substring(0, Math.Min(2, avatarText.Length));

        SKBitmap bitmap = new SKBitmap(size, size);
        using SKCanvas canvas = new SKCanvas(bitmap);

        // Background color
        SKPaint backgroundPaint = new SKPaint
        {
            Color = SKColor.Parse(color), // Example blue
            IsAntialias = true
        };

        // Draw Square Background
        canvas.DrawRect(0, 0, size, size, backgroundPaint);

        // Text settings
        using SKFont font = new SKFont(SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold), size / 3);
        using SKPaint textPaint = new SKPaint
        {
            Color = SKColors.White,
            IsAntialias = true
        };

        // Measure text size
        SKRect textBounds;
        float textWidth = font.MeasureText(text, out textBounds, textPaint);

        // Calculate position to center text
        float x = (size - textWidth) / 2;
        float y = (size + textBounds.Height) / 2;

        // Draw text
        canvas.DrawText(text, x, y, SKTextAlign.Left, font, textPaint);

        // Convert to PNG byte array
        using SKImage img = SKImage.FromBitmap(bitmap);
        using SKData data = img.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

}