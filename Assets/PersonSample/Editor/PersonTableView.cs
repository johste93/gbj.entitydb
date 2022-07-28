using GBJ.EntityDB.Editor;
using UnityEditor;

public class PersonTableView : TableView<PersonEntity>
{
    [MenuItem("Tables/Persons")]
    static void Init() => Init<PersonTableView>();
    
    protected override bool GenerateConstants => true;
    protected override string GetPropertyConstantName(PersonEntity entity) => entity.Name;

    protected override int maxColumnCount => 3;
    protected override void CreateNewEntry() => @new = new PersonEntity();

    protected override void DrawColumnNames()
    {
        DrawColumn("Name", x => x.Value.Name);
        DrawColumn("Age", x => x.Value.Age);
        DrawColumn("Prefab", x => x.Value.Prefab);
    }

    protected override void DrawRow(PersonEntity entry, PersonEntity unmodified, bool changeColorIfChanged = false)
    {
        this.DrawEntry(() => entry.Name, x => entry.Name = x, changeColorIfChanged, () => unmodified?.Name);
        this.DrawEntry(() => entry.Age, x => entry.Age = x, changeColorIfChanged, () => unmodified?.Age);
        this.DrawEntry(() => entry.Prefab, x => entry.Prefab = x, changeColorIfChanged, () => unmodified?.Prefab);
    }
}