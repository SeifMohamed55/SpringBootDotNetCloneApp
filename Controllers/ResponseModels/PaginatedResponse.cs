﻿using SpringBootCloneApp.Services;

namespace SpringBootCloneApp.Controllers.ResponseModels
{
    public class PaginatedResponse<T>
    {
        public PaginatedList<T> PaginatedList { get; init; }
        public int TotalPages { get; init; }
        public string CurrentlyViewing { get; init; }

        public PaginatedResponse(PaginatedList<T> list) 
        {
            PaginatedList = list;
            TotalPages = list.TotalPages;
            
            CurrentlyViewing =  $"({1 + ((list.PageIndex - 1) * list.PageSize)} - {list.PageIndex * list.PageSize})";
        }


    }
}
