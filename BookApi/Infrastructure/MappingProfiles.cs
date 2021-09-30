using AutoMapper;
using BookApi.Model;
using BookApi.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Infrastructure
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<BookVM, Book>().ReverseMap();
            //CreateMap<Book, BookWithAuthorsVM>()
            //    .ForMember(p => p.PublisherName, opt => opt.MapFrom(p => p.Publisher.Name))
            //    .ForMember(a => a.AuthorsName, opt => opt.MapFrom(a =>a.Book_Authors.Select(n => n.Author.Name).ToList()));
        }
    }
}
