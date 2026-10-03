using System;

namespace RegistroGastos
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = "";
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = "";

        public override string ToString()
        {
            return $"{Id,-3} {Descripcion,-20} Q {Monto,10:N2} {Categoria,-15}";
        }
    }
}