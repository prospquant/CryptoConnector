using System;
using System.Collections.Generic;
using System.Text;
using HQConnector.Dto.DTO.Enums.Exchange;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Dto.DTO.Response
{
    public abstract class DetailResponse<TData> : IResultResponse<TData>
    {
        public TData Data { get; set; }

        public IErrorResult ErrorResult { get; set; }

     
        protected DetailResponse(TData data, IErrorResult error)
        {
            Data = data;
            ErrorResult = error;
        }
        
    }
}
