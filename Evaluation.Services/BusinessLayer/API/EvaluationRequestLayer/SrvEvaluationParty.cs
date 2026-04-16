using AutoMapper;
using Evaluation.DAL.Helper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.DAL.Repositories;
using Evaluation.Services.BusinessLayer.API.DepartmentLayer;
using Evaluation.Services.BusinessLayer.API.FormBuilderLayer.Srvices;
using Evaluation.Services.Extensions;
using Evaluation.Services.Models.API;
using Evaluation.Services.Special;
using Evaluation.SharedHelper.Models;
using Evaluation.SharedHelper.Models.Api.EvaluationRequestEntities;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.BusinessLayer.API.EvaluationRequestLayer
{
	public class SrvEvaluationParty(
		IServiceScopeFactory serviceScopeFactory, CacheDataProvider cacheDataProvider, UnitOfWork uow, SrvNotification SrvNotification, SrvUser SrvUser,
		LoggingServices loggingServices, IMapper mapper, UserInfo userInfo, SrvAction SrvAction,
		SrvStatus SrvStatus, SrvAssignment SrvAssignment, SrvActionTransactionsLog SrvActionTransactionsLog, PerformActionBL _performActionBL,
		SrvService SrvService,   SrvAttachments _srvAttachments, IServiceProvider serviceProvider, RequestInfo _requestInfo)
			: ApiBase(serviceScopeFactory, cacheDataProvider, uow, loggingServices, mapper, userInfo, serviceProvider, _requestInfo)
	
	{

		public async Task<List<EvaluationParty>> GetAllEvaluationPartiesAsync(Guid DepartmentId)
		{
			var EvaluationParty=  serviceScopeFactory.CreateScopedUow()
										.GetRepository<EvaluationParty>()
				.GetAllActiveNonDeleted()
				.Where(x=>x.DepartmentId== DepartmentId)
				.Select(d => new EvaluationParty
				{
					Id = d.Id,
					NameAr = d.NameAr,
					NameEn = d.NameEn,
				
				}).ToList();

			return EvaluationParty;

		}

		public async Task<EvaluationParty?> GetEvaluationPartyByIdAsync(Guid evaluationPartyId)
		{
			return await serviceScopeFactory
				.CreateScopedUow()
				.GetRepository<EvaluationParty>()
				.GetAllActiveNonDeleted(x => x.Id == evaluationPartyId)
				.Select(d => new EvaluationParty
				{
					Id = d.Id,
					NameAr = d.NameAr,
					NameEn = d.NameEn,
					OrderNo = d.OrderNo,
					DepartmentId = d.DepartmentId
				})
				.FirstOrDefaultAsync();
		}
		public async Task<List<EvaluationPartyDTO>> GetPartiesWithServicesAndRequestsAsync(Guid evaluationRequestId,Guid DepartementId, Guid? requestStatusId,List<Guid> partyTypeIds)
		{
			var Lang=requestInfo.Lang;
			using var uow = serviceScopeFactory.CreateScopedUow();
			var parties = await uow
				.GetRepository<EvaluationParty>()
				.GetAllQueryFiltered()
				.Include(x=>x.PartyTypeEvalParties)
				.ThenInclude(x=>x.PartyTypeEvalPartyStatuses)
				.Where(p => p.DepartmentId== DepartementId  && (
							!partyTypeIds.Any() ||requestStatusId == null ||
							p.PartyTypeEvalParties.Any(pt =>partyTypeIds.Contains(pt.PartyTypeId) &&
															pt.PartyTypeEvalPartyStatuses.Any(s => s.ServiceStatusId == requestStatusId))
					))
				.OrderBy(p => p.OrderNo)
				.Select(p => new EvaluationPartyDTO
				{
					Id = p.Id,
					DepartmentId = p.DepartmentId,
					NameAr = p.NameAr,
					NameEn = p.NameEn,
					OrderNo = p.OrderNo,
                    IsSupportFiles = p.IsSupportFiles,
                    Services = new List<EvaluationPartyServiceDTO>()
				}).ToListAsync();

			if (!parties.Any())
				return parties;

			var partyIds = parties.Select(p => p.Id).ToList();

			var services = await uow
				.GetRepository<Service>()
				.GetAllActiveNonDeleted(s => s.EvaluationPartyId != null && partyIds.Contains(s.EvaluationPartyId.Value))
				.OrderBy(s => s.OrderNo)
				.Select(s => new
				{
					s.Id,
					s.NameAr,
					s.NameEn,
					s.BackendName,
					s.OrderNo,
					s.IsFreez,
					s.ShowInWebSite,
					s.StartDate,
					s.EndDate,
					s.EvaluationPartyId
				})
				.ToListAsync();

			var servicesByParty = services
				.GroupBy(s => s.EvaluationPartyId!.Value)
				.ToDictionary(
					g => g.Key,
					g => g.Select(s => new EvaluationPartyServiceDTO
					{
						Id = s.Id,
						NameAr = s.NameAr,
						NameEn = s.NameEn,
						BackendName = s.BackendName,
						OrderNo = s.OrderNo,
						IsFreez = s.IsFreez,
						ShowInWebSite = s.ShowInWebSite,
						StartDate = s.StartDate,
						EndDate = s.EndDate
					}).ToList()
				);

			foreach (var party in parties)
			{
				if (servicesByParty.TryGetValue(party.Id, out var list))
					party.Services = list;
			}

			var requests = await uow
				.GetRepository<ServiceRequest>()
				.GetAllActiveNonDeleted(r => r.EvaluationRequestId == evaluationRequestId).Include(x=>x.Status)
				.Select(r => new
				{
					r.Id,
					r.RequestNumber,
					r.StatusId,
					r.Status!.ServiceStatusType!.IsOpen,
					StatusNameAr = r.Status!.NameAr,
					StatusNameEn = r.Status!.NameEn,
					r.ServiceId,
					r.EvaluationPartyId,
					r.CreateDate,
					r.CreateBy,
					r.UpdateDate,
					r.Service
				})
				.ToListAsync();

			var requestsByService = requests
				.GroupBy(r => r.ServiceId)
				.ToDictionary(
					g => g.Key,
					g => g.Select(r => new ServiceRequestDTO
					{
						Id = r.Id,
						RequestNumber = r.RequestNumber,
						StatusId = r.StatusId,
						Service = Lang == "ar" ? r.Service!.NameAr : r.Service!.NameEn,
                        StatusISOPen = r.IsOpen,
						Status = Lang == "ar" ? r.StatusNameAr : r.StatusNameEn,
						ServiceId = r.ServiceId,
						EvaluationPartyId = r.EvaluationPartyId,
						CreateDate = r.CreateDate,
						UpdateDate = r.UpdateDate,
                        CreateBy=Lang == "ar" ? r.CreateBy.NameAr : r.CreateBy.NameEn

                    })
					.OrderByDescending(x => x.CreateDate)
					.ToList()
				);

			foreach (var party in parties)
			{
				foreach (var service in party.Services)
				{
					if (requestsByService.TryGetValue(service.Id, out var list))
						service.Requests = list;
				}
			}

			return parties;
		}


	}
}
