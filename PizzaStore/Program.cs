using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
// using PizzaStore.DB;
using PizzaStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "PizzaStore API",
            Description = "Fazendo pizzas que você ama",
            Version = "v1"
        }
    );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
});

app.MapGet("/", () => "Digite /swagger no final da URL para ir às opções!");

app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());

app.MapPost(
    "/pizzas",
    async (PizzaDb db, Pizza pizza) =>
    {
        await db.Pizzas.AddAsync(pizza);
        await db.SaveChangesAsync();
        return Results.Created($"/pizza/{pizza.Id}", pizza);
    }
);

app.MapGet("/pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));

app.MapPut(
    "/pizza/{id}",
    async (PizzaDb db, Pizza updatepizza, int id) =>
    {
        var pizza = await db.Pizzas.FindAsync(id);
        if (pizza is null)
            return Results.NotFound();
        pizza.Name = updatepizza.Name;
        pizza.Description = updatepizza.Description;
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
);

app.MapDelete(
    "/pizza/{id}",
    async (PizzaDb db, int id) =>
    {
        var pizza = await db.Pizzas.FindAsync(id);

        if (pizza is null)
        {
            return Results.NotFound();
        }

        db.Pizzas.Remove(pizza);
        await db.SaveChangesAsync();
        return Results.Ok();
    }
);

// Parei em 'Testar PUT'

// O código abaixo faz paste do arquivo (./Db.cs)
// app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));
// app.MapGet("/pizzas", () => PizzaDB.GetPizzas());
// app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));
// app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));
// app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));

app.Run();
