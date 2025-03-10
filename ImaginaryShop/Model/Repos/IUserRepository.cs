﻿namespace ImaginaryShop.Model.Repos
{
    public interface IUserRepository
    {

        /// <summary>
        /// Opretter en ny bruger i databasen.
        /// </summary>
        /// <param name="user">Brugerobjekt med oplysninger.</param>
        /// <returns>Den oprettede bruger med UserId.</returns>
        User CreateUser(User user);

        /// <summary>
        /// Henter en bruger baseret på e-mailadresse.
        /// </summary>
        /// <param name="email">Brugerens e-mail.</param>
        /// <returns>Brugerobjekt eller null, hvis brugeren ikke findes.</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Henter en bruger baseret på brugernavn.
        /// </summary>
        /// <param name="username">Brugerens brugernavn.</param>
        /// <returns>Brugerobjekt eller null, hvis brugeren ikke findes.</returns>
        User GetUserByUserName(string username);

        /// <summary>
        /// Henter en bruger baseret på ID.
        /// </summary>
        /// <param name="userId">Brugerens unikke ID.</param>
        /// <returns>Brugerobjekt eller null, hvis brugeren ikke findes.</returns>
        User GetUserById(int userId);

        /// <summary>
        /// Opdaterer oplysninger for en eksisterende bruger.
        /// </summary>
        /// <param name="user">Brugerobjekt med opdaterede oplysninger.</param>
        /// <returns>Den opdaterede bruger.</returns>
        User UpdateUser(User user);

        /// <summary>
        /// Sletter en bruger fra databasen.
        /// </summary>
        /// <param name="userId">Brugerens unikke ID.</param>
        void DeleteUser(int userId);

        /// <summary>
        /// Henter en liste over alle brugere i databasen.
        /// </summary>
        /// <returns>Liste over brugere.</returns>
        List<User> GetAllUsers();
    }
}
