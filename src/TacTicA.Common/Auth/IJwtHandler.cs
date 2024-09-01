using System;

namespace TacTicA.Common.Auth
{
    public interface IJwtHandler
    {
        JsonWebToken Create(Guid userId);
    }
}