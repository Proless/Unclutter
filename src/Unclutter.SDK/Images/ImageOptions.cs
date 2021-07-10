namespace Unclutter.SDK.Images
{
    public class ImageOptions
    {
        public bool IsDefaultSize => Width == 0 && Height == 0;
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
