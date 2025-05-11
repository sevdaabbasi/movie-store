using System.Collections.Generic;

namespace MovieStore.Api.DTOs.Director
{
    public class DirectorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> MovieNames { get; set; }
    }

    public class CreateDirectorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UpdateDirectorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
} 