using GBJ.EntityDB.Editor;
using UnityEditor;

public class PersonTableView : TableView<PersonEntity>
{
    [MenuItem("Tables/Persons")]
    static void Init() => Init<PersonTableView>();
    
    protected override bool GenerateConstants => true;
    protected override string GetPropertyConstantName(PersonEntity entity) => entity.Name;

    protected override int maxColumnCount => 5;
    protected override void CreateNewEntry() => @new = new PersonEntity();

    protected override void DrawColumnNames()
    {
        DrawColumn("Name", x => x.Value.Name);
        DrawColumn("Age", x => x.Value.Age);
        DrawColumn("Friend", x => x.Value.Friend);
        DrawColumn("Prefab", x => x.Value.Prefab);
        DrawColumn("Family", x => x.Value.Family.Count);
    }

    protected override void DrawRow(PersonEntity entry, PersonEntity unmodified, bool changeColorIfChanged = false)
    {
        this.DrawCell(() => entry.Name, x => entry.Name = x, changeColorIfChanged, () => unmodified?.Name);
        this.DrawCell(() => entry.Age, x => entry.Age = x, changeColorIfChanged, () => unmodified?.Age);
        this.DrawConstantDropdown(typeof(Person), () => entry.Friend, x => entry.Friend = x, changeColorIfChanged, () => unmodified?.Friend);
        this.DrawCell(() => entry.Prefab, x => entry.Prefab = x, changeColorIfChanged, () => unmodified?.Prefab);
        this.DrawConstantDropdownList(typeof(Person), () => entry.Family, x => entry.Family = x, changeColorIfChanged, () => unmodified?.Family);
    }
}