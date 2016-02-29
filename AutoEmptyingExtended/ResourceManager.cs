using System.Reflection;
using ColossalFramework.UI;
using UnityEngine;

namespace AutoEmptyingExtended
{
    public class ResourceManager
    {
        private static ResourceManager _instance;
        private readonly string _assemblyPath;
        private readonly string[] _spriteNames;
        private UITextureAtlas _atlas;
        
        private ResourceManager()
        {
            _assemblyPath = $"{Assembly.GetExecutingAssembly().GetName().Name}.Resources.";

            _spriteNames = new[] {
                "ClockIcon"
                        };
        }

        public static ResourceManager Instance => _instance ?? (_instance = new ResourceManager());

        public UITextureAtlas Atlas => _atlas ?? (_atlas = CreateTextureAtlas("AutoEmptyingUI", UIView.GetAView().defaultAtlas.material, _spriteNames));

        private UITextureAtlas CreateTextureAtlas(string atlasName, Material baseMaterial, string[] spriteNames)
        {
            const int size = 1024;

            var atlasTex = new Texture2D(size, size, TextureFormat.ARGB32, false);
            var textures = new Texture2D[spriteNames.Length];

            for (var i = 0; i < spriteNames.Length; i++)
            {
                textures[i] = loadTextureFromAssembly($"{_assemblyPath}{spriteNames[i]}.png", false);
            }

            var rects = atlasTex.PackTextures(textures, 2, size);

            var atlas = ScriptableObject.CreateInstance<UITextureAtlas>();

            // Setup atlas
            var material = Object.Instantiate(baseMaterial);
            material.mainTexture = atlasTex;
            atlas.material = material;
            atlas.name = atlasName;

            // Add SpriteInfo
            for (var i = 0; i < spriteNames.Length; i++)
            {
                var spriteInfo = new UITextureAtlas.SpriteInfo()
                {
                    name = spriteNames[i],
                    texture = atlasTex,
                    region = rects[i]
                };
                atlas.AddSprite(spriteInfo);
            }
            return atlas;
        }

        private Texture2D loadTextureFromAssembly(string path, bool readOnly = true)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var textureStream = assembly.GetManifestResourceStream(path))
            {
                var buf = new byte[textureStream.Length];  //declare arraysize
                textureStream.Read(buf, 0, buf.Length); // read from stream to byte array
                var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                tex.LoadImage(buf);
                tex.Apply(false, readOnly);
                return tex;
            }
        }
    }
}
