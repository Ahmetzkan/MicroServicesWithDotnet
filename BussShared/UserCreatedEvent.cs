﻿namespace BussShared
{
    public record UserCreatedEvent(Guid UserId, string Email, string Phone);
}