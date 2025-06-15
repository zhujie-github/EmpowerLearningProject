﻿using Company.Application.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Application.Share.Events
{
    public class LoginSucceededEvent : PubSubEvent<CurrentUser>
    {
    }
}
