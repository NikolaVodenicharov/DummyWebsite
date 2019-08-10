using System.Collections.Generic;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Constants;
using Tyres.Shared.DataTransferObjects.Tyres;

namespace Tyres.Service.Interfaces
{
    public interface ITyreService
    {
        IEnumerable<TyreSummaryDTO> GetAllListing(Width width, Ratio ration, Diameter diameter, Season season, int page = PageConstants.DefaultPage);

        int GetPagesCount(Width width, Ratio ratio, Diameter diameter, Season season);
        TyreDetailsDTO Get(int id);
    }
}
