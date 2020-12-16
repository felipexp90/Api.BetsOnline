using Business.Interfaces.Base;
using Business.Interfaces.General;
using Entities;
using Entities.Enums;
using Entities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.BetsOnline.Logic
{
    internal class BetProcess
    {
        private readonly IResponseService _responseService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IGames _gameService;
        private readonly IBets _betsService;

        internal BetProcess(IResponseService responseService,
                            IExceptionHandler exceptionHandler,
                            IGames gameService,
                            IBets betsService
                            )
        {
            _responseService = responseService;
            _exceptionHandler = exceptionHandler;
            _gameService = gameService;
            _betsService = betsService;
        }

        internal async Task<IResponseService> AddBet(Bet data)
        {
            try
            {
                Game response = await GetGame(data.IdGame);
                if (response != null && response.Id > 0 && response.Enabled)
                    return await _betsService.Add(data);
                else
                    throw new ArgumentException(MessagesEnum.GameNotAvailable);
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        internal async Task<IResponseService> UpdateBet(Bet data)
        {
            try
            {
                Bet betResponse = await GetBet(data.Id);
                if (betResponse != null)
                {
                    Game gameResponse = await GetGame(data.IdGame);
                    if (gameResponse != null && gameResponse.Id > 0 && gameResponse.Enabled)
                        return await _betsService.Update(data);
                    else
                        throw new ArgumentException(MessagesEnum.GameNotAvailable);
                }
                else throw new ArgumentException(MessagesEnum.BetNotAvailable);
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        internal async Task<IResponseService> CloseBet(int idGame)
        {
            try
            {
                Game gameResponse = await GetGame(idGame);
                if (gameResponse != null && gameResponse.Id > 0 && gameResponse.Enabled)
                {
                    gameResponse = await SendAndCloseDataGame(gameResponse);
                    IResponseService getListGame = await _betsService.GetByIdGame(idGame);
                    await UpdateDataWinningBets(gameResponse, getListGame);
                    return getListGame;
                }
                else
                    throw new ArgumentException(MessagesEnum.GameNotAvailable);
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
                return _responseService;
            }
        }

        private async Task UpdateDataWinningBets(Game gameResponse, IResponseService getListGame)
        {
            List<Bet> listBet = listBetByIdGame(getListGame);
            var winnerNumberList = listBet.Where(x => x.Number == gameResponse.WinningNumber).ToList();
            var winnerColorList = listBet.Where(x => x.Color == gameResponse.WinningColor).ToList();
            UpdateDataWinningBets(winnerNumberList, 5);
            UpdateDataWinningBets(winnerColorList, 1.8);
        }

        private async Task<Game> SendAndCloseDataGame(Game gameResponse)
        {
            int randomNumber = GenericUtil.RandomNumber(0, 36);
            gameResponse.WinningNumber = randomNumber;
            gameResponse.WinningColor = ColorWinner(randomNumber);
            gameResponse.Enabled = false;
            await _gameService.Update(gameResponse);
            return gameResponse;
        }

        private async void UpdateDataWinningBets(List<Bet> winnerList, double WinnerValue)
        {
            try
            {
                List<Bet> listBet = new List<Bet>();
                foreach (Bet dataWinner in winnerList)
                {
                    Bet dataBet = new Bet();
                    dataBet = dataWinner;
                    dataBet.EarnedMoney = dataWinner.MoneyBet * WinnerValue;
                    dataBet.IsWinner = true;
                    listBet.Add(dataBet);
                }
                await _betsService.BulkUpdate(listBet);
            }
            catch (Exception ex)
            {
                _responseService.Meta.Errors.Add(_exceptionHandler.GetMessage(ex));
                _responseService.Meta.HttpStatus = MessagesEnum.HttpStateBadRequest;
            }
        }

        private async Task<Game> GetGame(int idGame)
        {
            var getGame = await _gameService.Get(idGame);
            object elementObject = GenericUtil.GetElementsResultService(getGame.Result);
            return (Game)elementObject;
        }

        private async Task<Bet> GetBet(int idBet)
        {
            var getGame = await _betsService.Get(idBet);
            object elementObject = GenericUtil.GetElementsResultService(getGame.Result);
            return (Bet)elementObject;
        }

        private List<Bet> listBetByIdGame(IResponseService getListGame)
        {
            object elementObject = GenericUtil.GetElementsResultService(getListGame.Result);
            return (List<Bet>)elementObject;
        }

        private string ColorWinner(int random)
        {
            if ((random % 2) == 0)
                return "rojo";
            else
                return "negro";
        }
    }
}
