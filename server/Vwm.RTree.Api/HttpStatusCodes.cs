namespace Vwm.RTree.Api
{
  public static class HttpStatusCodes
  {
    #region 2XX

    public const int _Ok = 200;
    public const int _Created = 201;
    public const int _NoContent = 204;

    #endregion

    #region 3XX

    public const int _Found = 302;

    #endregion

    #region 4XX

    public const int _BadRequest = 400;
    public const int _Unauthorized = 401;
    public const int _Forbidden = 403;
    public const int _NotFound = 404;
    public const int _Conflict = 409;
    public const int _EmailNotVerified = 430;
    public const int _UserDisabled = 431;

    #endregion

    #region 5XX

    public const int _InternalServerError = 500;
    public const int _NotImplemented = 501;
    public const int _TaskCanceled = 515;

    #endregion
  }
}
