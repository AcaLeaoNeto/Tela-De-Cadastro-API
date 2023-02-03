﻿using Domain.Entitys.Login;
using Domain.Enums;
using Domain.Notifications;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml;

namespace Domain.Entitys.Usuario
{
    public class User
    {
        public User(string name, DateTime dataDeNacimento, string sexo, string setor)
        {
            Name = name;
            DataDeNacimento = dataDeNacimento;
            Sexo = sexo;
            Idade = CalcularIdade(dataDeNacimento);
            Setor = setor;
        }

        public static explicit operator User(UserDto dto)
        {
            return new User(dto.Name, dto.DataDeNacimento, dto.Sexo, dto.Setor);
        }

        [Required]
        [JsonIgnore]
        public Guid Id { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoUsuario { get; set; }
        [Required, MinLength(1), MaxLength(85)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime DataDeNacimento { get; set; }
        [Required]
        public string Sexo { get; set; } = string.Empty;
        public int Idade { get; set; }
        public string Setor { get; set; }
        [JsonIgnore]
        public Log Log { get; set; }
        public bool Ativo { get; set; } = true;


        public bool Validation(INotification notification)
        {
            if (!ValidarData())
                notification.AddMessage(" Erro, Data Inválida ");

            if (!ValidarIdade())
                notification.AddMessage(" Erro, Usuario precisa ser maior de idade ");

            if (!ValidarSexo())
                notification.AddMessage(" Erro, Generos incorreto");

            if (!ValidarSetor())
                notification.AddMessage(" Erro, Setor inexistente");

            return notification.Valid;
        }

        private bool ValidarData()
        {
            return DataDeNacimento <= DateTime.Now;
        }

        private bool ValidarIdade()
        {
            return CalcularIdade(DataDeNacimento) >= 18;
        }

        private bool ValidarSexo()
        {
            return new List<string>(Enum.GetNames(typeof(Generos))).Contains(Sexo);
        }

        private bool ValidarSetor()
        {
            return new List<string>(Enum.GetNames(typeof(Setores))).Contains(Setor);
        }

        private int CalcularIdade(DateTime Nacimento)
        {
            var idade = DateTime.Now.Year - Nacimento.Year;
            if (Nacimento > DateTime.Now.AddYears(-idade)) idade--;

            return idade;
        }

    }
}
