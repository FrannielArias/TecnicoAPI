using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicos.Data.Models;

public class Clientes
{
    [Key]
    public int ClienteId { get; set; }
    public string? Nombres { get; set; }
    public string? WhatsApp { get; set; }
}
