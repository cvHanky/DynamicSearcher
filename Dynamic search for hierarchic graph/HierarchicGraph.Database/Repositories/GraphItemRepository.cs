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

    public async Task<GraphItem?> GetItemById(int id)
    {
        return await context.Items.FindAsync(id);
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

    public List<GraphItem> GetSortedItems()
    {
        List<GraphItem> sortedList = new List<GraphItem>();
        List<GraphItem> fullList = GetAll().ToList();

        var rootItems = fullList.Where(i => i.ParentId is null);
        foreach (var rootItem in rootItems)
        {
            CollectDescendantsRecursively(rootItem, sortedList, fullList, 0);
        }

        return sortedList;
    }

    private void CollectDescendantsRecursively(GraphItem item, List<GraphItem> sortedList, List<GraphItem> fullList, int depth)
    {
        item.Depth = depth; // every time the recursion goes a layer deeper, depth is incremented by 1.
        sortedList.Add(item);

        if (fullList.Where(i => i.ParentId == item.Id).ToList() is List<GraphItem> children && children.Any()) // Figures out if the item has any children, and then also collects THEIR descendants.
        {
            foreach(var child in children)
            {
                CollectDescendantsRecursively(child, sortedList, fullList, depth+1);
            }
        }
    }
}
