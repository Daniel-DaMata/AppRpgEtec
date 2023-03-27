using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordString { get; set; }
        public string Perfil { get; set; }
        public string Token { get; set; }
        public byte[] Foto { get; set; }
        public string Email { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

    }
    public class Email
    {
        public string Remetente { get; set; }
        public string RemetentePassword { get; set; }
        public string Destinatario { get; set; }
        public string DestinatarioCopia { get; set; }
        public string DominioPrimario { get; set; }
        public int PortaPrimaria { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
    }
}
