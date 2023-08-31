using System;
using System.Collections.Generic;
using System.Text;
using HQConnector.Dto.DTO.Response.Interfaces;

namespace HQConnector.Dto.DTO.Response
{
    public class MessageResponse<TData> : DetailResponse<TData>
    {
        public MessageResponse(TData data, IErrorResult error) : base(data, error)
        {

        }
        
    }
}
