using Microsoft.AspNetCore.Components;
using Org.Domains.Nodes;

namespace Components.CreateNode;

partial class PersonLayout
{
    [Parameter] public NodePerson person { get; set; }

}