using System.Collections.Generic;
using System.Threading.Tasks;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Constants;
using Tyres.Shared.DataTransferObjects.Tyres;

namespace Tyres.Service.Interfaces
{
    public interface ITyreService
    {
        Task<IEnumerable<TyreSummaryDTO>> GetAllListingAsync(Width width, Ratio ration, Diameter diameter, Season season, int page = PageConstants.DefaultPage);

        Task<int> GetPagesCount(Width width, Ratio ratio, Diameter diameter, Season season);

        Task<TyreDetailsDTO> Get(int id);

        Task<bool> Create<T>(CreateTyreDTO<T> model);

        Task<bool> UpdateQuantity(int id, int quantity);
    }
}
