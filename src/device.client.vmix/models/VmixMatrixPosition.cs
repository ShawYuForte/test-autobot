namespace forte.devices.models
{
    public class VmixMatrixPosition
    {
        public bool Mirror { get; set; }
        public int ZoomX { get; set; }
        public VmixMatrixOrigin RotateOrigin { get; set; }
        public VmixMatrixOrigin Rotate { get; set; }
        public int PanX { get; set; }
        public int PanY { get; set; }
        public VmixMatrixPosition MultiplyPosition { get; set; }
    }

    public class VmixMatrixOrigin
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}