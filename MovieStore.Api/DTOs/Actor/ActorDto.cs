using System.Collections.Generic;

namespace MovieStore.Api.DTOs.Actor
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> MovieNames { get; set; }
    }

    public class CreateActorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UpdateActorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
} 