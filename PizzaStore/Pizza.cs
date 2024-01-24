using Microsoft.EntityFrameworkCore;

// Parei na unidade 6 - 'Adicionar o EF Core ao projeto'

namespace PizzaStore.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
