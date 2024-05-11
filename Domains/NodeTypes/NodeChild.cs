namespace Org.Domains.NodeTypes;

public class NodeChild : IOccurence
{
    public NodeChild(Guid childId,string nodeTypeName,int minValue, int maxValue)
    {
        NodeTypeId = childId;
        MinValue = minValue;
        MaxValue = maxValue;
        NodeTypeName = nodeTypeName;
    }

    public NodeChild()
    {
    }

    public Guid NodeTypeId { get; set; }
    public string NodeTypeName { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
}

public interface IOccurence
{
    int MinValue { get; set; }
    int MaxValue { get; set; }
}