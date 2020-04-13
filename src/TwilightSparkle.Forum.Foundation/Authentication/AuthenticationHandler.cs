using System.Threading.Tasks;

namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public delegate Task AuthenticationHandler(string username, int userId, bool rememberMe);
}