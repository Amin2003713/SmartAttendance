using System;

namespace Shifty.Domain.Constants;

public static class ApplicationConstant
{
    public const string DbServer = "localhost";
    public const string MultipleActiveResultSets = "MultipleActiveResultSets=true";
    public const string Encrypt = "Encrypt=false";

    public const string UserNameAndPass =
#if DEBUG
        "Integrated Security=True;";
#else
        "User Id=sa;Password=0Lx[~1W@zab^NT05u'364BwlQo;";

#endif
}