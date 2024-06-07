using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluraLibrary.Models;

public class CourseInformation
{
    public string Titulo { get; set; }
    public string Instrutores { get; set; }
    public string CargaHoraria { get; set; }
    public string Descricao { get; set; }

    public CourseInformation(string titulo = "", string instrutores = "", string cargaHoraria = "", string descricao = "")
    {
        Titulo = titulo;
        Instrutores = instrutores;
        CargaHoraria = cargaHoraria;
        Descricao = descricao;
    }
}