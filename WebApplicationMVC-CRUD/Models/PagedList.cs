using System;
using System.Collections.Generic;
using WebApplicationMVC_CRUD.DAL;

namespace WebApplicationMVC_CRUD.Models
{
    public class PagedList<T>
    {
        public PagedList(ISearchCondition searchConditions)
        {
            SearchConditions = searchConditions;
            CurrentPage = searchConditions.page;
            PageSize = searchConditions.pageSize;
        }
        public ISearchCondition SearchConditions { get; set; }
        public IEnumerable<T> Content { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalRecords / PageSize); }
        }
    }

    public interface ISearchCondition
    {
        string filter { get; set; }
        int page { get; set; }
        int pageSize { get; set; }
        string sort { get; set; }
        string sortdir { get; set; }
    }
    public class BasicSearchCondition : ISearchCondition
    {
        public string filter { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 25;
        public string sort { get; set; } = "";
        public string sortdir { get; set; } = "DESC";
    }
    public class PhoneSearchCondition : BasicSearchCondition, ISearchCondition
    {
        //default values
        public PhoneSearchCondition() {
            pageSize = 10;
        }
        public int? PhoneId { get; set; }
        public int? ManufacturerId { get; set; }
        public string Description { get; set; }
        public new string sort { get; set; } = "PhoneId";
    }
}