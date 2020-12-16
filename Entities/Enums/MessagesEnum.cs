namespace Entities.Enums
{
    public class MessagesEnum
    {
        public const string AdminError = "Error fatal, por favor contactese con el administrador";
        public const string BetNotAvailable = "Bet not available or not exists";
        public const string DbError = "Error en el motor de Base de Datos";
        public const string DbUpdateException = "Hubo un error al actualizar el modelo";
        public const string FatalError = "Error fatal";
        public const string GameNotAvailable = "Game not available or closed by Bet";
        public const string HttpStateAccepted = "202 - Accepted";
        public const string HttpStateBadRequest = "400 - Bad Request";
        public const string HttpStateNotFound = "404 - Not Found";
        public const string HttpStateOk = "200 - Ok";
        public const string HttpStateUnauthorized = "401 - Unauthorized";
        public const string InvalidModel = "Modelo inválido";
        public const string InvalidColor = "Invalid Color for Bet";
        public const string OwnError = "Error controlado o regla de negocio";
        public const string PropertyNotEmpty = "El campo no puede ser vacío.";
        public const string PropertyNotNull = "El campo no puede ser nulo.";
    }
}