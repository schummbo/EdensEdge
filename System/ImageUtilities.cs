using System.Collections.Concurrent;
using Godot;

public static class ImageUtilities
{
    private static readonly TileSet TileSet = GD.Load<TileSet>("res://Tilesets/main.tres");
    private static readonly ConcurrentDictionary<TextureCacheKey, ImageTexture> textureCache = new ConcurrentDictionary<TextureCacheKey, ImageTexture>();

    public static Texture2D TextureFromTileset(TileSets sourceSet, Vector2I tilePosition, bool skipCache = false)
    {
        if (!skipCache &&
        textureCache.TryGetValue(new TextureCacheKey(sourceSet, tilePosition), out ImageTexture texture))
        {
            return texture;
        }

        var atlasSource = TileSet.GetSource((int)sourceSet) as TileSetAtlasSource;
        Image atlasImage = atlasSource.Texture.GetImage();
        Rect2I tileRegion = atlasSource.GetTileTextureRegion(tilePosition);
        Image tileImage = atlasImage.GetRegion(tileRegion);
        ImageTexture tileTexture = ImageTexture.CreateFromImage(tileImage);

        return skipCache
        ? tileTexture
        : textureCache.GetOrAdd(new TextureCacheKey(sourceSet, tilePosition), tileTexture);
    }
}

public record TextureCacheKey(TileSets sourceSet, Vector2I tilePosition);