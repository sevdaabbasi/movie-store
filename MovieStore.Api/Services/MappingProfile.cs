using AutoMapper;
using MovieStore.Api.DTOs.Actor;
using MovieStore.Api.DTOs.Customer;
using MovieStore.Api.DTOs.Director;
using MovieStore.Api.DTOs.Movie;
using MovieStore.Api.DTOs.Order;
using MovieStore.Api.Entities;
using System.Linq;

namespace MovieStore.Api.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Movie
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.DirectorName, opt => opt.MapFrom(src => src.Director.FirstName + " " + src.Director.LastName))
                .ForMember(dest => dest.ActorNames, opt => opt.MapFrom(src => src.MovieActors.Select(ma => ma.Actor.FirstName + " " + ma.Actor.LastName).ToList()));
            CreateMap<CreateMovieDto, Movie>();
            CreateMap<UpdateMovieDto, Movie>();

            // Actor
            CreateMap<Actor, ActorDto>()
                .ForMember(dest => dest.MovieNames, opt => opt.MapFrom(src => src.MovieActors.Select(ma => ma.Movie.Name).ToList()));
            CreateMap<CreateActorDto, Actor>();
            CreateMap<UpdateActorDto, Actor>();

            // Director
            CreateMap<Director, DirectorDto>()
                .ForMember(dest => dest.MovieNames, opt => opt.MapFrom(src => src.Movies.Select(m => m.Name).ToList()));
            CreateMap<CreateDirectorDto, Director>();
            CreateMap<UpdateDirectorDto, Director>();

            // Customer
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.FavoriteGenres, opt => opt.MapFrom(src => src.CustomerGenres.Select(cg => cg.Genre.Name).ToList()))
                .ForMember(dest => dest.PurchasedMovies, opt => opt.MapFrom(src => src.Orders.Select(o => o.Movie.Name).ToList()));
            CreateMap<CreateCustomerDto, Customer>();

            // Order
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie.Name));
        }
    }
} 