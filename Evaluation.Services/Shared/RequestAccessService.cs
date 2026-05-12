using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.Models.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Exceptions;
using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Evaluation.SharedHelper.Enums.ConstantKeys;

namespace Evaluation.Services.Shared
{
	public class RequestAccessService(
		IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, 
		LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, SrvUser srvUser, 
		  IServiceProvider serviceProvider, RequestInfo _requestInfo)
			: ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
	
	{
		public async Task<IQueryable<EvaluationRequest>> ApplyEvaluationRequestAccess(IQueryable<EvaluationRequest> query)
		{
			var userId = userInfo.UserId
				?? throw new BusinessException(ExceptionMessage.UserNotFound);

			var user =await srvUser.GetByIDActiveNonDeleted(userId);

			var userPartyTypeIds = user!.UserPartTypes?
				.Select(x => x.PartyTypeId)
				.Distinct()
				.ToList() ?? new List<Guid>();

			var canViewAllRequests = user.UserPartTypes?
				.Any(x => x.PartyType != null &&
						  x.PartyType.CanViewAllRequests == true) == true;

			query = query
				.Include(x => x.Service)
					.ThenInclude(x => x!.RequestShowPartyType)
				.Include(x => x.ServiceStatus)
					.ThenInclude(x => x!.StatusPreventPartyTypes)
				.Include(x => x.EvaluationRequestAssignments);

			if (!canViewAllRequests)
			{
				query = query.Where(x =>
					x.EvaluationRequestAssignments != null &&
					x.EvaluationRequestAssignments.Any(a =>
						a.MinistryUserId == userId));
			}

			query = query.Where(x =>
				x.Service != null &&
				x.Service.RequestShowPartyType != null &&
				x.Service.RequestShowPartyType.Any(p =>
					userPartyTypeIds.Contains(p.PartyTypeId)));

			query = query.Where(x =>
				x.ServiceStatus != null &&
				x.ServiceStatus.StatusPreventPartyTypes != null &&
				!x.ServiceStatus.StatusPreventPartyTypes.Any(p =>
					userPartyTypeIds.Contains(p.PartyTypeId)));

			return query;
		}

		public async Task<IQueryable<ServiceRequest>> ApplyServiceRequestAccess(IQueryable<ServiceRequest> query)
		{
			var userId = userInfo.UserId
				?? throw new BusinessException(ExceptionMessage.UserNotFound);

			var user = await srvUser.GetByIDActiveNonDeleted(userId);

			if (user == null)
				throw new BusinessException(ExceptionMessage.UserNotFound);

			var userPartyTypeIds = user.UserPartTypes?
				.Select(x => x.PartyTypeId)
				.Distinct()
				.ToList() ?? new List<Guid>();

			var canViewAllRequests = user.UserPartTypes?
				.Any(x => x.PartyType != null &&
						  x.PartyType.CanViewAllRequests == true) == true;

			query = query
				.Include(x => x.Service)
					.ThenInclude(x => x!.RequestShowPartyType)
				.Include(x => x.Status)
					.ThenInclude(x => x!.StatusPreventPartyTypes);

			if (!canViewAllRequests)
			{
				query = query.Where(x =>
					(x.Assignments != null && x.Assignments.Any(a =>
						a.MinistryUserId == userId)) ||
					x.CreateById == userId);
				
			}

			query = query.Where(x =>
				x.Service != null &&
				x.Service.RequestShowPartyType != null &&
				x.Service.RequestShowPartyType.Any(p =>
					userPartyTypeIds.Contains(p.PartyTypeId)));

			query = query.Where(x =>
				x.Status != null &&
				x.Status.StatusPreventPartyTypes != null &&
				!x.Status.StatusPreventPartyTypes.Any(p =>
					userPartyTypeIds.Contains(p.PartyTypeId)));

			return query;
		}

		public async Task<IQueryable<Plan>> ApplyPlanAccess(IQueryable<Plan> query)
		{
			var userId = userInfo.UserId
				?? throw new BusinessException(ExceptionMessage.UserNotFound);

			var user = await srvUser.GetByIDActiveNonDeleted(userId);

			if (user == null)
				throw new BusinessException(ExceptionMessage.UserNotFound);

			var canViewAllPlans = user.UserPartTypes?
				.Any(x => x.PartyType != null &&
						  x.PartyType.CanViewAllPlan == true) == true;

			var canViewEvlRequestPlan = user.UserPartTypes?
				.Any(x => x.PartyType != null &&
						  x.PartyType.CanViewEvlRequetPlan == true) == true;

			if (canViewAllPlans)
				return query;

			if (canViewEvlRequestPlan)
			{
				query = query.Where(plan =>
					plan.EvaluationRequests != null &&
					plan.EvaluationRequests.Any(ev =>
						ev.EvaluationRequestAssignments != null &&
						ev.EvaluationRequestAssignments.Any(a =>
							a.MinistryUserId == userId)));
			}
			else
			{
				query = query.Where(x => false);
			}

			return query;
		}
	}
}
