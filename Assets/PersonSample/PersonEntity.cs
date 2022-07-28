using GBJ.EntityDB;
using GBJ.EntityDB.AssetReferenceHolders;
using Newtonsoft.Json;

public class PersonEntity : Entity
{
    public string Name;
    public int Age;
    
    [JsonConverter(typeof(AssetReferenceHolderConverter<AssetReferenceGameObjectHolder>))]
    public AssetReferenceGameObjectHolder Prefab = new AssetReferenceGameObjectHolder();
}