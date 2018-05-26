﻿#region Using Statements

using System;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

#endregion Using Statements

namespace GlancePay.OmnivoreIntegration.ExceptionHandling
{
    /// <summary>
    /// Specialized Omnivore Integration exception 
    /// <remarks>
    /// See "What is the correct way to make a custom .NET Exception serializable?"
    /// http://stackoverflow.com/questions/94488/what-is-the-correct-way-to-make-a-custom-net-exception-serializable
    /// </remarks>
    /// </summary>
    [Serializable]
    public abstract class ApiException : Exception
    {
        private Guid id = Guid.Empty;

        #region Constructors

        public ApiException(HttpStatusCode statusCode) : base()
        {
            ID = Guid.NewGuid();
            StatusCode = statusCode;
        }

        public ApiException(HttpStatusCode statusCode, string errorCode, string errorDescription)
            : base($"{errorCode}::{errorDescription}")
        {
            ID = Guid.NewGuid();
            StatusCode = statusCode;
        }

        public ApiException(Exception exception)
            : this(exception.Message, exception) { }

        public ApiException(string message, Exception exception)
            : base(message, exception)
        {
            ID = Guid.NewGuid();
        }

        // Without this constructor, deserialization will fail
        protected ApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ID = (Guid)info.GetValue("ID", typeof(Guid));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("ID", this.ID, typeof(Guid));

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Http Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        public new string Source
        {
            get
            {
                string source = (InnerException == null)
                    ? string.Empty
                    : InnerException.Source;
                return source;
            }
        }

        public new MethodBase TargetSite
        {
            get
            {
                MethodBase targetSite = InnerException?.TargetSite;
                return targetSite;
            }
        }

        public new string StackTrace
        {
            get
            {
                string stackTrace = (InnerException == null)
                    ? string.Empty
                    : InnerException.StackTrace;
                return stackTrace;
            }
        }

        #endregion Public Properties
    }
}
