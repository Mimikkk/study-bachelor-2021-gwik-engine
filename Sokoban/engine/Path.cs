using System;
using System.IO;

namespace GWiK_Sokoban.engine
{
    public class Path
    {
        private string Str { get; }

        private static readonly Path Working = new(Environment.CurrentDirectory);
        private static readonly Path Platform = Working / "..";
        private static readonly Path Bin = Platform / "..";
        private static readonly Path Project = Bin / "..";
        private static readonly Path Resources = Project / "resources";
        public static readonly Path Shaders = Resources / "shaders";
        public static readonly Path Textures = Resources / "textures";
        public static readonly Path Objects = Resources / "objects";

        private Path(string path)
        {
            Str = path;
        }

        public static Path operator /(Path a, Path b)
        {
            return new($"{a}/{b}");
        }

        public static Path operator /(Path a, string b)
        {
            return b switch
            {
                ".."   => new Path(Directory.GetParent(a.ToString())?.FullName),
                "../"  => new Path(Directory.GetParent(a.ToString())?.FullName),
                "..\\" => new Path(Directory.GetParent(a.ToString())?.FullName),
                "."    => a,
                "./"   => a,
                ".\\"  => a,
                _      => new Path($"{a}\\{b}")
            };
        }

        public override string ToString()
        {
            return Str;
        }

        public bool IsFile()
        {
            return File.Exists(Str);
        }

        public string LoadFileToString()
        {
            if (!IsFile()) throw new Exception($"File Loading from Path {Str} failed. Path is not pointing to File");
            return File.ReadAllText(Str);
        }

        public StreamReader LoadFileToStream()
        {
            if (!IsFile())
                throw new Exception($"Loading File Stream from Path {Str} failed. Path is not pointing to File");
            return new StreamReader(Str);
        }
    }
}
