namespace Matte
{
    public class Meta
    {
        public string Label { get; set; }
        public byte[] Sha256 { get; set; }
        public int SliceSize { get; set; }
        public int TotalBytes { get; set; }
    }
}