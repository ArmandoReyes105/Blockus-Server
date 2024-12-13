using Services.Dtos;
using System.Collections.Generic;
using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract]
    public interface IAccountService
    {
        /// <summary>
        /// Crear una nueva cuenta
        /// </summary>
        /// <param name="accountDTO">objeto que representa los datos de la cuenta a crear</param>
        /// <returns>El estado de la operación, Mayor a 0 = exito, 0 = fallo</returns>
        [OperationContract]
        int CreateAccount(AccountDTO accountDTO);

        /// <summary>
        /// Actualiza una cuenta existente
        /// </summary>
        /// <param name="accountDTO">Objeto que representa la cuenta a actualizar, con los nuevos datos</param>
        /// <returns>El estado de la operación, mayor a 0 = exito</returns>
        [OperationContract]
        int UpdateAccount(AccountDTO accountDTO);

        /// <summary>
        /// Obtiene los resultados de victorias y derrotas de la cuenta
        /// </summary>
        /// <param name="idAccount">El identificador único de la cuenta</param>
        /// <returns>Un objeto que contiene los resultados de la cuenta</returns>
        [OperationContract]
        ResultsDTO GetAccountResults(int idAccount);

        /// <summary>
        /// Obtiene la configuración de cuenta, estilo de tablero, imagen de perfil, etc.
        /// </summary>
        /// <param name="idAccount">El identificador único de la cuenta</param>
        /// <returns>Un objeto que contiene la configuración del perfil</returns>
        [OperationContract]
        ProfileConfigurationDTO GetProfileConfiguration(int idAccount);

        /// <summary>
        /// Crea una nueva relación entre la cuenta actual y la del amigo
        /// </summary>
        /// <param name="idAccount">Id de la cuenta principal</param>
        /// <param name="IdAccountFriend">Identificador de la cuenta que quiere agregar</param>
        /// <returns></returns>
        [OperationContract]
        int AddFriend(int idAccount, int IdAccountFriend);

        /// <summary>
        /// Obtiene las cuentas de las personas que tiene agregadas como amigos
        /// </summary>
        /// <param name="idAccount">Identificador de la cuenta actual</param>
        /// <returns></returns>
        [OperationContract]
        List<PublicAccountDTO> GetAddedFriends(int idAccount);

        /// <summary>
        /// Elimina la relación de amigos entre una cuenta y otra
        /// </summary>
        /// <param name="idFriend">Identificador único de la cuenta del amigo</param>
        /// <param name="idAccount">Identificador único de la cuenta actual</param>
        /// <returns></returns>
        [OperationContract]
        int DeleteFriend(int idFriend, int idAccount);

        /// <summary>
        /// Obtiene las cuentas que coincidan con el string de busqueda
        /// </summary>
        /// <param name="username">Cadena de texto que contiene un username o parte de el</param>
        /// <returns>Una lista de la información publica de las cuentas que coincidan</returns>
        [OperationContract]
        List<PublicAccountDTO> SearchByUsername(string username);
    }
}
