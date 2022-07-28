using GBJ.EntityDB;
using UnityEngine;

public class PersonTableUsageSample : MonoBehaviour
{
    private Table<PersonEntity> personTable;
    void Awake()
    {
        personTable.Load();
        PersonEntity person = personTable.GetById(Person.Dennis);
        Debug.Log($"{person.Name} is {person.Age} years old.");
    }
}
