using Company.Core.Models;

namespace Company.Application.Share.Events
{
    public class LoginSucceededEvent : PubSubEvent<CurrentUser>
    {
    }
}
