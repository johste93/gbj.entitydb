using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GBJ.EntityDB
{
    public class Entity
    {
        [JsonIgnore] public int Index { get; set; }
        [JsonIgnore] public string Id { get; set; }
    }
}