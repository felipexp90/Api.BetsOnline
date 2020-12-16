namespace Business.Interfaces.Base
{
    public interface IResponseService
    {
        dynamic Result { get; set; }
        public dynamic Meta { get; set; }
        void SetResponse(bool state, string httpStatus, dynamic result = null, int totalItems = 0);
    }
}