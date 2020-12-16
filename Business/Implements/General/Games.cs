using Business.Interfaces.Base;
using Business.Interfaces.General;
using Data.Interfaces;
using Entities;
using Entities.Enums;
using Entities.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Implements.General
{
    public class Games : IGames
    {
        private readonly IRepository<Game> _repository;
        private readonly IUnitOfWork _unit;
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly GenericUtil genericUtil = new GenericUtil();

        public Games(IRepository<Game> repository,
            IResponseService responseService,
            IExceptionHandler exceptionHandler,
            IUnitOfWork unit)
        {
            _repository = repository;
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
            _unit = unit;
        }

        public async Task<IResponseService> Add(Game entity)
        {
            try
            {
                entity.CreatedDate = GenericUtil.GetDateZone(DateTime.Now);
                _repository.Add(entity);
                await _unit.Commit();
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, $"Id: { entity.Id}");
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> Delete(int id)
        {
            try
            {
                await _repository.Delete(id);
                await _unit.Commit();
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, "Ok");
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> Get(int id)
        {
            try
            {
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, await _repository.Get(id));
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> Update(Game entity)
        {
            try
            {
                entity.UpdateDate = GenericUtil.GetDateZone(DateTime.Now);
                _repository.Update(entity);
                await _unit.Commit();
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, "Ok");
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        public async Task<IResponseService> List()
        {
            try
            {
                var list = await _repository.List(o => o.OrderByDescending(x => x.Id), null);
                _responseService.SetResponse(true, MessagesEnum.HttpStateOk, list, list.Count());
                return _responseService;
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }
    }
}
