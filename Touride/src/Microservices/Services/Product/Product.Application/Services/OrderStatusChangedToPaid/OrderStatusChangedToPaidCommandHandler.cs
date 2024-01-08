using AutoMapper;
using Castle.Core.Logging;
using Product.Domain.AggregatesModel.CatalogItemAggregate;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Abstractions.Data.TransactionManagement;
using Touride.Framework.MediatR.Configuration.Commands;

namespace Product.Application.Services.OrderStatusChangedToPaid
{
    public class OrderStatusChangedToPaidCommandHandler : ICommandHandler<OrderStatusChangedToPaidCommand, Result<List<CatalogItem>>>
    {
        private readonly ICatalogItemRepository _context;
        private static readonly SemaphoreSlim _updateLock = new SemaphoreSlim(1, 1);
        // readonly ILogger<OrderStatusChangedToPaidCommandHandler> _logger;
        private readonly IMapper _mapper;
        public OrderStatusChangedToPaidCommandHandler(ICatalogItemRepository context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Transactional]
        public virtual async Task<Result<List<CatalogItem>>> Handle(OrderStatusChangedToPaidCommand request, CancellationToken cancellationToken)
        {
            await _updateLock.WaitAsync();
            try
            {
                var updateModel = new List<CatalogItem>();

                foreach (var orderStockItem in request.OrderStockItems)
                {
                    var catalogItem = await _context.FindAsync(p => p.Id == orderStockItem.ProductId);

                    if (catalogItem != null)
                    {
                        catalogItem.AvailableStock -= orderStockItem.Units;

                        updateModel.Add(catalogItem);
                    }
                }

                _context.Update(updateModel);

                var save = await _context.SaveChangesAsync();

                return save > 0 ? new SuccessResult<List<CatalogItem>>(updateModel)
                                : new NoContentResult<List<CatalogItem>>();

            }
            catch (Exception ex)
            {

                throw;
            }
            finally { _updateLock.Release(); }


        }



    }
}
