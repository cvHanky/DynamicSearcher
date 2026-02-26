using HierarchicGraph.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicGraph.Database.Repositories;

public class GraphItemRepository
{
    private readonly GraphDbContext context;
    public GraphItemRepository(GraphDbContext dbContext)
    {
        context = dbContext;
    }

    public GraphItem? GetItemById(int id)
    {
        return context.Items.Find(id);
    }

    public List<GraphItem> GetAll()
    {
        return context.Items.ToList();
    }

    public bool AddItem(GraphItem item)
    {
        if (item is not null && item.Id == default && string.IsNullOrEmpty(item.Name) is not true)    // The id should not be set by the user.
        {
            context.Items.AddAsync(item);
            context.SaveChanges();

            Console.WriteLine($"Item with id {item.Id} was added successfully.");
        }
        return true;
    }

    /// <summary>
    /// Returns a list of ids of all descendants of the item with <paramref name="itemId"/>.
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public List<int> GetAllDescendants(int itemId)
    {
        var item = GetItemById(itemId);
        var result = new List<int>();

        if (item is null)
        {
            // Theoretically there should be some logging here, not necessary for the scope of this project.
            return result;
        }

        var childList = new List<GraphItem>();
        CollectDescendantsRecursively(item, sortedList: childList, fullList: GetAll());

        foreach (var child in childList )
        {
            result.Add(child.Id);
        }

        return result;
    }

    /// <summary>
    /// Returns a sorted list of a full heritage tree of <see cref="GraphItem"/>. Can optionally be sorted alphabetically (not implemented yet).
    /// </summary>
    /// <returns></returns>
    public List<GraphItem> GetSortedItems(bool sortAlphabetically = false)
    {
        List<GraphItem> sortedList = new List<GraphItem>();
        List<GraphItem> fullList = GetAll().ToList();

        var rootItems = fullList.Where(i => i.ParentId is null);
        foreach (var rootItem in rootItems)
        {
            CollectDescendantsRecursively(rootItem, sortedList, fullList, setDepth: true, depth: 0);
        }

        return sortedList;
    }

    /// <summary>
    /// Given a list <paramref name="sortedList"/>, adds all descendants of <paramref name="item"/> to that list. 
    /// Optionally sets the <paramref name="depth"/> of all items if <paramref name="setDepth"/> is true.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="sortedList"></param>
    /// <param name="fullList"></param>
    /// <param name="setDepth"></param>
    /// <param name="depth"></param>
    private static void CollectDescendantsRecursively(GraphItem item, List<GraphItem> sortedList, List<GraphItem> fullList, bool setDepth = false, int depth = 0)
    {
        if (setDepth)
        {
            item.Depth = depth; // every time the recursion goes a layer deeper, depth is incremented by 1.
        }
        sortedList.Add(item);

        if (fullList.Where(i => i.ParentId == item.Id).ToList() is List<GraphItem> children && children.Any()) // Figures out if the item has any children, and then also collects THEIR descendants.
        {
            foreach (var child in children)
            {
                CollectDescendantsRecursively(child, sortedList, fullList, setDepth:setDepth, depth:depth + 1);
            }
        }
    }
}
