using System;
using System.Collections.Generic;
using System.Text;

namespace HQConnector.Dto.DTO.Response.Interfaces
{
    public interface IResultResponse<TDataResponse>
    {
        TDataResponse Data { get; set; }

        IErrorResult ErrorResult { get; set; }
    }
}
