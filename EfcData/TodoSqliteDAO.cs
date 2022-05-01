using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcData; 

public class TodoSqliteDAO : ITodoHome {

    private readonly TodoContext context;

    public TodoSqliteDAO(TodoContext context) {
        this.context = context;
    }

    public async Task<ICollection<Todo>> GetAsync() {
        ICollection<Todo> todos = await context.Todos.ToListAsync();
        return todos;
    }

    public async Task<Todo> GetById(int id) {
        Todo? todo = await context.Todos.FindAsync(id);
        return todo;
    }

    public async Task<Todo> AddAsync(Todo todo) {
        EntityEntry<Todo> added = await context.AddAsync(todo);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task DeleteAsync(int id) {
        Todo todo =  await GetById(id);
        context.Remove(todo);
        await context.SaveChangesAsync();
    }

    public Task UpdateAsync(Todo todo) {
        EntityEntry<Todo> updated = context.Update(todo);
        return context.SaveChangesAsync();
    }

    public async Task<ICollection<Todo>> GetAsync(int? userId, bool? isCompleted) {
        IQueryable<Todo> todos = context.Todos.AsQueryable();
        if (userId != null) {
            todos = todos.Where(todo => todo.OwnerId.Equals(userId));
        }
        if (isCompleted != null) {
            todos = todos.Where(todo => todo.IsCompleted == isCompleted);
        }

        ICollection<Todo> result = await todos.ToListAsync();
        return result;
    }
}