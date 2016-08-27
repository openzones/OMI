using AutoMapper;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMInsurance.WebApps.Models
{
    public class ModelMapper
    {
        public static void Configure()
        {
            Mapper.CreateMap<FundResponse, FundResponseModel>()
              .Include<S5FundResponse, S5FundResponseModel>()
              .Include<S6FundResponse, S6FundResponseModel>()
              .Include<S9FundResponse, S9FundResponseModel>()
              .Include<SnilsFundResponse, SnilsFundResponseModel>()
              .Include<SvdFundResponse, SvdFundResponseModel>()
              .Include<FundErrorResponse, FundErrorResponseModel>()
              .Include<GoznakResponse, GoznakResponseModel>();

            Mapper.CreateMap<S5FundResponse, S5FundResponseModel>();
            Mapper.CreateMap<S6FundResponse, S6FundResponseModel>();
            Mapper.CreateMap<S9FundResponse, S9FundResponseModel>();
            Mapper.CreateMap<SnilsFundResponse, SnilsFundResponseModel>();
            Mapper.CreateMap<SvdFundResponse, SvdFundResponseModel>();
            Mapper.CreateMap<FundErrorResponse, FundErrorResponseModel>();
            Mapper.CreateMap<GoznakResponse, GoznakResponseModel>();
            Mapper.CreateMap<FundResponse.UploadReportData, FundResponseUploadReportModel>();

            Mapper.Configuration.Seal();
            Mapper.AssertConfigurationIsValid();
        }
    }
}