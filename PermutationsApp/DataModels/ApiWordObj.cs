using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace PermutationsApp.DataModels;

[DataContract]
public class ApiWordObj
{
    [FromQuery] public string Word { get; set; }
}