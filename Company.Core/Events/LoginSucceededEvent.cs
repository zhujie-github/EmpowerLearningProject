using Company.Core.Models;

namespace Company.Core.Events
{
    public class LoginSucceededEvent : PubSubEvent<CurrentUser>
    {
    }
}
