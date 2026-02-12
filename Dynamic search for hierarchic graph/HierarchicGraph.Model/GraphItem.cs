namespace HierarchicGraph.Model;

public class GraphItem
{
    public required string Name { get; set; }

    public int Id { get; set; }

    public int? ParentId { get; set; }

    public GraphItem? Parent { get; set; }

    public ICollection<GraphItem> Children { get; set; } = new List<GraphItem>();
}