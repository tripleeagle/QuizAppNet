using System.Collections.Generic;

namespace QuizappNet.Values
{
  public class GroupNames
  {
    public const string Admins = "admins";
    public const string SuperUsers = "superusers";
    public const string Users = "users";
    public static List<string> GroupNameList = new List<string> () {Admins, SuperUsers, Users};
  }
}