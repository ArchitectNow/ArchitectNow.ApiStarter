using System;
using System.Net;
using Newtonsoft.Json;

namespace ArchitectNow.ApiStarter.Common.Models.Exceptions
{
	public abstract class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
	    protected object InternalContent { get; set; }

		protected ApiException(string message, object content = null) : this(HttpStatusCode.BadRequest, message, null, content)
        {
        }

	    protected ApiException(HttpStatusCode statusCode, string message, object content = null) : this(statusCode, message, null, content)
        {
        }

	    protected ApiException(HttpStatusCode statusCode, string message, Exception innerException, object content = null) : base(message, innerException)
        {
            StatusCode = statusCode;
            InternalContent = content;
        }

        public abstract string GetContent();

	    public virtual object GetRawContent()
	    {
		    return InternalContent;
	    }
    }

    public class ApiException<TContent>: ApiException, IApiException<TContent>
    {
	    public TContent Content
	    {
		    get => (TContent) InternalContent;
		    set => InternalContent = value;
	    }

	    public ApiException(string message, TContent content = default(TContent)) : this(HttpStatusCode.BadRequest, message, null, content)
        {
        }

	    public ApiException(HttpStatusCode statusCode, string message, TContent content = default(TContent)) : this(statusCode, message, null, content)
        {
        }

	    public ApiException(HttpStatusCode statusCode, string message, Exception innerException, TContent content = default(TContent)) : base(statusCode, message, innerException, content)
        {
            
        }

		public override string GetContent()
		{
			if (Content != null)
			{
				var body = JsonConvert.SerializeObject(Content);
				return body;
			}
			return null;
		}
    }
}
