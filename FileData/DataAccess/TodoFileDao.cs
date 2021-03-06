using Domain.Contracts;
using Domain.Models;

namespace FileData.DataAccess; 

public class TodoFileDao : ITodoHome {

    private FileContext fileContext;

    public TodoFileDao(FileContext fileContext) {
        this.fileContext = fileContext;
    }

    public async Task<ICollection<Todo>> GetAsync() {
        ICollection<Todo> todos = fileContext.Todos;
        return todos;
    }

    public async Task<Todo> GetById(int id) {
        return fileContext.Todos.First(t => t.Id == id);
    }

    public async Task<Todo> AddAsync(Todo todo) {
        int nextId = fileContext.Todos.Max(t => t.Id) + 1;
        todo.Id = nextId;
        fileContext.Todos.Add(todo);
        fileContext.SaveChanges();
        return todo;
    }

    public async Task DeleteAsync(int id) {
        fileContext.Todos.Remove(GetById(id).Result);
        fileContext.SaveChanges();
    }

    public Task UpdateAsync(Todo todo) {
        Todo toUpdate = fileContext.Todos.First(t => t.Id == todo.Id);
        toUpdate.IsCompleted = todo.IsCompleted;
        toUpdate.OwnerId = todo.OwnerId;
        toUpdate.Title = todo.Title;
        fileContext.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<ICollection<Todo>> GetAsync(int? userId, bool? isCompleted) {
        throw new NotImplementedException();
    }
}