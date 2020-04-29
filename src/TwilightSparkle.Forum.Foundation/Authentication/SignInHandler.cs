using System.Threading.Tasks;

namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public delegate Task SignInHandler(string username, int userId, bool rememberMe);
}