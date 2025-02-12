namespace SnapTalk.Domain.Constants;

public static class UserNameColors
{
    private static readonly string[] Colors = 
    {
        "#F87171", // Red
        "#FACC15", // Yellow
        "#4ADE80", // Green
        "#60A5FA", // Blue
        "#A78BFA", // Purple
        "#F472B6", // Pink
        "#FB923C", // Orange
        "#14B8A6"  // Teal
    };
    
    public static string GetRandomColor()
    {
        Random random = new Random();
        return Colors[random.Next(Colors.Length)];
    }
}