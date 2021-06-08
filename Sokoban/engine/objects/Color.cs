namespace Sokoban.engine.objects
{
    internal struct Color
    {
        public static readonly Color Black = new(0, 0, 0, 1);
        public static readonly Color White = new(1, 1, 1, 1);
        public static readonly Color Gray = new(0.2f, 0.2f, 0.2f, 1);

        public Color(float r = 0, float g = 0, float b = 0, float a = 1)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public override string ToString()
        {
            return $"Color(<c9 {R:F2}|> <c10 {G:F2}|> <c12 {B:F2}|> <c15 {A:F2}|>)";
        }
    }
}
