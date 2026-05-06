using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Master;
using Evaluation.DAL.Models.PermissionEntity;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.Extensions;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.BusinessLayer.API
{
	public class UserBL : ApiBase
	{
		private readonly MSJsonWT _msJsonWT;
		private readonly RequestInfo _requestInfo;
		private readonly SrvUser srvUser;

		public UserBL(
			IServiceScopeFactory serviceScopeFactory,
			CacheDataProvider cacheDataProvider,
			UnitOfWork uow,
			LoggingServices loggingServices,
			UserInfo userInfo,
			MSJsonWT msJsonWT,
			IMapper mapper,
			SrvUser srvUser,
			RequestInfo requestInfo,
			IServiceProvider serviceProvider)
			: base(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, requestInfo)
		{
			_msJsonWT = msJsonWT;
			this.srvUser = srvUser;
			_requestInfo = requestInfo;
		}
		
		public async Task<UserProfileDTO> GetUserDetails()
		{
				var userProfile = await serviceScopeFactory.CreateScopedUow().GetRepository<MinistryUser>()
					.GetAllActiveNonDeleted(x => x.Id == userInfo.UserId)
					.FirstOrDefaultAsync();

				var result = mapper.Map<UserProfileDTO>(userProfile);
				return result;
			
		}

	}

}
