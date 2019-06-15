
namespace QuizappNet.Utils
{
    public struct EngStrings
    {
        public static readonly string InvalidObject = "The object isn't valid";
        public static readonly string InvalidLoginOrPassword = "Login or/and password isn't correct ";
        public static string NotFound ( long id ) =>  "The item with id " + id + " wasn't found";
        public static string UserAlreadyExists ( string username ) =>  "The user with username " + username + " already exists";
    }
}