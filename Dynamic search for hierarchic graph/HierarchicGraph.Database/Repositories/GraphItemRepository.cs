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
}
