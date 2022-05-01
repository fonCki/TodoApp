using System.Collections;
using System.Text;
using System.Text.Json;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualBasic;

namespace HttpServices;

public class TodoHttpClient : ITodoHome {
    public async Task<ICollection<Todo>> GetAsync() {
        using HttpClient client = new();
        HttpResponseMessage response = await client.GetAsync("https://localhost:7193/Todos");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        ICollection<Todo> todos = JsonSerializer.Deserialize<ICollection<Todo>>(content, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;
        return todos;
    }

    public async Task<Todo> GetById(int id) {
        using HttpClient client = new();
        HttpResponseMessage response = await client.GetAsync($"https://localhost:7193/Todos/{id}");
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Error: {response.StatusCode}, {responseContent}");
        }

        Todo returned = JsonSerializer.Deserialize<Todo>(responseContent, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;

        return returned;
    }

    public async Task<Todo> AddAsync(Todo todo) {
        using HttpClient client = new();

        string todoAsJson = JsonSerializer.Serialize(todo);

        StringContent content = new(todoAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("https://localhost:7193/Todos", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Error: {response.StatusCode}, {responseContent}");
        }

        Todo returned = JsonSerializer.Deserialize<Todo>(responseContent, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;

        return returned;
    }

    public async Task DeleteAsync(int id) {
        using HttpClient client = new();
        HttpResponseMessage response = await client.DeleteAsync($"https://localhost:7193/Todos/{id}");
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Error: {response.StatusCode}");
        }

    }

    public async Task UpdateAsync(Todo todo) {
        using HttpClient client = new();

        string todoAsJson = JsonSerializer.Serialize(todo);

        StringContent content = new(todoAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PatchAsync("https://localhost:7193/Todos", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Error: {response.StatusCode}, {responseContent}");
        }

        Todo returned = JsonSerializer.Deserialize<Todo>(responseContent, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;

    }

    public async Task<ICollection<Todo>> GetAsync(int? userId, bool? isCompleted) {

        QueryString queryString = new QueryString();

        if (userId != null) queryString.Add("userId", userId.ToString());
        if (isCompleted != null) queryString.Add("isCompleted", isCompleted.ToString());

        using HttpClient client = new();
        HttpResponseMessage response = await client.GetAsync($"https://localhost:7193/search{queryString}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        ICollection<Todo> todos = JsonSerializer.Deserialize<ICollection<Todo>>(content, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;


        return todos;

    }
}