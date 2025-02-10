namespace ItemsDeTrabajo.Models;

public partial class Itemstrabajo
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateTime Fechaentrega { get; set; }

    public string? Usuarioasignado { get; set; }

    public string? Relevancia { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }
}