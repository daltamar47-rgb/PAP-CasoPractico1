using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Core
{
    /// <summary>
    /// Word lists and random helpers used to build realistic sample values.
    /// </summary>
    public class SampleData
    {
        public static readonly string[] FirstNames =
        {
            "Ana", "Carlos", "María", "José", "Lucía", "Andrés", "Valeria", "Diego", "Sofía", "Daniel",
            "Camila", "Javier", "Paula", "Ricardo", "Fernanda", "Luis", "Gabriela", "Mateo", "Isabel", "Sebastián",
            "Elena", "Pablo", "Natalia", "Fabián", "Mariana", "Esteban", "Laura", "Tomás", "Adriana", "Kevin"
        };

        public static readonly string[] LastNames =
        {
            "Rodríguez", "Jiménez", "Vargas", "Mora", "Rojas", "Castro", "Solís", "Araya", "Chaves", "Herrera",
            "Méndez", "Ramírez", "Sánchez", "Alvarado", "Quesada", "Campos", "Salazar", "Brenes", "Calderón", "Murillo",
            "Navarro", "Porras", "Ulate", "Zúñiga", "Fernández", "Montero", "Arias", "Cordero", "Vega", "Soto"
        };

        public static readonly string[] Countries =
        {
            "Costa Rica", "México", "España", "Argentina", "Colombia", "Chile", "Perú", "Estados Unidos", "Reino Unido", "Francia"
        };

        public static readonly string[] Cities =
        {
            "San José", "Heredia", "Alajuela", "Cartago", "Liberia", "Puntarenas", "Limón", "Escazú", "Santa Ana", "Curridabat"
        };

        public static readonly string[] Provinces =
        {
            "San José", "Heredia", "Alajuela", "Cartago", "Guanacaste", "Puntarenas", "Limón"
        };

        public static readonly string[] Languages = { "Español", "Inglés", "Francés", "Portugués" };

        private static readonly string[] TitleStarts =
        {
            "El secreto de", "La sombra de", "Crónicas de", "El último", "Historias de", "La ciudad de",
            "El jardín de", "Memorias de", "El camino a", "La voz de", "El silencio de", "Cartas desde"
        };

        private static readonly string[] TitleEnds =
        {
            "la montaña", "los vientos", "un viajero", "la medianoche", "las estrellas", "un sueño", "la marea",
            "los espejos", "la lluvia", "un reino perdido", "la frontera", "los océanos", "un poeta", "la biblioteca"
        };

        private static readonly string[] Words =
        {
            "historia", "personajes", "lectura", "aventura", "misterio", "capítulos", "narración", "final",
            "estilo", "trama", "emoción", "ritmo", "ideas", "autor", "mundo", "detalles"
        };

        private static readonly string[] StreetNames =
        {
            "Avenida Central", "Calle 5", "Barrio Escalante", "Paseo Colón", "Avenida Segunda", "Calle Real",
            "Barrio Amón", "Sabana Norte", "Los Yoses", "Barrio Otoya"
        };

        private static readonly string[] CompanyWords =
        {
            "Libros", "Ediciones", "Distribuidora", "Papiros", "Letras", "Páginas", "Imprenta", "Grupo"
        };

        private static readonly string[] CompanyNames =
        {
            "del Valle", "Central", "Atlántico", "Horizonte", "Aurora", "Pacífico", "Andina", "Norte", "Del Sol", "Cumbre"
        };

        private readonly Random _random;

        public SampleData()
        {
            _random = new Random();
        }

        public T Pick<T>(IList<T> values)
        {
            return values[_random.Next(values.Count)];
        }

        /// <summary>Several different values from the list (never repeated).</summary>
        public IList<T> PickMany<T>(IList<T> values, int count)
        {
            return values.OrderBy(value => _random.Next()).Take(Math.Min(count, values.Count)).ToList();
        }

        public int Between(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public decimal Money(decimal min, decimal max)
        {
            var value = min + (decimal)_random.NextDouble() * (max - min);
            return Math.Round(value, 2);
        }

        public bool Chance(int percent)
        {
            return _random.Next(100) < percent;
        }

        public DateTime DateBetween(DateTime from, DateTime to)
        {
            var range = (to - from).TotalMinutes;
            return from.AddMinutes(_random.NextDouble() * range);
        }

        public DateTime RecentDate(int maxDaysAgo)
        {
            return DateTime.Now.AddDays(-_random.Next(0, maxDaysAgo)).AddMinutes(-_random.Next(0, 1440));
        }

        public string FirstName()
        {
            return Pick(FirstNames);
        }

        public string LastName()
        {
            return Pick(LastNames) + " " + Pick(LastNames);
        }

        /// <summary>Unique email: the index is part of the address.</summary>
        public string Email(string firstName, string lastName, int index, string domain)
        {
            var user = RemoveAccents(firstName + "." + lastName.Split(' ')[0]).ToLowerInvariant();
            return string.Format("{0}{1}@{2}", user, index, domain);
        }

        public string Phone()
        {
            return string.Format("+506 {0}{1:000}-{2:0000}", Pick(new[] { "6", "7", "8", "2" }), _random.Next(1000), _random.Next(10000));
        }

        public string BookTitle()
        {
            return Pick(TitleStarts) + " " + Pick(TitleEnds);
        }

        public string CompanyName()
        {
            return Pick(CompanyWords) + " " + Pick(CompanyNames);
        }

        public string Street()
        {
            return string.Format("{0}, casa {1}", Pick(StreetNames), _random.Next(1, 300));
        }

        public string PostalCode()
        {
            return _random.Next(10101, 70703).ToString();
        }

        public string Sentence(int words)
        {
            var text = string.Join(" ", Enumerable.Range(0, words).Select(index => Pick(Words)));
            return char.ToUpper(text[0]) + text.Substring(1) + ".";
        }

        /// <summary>13-digit ISBN; the index makes it unique.</summary>
        public string Isbn(int index)
        {
            return string.Format("978{0:000}{1:0000000}", _random.Next(1000), index);
        }

        private static string RemoveAccents(string text)
        {
            return text
                .Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                .Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U")
                .Replace("ñ", "n").Replace("Ñ", "N").Replace(" ", string.Empty);
        }
    }
}
