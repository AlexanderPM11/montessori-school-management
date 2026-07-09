using MontessoriSystem.Core.Application.Interface.Services;
using MontessoriSystem.Core.Application.Specialization;
using MontessoriSystem.Core.Application.ViewModels.EducationalLevel;
using MontessoriSystem.Core.Application.ViewModels.Nationality;
using MontessoriSystem.Core.Application.ViewModels.Professions;
using MontessoriSystem.Core.Application.ViewModels.Province;
using MontessoriSystem.Core.Application.ViewModels.RelationshipPerson;
using MontessoriSystem.Core.Application.ViewModels.TipoAdjunto;
using MontessoriSystem.Core.Application.ViewModels.TitlesAchieved;
using MontessoriSystem.Core.Application.ViewModels.TypeRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontessoriSystem.Infrastructure.Persistence.Seed
{
    public static class MasterDataSeed
    {
        public static async Task SeedProvinces(IProvinceDomService provinceService)
        {
            var provinces = new List<string>
            {
                "Azua", "Bahoruco", "Barahona", "Dajabón", "Distrito Nacional", "Duarte",
                "Elías Piña", "El Seibo", "Espaillat", "Hato Mayor", "Independencia",
                "La Altagracia", "La Romana", "La Vega", "María Trinidad Sánchez",
                "Monseñor Nouel", "Monte Cristi", "Monte Plata", "Pedernales", "Peravia",
                "Puerto Plata", "Samaná", "San Cristóbal", "San José de Ocoa", "San Juan",
                "San Pedro de Macorís", "Sánchez Ramírez", "Santiago", "Santiago Rodríguez",
                "Valverde"
            };

            foreach (var province in provinces)
            {
                await provinceService.Add(new ProvinceDomSaveViewModel { Name = province });
            }
        }

        public static async Task SeedNationalities(INacionalityService nationalityService)
        {
            var nationalities = new List<(string Name, string Description)>
            {
                ("Dominicana", "Nacionalidad de México"), // Note: The SQL says "Nacionalidad de México" for Dominicana
                ("Argentina", "Nacionalidad de Argentina"),
                ("Española", "Nacionalidad de España"),
                ("Chilena", "Nacionalidad de Chile"),
                ("Colombiana", "Nacionalidad de Colombia"),
                ("Peruana", "Nacionalidad de Perú"),
                ("Venezolana", "Nacionalidad de Venezuela"),
                ("Cubana", "Nacionalidad de Cuba"),
                ("Ecuatoriana", "Nacionalidad de Ecuador"),
                ("Boliviana", "Nacionalidad de Bolivia")
            };

            foreach (var item in nationalities)
            {
                await nationalityService.Add(new NationalitySaveViewModel { Name = item.Name, Description = item.Description });
            }
        }

        public static async Task SeedSpecializations(ISpecializationService specializationService)
        {
            var specializations = new List<(string Name, string Description)>
            {
                ("Matemáticas", "Especialización en enseñanza de matemáticas"),
                ("Ciencias", "Especialización en enseñanza de ciencias"),
                ("Lenguaje", "Especialización en enseñanza de lenguaje"),
                ("Historia", "Especialización en enseñanza de historia"),
                ("Arte", "Especialización en enseñanza de arte"),
                ("Música", "Especialización en enseñanza de música"),
                ("Geografía", "Especialización en enseñanza de geografía"),
                ("Educación Física", "Especialización en enseñanza de educación física"),
                ("Química", "Especialización en enseñanza de química"),
                ("Física", "Especialización en enseñanza de física"),
                ("Biología", "Especialización en enseñanza de biología"),
                ("Tecnología", "Especialización en enseñanza de tecnología"),
                ("Informática", "Especialización en enseñanza de informática"),
                ("Programación", "Especialización en enseñanza de programación"),
                ("Literatura", "Especialización en enseñanza de literatura"),
                ("Filosofía", "Especialización en enseñanza de filosofía"),
                ("Psicología", "Especialización en enseñanza de psicología"),
                ("Derecho", "Especialización en enseñanza de derecho"),
                ("Economía", "Especialización en enseñanza de economía"),
                ("Sociología", "Especialización en enseñanza de sociología")
            };

            foreach (var item in specializations)
            {
                await specializationService.Add(new SpecializationsSaveViewModel { Name = item.Name, Description = item.Description });
            }
        }

        public static async Task SeedTitlesAchieved(ITitlesAchievedsService titlesService)
        {
            var titles = new List<(string Name, string Description)>
            {
                ("Licenciado en Educación", "Título otorgado por completar la carrera de educación"),
                ("Doctor en Filosofía", "Título otorgado por completar estudios doctorales en filosofía"),
                ("Ingeniero Civil", "Título otorgado por completar la carrera de ingeniería civil"),
                ("Maestro en Ciencias", "Título de maestría en el área de ciencias"),
                ("Licenciado en Psicología", "Título otorgado por completar la carrera de psicología"),
                ("Doctor en Derecho", "Título otorgado por completar estudios doctorales en derecho"),
                ("Maestro en Administración de Empresas", "Título de maestría en administración de empresas"),
                ("Licenciado en Matemáticas", "Título otorgado por completar la carrera de matemáticas"),
                ("Doctor en Medicina", "Título otorgado por completar estudios doctorales en medicina"),
                ("Maestro en Ciencias de la Computación", "Título de maestría en ciencias de la computación"),
                ("Licenciado en Física", "Título otorgado por completar la carrera de física"),
                ("Doctor en Química", "Título otorgado por completar estudios doctorales en química"),
                ("Licenciado en Biología", "Título otorgado por completar la carrera de biología"),
                ("Maestro en Educación", "Título de maestría en el área de educación"),
                ("Licenciado en Historia", "Título otorgado por completar la carrera de historia"),
                ("Doctor en Ciencias Políticas", "Título otorgado por completar estudios doctorales en ciencias políticas"),
                ("Maestro en Derecho", "Título de maestría en derecho"),
                ("Licenciado en Filosofía", "Título otorgado por completar la carrera de filosofía"),
                ("Doctor en Ingeniería de Software", "Título otorgado por completar estudios doctorales en ingeniería de software"),
                ("Maestro en Psicología Clínica", "Título de maestría en psicología clínica")
            };

            foreach (var item in titles)
            {
                await titlesService.Add(new TitlesAchievedSaveViewModel { Name = item.Name, Description = item.Description });
            }
        }

        public static async Task SeedEducationalLevels(IEducationalLevelService levelService)
        {
            var levels = new List<(string Name, string Description)>
            {
                ("Primaria", "Nivel educativo básico inicial para niños"),
                ("Secundaria", "Nivel educativo para adolescentes, intermedio entre la primaria y la educación superior"),
                ("Bachillerato", "Último nivel de la educación secundaria, previo a la educación superior"),
                ("Educación Técnica", "Nivel educativo enfocado en la formación técnica y profesional"),
                ("Educación Universitaria", "Nivel educativo en universidades, para la obtención de grados de licenciatura o títulos profesionales"),
                ("Maestría", "Nivel de educación superior enfocado en estudios avanzados después de la licenciatura"),
                ("Doctorado", "Máximo nivel académico, enfocado en investigación y desarrollo avanzado en una disciplina específica"),
                ("Preescolar", "Nivel educativo previo a la educación primaria, dirigido a niños pequeños"),
                ("Postdoctorado", "Nivel educativo para investigadores que desean profundizar en áreas específicas después de obtener un doctorado"),
                ("Diplomado", "Curso especializado de corta duración, que no constituye un grado académico"),
                ("Certificación Profesional", "Curso especializado orientado a la obtención de competencias técnicas o profesionales"),
                ("Educación para Adultos", "Programa educativo diseñado específicamente para adultos que no completaron la educación formal")
            };

            foreach (var item in levels)
            {
                await levelService.Add(new EducationalLevelSaveViewModel { Name = item.Name, Description = item.Description });
            }
        }

        public static async Task SeedRelationshipPersons(IRelationshipPersonService relationshipService)
        {
            var relationships = new List<(string Name, string Description)>
            {
                ("Padre", "Relación de padre con la persona"),
                ("Madre", "Relación de madre con la persona"),
                ("Hermano", "Relación de hermano con la persona"),
                ("Tío", "Relación de tío con la persona"),
                ("Abuelo", "Relación de abuelo con la persona")
            };

            foreach (var item in relationships)
            {
                await relationshipService.Add(new RelationshipPersonSaveViewModel { Name = item.Name, Description = item.Description });
            }
        }

        public static async Task SeedProfessions(IProfessionsService professionsService)
        {
            var professions = new List<(string Name, string Description)>
            {
                ("Médico", "Profesional de la medicina"),
                ("Ingeniero Civil", "Profesional en ingeniería civil"),
                ("Ingeniero en Sistemas", "Profesional en ingeniería de software y sistemas"),
                ("Abogado", "Profesional del derecho"),
                ("Contador", "Profesional de la contabilidad"),
                ("Profesor", "Docente en instituciones educativas"),
                ("Enfermero", "Profesional del cuidado de la salud"),
                ("Farmacéutico", "Especialista en farmacia y medicamentos"),
                ("Arquitecto", "Diseñador y planificador de estructuras y edificios"),
                ("Psicólogo", "Especialista en la conducta y salud mental"),
                ("Electricista", "Especialista en instalación y reparación de sistemas eléctricos"),
                ("Carpintero", "Trabajador especializado en carpintería y trabajos de madera"),
                ("Chef", "Cocinero profesional"),
                ("Mecánico", "Técnico en reparación y mantenimiento de vehículos"),
                ("Diseñador Gráfico", "Especialista en diseño visual y gráfico"),
                ("Veterinario", "Profesional en el cuidado de la salud de animales"),
                ("Piloto", "Profesional que opera aeronaves"),
                ("Bombero", "Personal encargado de la extinción de incendios"),
                ("Policía", "Agente de la ley y el orden público"),
                ("Científico", "Investigador en diversas ramas de la ciencia"),
                ("Administrador de Empresas", "Profesional en la gestión de negocios y empresas"),
                ("Periodista", "Reportero y escritor de noticias y artículos"),
                ("Fotógrafo", "Profesional en la captura de imágenes fotográficas"),
                ("Traductor", "Especialista en traducción de lenguajes")
            };

            foreach (var item in professions)
            {
                await professionsService.Add(new ProfessionsSaveViewModel { Name = item.Name, Description = item.Description });
            }
        }

        public static async Task SeedTypeRegister(ITypeRegisterService typeRegisterService)
        {
            var types = new List<(string Name, int MinAge, int MaxAge)>
            {
                ("Inicial", 1, 5),
                ("Primaria", 1, 5),
                ("Secundaria", 1, 5)
            };

            foreach (var item in types)
            {
                await typeRegisterService.Add(new TypeRegisterSaveViewModel { Name = item.Name, MinAge = item.MinAge, MaxAge = item.MaxAge });
            }
        }

        public static async Task SeedTipoAdjunto(ITipoAdjuntoService tipoAdjuntoService)
        {
            var types = new List<string> { "PDF", "IMG" };

            foreach (var type in types)
            {
                await tipoAdjuntoService.Add(new SaveTipoAdjuntoViewModel { Description = type });
            }
        }
    }
}
