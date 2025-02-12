using SkiaSharp;
using SnapTalk.BLL.Helpers;

namespace SnapTalk.BLL.Interfaces;

public class AvatarService(IBlobService blobService) : IAvatarService
{
    private const int DefaultAvatarSize = 100;
    public byte[] GenerateAvatar(string avatarText, string color)
    {
        return GenerateAvatar(avatarText, color, DefaultAvatarSize);
    }
    
    private byte[] GenerateAvatar(string avatarText, string color, int size)
    {
        // Ensure text is uppercase and max 2 characters
        var text = avatarText.ToUpper().Substring(0, Math.Min(2, avatarText.Length));

        var bitmap = new SKBitmap(size, size);
        using var canvas = new SKCanvas(bitmap);

        // Background color
        var backgroundPaint = new SKPaint
        {
            Color = SKColor.Parse(color),
            IsAntialias = true
        };

        // Draw Square Background
        canvas.DrawRect(0, 0, size, size, backgroundPaint);

        // Text settings
        using var font = new SKFont(SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold), size / 3);
        using var textPaint = new SKPaint
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
        using var img = SKImage.FromBitmap(bitmap);
        using var data = img.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

}