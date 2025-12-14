using System.ComponentModel.DataAnnotations;

namespace LoveShop.Models;

public record Product
(
    [Required] Guid Id,
    [Required] string Name,
    string? Description,
    [Required] decimal Price,
    [Required] IEnumerable<Guid> CategoryId
);