using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEHO.Baseball.API.Helpers
{
    public class PagingInfo
    {
        public int TotalPlayers { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string PreviousPageLink { get; set; }
        public string NextPageLink { get; set; }

        public PagingInfo(int totalPlayers, int totalPages, int currentPage, int pageSize, string previousPageLink,
            string nextPageLink)
        {
            TotalPlayers = totalPlayers;
            TotalPages = totalPages;
            CurrentPage = currentPage;
            PageSize = pageSize;
            PreviousPageLink = previousPageLink;
            NextPageLink = nextPageLink;
        }
    }
}