namespace ABOBAEngine;

internal static class Program
{
    public static void Main(string[] args)
    {
        using (ApplicationWindow window = new ApplicationWindow(600, 600, "RealEngine"))
        {
            window.Run();
        }
    }
}